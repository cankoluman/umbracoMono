using System;
using System.Globalization;
using Umbraco.Core;
using Umbraco.Core.Persistence;

namespace Umbraco.Core.Multiplatform
{
	public class DatabaseHelper
	{
		public static bool IsDbMySql
		{
			get 
			{
				var dbContext = ApplicationContext.Current.DatabaseContext.DatabaseProvider;
				return dbContext == DatabaseProviders.MySql;
			}
		}

		public static string GetDbSanitisedDateTimeFormat(object value)
		{
			if (IsDbMySql)
				return ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss");

			return ((DateTime)value).ToString(CultureInfo.InvariantCulture);
		}

		public static string GetDbSanitisedRepublishAll()
		{
			if (IsDbMySql)
				return @"DELETE FROM cmsContentXml WHERE nodeId IN
						(
							SELECT * FROM 
							(
								SELECT DISTINCT cmsContentXml.nodeId FROM cmsContentXml 
								INNER JOIN cmsDocument ON cmsContentXml.nodeId = cmsDocument.nodeId
							) 
							as nodeId
						)";

			return @"DELETE FROM cmsContentXml WHERE nodeId IN
                    (SELECT DISTINCT cmsContentXml.nodeId FROM cmsContentXml 
                    INNER JOIN cmsDocument ON cmsContentXml.nodeId = cmsDocument.nodeId)";
		}
	}
}

