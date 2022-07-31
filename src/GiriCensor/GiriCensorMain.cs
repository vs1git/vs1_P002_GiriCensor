using System;

namespace Vs1Plugin
{
	public partial class GiriCensorMain : MainBase
	{
		GiriCensorSubComponent giriCensorComponent;
		bool defaultEnabled;

		public GiriCensorMain(MVRScript mvr, bool defaultEnabled, string pluginName) : base(mvr, pluginName, "v1.0.0")
        {
			Log.debugEnabled = false;

			this.defaultEnabled = defaultEnabled;
		}

        public override void Init()
		{
			try
			{
				giriCensorComponent = new GiriCensorSubComponent(panelManager);
				giriCensorComponent.defaultEnabled = defaultEnabled;

				subComponentManager.Add(giriCensorComponent);
				//subComponentManager.Init();

				base.Init();

				panelManager.PushPanel(giriCensorComponent.panel);
			}
			catch (Exception e)
			{
				Log.LogError(logPrefix + e);
			}
		}

		public override void OnDisable()
		{
			try
			{
				giriCensorComponent.OnDisable();

			}
			catch (Exception e)
			{
				Log.LogError(logPrefix + e);
			}
		}
	}
}