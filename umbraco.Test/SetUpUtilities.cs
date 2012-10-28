using System;
using System.Collections.Specialized;

namespace umbraco.Test
{
	public class SetUpUtilities
	{
		public SetUpUtilities () {}

		private const string _umbracoDbDSN = "server=127.0.0.1;database=umbraco_test;user id=umbracouser;password=P@ssword1;datalayer=MySql";

		public static NameValueCollection GetAppSettings()
		{
			NameValueCollection appSettings = new NameValueCollection();

			//add application settings
			appSettings.Add("umbracoDbDSN", _umbracoDbDSN);

			return appSettings;
		}

	}
}

