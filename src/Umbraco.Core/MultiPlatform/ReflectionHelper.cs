using System;
using System.Reflection;

namespace Umbraco.Core.MultiPlatform
{
	public class ReflectionHelper
	{
		public static BindingFlags GetBindingFlagsCasingSafe(BindingFlags bindingFlags)
		{
			if (PlatformHelper.IsMono)
				return bindingFlags | BindingFlags.IgnoreCase;

			return bindingFlags;
		}

		public static bool IsMissingMemberExceptionSafe(Exception ex)
		{
			if (PlatformHelper.IsMono)
				return (ex is MissingMemberException);

			return (ex is MissingMethodException); 
		}
	}
}

