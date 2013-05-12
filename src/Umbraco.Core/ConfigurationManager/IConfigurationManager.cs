using System;
using System.Configuration;
using System.Collections.Specialized;

namespace Umbraco.Core.Configuration
{
	public interface IConfigurationManager
	{
		NameValueCollection AppSettings {get;}

		T GetSection<T>(string SectionName);
		void RefreshSection(string SectionName);

		void SetAppSetting(string key, string val);
		void ClearAppSetting(string key);

		ConnectionStringSettingsCollection ConnectionStrings {get;}
	}
}