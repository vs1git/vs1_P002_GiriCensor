using System;
using SimpleJSON;

namespace Vs1Plugin
{
	public partial class MainBase
	{
		protected MVRScript _mvr;

		public MVRScript mvr
        {
			get { return _mvr;  }
        }

		protected string _pluginName = "VaMPlugin";
		protected string _pluginVersion = "v1.0.0";

		public string pluginName
        {
			get { return _pluginName; }
		}

		public string pluginVersion
		{
			get { return _pluginVersion; }
		}

		public string pluginTitle
		{
			get { return $"{pluginName} {pluginVersion}"; }
		}

		public string logPrefix
		{
			get { return $"{pluginName}: "; }
		}

		// --------------------

		public PanelManager panelManager;
		public SubComponentManager subComponentManager;

		// --------------------

		public MainBase(MVRScript mvr, string pluginName, string pluginVersion)
		{
			_mvr = mvr;
			_pluginName = pluginName;
			_pluginVersion = pluginVersion;

			panelManager = new PanelManager(mvr);
			subComponentManager = new SubComponentManager();
		}

		public virtual void Init()
		{
			mvr.pluginLabelJSON.val = pluginTitle;

			try
			{
				subComponentManager.Init();
			}
			catch (Exception e)
			{
				Log.LogError(logPrefix + e);
			}
		}

		public virtual void Start()
		{
			try
			{
				subComponentManager.Start();
			}
			catch (Exception e)
			{
				Log.LogError(logPrefix + e);
			}
		}

		public virtual void FixedUpdate()
		{
			try
			{
				if (SuperController.singleton.isLoading || SuperController.singleton.freezeAnimation)
				{
					return;
				}

				subComponentManager.FixedUpdate();
			}
			catch (Exception e)
			{
				Log.LogError(logPrefix + e);
			}
		}
		public virtual void Update()
		{
			try
			{
				if (SuperController.singleton.isLoading || SuperController.singleton.freezeAnimation)
				{
					return;
				}

				subComponentManager.Update();
			}
			catch (Exception e)
			{
				Log.LogError(logPrefix + e);
			}
		}


		public virtual void OnDestroy()
		{
			try
			{
				subComponentManager.OnDestroy();
			}
			catch (Exception e)
			{
				Log.LogError(logPrefix + e);
			}
		}

		// VaM Event ////////////////////////////

		public virtual JSONClass GetJSON(bool includePhysical = true, bool includeAppearance = true, bool forceStore = false)
		{
			try
			{
				JSONClass jc = ((VaMPluginI)mvr).GetJSONBase(includePhysical, includeAppearance, forceStore);
				if (includePhysical || forceStore)
				{
					mvr.needsStore = true;
				}
				return jc;
			}
			catch (Exception e)
			{
				Log.LogError(logPrefix + e);
			}

			return null;
		}

		public virtual void RestoreFromJSON(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, JSONArray presetAtoms = null, bool setMissingToDefault = true)
		{
			try
			{
				((VaMPluginI)mvr).RestoreFromJSONBase(jc, restorePhysical, restoreAppearance, presetAtoms, setMissingToDefault);

				if (string.IsNullOrEmpty(mvr.pluginLabelJSON.val))
				{
					mvr.pluginLabelJSON.val = pluginTitle;
				}
			}
			catch (Exception e)
			{
				Log.LogError(logPrefix + e);
			}
		}

		public virtual void LateRestoreFromJSON(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, bool setMissingToDefault = true)
		{
			try
			{
				((VaMPluginI)mvr).LateRestoreFromJSONBase(jc, restorePhysical, restoreAppearance, setMissingToDefault);
			}
			catch (Exception e)
			{
				Log.LogError(logPrefix + e);
			}
		}

		public virtual void OnDisable()
		{
			try
			{
			}
			catch (Exception e)
			{
				Log.LogError(logPrefix + e);
			}
		}
	}
}