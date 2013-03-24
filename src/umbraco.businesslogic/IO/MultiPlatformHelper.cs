using System;
using Umbraco.Core.IO;

namespace umbraco.IO
{
	[Obsolete("Use Umbraco.Core.IO.MultiPlatformHelper instead")]
	public static class MultiPlatformHelper
	{
		
		public static string OSPlatform
		{
			get
			{
				return Umbraco.Core.IO.MultiPlatformHelper.OSPlatform;
			}
		}
		
		public static bool IsWindows
		{
			get
			{
				return Umbraco.Core.IO.MultiPlatformHelper.IsWindows;
			}
		}
		
		public static bool IsUnix
		{
			get
			{
				return Umbraco.Core.IO.MultiPlatformHelper.IsUnix;
			}
		}
		
		public static string MapUnixPath(string path)
		{
			return Umbraco.Core.IO.MultiPlatformHelper.MapUnixPath(path);
		}
		
		public static string MapUnixPath(string path, bool useHttpContext)
		{
			return Umbraco.Core.IO.MultiPlatformHelper.MapUnixPath(path, useHttpContext);
		}
		
		public static string ConvertPathFromUnixToWin(string path)
		{
			return Umbraco.Core.IO.MultiPlatformHelper.ConvertPathFromUnixToWin(path);
		}
		
		public static string EnsureRootAppPath (string path)
		{
			return Umbraco.Core.IO.MultiPlatformHelper.EnsureRootAppPath(path);
		}
		
	}
}

