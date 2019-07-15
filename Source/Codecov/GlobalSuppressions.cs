// This file is used by Code Analysis to maintain SuppressMessage attributes that are applied to this
// project. Project-level suppressions either have no target or are given a specific target and
// scoped to a namespace, type, member, etc.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Wrong Usage", "DF0020:Marks undisposed objects assinged to a field, originated in an object creation.", Justification = "It gets disposed when application exits", Scope = "member", Target = "~M:Codecov.Upload.CodecovUploader.#cctor")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Wrong Usage", "DF0021:Marks undisposed objects assinged to a field, originated from method invocation.", Justification = "It gets disposed when application exits", Scope = "member", Target = "~M:Codecov.Logger.Log.Create(System.Boolean,System.Boolean)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Wrong Usage", "DF0022:Marks undisposed objects assinged to a property, originated in an object creation.", Justification = "It gets disposed when parent is disposed", Scope = "member", Target = "~M:Codecov.Upload.CodecovUploader.CreateResponse(System.Net.Http.HttpRequestMessage)~System.Net.Http.HttpResponseMessage")]
