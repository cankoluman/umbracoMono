using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Reflection;
using umbraco.DataLayer.SqlHelpers.MySql;
using log4net.Config;
using umbraco.DataLayer;
using GlobalSettings = umbraco.GlobalSettings;
using Umbraco.Core.Configuration;
using umbraco.DataLayer.SqlHelpers.MySqlTest;

namespace Umbraco.Tests.TestHelpers
{
	/// <summary>
	/// Common helper properties and methods useful to testing
	/// </summary>

	public static class TestHelper
	{
		public const string umbracoDbDsn = @"server=127.0.0.1;database=umbraco411_test;user id=umbracouser;password=P@ssword1;datalayer=MySqlTest";

		/// <summary>
		/// Clears an initialized database
		/// </summary>
		public static void ClearDatabase(string DbDSN)
		{
			var dataHelper = DataLayerHelper.CreateSqlHelper(DbDSN) as MySqlTestHelper;
			if (dataHelper == null)
				throw new InvalidOperationException("The sql helper for unit tests must be of type MySqlTestHelper, check the ensure the connection string used for this test is set to use MySqlTest");
			dataHelper.ClearDatabase();
		}

		/// <summary>
		/// Initializes a new database
		/// </summary>
		public static void InitializeDatabase(string DbDSN)
		{
			ClearDatabase(DbDSN);

			var dataHelper = DataLayerHelper.CreateSqlHelper(DbDSN);
			var installer = dataHelper.Utility.CreateInstaller();
			if (installer.CanConnect)
			{
				installer.Install();
			}
		}

		/// <summary>
		/// Gets the current assembly directory.
		/// </summary>
		/// <value>The assembly directory.</value>
		static public string CurrentAssemblyDirectory
		{
			get
			{
				var codeBase = typeof(TestHelper).Assembly.CodeBase;
				var uri = new Uri(codeBase);
				var path = uri.LocalPath;
				return Path.GetDirectoryName(path);
			}
		}

		/// <summary>
		/// Maps the given <paramref name="relativePath"/> making it rooted on <see cref="CurrentAssemblyDirectory"/>. <paramref name="relativePath"/> must start with <code>~/</code>
		/// </summary>
		/// <param name="relativePath">The relative path.</param>
		/// <returns></returns>
		public static string MapPathForTest(string relativePath)
		{
			if (!relativePath.StartsWith("~/"))
				throw new ArgumentException("relativePath must start with '~/'", "relativePath");

			return relativePath.Replace("~/", CurrentAssemblyDirectory + "/");
		}

		public static void SetupLog4NetForTests()
		{
			XmlConfigurator.Configure(new FileInfo(MapPathForTest("~/unit-test-log4net.config")));
		}

		/*
		public static IConfigurationManager GetTestConfigManager()
		{
			ConfigurationManagerService.ConfigManager = new ConfigurationManagerTest(new NameValueCollection());
			
			return ConfigurationManagerService.Instance;
		}
		*/
	}
}