using System;
using System.Xml.XPath;

namespace Umbraco.Core.MultiPlatform
{
	public static class XmlHelper
	{
		public static XPathNavigator Current(this XPathNodeIterator xpi)
		{
			if (PlatformHelper.IsMono)
				xpi.MoveNext();
			return xpi.Current;
		}
	}
}

