using System;
using System.Collections.Generic;
using MeshVR;
using Request = MeshVR.AssetLoader.AssetBundleFromFileRequest;

namespace Vs1Plugin
{
	public class AssetLodingManager
    {
		string assetBundlePath;
		List<Action<AssetLodingManager>> callbacks;

		Request _request;
		int _referenceCount = 0;

		public Request request { 
			get { return _request; }
		}

		public AssetLodingManager(string assetBundlePath)
        {
			this.assetBundlePath = assetBundlePath;
			this.callbacks = new List<Action<AssetLodingManager>>();
			_referenceCount = 0;
		}

		public void Request(Action<AssetLodingManager> callback)
        {
			if (_request != null)
            {
				_referenceCount++;
				callback.Invoke(this);

				return;
			}

			var callbacksCount = callbacks.Count;

			callbacks.Add(callback);

			if (request == null && callbacksCount <= 0)
            {
				RequestAssetBundle();
			}
		}

		public void Release()
        {
			if (_referenceCount <= 0)
            {
				throw new Exception("AssetLodingManager: Release() error");
				return;
            }

			_referenceCount--;

			if (_referenceCount <= 0)
            {
				PurgeAssetBundle();
			}
		}

		void RequestAssetBundle()
        {
			Log.LogMessage("AssetLodingManager.RequestAssetBundle()");

			Request request = new AssetLoader.AssetBundleFromFileRequest
			{
				path = assetBundlePath,
				callback = OnLoaded
			};

			AssetLoader.QueueLoadAssetBundleFromFile(request);
		}

		void PurgeAssetBundle()
        {
			Log.LogMessage("AssetLodingManager.PurgeAssetBundle()");

			AssetLoader.DoneWithAssetBundleFromFile(assetBundlePath);
			_request = null;
		}

		private void OnLoaded(Request request)
        {
			Log.LogMessage("AssetLodingManager.OnLoaded()");

			_request = request;

			foreach (var cb in callbacks)
            {
				try
                {
					_referenceCount++;
					cb.Invoke(this);
				}
				catch (Exception e)
				{
					Log.LogError("AssetLodingManager.OnLoaded(): " + e);
				}
			}

			callbacks.Clear();
		}
	}
}