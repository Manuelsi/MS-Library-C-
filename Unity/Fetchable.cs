using System.Collections.Generic;
using UnityEngine;

namespace NullrefLib.Unity {
	public class Fetchable : MonoBehaviour {
		new public string name;
		private static Dictionary<string, Fetchable> dictionary;

		private void OnEnable() {
			dictionary ??= new Dictionary<string, Fetchable>();

			if (dictionary.ContainsKey(name))
				Debug.LogWarning($"There already exists a fetchable item with name {name}. Attempting to fetch this item will yield unreliable results.");

			dictionary[name] = this;
		}

		private void OnDisable() {
			if (dictionary.ContainsKey(name) && dictionary[name] == this)
				dictionary.Remove(name);
		}

		public static GameObject Fetch(string name) {
			if (dictionary.ContainsKey(name))
				return dictionary[name].gameObject;

			throw new KeyNotFoundException("Fetchable key not found: " + name);
		}

		public static Fetchable FetchRaw(string name) {
			if (dictionary.ContainsKey(name))
				return dictionary[name];

			throw new KeyNotFoundException("Fetchable key not found: " + name);
		}

		public static T FetchComponent<T>(string name) where T : Component {
			if (dictionary.ContainsKey(name))
				return dictionary[name].GetComponent<T>();

			throw new KeyNotFoundException("Fetchable key not found: " + name);
		}
	}
}