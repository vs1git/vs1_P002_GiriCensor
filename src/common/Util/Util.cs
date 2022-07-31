using UnityEngine;

namespace Vs1Plugin
{
	public class Util
    {
		public static string GetFullPath(GameObject gameObject)
		{
			return GetFullPath(gameObject.transform);
		}

		public static string GetFullPath(Transform transform)
		{
			string path = transform.name;
			var parent = transform.parent;
			while (parent)
			{
				path = $"{parent.name}/{path}";
				parent = parent.parent;
			}
			return path;
		}

	}
}