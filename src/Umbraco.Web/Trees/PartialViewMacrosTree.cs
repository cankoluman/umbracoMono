﻿using System.Text;
using Umbraco.Core.IO;
using umbraco.businesslogic;
using umbraco.cms.presentation.Trees;

namespace Umbraco.Web.Trees
{
	/// <summary>
	/// Tree for displaying partial view macros in the developer app
	/// </summary>
	[Tree("developer", "partialViewMacros", "Partial View Macro Files", sortOrder: 6)]
	public class PartialViewMacrosTree : PartialViewsTree
	{
		public PartialViewMacrosTree(string application) : base(application)
		{
		}

		protected override string FilePath
		{
			get { return SystemDirectories.MvcViews + "/MacroPartials/"; }
		}

		public override void RenderJS(ref StringBuilder javascript)
		{
			//NOTE: Notice the MacroPartials%2f string below, this is a URLEncoded string of "MacroPartials/" so that the editor knows
			// to load the file from the correct location
			javascript.Append(
                @"
		                 function openMacroPartialView(id) {
		                    UmbClientMgr.contentFrame('settings/views/EditView.aspx?treeType=partialViewMacros&file=MacroPartials%2f' + id);
					    }
		                ");
		}

		protected override void ChangeNodeAction(XmlTreeNode xNode)
		{
			xNode.Action = xNode.Action.Replace("openFile", "openMacroPartialView");
		}
	}
}