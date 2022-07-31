using SimpleJSON;

namespace Vs1Plugin
{
	public partial class GiriCensor : MVRScript, VaMPluginI
	{
		MainBase main;

		public GiriCensor()
		{
			main = new GiriCensorMain(this, true, "GiriCensor");
		}

		// -------------------------------------------

		public override void Init()
		{
			main.Init();
		}

		void Start()
		{
			main.Start();
		}

		protected void FixedUpdate()
		{
			main.FixedUpdate();
		}

		void Update()
		{
			main.Update();
		}

		void OnDestroy()
		{
			main.OnDestroy();
		}

		// VaM Event ////////////////////////////

		public override JSONClass GetJSON(bool includePhysical = true, bool includeAppearance = true, bool forceStore = false)
		{
			return main.GetJSON(includePhysical, includeAppearance, forceStore);
		}

		public JSONClass GetJSONBase(bool includePhysical = true, bool includeAppearance = true, bool forceStore = false)
		{
			return base.GetJSON(includePhysical, includeAppearance, forceStore);
		}

		public override void RestoreFromJSON(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, JSONArray presetAtoms = null, bool setMissingToDefault = true)
		{
			main.RestoreFromJSON(jc, restorePhysical, restoreAppearance, presetAtoms, setMissingToDefault);
		}

		public void RestoreFromJSONBase(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, JSONArray presetAtoms = null, bool setMissingToDefault = true)
		{
			base.RestoreFromJSON(jc, restorePhysical, restoreAppearance, presetAtoms, setMissingToDefault);
		}

		public override void LateRestoreFromJSON(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, bool setMissingToDefault = true)
		{
			main.LateRestoreFromJSON(jc, restorePhysical, restoreAppearance, setMissingToDefault);
		}

		public void LateRestoreFromJSONBase(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, bool setMissingToDefault = true)
		{
			base.LateRestoreFromJSON(jc, restorePhysical, restoreAppearance, setMissingToDefault);
		}

		void OnDisable()
		{
			main.OnDisable();
		}
	}
}