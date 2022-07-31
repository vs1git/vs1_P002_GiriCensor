namespace Vs1Plugin {

	public interface PanelComponentI {
		PanelManager panelManager { get; set; }

		Panel panel { get; set; }
	}

	public class PanelComponent : PanelComponentI
	{
		protected PanelManager _panelManager;
		protected Panel _panel;

		public PanelComponent()
        {
        }

		public PanelManager panelManager {
			get { return _panelManager; }
			set { _panelManager = value; }
		}

		public Panel panel
		{
			get { return _panel; }
			set { _panel = value; }
		}
    }
}
