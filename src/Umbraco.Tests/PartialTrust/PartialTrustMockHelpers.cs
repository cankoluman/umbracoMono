using System;
using System.Runtime.InteropServices;

namespace Umbraco.Tests.PartialTrust
{
	public interface IRunTimeEnvironment
	{
		string SystemConfigurationFile {get; set;}
	}
}

