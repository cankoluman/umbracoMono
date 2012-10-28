using System;
using System.Collections.Specialized;
using System.Xml;
using System.Web;
using System.Web.Caching;

using umbraco.BusinessLogic;

namespace umbraco.Test
{
	public class SetUpUtilities
	{
		public SetUpUtilities () {}

		private const string _umbracoDbDSN = "server=127.0.0.1;database=umbraco_test;user id=umbracouser;password=P@ssword1;datalayer=MySql";
		private const string _umbracoConfigFile = "/home/kol3/Development/umbraco/290912/u4.7.2/umbraco/presentation/config/umbracoSettings.config";
		private const string _dynamicBase = "/tmp/kol3-temp-aspnet-0";
		public static NameValueCollection GetAppSettings()
		{
			NameValueCollection appSettings = new NameValueCollection();

			//add application settings
			appSettings.Add("umbracoDbDSN", _umbracoDbDSN);

			return appSettings;
		}

		public static void AddUmbracoConfigFileToHttpCache()
		{
			XmlDocument temp = new XmlDocument();
			XmlTextReader settingsReader = new XmlTextReader(_umbracoConfigFile);

			temp.Load(settingsReader);
			HttpRuntime.Cache.Insert("umbracoSettingsFile", temp,
										new CacheDependency(_umbracoConfigFile));
		}

		public static void RemoveUmbracoConfigFileFromHttpCache()
		{
			HttpRuntime.Cache.Remove("umbracoSettingsFile");
		}

		public static void InitConfigurationManager()
		{
			ConfigurationManagerService.ConfigManager = new ConfigurationManagerTest(SetUpUtilities.GetAppSettings());
		}

		public static void InitAppDomainDynamicBase()
		{
			AppDomain.CurrentDomain.SetDynamicBase(_dynamicBase);
			//AppDomain.CurrentDomain.SetupInformation.DynamicBase = "/tmp/kol3-temp-aspnet-0";
		}

	}
}

