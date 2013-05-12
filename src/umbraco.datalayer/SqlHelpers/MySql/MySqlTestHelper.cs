using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using umbraco.DataLayer.SqlHelpers.MySql;
using MSC = MySql.Data.MySqlClient;

namespace umbraco.DataLayer.SqlHelpers.MySqlTest
{
	public class MySqlTestHelper : MySqlHelper
	{
		private string _catalog;

		public MySqlTestHelper (string connectionString) : base(connectionString)
		{
			var regex = Regex.Match(connectionString, "database=(.+);user");
			_catalog = regex.Groups[1].Value;
		}

		/// <summary>
		/// Checks if the actual database exists, if it doesn't then it will create it
		/// </summary>
		public void CreateEmptyDatabase()
		{
				//sqlCeEngine.CreateDatabase();
		}
		
		/// <summary>
		/// Most likely only will be used for unit tests but will remove all tables from the database
		/// </summary>
		public void ClearDatabase()
		{
			var tables = new List<string>();
			var query = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = @catalog AND TABLE_TYPE = 'BASE TABLE'";
			using (var reader = ExecuteReader(query, this.CreateParameter("catalog", _catalog)))
			{
				while (reader.Read())
				{
					tables.Add(reader.GetString("TABLE_NAME"));
				}
			}

			while(tables.Any())
			{
				for (var i = 0; i < tables.Count; i++)
				{
					var dropTable = "SET foreign_key_checks = 0;DROP TABLE " + tables[i] + ";SET foreign_key_checks = 1;";
					
					try
					{
						ExecuteNonQuery(dropTable, (MSC.MySqlParameter[])null);
						tables.Remove(tables[i]);
					}
					catch (SqlHelperException ex)
					{
						//this will occur because there is no cascade option, so we just wanna try the next one       
					}
				}
			}
		}

		public void DropForeignKeys(string table)
		{
			throw new NotImplementedException();
		}


	}
}

