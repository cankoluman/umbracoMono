using System;
using Umbraco.Core.IO;

namespace Umbraco.Core.MultiPlatform
{
	public class PlatformHelper
	{
		public const string OS_PLATFORM_UNIX = "UNIX";
		public const string OS_PLATFORM_WIN = "WIN";

		public const string NET_PLATFORM_WIN = "Windows";
		public const string NET_PLATFORM_MONO = "Mono";

		public const string WIN_DIRSEP = "\\";
		public const string UNIX_DIRSEP = "/";

		public static int NetPlatform
		{
			get
			{
				return System.Environment.Version.Major;
			}
		}

		public static string OSPlatform
		{
			get
			{
				return System.Environment.OSVersion.Platform.ToString().ToUpper();
			}
		}

		public static bool IsWindows
		{
			get
			{
				return OSPlatform.Contains(OS_PLATFORM_WIN);
			}
		}

		public static bool IsUnix
		{
			get
			{
				return OSPlatform.Contains(OS_PLATFORM_UNIX);
			}
		}

		public static bool IsMono
		{
			get
			{
				return Type.GetType ("Mono.Runtime") != null;
			}
		}

		public static string DirSepChar
		{
			get
			{
				if (IsUnix)
					return UNIX_DIRSEP;
				else
					return WIN_DIRSEP;
			}
		}

		public static string MapUnixPath(string path)
		{
			string filePath = path;

			if (filePath.StartsWith("~"))
				filePath = IOHelper.ResolveUrl(filePath);

			filePath = IOHelper.MapPathBase(filePath, System.Web.HttpContext.Current != null);

			return filePath;
		}

		public static string MapUnixPath(string path, bool useHttpContext)
		{
			string filePath = path;

			if (filePath.StartsWith("~"))
				filePath = IOHelper.ResolveUrl(filePath);

			filePath = IOHelper.MapPathBase(filePath, useHttpContext);

			return filePath;
		}

		public static string ConvertPathFromUnixToWin(string path)
		{
			return path.Replace(UNIX_DIRSEP, WIN_DIRSEP);
		}

		public static string EnsureRootAppPath (string path)
		{
			if (IsUnix && (path == String.Empty))
				return "/";
			return path;
		}

	}
}

