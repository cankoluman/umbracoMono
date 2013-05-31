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
	}
}

