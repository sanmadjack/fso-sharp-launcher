using System;
using Microsoft.Win32;

namespace fsosharplauncher {
	public class registryHandler {
		RegistryKey the_key;
		bool uac;

		public registryHandler (string key) {
			RegistryKey uac_key = Registry.LocalMachine.OpenSubKey ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\System");
			string platform = Environment.GetEnvironmentVariable ("PROGRAMFILES(X86)");
			if (uac_key.GetValue ("EnableLUA") == null || uac_key.GetValue ("EnableLUA").ToString () != "1")
				uac = false;
			else
				uac = true;
			if (!uac) {
				if (platform == null) {
					the_key = Registry.LocalMachine.OpenSubKey (key, true);
				} else {
					the_key = Registry.LocalMachine.OpenSubKey (key.Replace ("SOFTWARE", "Software\\Wow6432Node"), true);
				}
			} else {
				if (platform == null) {
					the_key = Registry.CurrentUser.OpenSubKey (key.Replace ("SOFTWARE", "Software\\Classes\\VirtualStore\\MACHINE\\SOFTWARE"), true);
				} else {
					the_key = Registry.CurrentUser.OpenSubKey (key.Replace ("SOFTWARE", "Software\\Classes\\VirtualStore\\MACHINE\\SOFTWARE\\Wow6432Node"), true);
				}
			}
		}

		public string getKey (string get_me) {
			if (the_key != null) {
				if (the_key.GetValue (get_me) != null)
					return the_key.GetValue (get_me).ToString ();
			}
			return null;
		}

		public bool setKey (string set_me, object to_me, RegistryValueKind as_me) {
			if (the_key != null) {
				the_key.SetValue (set_me, to_me, as_me);
				return true;
			}
			return false;
		}

		public bool deleteValue (string delete_me) {
			if (the_key != null) {
				if (the_key.GetValue (delete_me) != null) {
					the_key.DeleteValue (delete_me, false);
					return true;
				}
				return false;
			}
			return false;
		}
	}
}
//OGL -(1920x1080)x32 bit
//OGL -(1920x1080)x32 bit
//OGL -(640x480)x32 bit

