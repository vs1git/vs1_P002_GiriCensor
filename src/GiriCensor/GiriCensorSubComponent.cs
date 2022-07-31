using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Vs1Plugin
{
	public class GiriCensorSubComponent : SubComponent, PanelComponentI
	{
		static AssetLodingManager assetLodingManager;

		const string assetBundlePath = "/asset/giri_censor.assetbundle";
        string assetBundleRequestPath;

		// -----------------------------

		PanelManager _panelManager;
		Panel _panel;
		bool isPerson;
		bool isFemale;
		bool isAssetLoaded;

		public bool defaultEnabled = true;
		public bool defaultRangeCheck = false;

		public Dictionary<string, float> defaultValuesBase;
		public Dictionary<string, Dictionary<string, float>> defaultValues;


		public string GetType()
		{
			if (isPerson)
			{
				if (isFemale)
                {
					return "female";
                }
				else
                {
					return "male";
                }
			}
			else
            {
				return "other";
            }
		}

		public GiriCensorSubComponent(PanelManager panelManager)
		{
			this._panelManager = panelManager;

			defaultValuesBase = new Dictionary<string, float>() {
				{ "mosaicScale", 7f },
				{ "scale", 10f },
				{ "scaleX", 1f },
				{ "scaleY", 1f },
				{ "scaleZ", 1f },
				{ "positionX", 0f },
				{ "positionY", 0f },
				{ "positionZ", 0f },
				{ "rotationX", 0f },
			};

			defaultValues = new Dictionary<string, Dictionary<string, float>>
			{
				{
					"female",
					new Dictionary<string, float>
					{
						{ "scaleX", 0.7f },
						{ "scaleZ", 1.3f },
						{ "positionY", 1f },
					}
				},
				{
					"male",
					new Dictionary<string, float>
					{
						{ "scaleX", 0.9f },
						{ "scaleY", 0.9f },
						{ "scaleZ", 3f },
					}
				},
				{
					"male2",
					new Dictionary<string, float>
					{
						{ "scaleY", 1.5f },
						{ "scaleZ", 1.1f },
						{ "positionY", -0.5f },
					}
				},
				{
					"other",
					new Dictionary<string, float>
					{
						{ "scale", 50f },
					}
				},
			};
		}

		public PanelManager panelManager
		{
			get { return _panelManager; }
			set { _panelManager = value; }
		}

		public Panel panel
		{
			get { return _panel; }
			set { _panel = value; }
		}


		public GiriCensorModule censorModule1;
		public GiriCensorModule censorModule2;
		CollisionTrigger labiaTrigger;
		Transform shaft;
		Transform scrotum;

		Dictionary<string, GameObject> prefabs;
		GameObject censorSpherePrefab;
		Material materialCensor;
		Material materialCensorTest;

		float GetDefaultValueFloat(string type, string name)
		{
			Log.LogMessage($"GetDefaultValueFloat('{type}', '{name}')");

			var defaults = defaultValues[type];

			if (defaults.ContainsKey(name))
			{
				return defaults[name];
			}

			return defaultValuesBase[name];
		}

		override public void Init()
		{
			try
			{
				var mvr = panelManager.mvr;
				var containingAtom = mvr.containingAtom;

				assetBundleRequestPath = VaMUtil.GetPluginPath(mvr) + assetBundlePath;

				if (assetLodingManager == null)
                {
					assetLodingManager = new AssetLodingManager(assetBundleRequestPath);
				}

				prefabs = new Dictionary<string, GameObject>();

				isPerson = (containingAtom.type == "Person");
                if (isPerson)
                {
					labiaTrigger = containingAtom.GetComponentsInChildren<CollisionTrigger>().FirstOrDefault(t => t.name == "LabiaTrigger");
					isFemale = (labiaTrigger != null);

					if (!isFemale)
					{
						var autoColliders = containingAtom.GetComponentsInChildren<AutoCollider>();

						// for Debug
                        foreach (var t in autoColliders)
                        {
                            Log.LogMessage(t.name);
                        }

                        var autoCollider = autoColliders.FirstOrDefault(t => t.name == "AutoColliderGen2");
						if (autoCollider != null)
						{
							shaft = autoCollider.transform;

							var scrotumObj = containingAtom.GetComponentsInChildren<Collider>().FirstOrDefault(t => t.name == "_Collider3");
							if (scrotumObj != null)
                            {
								scrotum = scrotumObj.transform;
							}
						}
                        else
						{
							isPerson = false;
						}
					}
				}

				panel = new GiriCensorSubComponentPanel(panelManager, this);

				censorModule1 = new GiriCensorModule(this, 1, "Censor1 ");

				if (isPerson)
                {
					if (isFemale)
					{
						censorModule1.target = labiaTrigger.transform;
					}
					else
					{
						censorModule2 = new GiriCensorModule(this, 2, "Censor2 ");
						censorModule2.uiRightSide = true;

						censorModule1.target = shaft;
						censorModule2.target = scrotum;
					}
				}
				else
                {
					//censorModule1.target = containingAtom.transform;
					Log.LogMessage("Transform Path: " + Util.GetFullPath(containingAtom.transform));
					censorModule1.target = containingAtom.transform.Find("reParentObject/object");
				}

				censorModule1.Init();
				if (censorModule2 != null)
                {
					censorModule2.Init();
				}

				// Request load assetbundle -------------------
				Log.LogMessage($"isPerson={isPerson} isFemale={isFemale} assetBundleRequestPath={assetBundleRequestPath}");

				//try
				//            {
				//	AssetLoader.DoneWithAssetBundleFromFile(assetBundleRequestPath);
				//}
				//catch (Exception e)
				//            {
				//
				//            }

				assetLodingManager.Request(OnAssetBundleLoaded);

				//Request request = new AssetLoader.AssetBundleFromFileRequest { 
				//	path = assetBundleRequestPath,
				//	callback = OnAssetBundleLoaded
				//};
				//AssetLoader.QueueLoadAssetBundleFromFile(request);
			}
			catch (Exception e)
			{
				Log.LogError("GiriCensorSubComponentComponent: " + e);
			}
		}

        public override void Update()
        {
            base.Update();

			if (censorModule1 == null)
            {
				return;
            }

			try
            {
				censorModule1.Update();
				if (censorModule2 != null)
                {
					censorModule2.Update();
				}
			}
			catch (Exception e)
			{
				Log.LogError("GiriCensorSubComponentComponent: " + e);
			}
		}

		public override void OnDestroy()
        {
			assetLodingManager.Release();

			//try
			//{
			//	AssetLoader.DoneWithAssetBundleFromFile(assetBundleRequestPath);
			//}
			//catch (Exception e)
			//{
			//	// If already loaded on other atom. 
			//}

			if (censorModule1 != null)
            {
				censorModule1.OnDestroy();
				censorModule1 = null;
			}

			if (censorModule2 != null)
			{
				censorModule2.OnDestroy();
				censorModule2 = null;
			}

			base.OnDestroy();
        }

		public void OnDisable()
		{
			try
			{
				if (censorModule1 != null)
				{
					censorModule1.OnDisable();
				}

				if (censorModule2 != null)
				{
					censorModule2.OnDisable();
				}
			}
			catch (Exception e)
			{
				Log.LogError("" + e);
			}
		}


		private void OnAssetBundleLoaded(AssetLodingManager alm)
		{
			try
			{
				var request = alm.request;

				Log.LogMessage("GiriCensorSubComponentComponent.OnAssetBundleLoaded()");

				foreach (var item in request.assetBundle.LoadAllAssets<GameObject>())
				{
					prefabs.Add(item.name, item);
				}

				var names = string.Join(",", prefabs.Keys.ToArray<string>());
				Log.LogMessage($"Asset items : {names}");

				censorSpherePrefab = prefabs["censor_sphere"];
				materialCensor = censorSpherePrefab.GetComponent<MeshRenderer>().sharedMaterial;
				materialCensorTest = prefabs["censor_sphere_test"].GetComponent<MeshRenderer>().sharedMaterial;

				isAssetLoaded = true;
            }
            catch (Exception e)
			{
				Log.LogError("GiriCensorSubComponentComponent.OnAssetBundleLoaded: " + e);
			}
		}


		// ////////////////////////////////////////////

		public class GiriCensorModule
		{
			public int id;

			string paramPrefix;

			GiriCensorSubComponent parent;

			public Transform target;

			public JSONStorableBool enabled;
			public JSONStorableBool rangeCheck;
			public Dictionary<string, JSONStorableFloat> paramFloats;

			public bool uiRightSide;

			GameObject censorGameObject;
			MeshRenderer meshRenderer;
			Material materialCensor;
			Material materialCensorTest;

			public GiriCensorModule(GiriCensorSubComponent parent, int id, string paramPrefix)
			{
				this.paramPrefix = paramPrefix;
				this.id = id;
				this.parent = parent;

			}

			float GetDefaultValueFloat(string type, string name)
			{
				return parent.GetDefaultValueFloat(type, name);
			}

			public void Init()
			{
				paramFloats = new Dictionary<string, JSONStorableFloat>();

				var mvr = parent.panel.mvr;

				enabled = new JSONStorableBool(
					paramPrefix + "Enable", parent.defaultEnabled,
					(v) => {
						enabled = v;
					}
				);
				enabled.storeType = JSONStorableParam.StoreType.Full;
				mvr.RegisterBool(enabled);

				rangeCheck = new JSONStorableBool(
					paramPrefix + "Range Check Mode", parent.defaultRangeCheck,
					(v) => {
						rangeCheck = v;
					}
				);
				rangeCheck.storeType = JSONStorableParam.StoreType.Full;
				mvr.RegisterBool(rangeCheck);


				JSONStorableFloat f;

				// Scale ------

				var type = parent.GetType();
				if (type == "male" && id == 2)
				{
					type = "male2";
				}

				f = new JSONStorableFloat(
					paramPrefix + "Mosaic Scale", GetDefaultValueFloat(type, "mosaicScale"),
					0.01f, 50f, false, true
				);
				f.storeType = JSONStorableParam.StoreType.Full;
				mvr.RegisterFloat(f);
				paramFloats.Add("mosaicScale", f);

				f = new JSONStorableFloat(
					paramPrefix + "Scale", GetDefaultValueFloat(type, "scale"),
					0.01f, 200f, false, true
				);
				f.storeType = JSONStorableParam.StoreType.Full;
				mvr.RegisterFloat(f);
				paramFloats.Add("scale", f);

				float scaleMin = 0.01f;
				float scaleMax = 10f;

				f = new JSONStorableFloat(
					paramPrefix + "Scale X", GetDefaultValueFloat(type, "scaleX"),
					scaleMin, scaleMax, false, true
				);
				f.storeType = JSONStorableParam.StoreType.Full;
				mvr.RegisterFloat(f);
				paramFloats.Add("scaleX", f);

				f = new JSONStorableFloat(
					paramPrefix + "Scale Y", GetDefaultValueFloat(type, "scaleY"),
					scaleMin, scaleMax, false, true
				);
				f.storeType = JSONStorableParam.StoreType.Full;
				mvr.RegisterFloat(f);
				paramFloats.Add("scaleY", f);

				f = new JSONStorableFloat(
					paramPrefix + "Scale Z", GetDefaultValueFloat(type, "scaleZ"),
					scaleMin, scaleMax, false, true
				);
				f.storeType = JSONStorableParam.StoreType.Full;
				mvr.RegisterFloat(f);
				paramFloats.Add("scaleZ", f);

				// PositionX ------
				f = new JSONStorableFloat(
					paramPrefix + "Position X", GetDefaultValueFloat(type, "positionX"),
					-180f, 180f, false, true
				);
				f.storeType = JSONStorableParam.StoreType.Full;
				mvr.RegisterFloat(f);
				paramFloats.Add("positionX", f);

				// PositionY ------
				f = new JSONStorableFloat(
					paramPrefix + "Position Y", GetDefaultValueFloat(type, "positionY"),
					-100f, 100f, false, true
				);
				f.storeType = JSONStorableParam.StoreType.Full;
				mvr.RegisterFloat(f);
				paramFloats.Add("positionY", f);

				// PositionZ ------
				f = new JSONStorableFloat(
					paramPrefix + "Position Z", GetDefaultValueFloat(type, "positionZ"),
					-100f, 100f, false, true
				);
				f.storeType = JSONStorableParam.StoreType.Full;
				mvr.RegisterFloat(f);
				paramFloats.Add("positionZ", f);

				// Rotation ------
				f = new JSONStorableFloat(
					paramPrefix + "Rotation", GetDefaultValueFloat(type, "rotationX"),
					-180f, 180f, false, true
				);
				f.storeType = JSONStorableParam.StoreType.Full;
				mvr.RegisterFloat(f);
				paramFloats.Add("rotationX", f);
			}

			public void InitUI()
			{
				var panel = parent.panel;
				var mvr = panel.mvr;
				var rightSide = uiRightSide;

				var toggle = mvr.CreateToggle(enabled, rightSide);
				panel.AddUI(toggle);

				toggle = mvr.CreateToggle(rangeCheck, rightSide);
				panel.AddUI(toggle);

				foreach (var kv in paramFloats)
				{
					var slider = mvr.CreateSlider(kv.Value, rightSide);
					panel.AddUI(slider);
				}
			}

			public void Update()
			{
				if (!parent.panelManager.mvr.enabledJSON.val)
				{
					HideCensor();
					return;
				}

				if (!parent.isAssetLoaded)
				{
					return;
				}

				if (!enabled.val || target == null)
				{
					HideCensor();
					return;
				}

				ShowCensor();

				if (target != null)
				{
					var t = censorGameObject.transform;

					t.parent = target;

					var x = paramFloats["positionX"].val / 100;
					var y = paramFloats["positionY"].val / 100;
					var z = paramFloats["positionZ"].val / 100;
					var rotationX = paramFloats["rotationX"].val;

					var scale = paramFloats["scale"].val / 100;
					var scaleX = paramFloats["scaleX"].val;
					var scaleY = paramFloats["scaleY"].val;
					var scaleZ = paramFloats["scaleZ"].val;

					var mosaicScale = paramFloats["mosaicScale"].val / 1000;

					t.localPosition = new Vector3(x, y, z);
					t.localRotation = Quaternion.Euler(rotationX, 0, 0);
					t.localScale = new Vector3(scale * scaleX, scale * scaleY, scale * scaleZ);

					if (rangeCheck.val)
					{
						meshRenderer.material = materialCensorTest;
					}
					else
					{
						meshRenderer.material = materialCensor;
						materialCensor.SetFloat("_MosaicScale", mosaicScale);
					}
				}
			}

			void ShowCensor()
			{
				CreateCensor();
				censorGameObject.SetActive(true);
			}

			void HideCensor()
			{
				if (censorGameObject != null)
				{
					censorGameObject.SetActive(false);
				}
			}


			void CreateCensor()
			{
				if (censorGameObject == null)
				{
					materialCensor = new Material(parent.materialCensor);
					materialCensorTest = parent.materialCensorTest;


					censorGameObject = UnityEngine.Object.Instantiate(parent.censorSpherePrefab);
					censorGameObject.SetActive(true);

					meshRenderer = censorGameObject.GetComponent<MeshRenderer>();
					meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
					meshRenderer.material = materialCensor;
				}
			}

			void RemoveCensor()
			{
				if (censorGameObject != null)
				{
					UnityEngine.Object.Destroy(censorGameObject);
					UnityEngine.Object.Destroy(materialCensor);
				}
			}

			public void OnDestroy()
			{
				RemoveCensor();
			}

			public void OnDisable()
			{
				Update();
			}
		}
	}

	public class GiriCensorSubComponentPanel : Panel
	{
		GiriCensorSubComponent component;

		public GiriCensorSubComponentPanel(PanelManager panelManager, GiriCensorSubComponent component) : base(panelManager)
		{
			this.component = component;
		}

		override public bool Show(bool needBackButton)
		{
			if (!base.Show(needBackButton))
			{
				return false;
			}

			if (component == null || component.censorModule1 == null)
			{
				return true;
			}

			try
			{
				var panel = component.panel;

				var testField = VaMUtil.CreateTextField(mvr, "Giri Censor Title", $"<color=#000><size=35><b>Giri Censor</b></size>  <size=30>v1.0.0</size></color>", false, 40);
				panel.AddUI(testField);

				component.censorModule1.InitUI();

				testField = VaMUtil.CreateTextField(mvr, "Giri Censor Status", $"<color=#000><size=30>Atom Type:</size> <size=30>{component.GetType()}</size></color>", true, 40);
				panel.AddUI(testField);

				if (component.censorModule2 != null)
                {
					component.censorModule2.InitUI();
				}
			}
			catch (Exception e)
			{
				Log.LogError("GiriCensorSubComponentPanel: " + e);
			}

			return true;
		}
	}
}