// This file is used by Code Analysis to maintain SuppressMessage attributes that are applied to
// this project. Project-level suppressions either have no target or are given a specific target and
// scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Wrong Usage", "DF0020:Marks undisposed objects assinged to a field, originated in an object creation.", Justification = "It gets disposed when application exits", Scope = "member", Target = "~M:Codecov.Upload.CodecovUploader.#cctor")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Wrong Usage", "DF0021:Marks undisposed objects assinged to a field, originated from method invocation.", Justification = "It gets disposed when application exits", Scope = "member", Target = "~M:Codecov.Logger.Log.Create(System.Boolean,System.Boolean)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Wrong Usage", "DF0022:Marks undisposed objects assinged to a property, originated in an object creation.", Justification = "It gets disposed when parent is disposed", Scope = "member", Target = "~M:Codecov.Upload.CodecovUploader.CreateResponse(System.Net.Http.HttpRequestMessage)~System.Net.Http.HttpResponseMessage")]
[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "We want to catch the general exception for logging purposes.", Scope = "member", Target = "~M:Codecov.Program.Run.Runner(System.Collections.Generic.IEnumerable{System.String})~System.Int32")]
[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "In theory an exception should not happen here, but catch it just in case.", Scope = "member", Target = "~M:Codecov.Program.Program.Main(System.String[])~System.Int32")]
