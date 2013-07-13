using System;
using System.Globalization;
using Umbraco.Core;
using Umbraco.Core.Persistence;

namespace Umbraco.Core.Multiplatform
{
	public class DatabaseHelper
	{
		public static string GetDbSanitisedDateTimeFormat(object value)
		{
			var dbContext = ApplicationContext.Current.DatabaseContext.DatabaseProvider;
			if (dbContext == DatabaseProviders.MySql)
				return ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss");

			return ((DateTime)value).ToString(CultureInfo.InvariantCulture);
		}
	}
}

