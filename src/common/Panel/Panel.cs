using System.Collections.Generic;

namespace Vs1Plugin {

	public class Panel
    {
		public string panelName = "";

		protected PanelManager _manager;

		protected bool _active;
		protected List<UIDynamic> _uiDynamics;

		public PanelManager manager => _manager;
		public MVRScript mvr => _manager.mvr;

		protected UIDynamicButton backButton;

		public Panel(PanelManager manager)
        {
			_manager = manager;
			_uiDynamics = new List<UIDynamic>();
		}

		virtual public void OnClickBackButton()
        {
			PopPanel();
		}

		public void PopPanel()
        {
			manager.PopPanel();
		}

		public void AddUI(UIDynamic uiDynamic)
        {
			_uiDynamics.Add(uiDynamic);
		}

		virtual public bool Show(bool needBackButton)
        {
			if (_active)
			{
				return false;
			}
			_active = true;

			var mvr = _manager.mvr;

			if (needBackButton)
            {
				var buttonName = "< Back";
				if (!string.IsNullOrEmpty(panelName))
                {
					buttonName += " : " + panelName;
				}

				backButton = mvr.CreateButton(buttonName);
				backButton.button.onClick.AddListener(OnClickBackButton);
				_uiDynamics.Add(backButton);
			}

			// Imprement by children

			return true;
		}

		virtual public bool Hide()
        {
			if (!_active)
			{
				return false;
			}
			_active = false;

			var mvr = _manager.mvr;

			foreach (var uiDynamic in _uiDynamics)
			{
				if (uiDynamic is UIDynamicToggle)
				{
					mvr.RemoveToggle((UIDynamicToggle)uiDynamic);
				}
				else if (uiDynamic is UIDynamicSlider)
				{
					mvr.RemoveSlider((UIDynamicSlider)uiDynamic);
				}
				else if (uiDynamic is UIDynamicPopup)
				{
					var uiPopup = (UIDynamicPopup)uiDynamic;
					uiPopup.popup.visible = false;
					mvr.RemovePopup((UIDynamicPopup)uiDynamic);
				}
				else if (uiDynamic is UIDynamicTextField)
				{
					mvr.RemoveTextField((UIDynamicTextField)uiDynamic);
				}
				else if (uiDynamic is UIDynamicButton)
				{
					mvr.RemoveButton((UIDynamicButton)uiDynamic);
				}
				else if (uiDynamic is UIDynamic)
				{
					mvr.RemoveSpacer((UIDynamic)uiDynamic);
				}
			}

			_uiDynamics.Clear();

			return true;
		}
	}
}
