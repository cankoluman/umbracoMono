using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Web;

using umbraco.interfaces;

namespace umbraco.BusinessLogic
{
	public class ConfigurationManagerDefault : IConfigurationManager
	{
		public ConfigurationManagerDefault (){}

		public NameValueCollection AppSettings
		{
			get
			{
				return ConfigurationManager.AppSettings;
			}
		}

		public Object GetSection(string SectionName)
		{
			return ConfigurationManager.GetSection(SectionName);
		}

		public void RefreshSection(string SectionName)
		{
			ConfigurationManager.RefreshSection(SectionName);
		}
	}
}

