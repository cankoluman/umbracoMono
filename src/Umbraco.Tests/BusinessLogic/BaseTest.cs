using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using NUnit.Framework;
using SqlCE4Umbraco;
using Umbraco.Tests.TestHelpers;
using umbraco.BusinessLogic;
using umbraco.DataLayer;
using umbraco.IO;
using GlobalSettings = umbraco.GlobalSettings;

using Umbraco.Core.Configuration;
using umbraco.DataLayer.SqlHelpers.MySqlTest;

namespace Umbraco.Tests.BusinessLogic
{
	[TestFixture, RequiresSTA]
    public abstract class BaseTest
    {

		protected IConfigurationManager configManagerTest = null;

		[TestFixtureSetUp]
		public void SetUp()
		{
			ConfigurationManagerService
				.Instance
				.SetManager(
						new ConfigurationManagerTest(new NameValueCollection())
                 );

			configManagerTest = 
				ConfigurationManagerService
				.Instance
				.GetConfigManager();
		}

		/// <summary>
        /// Removes any resources that were used for the test
        /// </summary>
        [TearDown]
        public void Dispose()
        {
            ClearDatabase();
        }

        /// <summary>
        /// Ensures everything is setup to allow for unit tests to execute for each test
        /// </summary>
        [SetUp]
        public void Initialize()
        {
			configManagerTest.SetAppSetting("umbracoDbDSN", TestHelper.umbracoDbDsn);

			InitializeDatabase();
            InitializeApps();
            InitializeAppConfigFile();
            InitializeTreeConfigFile();
        }

        private void ClearDatabase()
        {
			var dataHelper = DataLayerHelper.CreateSqlHelper(GlobalSettings.DbDSN) as MySqlTestHelper;
            if (dataHelper == null)
				throw new InvalidOperationException("The sql helper for unit tests must be of type MySqlTestHelper, check the ensure the connection string used for this test is set to use MySqlTest");
            dataHelper.ClearDatabase();

            AppDomain.CurrentDomain.SetData("DataDirectory", null);
        }

        private void InitializeDatabase()
        {

			ClearDatabase();

            AppDomain.CurrentDomain.SetData("DataDirectory", TestHelper.CurrentAssemblyDirectory);
            var dataHelper = DataLayerHelper.CreateSqlHelper(GlobalSettings.DbDSN);
            var installer = dataHelper.Utility.CreateInstaller();
            if (installer.CanConnect)
            {
                installer.Install();
            }
        }

        private void InitializeApps()
        {
            Application.Apps = new List<Application>()
                {
                    new Application("content", "content", "content", 0)
                };
        }

        private void InitializeAppConfigFile()
        {
            Application.AppConfigFilePath = IOHelper.MapPath(SystemDirectories.Config + "/" + Application.AppConfigFileName, false);
        }

        private void InitializeTreeConfigFile()
        {
            ApplicationTree.TreeConfigFilePath = IOHelper.MapPath(SystemDirectories.Config + "/" + ApplicationTree.TreeConfigFileName, false);
        }

    }
}