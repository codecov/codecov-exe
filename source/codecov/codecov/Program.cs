using System;
using codecov.Coverage;
using codecov.Services;
using codecov.Services.Utils;

namespace codecov
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //try
            //{
            var options = new Options();
            var isValid = CommandLine.Parser.Default.ParseArgumentsStrict(args, options);
            if (!isValid)
            {
                Console.WriteLine("The command line arguments are invalid.");
                return;
            }

            var services = new IDetect[] { new AppVeyor(), new Git(), new Hg() };
            var codeCov = new Uploder(options, new Report(options.FilePath), new Service(services));
            codeCov.Upload();
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //    Console.WriteLine(ex.StackTrace);
            //}
        }
    }
}