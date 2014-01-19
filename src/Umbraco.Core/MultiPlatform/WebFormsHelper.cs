using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;

namespace Umbraco.Core.MultiPlatform
{
	public class WebFormsHelper
	{
		public const char CurrencyChar = '\u00A4';

		public static void SetCheckBoxStates(CheckBoxList cbl, NameValueCollection formData)
		{
			var cblFormID = cbl.ClientID.Replace("_","$");
			int i = 0;
			foreach (var item in cbl.Items)
			{
				string itemSelected = formData[cblFormID + "$" + i];
				if (itemSelected != null && itemSelected != String.Empty)
					((ListItem)item).Selected = true;
				i++;
			}
		}

		public static void SetCheckBoxState(CheckBox cb, NameValueCollection formData)
		{
			var cbId = cb.ClientID.Replace ("_", "$");
			cbId = cbId.Substring(0, cbId.LastIndexOf("$")) ;

			string itemChecked = formData[cbId];
			if (!String.IsNullOrEmpty (itemChecked))
				cb.Checked = true;
			else
				cb.Checked = false;
		}
	}
}

