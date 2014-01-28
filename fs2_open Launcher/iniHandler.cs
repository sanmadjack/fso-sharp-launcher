
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class iniHandler
{
	private Dictionary<string,Dictionary<string,string>> iniFile = new Dictionary<string,Dictionary<string,string>>();
//	private string[] iniFile = new string[256];
	private string iniLocation = "unset";
	
	public iniHandler(string open_me) {
		string line = null;
		string section = null;
		string[] split = new string[2];
		iniLocation = open_me;
		if(File.Exists(open_me)) {
			StreamReader reader = new StreamReader(open_me);
			for(int i = 0;-1!=reader.Peek();i++) {
				line = reader.ReadLine();
				if(line.StartsWith("[")&&line.EndsWith("]")) {
					section = line;
					iniFile.Add(line, new Dictionary<string,string>());
				} else if(line.Contains("=")) {
					split = line.Split('=');
					iniFile[section].Add(split[0].Trim(),split[1].Trim(';').Trim());
				}
			}
			reader.Close();
		}
	}
	
	// No sense doubling up the code
	public bool writeIni() {
		if(writeIni(iniLocation))
			return true;
		else
			return false;
	}
	
	public bool writeIni(string write_here) {
		if(!Directory.Exists(Path.GetDirectoryName(write_here)))
			Directory.CreateDirectory(Path.GetDirectoryName(write_here));
		TextWriter writer = new StreamWriter(write_here);
		foreach(KeyValuePair<string,Dictionary<string,string>> write_me in iniFile) {
			writer.WriteLine(write_me.Key);
			foreach(KeyValuePair<string,string> and_me in write_me.Value) {
				writer.WriteLine(and_me.Key + "=" + and_me.Value + ";");
			}
		}
		writer.Close();
		return true;
	}
	
	public bool set(string set_here, string set_me, string to_me) {
		if(iniFile.ContainsKey(set_here)) {
			if(iniFile[set_here].ContainsKey(set_me)) {
				iniFile[set_here][set_me] = to_me;
				return true;
			} else {
				iniFile[set_here].Add(set_me,to_me);
				return true;
			}
		} else {
			iniFile.Add(set_here,new Dictionary<string,string>());
			iniFile[set_here].Add(set_me,to_me);
			return true;
		}
	}
	
	public string get(string from_here, string get_me) {
		if(iniFile.ContainsKey(from_here)) {
			if(iniFile[from_here].ContainsKey(get_me)) {
				return iniFile[from_here][get_me];
			} else {
				return null;
			}
		} else {
			return null;
		}
	}
	
	public bool delete(string in_here, string delete_me) {
		if(iniFile.ContainsKey(in_here)) {
			if(iniFile[in_here].ContainsKey(delete_me)) {
				iniFile[in_here].Remove(delete_me);
				return true;
			} else {
				return false;
			}
		} else {
			return false;
		}
	}
	
}
