using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;

namespace Umbraco.Core.MultiPlatform
{
	public class WebFormsHelper
	{
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
	}
}

