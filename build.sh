#!/usr/bin/env bash

# Define variables
SCRIPT_DIR=$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )
source $SCRIPT_DIR/build.config
TOOLS_DIR=$SCRIPT_DIR/tools
CAKE_EXE=$TOOLS_DIR/dotnet-cake
CAKE_PATH=$TOOLS_DIR/.store/cake.tool/$CAKE_VERSION
DOTNET_EXE=$(which dotnet 2>/dev/null)
INSTALL_NETCORE=0

if [ "$CAKE_VERSION" = "" ] || [ "$DOTNET_SDKS" = "" ]; then
    echo "An error occured while parsing Cake / .NET Core SDK version."
    exit 1
fi

if [ ! -d "$SCRIPT_DIR/.dotnet" ]; then
    DOTNET_EXE="$SCRIPT_DIR/.dotnet/dotnet"
    export PATH="$SCRIPT_DIR/.dotnet:$PATH"
    export DOTNET_ROOT="$SCRIPT_DIR/.dotnet"
fi

if [ "$DOTNET_EXE" = "" ]; then
    INSTALL_NETCORE=1
elif [ "$DOTNET_SDKS" != "" ]; then
    for sdk in $(echo $DOTNET_SDKS | sed "s/,/ /g")
    do
        DOTNET_INSTALLED_VERSION=$($DOTNET_EXE --list-sdks 2>/dev/null | grep "^\s*$sdk\+")
        if [ "$DOTNET_INSTALLED_VERSION" == "" ]; then
            INSTALL_NETCORE=1
            break
        fi
    done
fi

if [ "$INSTALL_NETCORE" = "1" ]; then
    if [ ! -d "$SCRIPT_DIR/.dotnet" ]; then
        mkdir "$SCRIPT_DIR/.dotnet"
    fi
    curl -Lsfo "$SCRIPT_DIR/.dotnet/dotnet-install.sh" https://dot.net/v1/dotnet-install.sh

    for sdk in $(echo $DOTNET_SDKS | sed "s/,/ /g")
    do
        bash "$SCRIPT_DIR/.dotnet/dotnet-install.sh" --version $sdk --install-dir "$SCRIPT_DIR/.dotnet" --no-path
    done

    DOTNET_EXE="$SCRIPT_DIR/.dotnet/dotnet"
    export PATH="$SCRIPT_DIR/.dotnet:$PATH"
    export DOTNET_ROOT="$SCRIPT_DIR/.dotnet"
elif [ -d "/opt/dotnet/sdk" ]; then # Fix for dotnet-cake not finding sdk version
    export DOTNET_ROOT="/opt/dotnet"
fi

export DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1
export DOTNET_CLI_TELEMETRY_OPTOUT=1

CAKE_INSTALLED_VERSION=$(dotnet-cake --version 2>&1)

if [ "$CAKE_VERSION" != "$CAKE_INSTALLED_VERSION" ]; then
    if [ ! -f "$CAKE_EXE" ] || [ ! -d "$CAKE_PATH" ]; then
        if [ -f "$CAKE_EXE" ]; then
            $DOTNET_EXE tool uninstall --tool-path $TOOLS_DIR Cake.Tool
        fi

        echo "Installing Cake $CAKE_VERSION..."
        $DOTNET_EXE tool install --tool-path $TOOLS_DIR --version $CAKE_VERSION Cake.Tool
        if [ $? -ne 0 ]; then
            echo "An error occured while installing Cake."
            exit 1
        fi
    fi

    # Make sure that Cake has been installed.
    if [ ! -f "$CAKE_EXE" ]; then
        echo "Could not find Cake.exe at '$CAKE_EXE'."
        exit 1
    fi
else
    CAKE_EXE="dotnet-cake"
fi

###########################################################################
# RUN BUILD SCRIPT
###########################################################################

# Start Cake
(exec "$CAKE_EXE" setup.cake --bootstrap) && (exec "$CAKE_EXE" setup.cake "$@")
