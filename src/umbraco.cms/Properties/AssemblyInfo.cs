using System.Reflection;
using System.Runtime.CompilerServices;

//
// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
//
[assembly: AssemblyTitle("umbraco.cms")]
[assembly: AssemblyDescription("Core assembly containing CMS functionality")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyProduct("Umbraco CMS")]

//AllInternalsVisible = true breaks visibility in mono
[assembly: InternalsVisibleTo("umbraco")]
[assembly: InternalsVisibleTo("Umbraco.LegacyTests")]
[assembly: InternalsVisibleTo("Umbraco.Tests")]
[assembly: InternalsVisibleTo("umbraco.editorControls")]