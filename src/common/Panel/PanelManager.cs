using System.Collections.Generic;

namespace Vs1Plugin {

	public class PanelManager {

		MVRScript _mvr;

		public MVRScript mvr => _mvr;

		protected List<Panel> _panels;

		public Panel currentPanel => (_panels.Count <= 0) ? null : _panels[_panels.Count - 1];

		public PanelManager(MVRScript mvr)
		{
			_mvr = mvr;
			_panels = new List<Panel>();
		}

		public void PushPanel(Panel panel)
		{
			if (currentPanel == null)
            {
				_panels.Add(panel);
				panel.Show(false);
			}
			else
            {
				currentPanel.Hide();
				_panels.Add(panel);
				panel.Show(true);
			}
		}

		public void PopPanel()
		{
			if (_panels.Count <= 1)
			{
				return;
			}

			currentPanel.Hide();
			_panels.RemoveAt(_panels.Count - 1);
			
			currentPanel.Show(_panels.Count > 1);
		}
	}
}
