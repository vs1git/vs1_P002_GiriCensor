using UnityEngine;
using UnityEngine.UI;

namespace Vs1Plugin
{
    public class VaMUtil
    {

		// Util ///////////////////////////

		public static UIDynamicTextField CreateTextField(MVRScript mvr, string paramName, string text, bool rightSide, int fieldHeight)
		{
			JSONStorableString jsonStorableString = new JSONStorableString(paramName, text);
			UIDynamicTextField textField = mvr.CreateTextField(jsonStorableString, rightSide);
			textField.backgroundColor = new Color(1f, 1f, 1f, 0f);

			LayoutElement layoutElement = textField.GetComponent<LayoutElement>();
			layoutElement.preferredHeight = layoutElement.minHeight = fieldHeight;
			textField.height = fieldHeight;

			ScrollRect scrollRect = textField.UItext.transform.parent.transform.parent.transform.parent.GetComponent<ScrollRect>();
			if (scrollRect != null)
			{
				scrollRect.horizontal = false;
				scrollRect.vertical = false;
			}

			return textField;
		}

		// From MacGruber_utils.cs ////////////////////////

		public static string GetPluginPath(MVRScript mvr)
		{
			string id = mvr.name.Substring(0, mvr.name.IndexOf('_'));
			string filename = mvr.manager.GetJSON()["plugins"][id].Value;
			return filename.Substring(0, filename.LastIndexOfAny(new char[] { '/', '\\' }));
		}
	}
}