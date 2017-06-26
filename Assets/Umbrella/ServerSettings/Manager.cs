using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Umbrella.MiniJSON;
using System.IO;

namespace Umbrella.ServerSettings {
	public class Manager : MonoBehaviour {

		public static Manager instance;
		public static bool paused;

		private static string _country;

		public static string country {
			get {
				if (string.IsNullOrEmpty (_country)) {
					_country = PlayerPrefs.GetString ("Umbrella.ServerSettings.Country", "");
				}
				return _country;
			}
			set {
				_country = value;
				PlayerPrefs.SetString ("Umbrella.ServerSettings.Country", value);
			}
		}

		public static Dictionary<string, object> settings = new Dictionary<string, object>();

		public static T Get<T>(string key, T def = default(T)) {
			if (!settings.ContainsKey (key))
				return def;
			if (!(settings [key] is T))
				return def;

			return (T)settings [key];
		}

		public static T GetIndexed<T>(string key, int index, T def = default(T)) {
			
			if (!settings.ContainsKey (key))
				return def;
			
			if (!(settings [key] is List<object>))
				return def;
			
			if (index >= ((List<object>)settings [key]).Count)
				return def;

			if (index < 0)
				return def;

			if (!(((List<object>)settings [key]) [index] is T))
				return def;
				

			return (T)((List<object>)settings [key])[index];
		}

		public static int Count(string key) {
			if (!settings.ContainsKey (key))
				return 0;

			if (!(settings [key] is List<object>))
				return 0;

			return ((List<object>)settings [key]).Count;
		}

		public static T GetKeyed<T>(string key1, string key2, T def = default(T)) {
			if (!settings.ContainsKey (key1))
				return def;

			if (!(settings [key1] is Dictionary<string, object>))
				return def;

			if (!((Dictionary<string, object>)settings [key1]).ContainsKey (key2))
				return def;

			if (!(((Dictionary<string, object>)settings [key1]) [key2] is T))
				return def;

			return (T)((Dictionary<string, object>)settings [key1])[key2];
		}

		public string url;
		public bool getCountry;
		public int refreshRate = 300;

		// Use this for initialization
		void Awake () {
			if (instance != null) {
				Destroy (this.gameObject);
				return;
			}

			instance = this;
			DontDestroyOnLoad (this.gameObject);
			this.transform.parent = null;
		}
		
		IEnumerator Start() {

			string platform = "";

			#if UNITY_IOS
			platform = "ios";
			#endif
			#if UNITY_ANDROID
			platform = "android";
			#endif

			string filepath = Application.persistentDataPath + "/UmbrellaServerSettings.json";


			if (File.Exists (filepath)) {
				try {
					Dictionary<string, object> src = (Dictionary<string, object>)Json.Deserialize(File.ReadAllText(filepath));
					if(src.ContainsKey("global")) {
						GetData((Dictionary<string, object>)src["global"]);
					}
					if(src.ContainsKey(platform)) {
						GetData((Dictionary<string, object>)src[platform]);
					}

				} catch(System.Exception e) {
					Debug.Log (e);
				}

				yield return new WaitForSeconds (2);
			}

			if (getCountry) {

				while (paused)
					yield return 0;
				WWW www = new WWW("https://country.umbrella.wtf");
				www.threadPriority = ThreadPriority.Low;
				yield return www;
				if (string.IsNullOrEmpty (www.error)) {
					try {

						if(www.text != null) country = www.text;

					} catch (System.Exception e) {
						Debug.Log (e);
					}
				}
				yield return new WaitForSeconds (2);

			}



			while (true) {

				while (paused)
					yield return 0;
				
				WWW www = new WWW (url);
				yield return www;
				www.threadPriority = ThreadPriority.Low;
				if (string.IsNullOrEmpty (www.error)) {
					try {
						Dictionary<string, object> src = (Dictionary<string, object>)Json.Deserialize (www.text);
						if (src.ContainsKey ("global")) {
							GetData ((Dictionary<string, object>)src ["global"]);
						}
						if (src.ContainsKey (platform)) {
							GetData ((Dictionary<string, object>)src [platform]);
						}

						if (src != null) {
							File.WriteAllText (filepath, www.text);
						}

					} catch (System.Exception e) {
						Debug.Log (e);
					}
				}

				yield return new WaitForSeconds (refreshRate);
			}
		}

		void GetData(Dictionary<string, object> src) {
			foreach(KeyValuePair<string, object> kv in src) {
				settings [kv.Key] = kv.Value;
			}
		}
	}
}
