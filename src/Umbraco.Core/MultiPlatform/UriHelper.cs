using System;
using System.Web;

namespace Umbraco.Core.MultiPlatform
{
	public class UriHelper
	{
		public static string GetHostName(Uri absoluteUri)
		{
			if (PlatformHelper.IsUnix)
				return GetAbsoluteUriHostNameForMono(absoluteUri);

			return absoluteUri.Host;
		}

		//mono: fix for absolute uri from absolute path
		//has no host name
		public static string GetAbsoluteUriHostNameForMono(Uri uri)
		{
			if (uri.Scheme == Uri.UriSchemeFile)
				return HttpContext.Current.Request.Url.Host;

			return uri.Host;
		}
	}
}

