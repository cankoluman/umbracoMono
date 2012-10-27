using System;
using System.Collections.Specialized;
using System.Web;

namespace umbraco.interfaces
{
	public interface IConfigurationManager
	{
		NameValueCollection AppSettings {get;}

		Object GetSection(string SectionName);
		void RefreshSection(string SectionName);
	}
}

