using System;
using System.Collections;
using System.Collections.Generic;

namespace fsosharplauncher {
	public class switchManager {
		public Dictionary<string,string> settings = new Dictionary<string,string> ();
		public switchManager () {
		}

		public switchManager (string read_me) {
			string[] loader;
			loader = read_me.Split (' ');
			string last_key = null;
			foreach (string load_me in loader) {
				if (load_me.StartsWith ("-")) {
					if (!settings.ContainsKey (load_me.TrimEnd (';'))) {
						last_key = load_me.TrimEnd (';');
						settings.Add (load_me.TrimEnd (';'), null);
					}
				} else {
					settings [last_key] = load_me.TrimEnd (';');
				}
			}
		}
		// This adds the passed string into the settings dictionary
		//
		public bool set (string set_me) {
			if (!settings.ContainsKey (set_me)) {
				settings.Add (set_me, null);
				return true;
			} else {
				return false;
			}
		}
		// An overload of the previous method,
		// But allowing a value to be set to a switch
		public bool set (string set_me, string to_me) {
			if (settings.ContainsKey (set_me)) {
				settings [set_me] = to_me;
				return true;
			} else {
				settings.Add (set_me, to_me);
				return true;
			}
		}
		// This checks if a string exists in the settings array,
		// then sets it to "unset" if it is;
		// TODO: make this disable values as well!
		public bool disable (string set_me) {
			if (settings.ContainsKey (set_me)) {
				settings.Remove (set_me);
				return true;
			} else {
				return false;
			}
		}

		public bool disable_value (string set_me) {
			//if(settings.ContainsKey(set_me)) {
			//    settings[set_me] = null;
			//    return true;
			//} else {
			//    return false;
			//}		
			return disable (set_me);
		}

		public string get_value (string get_me) {
			if (settings.ContainsKey (get_me))
				return settings [get_me];
			else
				return null;
		}

		public string output_settings () {
			string send_me = "";
			foreach (KeyValuePair<string,string> now_me in settings) {
				if (now_me.Value == null)
					send_me += " " + now_me.Key;
				else
					send_me += " " + now_me.Key + " " + now_me.Value;
			}
			return send_me;
		}

		public string output_everything () {
			string send_me = "";
			foreach (KeyValuePair<string,string> now_me in settings) {
				if (now_me.Value == null)
					send_me += " " + now_me.Key;
				else
					send_me += " " + now_me.Key + " " + now_me.Value;
			}
			return send_me;
		}
	}
}

