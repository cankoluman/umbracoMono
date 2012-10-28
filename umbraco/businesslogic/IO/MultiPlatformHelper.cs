using System;
using System.Text;

namespace umbraco.IO
{
	public static class MultiPlatformHelper
	{

		public const string PLATFORM_UNIX = "UNIX";
		public const string PLATFORM_WIN = "WIN";

		public const string WIN_DIRSEP = "\\";
		public const string UNIX_DIRSEP = "/";

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
				return OSPlatform.Contains(PLATFORM_WIN);
			}
		}

		public static bool IsUnix
		{
			get
			{
				return OSPlatform.Contains(PLATFORM_UNIX);
			}
		}

		public static string MapUnixPath(string path)
		{
			string filePath = path;

			if (filePath.StartsWith("~"))
				filePath = IOHelper.ResolveUrl(filePath);

			filePath = IOHelper.MapPath(filePath, true);

			return filePath;
		}

		public static string ConvertPathFromUnixToWin(string path)
		{
			return path.Replace(MultiPlatformHelper.UNIX_DIRSEP, MultiPlatformHelper.WIN_DIRSEP);
		}

		public static string EnsureRootAppPath (string path)
		{
			if (IsUnix && (path == String.Empty))
				return "/";
			return path;
		}

	}
}

