
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public struct mod_holder {
	public string name, image, info, website, forum, primarymods, secondarymods;
}

public class modManager
{
	private ArrayList mods = new ArrayList();
	
	public modManager(string load_me)
	{
		iniHandler read_me;
		
		DirectoryInfo root = new DirectoryInfo(load_me);
		
		DirectoryInfo[] potential_mods = root.GetDirectories();
		mod_holder new_mod;
		string path = load_me + "/mod.ini";
		if(File.Exists(path)) {
			read_me = new iniHandler(path);
			new_mod.name = "Freespace 2 - No Mod";
			new_mod.image = load_me + "/" + read_me.get("[launcher]","image255x112");
			new_mod.info = read_me.get("[launcher]","infotext");
			new_mod.website = read_me.get("[launcher]","website");
			new_mod.forum = read_me.get("[launcher]","forum");
			new_mod.primarymods = read_me.get("[multimod]","primarylist");
			new_mod.secondarymods = read_me.get("[multimod]","secondarylist");
			mods.Add(new_mod);
		} else {
			new_mod.name = "Freespace 2 - No Mod";
			new_mod.image = "none";
			new_mod.info = "Choose this to not play any Mods";
			new_mod.website = "http://www.hard-light.net/";
			new_mod.forum = "http://www.hard-light.net/forums";
			new_mod.primarymods = "";
			new_mod.secondarymods = "";
			mods.Add(new_mod);
		}
	
		foreach(DirectoryInfo parse_me in potential_mods) {
			path = load_me + "/" + parse_me.Name + "/mod.ini";
			if(File.Exists(path)) {
				read_me = new iniHandler(path);
				new_mod.name = parse_me.Name;
				new_mod.image = load_me + "/" + parse_me.Name + "/" + read_me.get("[launcher]","image255x112");
				new_mod.info = read_me.get("[launcher]","infotext");
				new_mod.website = read_me.get("[launcher]","website");
				new_mod.forum = read_me.get("[launcher]","forum");
				new_mod.primarymods = read_me.get("[multimod]","primarylist");
				new_mod.secondarymods = read_me.get("[multimod]","secondarylist");
				mods.Add(new_mod);
			}
		}
	}
	
	private mod_holder get_mod(string find_me) {
		mod_holder return_me;
		return_me.name = null;
		return_me.forum = null;
		return_me.image = null;
		return_me.info = null;
		return_me.primarymods = null;
		return_me.secondarymods = null;
		return_me.website = null;
		for(int i = 0;i<mods.Count;i++) {
			if(((mod_holder)mods[i]).name==find_me)
				return_me = (mod_holder)mods[i];
		}
		return return_me;
	}
	
	public int count() {
		return mods.Count;
	}
	
	public string get_name(string this_one) {
		mod_holder me = get_mod(this_one);
		if(me.name!=null)
			return me.name;
		else
			return null;
	}
	public string get_image(string this_one) {
		mod_holder me = get_mod(this_one);
		if(me.name!=null)
			return me.image;
		else
			return null;
	}
	public string get_info(string this_one) {
		mod_holder me = get_mod(this_one);
		if(me.name!=null)
			return me.info;
		else
			return null;
	}
	public string get_website(string this_one) {
		mod_holder me = get_mod(this_one);
		if(me.name!=null)
			return me.website;
		else
			return null;
	}
	public string get_forum(string this_one) {
		mod_holder me = get_mod(this_one);
		if(me.name!=null)
			return me.forum;
		else
			return null;
	}
	public string get_primarymods(string this_one) {
		mod_holder me = get_mod(this_one);
		if(me.name!=null)
			return me.primarymods;
		else
			return null;
	}
	public string get_secondarymods(string this_one) {
		mod_holder me = get_mod(this_one);
		if(me.name!=null)
			return me.secondarymods;
		else
			return null;
	}
	public ArrayList get_names() {
		ArrayList return_me = new ArrayList();
		foreach(mod_holder add_me in mods) {
			return_me.Add(add_me.name);
		}
		return return_me;       
	}
}
