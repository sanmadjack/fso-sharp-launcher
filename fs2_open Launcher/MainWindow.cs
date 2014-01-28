using System;
using Gtk;
using Gdk;
using System.IO;
using System.Diagnostics;

public partial class MainWindow: Gtk.Window
{	
	// Holds fs2_open.ini, for resolution and bit depth
	private iniHandler fs2_openIni;
	// The Windows equivalent
	registryHandler fs2_registry;
	// Holds launcher.ini, for the switched, exe paths, and your mother
	private iniHandler launcherIni;
	// Loads up data on all the available mods
	private modManager mods;
	// Holds the current states of all the switches, up to 256 of them
	private switchManager switches;
	
	// Gets the name of the user's OS
	private string os_name = System.Environment.OSVersion.ToString();
	// Thsi will hold the base path for the user config files
	private string home_path;
	
	private string current_mod;
	
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
		if(os_name.StartsWith("Unix")) {
			os_name = "Linux";
			home_path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			fs2_openIni = new iniHandler(home_path + System.IO.Path.DirectorySeparatorChar + ".fs2_open" + System.IO.Path.DirectorySeparatorChar + "fs2_open.ini");
			speechFrame.Visible = false;
		} else if(os_name.StartsWith("Microsoft")) {
			os_name = "Windows";
			home_path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			fs2_registry = new registryHandler("SOFTWARE\\Volition\\FreeSpace2");
			checkNoGrab.Visible = false;
		} else {
			os_name = "OSX";
			home_path = "/home/sanmadjack/";
			speechFrame.Visible = false;
		}

		launcherIni = new iniHandler(home_path + System.IO.Path.DirectorySeparatorChar + ".fs2_open" + System.IO.Path.DirectorySeparatorChar + "launcher.ini");
		
		string read_me;
		
		if(os_name=="Windows") {
			read_me = fs2_registry.getKey("VideocardFs2open");
		} else {
			read_me = fs2_openIni.get("[Default]","VideocardFs2open");
		}
		if(read_me!=null) {
			string[] parse_me = read_me.Split('(',')');
			comboResolution.Entry.Text = parse_me[1];
			if(parse_me[2]=="x16 bit") {
				comboColorDepth.Active = 0;
			} else if(parse_me[2]=="x32 bit") {
				comboColorDepth.Active = 1;
			}
		}
		
		if(os_name=="Windows") {
			read_me = fs2_registry.getKey("TextureFilter");
		} else {
			read_me = fs2_openIni.get("[Default]","TextureFilter");
		}
		if(read_me!=null) {
			comboAF.Active = int.Parse(read_me);
		}

		if(os_name=="Windows") {
			read_me = fs2_registry.getKey("OGL_AnisotropicFilter");
		} else {
			read_me = fs2_openIni.get("[Default]","OGL_AnisotropicFilter");
		}
		if(read_me!=null) {
			switch(read_me) {
				case "2.0":
					comboAF.Active = 2;
					break;
				case "4.0":
					comboAF.Active = 3;
					break;
				case "8.0":
					comboAF.Active = 4;
					break;
				case "16.0":
					comboAF.Active = 5;
					break;
			}
		}

		if(os_name=="Windows") {
			read_me = fs2_registry.getKey("OGL_FSAA");
		} else {
			read_me = fs2_openIni.get("[Default]","OGL_FSAA");
		}
		if(read_me!=null) {
			switch(read_me) {
				case "2":
					comboAA.Active = 1;
					break;
				case "4":
					comboAA.Active = 2;
					break;
				case "8":
					comboAA.Active = 3;
					break;
				case "16":
					comboAA.Active = 4;
					break;
			}
		}


		if(os_name=="Windows") {
			read_me = fs2_registry.getKey("ComputerSpeed");
		} else {
			read_me = fs2_openIni.get("[Default]","ComputerSpeed");
		}
		if(read_me!=null) 
			generalGraphicsCombo.Active = int.Parse(read_me)-1;

		if(os_name=="Windows") {
			read_me = fs2_registry.getKey("SpeechVoice");
		} else {
			read_me = fs2_openIni.get("[Default]","SpeechVoice");
		}
		if(read_me!=null) 
			voiceCombo.Active = int.Parse(read_me);
		else
			voiceCombo.Active = 0;


		if(os_name=="Windows") {
			read_me = fs2_registry.getKey("SpeechBriefings");
		} else {
			read_me = fs2_openIni.get("[Default]","SpeechBriefings");
		}
		if(read_me!=null) {
			if(read_me=="1") {
				briefingsVoiceCheck.Active = true;
			} else {
				briefingsVoiceCheck.Active = false;
			}
		}

		if(os_name=="Windows") {
			read_me = fs2_registry.getKey("SpeechIngame");
		} else {
			read_me = fs2_openIni.get("[Default]","SpeechIngame");
		}
		if(read_me!=null) {
			if(read_me=="1") {
				ingameVoiceCheck.Active = true;
			} else {
				ingameVoiceCheck.Active = false;
			}
		}
		
		if(os_name=="Windows") {
			read_me = fs2_registry.getKey("SpeechTechRoom");
		} else {
			read_me = fs2_openIni.get("[Default]","SpeechTechRoom");
		}
		if(read_me!=null) {
			if(read_me=="1") {
				techroomVoiceCheck.Active = true;
			} else {
				techroomVoiceCheck.Active = false;
			}
		}

		if(os_name=="Windows") {
			read_me = fs2_registry.getKey("SpeechVolume");
		} else {
			read_me = fs2_openIni.get("[Default]","SpeechVolume");
		}
		if(read_me!=null) {
			voiceVolumeScale.Value = int.Parse(read_me);
		}

	
		if(os_name=="Windows") {
			read_me = fs2_registry.getKey("NetworkConnection");
		} else {
			read_me = fs2_openIni.get("[Default]","NetworkConnection");
		}
		if(read_me!=null) {
			switch(read_me) {
				case "None":
					connectionTypeCombo.Active = 0;
					break;
				case "Dialup":
					connectionTypeCombo.Active = 1;
					break;
				case "LAN":
					connectionTypeCombo.Active = 2;
					break;
			}
		}

		if(os_name=="Windows") {
			read_me = fs2_registry.getKey("ConnectionSpeed");
		} else {
			read_me = fs2_openIni.get("[Default]","ConnectionSpeed");
		}
		if(read_me!=null) {
			switch(read_me) {
				case "None":
					connectionSpeedCombo.Active = 0;
					break;
				case "Slow":
					connectionSpeedCombo.Active = 1;
					break;
				case "56K":
					connectionSpeedCombo.Active = 2;
					break;
				case "ISDN":
					connectionSpeedCombo.Active = 3;
					break;
				case "Cable":
					connectionSpeedCombo.Active = 4;
					break;
				case "Fast":
					connectionSpeedCombo.Active = 5;
					break;
			}
		}




		if(os_name=="Windows") {
			read_me = fs2_registry.getKey("EnableJoystickFF");
		} else {
			read_me = fs2_openIni.get("[Default]","EnableJoystickFF");
		}
		if(read_me=="1") 
			forceFeedbackCheck.Active= true;
		else
			forceFeedbackCheck.Active = false;

		if(os_name=="Windows") {
			read_me = fs2_registry.getKey("EnableHitEffect");
		} else {
			read_me = fs2_openIni.get("[Default]","EnableHitEffect");
		}
		Console.WriteLine(read_me);
		if(read_me=="1") 
			directionalHitCheck.Active= true;
		else
			directionalHitCheck.Active = false;


		if(os_name=="Windows") {
			read_me = fs2_registry.getKey("CurrentJoystick");
		} else {
			read_me = fs2_openIni.get("[Default]","CurrentJoystick");
		}
		if(read_me!=null) {
			if(read_me=="9999") {
				joystickCombo.Active = 0;
			} else {
				joystickCombo.Active = int.Parse(read_me)+1;
			}
		}



		if(os_name=="Windows") {
			read_me = fs2_registry.getKey("ForcePort");
		} else {
			read_me = fs2_openIni.get("[Default]","ForcePort");
		}
		if(read_me!=null) {
			forceLocalPortSpinner.Value = int.Parse(read_me);
		}

		if(os_name=="Windows") {
			registryHandler fs2_network_registry = new registryHandler("SOFTWARE\\Volition\\FreeSpace2\\Network");
			read_me = fs2_network_registry.getKey("CustomIP");
		} else {
			read_me = fs2_openIni.get("[Network]","CustomIP");
		}
		if(read_me!=null) {
			forceIpAddressEntry.Text = read_me;
		}



		read_me = launcherIni.get("[launcher]","game_flags");
		switches = new switchManager();
		if(read_me!=null) {
			load_switches(read_me);
		}

		current_mod = launcherIni.get("[launcher]","active_mod");

		read_me = launcherIni.get("[launcher]","exe_filepath");
		if(read_me!=null) {
			if(File.Exists(read_me)) {
				binaryPicker.SelectFilename(read_me);
			}
		}
		

		
	}
	
	private string generateArguments() {
		string mod_list = "";
		string mod_argument = "";
		if(comboMod.Active!=0&&current_mod!=null) {
			// Put together the mod argument
			if(mods.get_primarymods(current_mod)!=null) {
				mod_list += mods.get_primarymods(current_mod).Trim().Trim(',') + ",";
			}
			mod_list += current_mod;
			if(mods.get_secondarymods(current_mod)!=null) {
				mod_list += "," + mods.get_secondarymods(current_mod).Trim().Trim(',');
			}
			mod_argument = " -mod " + mod_list.Trim(',');
		}
		return switches.output_settings().Trim() + " " + entryCustom.Text.Trim() + mod_argument;
	}
	
	protected virtual void launch_game() {
		write_settings();
		Process binary = new Process();
		
		if(File.Exists(binaryPicker.Filename))	{
			binary.StartInfo.FileName = binaryPicker.Filename;
			binary.StartInfo.Arguments = generateArguments();
			Console.WriteLine(binary.StartInfo.Arguments);
			binary.StartInfo.WorkingDirectory = binaryPicker.CurrentFolder;
			binary.StartInfo.UseShellExecute = false;
			binary.StartInfo.RedirectStandardOutput = true;
			binary.StartInfo.RedirectStandardError = true;
			using(Process p = Process.Start(binary.StartInfo)) {
				string output = p.StandardOutput.ReadToEnd();
				string errors = p.StandardError.ReadToEnd();
				p.WaitForExit();
				textErrorOutput.Buffer.Clear();
				textStandardOutput.Buffer.Clear();
				textErrorOutput.Buffer.Text = errors;
				textStandardOutput.Buffer.Text += output;
			}
		}
	}
	
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected virtual void checkbox_handler(bool status, string set_me) {
		if(status) {
			if(!switches.set(set_me)) {
				Console.WriteLine(set_me + " Set Failed");
			}
		} else {
			if(!switches.disable(set_me)) {
				Console.WriteLine(set_me + " Disable Failed");
			}
		}
		switchTextView.Buffer.Clear();
		switchTextView.Buffer.Text = generateArguments();
	}
	protected virtual void checkbox_handler(bool status, string set_me, string to_me) {
		if(status) {
			if(!switches.set(set_me,to_me)) {
				Console.WriteLine(set_me + " Set Failed");
			}
		} else {
			if(!switches.disable_value(set_me)) {
				Console.WriteLine(set_me + " Disable Failed");
			}
		}
		Console.WriteLine(switches.output_settings());
		switchTextView.Buffer.Clear();
		switchTextView.Buffer.Text = generateArguments();
	}
	
	protected virtual string resolution_generator() {
		if(comboColorDepth.Active==0) {
			return "OGL -(" + comboResolution.ActiveText + ")x16 bit";
		} else {
			return "OGL -(" + comboResolution.ActiveText + ")x32 bit";
		}
	}
	
	protected virtual bool write_settings() {
		if(os_name=="Windows") {
			fs2_registry.setKey("VideocardFs2open",resolution_generator(),Microsoft.Win32.RegistryValueKind.String);
		} else {
			fs2_openIni.set("[Default]","VideocardFs2open",resolution_generator());
		}

		
		string af = "0.0";
		int filtering = 0;
		switch(comboAF.Active) {
		case 0:
			filtering = 0;
			break;
		case 1:
			filtering = 1;
			break;
		case 2:
			filtering = 1;
			af = "2.0";
			break;
		case 3:
			filtering = 1;
			af = "4.0";
			break;
		case 4:
			filtering = 1;
			af = "8.0";
			break;
		case 5:
			filtering = 1;
			af = "16.0";
			break;
		}
		int aa = 0;
		switch(comboAA.Active) {
		case 1:
			aa = 2;
			break;
		case 2:
			aa = 4;
			break;
		case 3:
			aa = 8;
			break;
		case 4:
			aa = 16;
			break;
		}
		
		if(os_name=="Windows") {
			fs2_registry.setKey("OGL_AnisotropicFilter",af,Microsoft.Win32.RegistryValueKind.String);
		} else {
			fs2_openIni.set("[Default]","OGL_AnisotropicFilter",af);
		}

		if(os_name=="Windows") {
			fs2_registry.setKey("TextureFilter",filtering,Microsoft.Win32.RegistryValueKind.DWord);
		} else {
			fs2_openIni.set("[Default]","TextureFilter",filtering.ToString());
		}
		
		if(os_name=="Windows") {
			fs2_registry.setKey("OGL_FSAA",aa,Microsoft.Win32.RegistryValueKind.DWord);
		} else {
			fs2_openIni.set("[Default]","OGL_FSAA",aa.ToString());
		}
		
		if(os_name=="Windows") {
			fs2_registry.setKey("ComputerSpeed",generalGraphicsCombo.Active+1,Microsoft.Win32.RegistryValueKind.DWord);
		} else {
			fs2_openIni.set("[Default]","ComputerSpeed",(generalGraphicsCombo.Active+1).ToString());
		}

		int toggle;
		if(briefingsVoiceCheck.Active) {
			toggle = 1;
		} else {
			toggle = 0;
		}
		if(os_name=="Windows") {
			fs2_registry.setKey("SpeechBriefings",toggle,Microsoft.Win32.RegistryValueKind.DWord);
		} else {
			fs2_openIni.set("[Default]","SpeechBriefings",toggle.ToString());
		}
		if(ingameVoiceCheck.Active) {
			toggle = 1;
		} else {
			toggle = 0;
		}
		if(os_name=="Windows") {
			fs2_registry.setKey("SpeechIngame",toggle,Microsoft.Win32.RegistryValueKind.DWord);
		} else {
			fs2_openIni.set("[Default]","SpeechIngame",toggle.ToString());
		}
		if(techroomVoiceCheck.Active) {
			toggle = 1;
		} else {
			toggle = 0;
		}
		if(os_name=="Windows") {
			fs2_registry.setKey("SpeechTechRoom",toggle,Microsoft.Win32.RegistryValueKind.DWord);
		} else {
			fs2_openIni.set("[Default]","SpeechTechRoom",toggle.ToString());
		}

		if(os_name=="Windows") {
			fs2_registry.setKey("SpeechVolume",voiceVolumeScale.Value,Microsoft.Win32.RegistryValueKind.DWord);
		} else {
			fs2_openIni.set("[Default]","SpeechVolume",voiceVolumeScale.Value.ToString());
		}

		//This will be the voice comboe=
		if(os_name=="Windows") {
			fs2_registry.setKey("SpeechVoice",voiceCombo.Active,Microsoft.Win32.RegistryValueKind.DWord);
		} else {
			fs2_openIni.set("[Default]","SpeechVoice",voiceCombo.Active.ToString());
		}

		
		string connection = null;
		switch(connectionTypeCombo.Active) {
		case 0:
			connection = "None";
			break;
		case 1:
			connection = "Dialup";
			break;
		case 2:
			connection = "LAN";
			break;
		}
		if(os_name=="Windows") {
			fs2_registry.setKey("NetworkConnection",connection,Microsoft.Win32.RegistryValueKind.String);
		} else {
			fs2_openIni.set("[Default]","NetworkConnection",connection);
		}

		switch(connectionSpeedCombo.Active) {
		case 0:
			connection = "None";
			break;
		case 1:
			connection = "Slow";
			break;
		case 2:
			connection = "56K";
			break;
		case 3:
			connection = "ISDN";
			break;
		case 4:
			connection = "Cable";
			break;
		case 5:
			connection = "Fast";
			break;
		}
		if(os_name=="Windows") {
			fs2_registry.setKey("ConnectionSpeed",connection,Microsoft.Win32.RegistryValueKind.String);
		} else {
			fs2_openIni.set("[Default]","ConnectionSpeed",connection);
		}

		if(forceFeedbackCheck.Active) {
			toggle = 1;
		} else {
			toggle = 0;
		}
		if(os_name=="Windows") {
			fs2_registry.setKey("EnableJoystickFF",toggle,Microsoft.Win32.RegistryValueKind.DWord);
		} else {
			fs2_openIni.set("[Default]","EnableJoystickFF",toggle.ToString());
		}
		if(directionalHitCheck.Active) {
			toggle = 1;
		} else {
			toggle = 0;
		}
		if(os_name=="Windows") {
			fs2_registry.setKey("EnableHitEffect",toggle,Microsoft.Win32.RegistryValueKind.DWord);
		} else {
			fs2_openIni.set("[Default]","EnableHitEffect",toggle.ToString());
		}

		if(joystickCombo.Active==0) {
			toggle = 9999;
		} else {
			toggle = joystickCombo.Active-1;
		}
		if(os_name=="Windows") {
			fs2_registry.setKey("CurrentJoystick",toggle,Microsoft.Win32.RegistryValueKind.DWord);
		} else {
			fs2_openIni.set("[Default]","CurrentJoysick",toggle.ToString());
		}

		if(forceLocalPortSpinner.Value!=0) {
			if(os_name=="Windows") {
				fs2_registry.setKey("ForcePort",forceLocalPortSpinner.Value,Microsoft.Win32.RegistryValueKind.DWord);
			} else {
				fs2_openIni.set("[Default]","ForcePort",forceLocalPortSpinner.Value.ToString());
			}
		} else {
			if(os_name=="Windows") {
				fs2_registry.deleteValue("ForcePort");
			} else {
				fs2_openIni.delete("[Default]","ForcePort");
			}
		}

		if(forceIpAddressEntry.Text!="") {
			if(os_name=="Windows") {
				registryHandler fs2_network_registry = new registryHandler("SOFTWARE\\Volition\\FreeSpace2\\Network");
				fs2_network_registry.setKey("CustomIP",forceIpAddressEntry.Text,Microsoft.Win32.RegistryValueKind.String);
			} else {
				fs2_openIni.set("[Network]","CustomIP",forceIpAddressEntry.Text);
			}
		} else {
			if(os_name=="Windows") {
				registryHandler fs2_network_registry = new registryHandler("SOFTWARE\\Volition\\FreeSpace2\\Network");
				fs2_network_registry.deleteValue("CustomIP");
			} else {
				fs2_openIni.delete("[Network]","CustomIP");
			}
		}

		if(os_name!="Windows")
			fs2_openIni.writeIni();
		
		
		launcherIni.set("[launcher]","exe_filepath", binaryPicker.Filename.ToString());
		if(switches.output_settings().Trim()!=""||entryCustom.Text.Trim()!="") {
			launcherIni.set("[launcher]","game_flags", switches.output_settings().Trim() + " " + entryCustom.Text.Trim());
		} else {
			launcherIni.delete("[launcher]","game_flags");
		}
		if(comboMod.Active==0) {
			launcherIni.delete("[launcher]","active_mod");
		} else { 
			launcherIni.set("[launcher]","active_mod", current_mod);
		}
		launcherIni.writeIni();
		return true;
	}
	
	protected virtual bool load_mods(string path) {
		mods = new modManager(path);
		
		comboMod.Clear();
		CellRendererText cell = new CellRendererText();
		comboMod.PackStart(cell, false);
		comboMod.AddAttribute(cell, "text", 0);
		ListStore store = new ListStore(typeof(string));
		comboMod.Model = store;
		foreach(string mod_name in mods.get_names()) {
			store.AppendValues(mod_name);
		}
		if(current_mod==null||current_mod=="") {
			comboMod.Active = 0;
		} else {
			int original_mod = comboMod.Active;
			for(int i = 0;i<mods.count();i++) {
				comboMod.Active = i;
				if(comboMod.ActiveText==current_mod)
					break;
				comboMod.Active = original_mod;
			}
		}
		comboMod.Sensitive = true;
		return true;
		
	}
	
	protected virtual void OnComboModChanged (object sender, System.EventArgs e)
	{
		// Loads up the description
		textMod.Buffer.Clear();
		if(mods.get_website(comboMod.ActiveText)!=null)
			textMod.Buffer.Text = "Website: " + mods.get_website(comboMod.ActiveText) + "\n";
		if(mods.get_forum(comboMod.ActiveText)!=null)
			textMod.Buffer.Text += "Forum: " + mods.get_forum(comboMod.ActiveText) + "\n";
		if(mods.get_website(comboMod.ActiveText)!=null||mods.get_forum(comboMod.ActiveText)!=null)
			textMod.Buffer.Text += "\n";

		
		textMod.Buffer.Text += mods.get_info(comboMod.ActiveText);
		// Loads up the image
		if(File.Exists(mods.get_image(comboMod.ActiveText))) {
			imageMod.Visible = true;
			imageMod.Pixbuf = new Pixbuf(mods.get_image(comboMod.ActiveText));
		} else {
			imageMod.Visible = false;
		}
		current_mod = mods.get_name(comboMod.ActiveText);
		
		switchTextView.Buffer.Clear();
		switchTextView.Buffer.Text = generateArguments();
	
	}

	
	
	// This is the shame of the program, a long list of the check boxes
	// For when loading existing settings from the launcher.ini file
	protected virtual void load_switches(string read_me) {
		string[] parse_me = read_me.Split(' ');
		string use_me;
		for(int i = 0;i<parse_me.Length;i++) {
			switch (parse_me[i]) {
			case "-window":
				checkWindow.Active = true;
				break;
			case "-fps":
				checkFPS.Active = true;
				break;
			case "-no_vsync":
				checkVSync.Active = true;
				break;
			case "-nohtl":
				checkHTL.Active = true;
				break;
			case "-no_set_gamma":
				checkGamma.Active = true;
				break;
			case "-no_glsl":
				checkGLSL.Active = true;
				break;
			case "-nograb":
				checkNoGrab.Active = true;
				break;
			case "-spec":
				checkSpec.Active = true;
				break;
			case "-env":
				checkEnvironment.Active = true;
				break;
			case "-glow":
				checkGlow.Active = true;
				break;
			case "-missile_lighting":
				checkMissileLighting.Active = true;
				break;
			case "-3dshockwave":
				checkShockwave.Active = true;
				break;
			case "-3dwarp":
				checkWarp.Active = true;
				break;
			case "-ship_choice_3d":
				checkShipChoice.Active = true;
				break;
			case "-weapon_choice_3d":
				checkWeaponChoice.Active = true;
				break;
			case "-normal":
				checkNormal.Active = true;
				break;
			case "-height":
				checkHeight.Active = true;
				break;
			case "-nomotiondebris":
				checkDebris.Active = true;
				break;
			case "-noscalevid":
				checkVideoScaling.Active = true;
				break;
			case "-nomovies":
				checkMovies.Active = true;
				break;
			case "-snd_preload":
				checkSoundPreload.Active = true;
				break;
			case "-nomusic":
				checkMusic.Active = true;
				break;
			case "-nosound":
				checkSound.Active = true;
				break;
			case "-dualscanlines":
				checkDualScanLines.Active = true;
				break;
			case "-targetinfo":
				checkTargetInfo.Active = true;
				break;
			case "-orbradar":
				checkOrbRadar.Active = true;
				break;
			case "-ballistic_gauge":
				checkBallisticGauge.Active = true;
				break;
			case "-rearm_timer":
				checkRearmTimer.Active = true;
				break;
			case "-tbp":
				checkBabylon.Active = true;
				break;
			case "-wcsaga":
				checkWIngCommander.Active = true;
				break;
			case "-ambient_factor":
				checkAmbient.Active = true;
				if(parse_me[i+1]!=null)
					spinAmbient.Value = Convert.ToDouble(parse_me[++i]);
				break;
			case "-no_emissive_light":
				checkEmissiveLight.Active = true;
				break;
			case "-spec_exp":
				checkSpecExp.Active = true;
				if(parse_me[i+1]!=null)
					spinSpecExp.Value = Convert.ToDouble(parse_me[++i]);
				break;
			case "-spec_point":
				checkSpecPoint.Active = true;
				if(parse_me[i+1]!=null)
					spinSpecPoint.Value = Convert.ToDouble(parse_me[++i]);
				break;
			case "-spec_static":
				checkSpecStatic.Active = true;
				if(parse_me[i+1]!=null)
					spinSpecStatic.Value = Convert.ToDouble(parse_me[++i]);
				break;
			case "-spec_tube":
				checkSpecTube.Active = true;
				if(parse_me[i+1]!=null)
					spinSpecTube.Value = Convert.ToDouble(parse_me[++i]);
				break;
			case "-ogl_spec":
				checkOGLSpec.Active = true;
				if(parse_me[i+1]!=null)
					spinOGLSpec.Value = Convert.ToDouble(parse_me[++i]);
				break;
			case "-clipdist":
				checkClipDist.Active = true;
				if(parse_me[i+1]!=null)
					spinClipDist.Value = Convert.ToDouble(parse_me[++i]);
				break;
			case "-fov":
				checkFOV.Active = true;
				if(parse_me[i+1]!=null)
					spinFOV.Value = Convert.ToDouble(parse_me[++i]);
				break;
			case "-standalone":
				checkStandalone.Active = true;
				break;
			case "-startgame":
				checkStartServer.Active = true;
				break;
			case "-closed":
				checkClosed.Active = true;
				break;
			case "-restricted":
				checkRestricted.Active = true;
				break;
			case "-multilog":
				checkMultilog.Active = true;
				break;
			case "-mpnoreturn":
				checkMPNoReturn.Active = true;
				break;
			case "-gamename":
				checkGameName.Active = true;
				use_me = parse_me[++i].Trim('"','\'');
				while(i+1<parse_me.Length&&!parse_me[i+1].StartsWith("-")) {
					use_me += " " + parse_me[++i].Trim('"','\'');
				}
				entryGameName.Text = use_me;
				break;
			case "-password":
				checkPassword.Active = true;
				use_me = parse_me[++i].Trim('"','\'');
				while(i+1<parse_me.Length&&!parse_me[i+1].StartsWith("-")) {
					use_me += " " + parse_me[++i].Trim('"','\'');
				}
				entryPassword.Text = use_me;
				break;
			case "-allowabove":
				checkAllowAbove.Active = true;
				if(parse_me[i+1]!=null)
					spinAllowAbove.Value = Convert.ToDouble(parse_me[++i]);
				break;
			case "-allowbelow":
				checkAllowBelow.Active = true;
				if(parse_me[i+1]!=null)
					spinAllowBelow.Value = Convert.ToDouble(parse_me[++i]);
				break;
			case "-port":
				checkPort.Active = true;
				if(parse_me[i+1]!=null)
					spinPort.Value = Convert.ToDouble(parse_me[++i]);
				break;
			case "-timeout":
				checkTimeout.Active = true;
				if(parse_me[i+1]!=null)
					spinTimeout.Value = Convert.ToDouble(parse_me[++i]);
				break;
			case "-cap_object_update":
				checkCapObjectUpdate.Active = true;
				if(parse_me[i+1]!=null)
					spinCapObjectUpdate.Value = Convert.ToDouble(parse_me[++i]);
				break;
			case "-pos":
				checkPos.Active = true;
				break;
			case "-coords":
				checkCoords.Active = true;
				break;
			case "-timerbar":
				checkTimerBar.Active = true;
				break;
			case "-stats":
				checkStats.Active = true;
				break;
			case "-show_mem_usage":
				checkShowMumUsage.Active = true;
				break;
			case "-pofspew":
				checkPofSpew.Active = true;
				break;
			case "-tablecrcs":
				checkTableCRCs.Active = true;
				break;
			case "-missioncrcs":
				checkMissionCRCs.Active = true;
				break;
			case "-dis_collisions":
				checkDisableCollisions.Active = true;
				break;
			case "-dis_weapons":
				checkDisableWeapons.Active = true;
				break;
			case "-output_sexps":
				checkOutputSEXPs.Active = true;
				break;
			case "-output_scripting":
				checkOutputScripting.Active = true;
				break;
			case "-img2dds":
				checkImgToDDS.Active = true;
				break;
			case "-cache_bitmaps":
				checkCacheBitmaps.Active = true;
				break;
			case "-no_ap_interrupt":
				checkNoAPInterrupt.Active = true;
				break;
			case "-noparseerrors":
				checkNoParseErrors.Active = true;
				break;
			case "-safeloading":
				checkSafeLoading.Active = true;
				break;
			case "-novbo":
				checkNoVBO.Active = true;
				break;
			case "-noibx":
				checkNoIBX.Active = true;
				break;
			case "-loadallweps":
				checkLoadAllWeps.Active = true;
				break;
			default:
				entryCustom.Text += parse_me[i] + " ";
				break;
			}
		}
	}
	
	protected virtual void OnBinaryPickerSelectionChanged (object sender, System.EventArgs e)
	{
		load_mods(binaryPicker.CurrentFolder.ToString());
		ApplyButton.Sensitive = true;
		LauchButton.Sensitive = true;
	}
	
	protected virtual void OnLauchButtonClicked (object sender, System.EventArgs e)
	{
		write_settings();
		launch_game();
	}

	protected virtual void OnCancelButtonClicked (object sender, System.EventArgs e)
	{
		Application.Quit ();
	}

	protected virtual void OnApplyButtonClicked (object sender, System.EventArgs e)
	{
		write_settings();
	}

	
	// These are all the checkbox toggles
	// I know there's a lot, and one day I'll put together a way to simplify all this
	protected virtual void OnCheckWindowToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkWindow.Active,"-window");
	}

	protected virtual void OnCheckFPSToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkFPS.Active,"-fps");
	}

	protected virtual void OnCheckVSyncToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkVSync.Active,"-no_vsync");
	}

	protected virtual void OnCheckHTLToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkHTL.Active,"-nohtl");
	}

	protected virtual void OnCheckGammaToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkGamma.Active,"-no_set_gamma");
	}

	protected virtual void OnCheckGLSLToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkGLSL.Active,"-no_glsl");
	}

	protected virtual void OnCheckNoGrabToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkNoGrab.Active,"-nograb");
	}

	protected virtual void OnCheckSpecToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkSpec.Active,"-spec");
	}

	protected virtual void OnCheckEnvironmentToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkEnvironment.Active,"-env");
	}

	protected virtual void OnCheckGlowToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkGlow.Active,"-glow");
	}

	protected virtual void OnCheckMissileLightingToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkMissileLighting.Active,"-missile_lighting");
	}

	protected virtual void OnCheckShockwaveToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkShockwave.Active,"-3dshockwave");
	}

	protected virtual void OnCheckWarpToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkWarp.Active,"-3dwarp");
	}

	protected virtual void OnCheckShipChoiceToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkShipChoice.Active,"-ship_choice_3d");
	}

	protected virtual void OnCheckWeaponChoiceToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkWeaponChoice.Active,"-weapon_choice_3d");
	}

	protected virtual void OnCheckNormalToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkNormal.Active,"-normal");
	}

	protected virtual void OnCheckHeightToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkHeight.Active,"-height");
	}

	protected virtual void OnCheckDebrisToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkDebris.Active,"-nomotiondebris");
	}

	protected virtual void OnCheckVideoScalingToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkVideoScaling.Active,"-noscalevid");
	}

	protected virtual void OnCheckMoviesToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkMovies.Active,"-nomovies");
	}

	protected virtual void OnCheckSoundPreloadToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkSoundPreload.Active,"-snd_preload");
	}

	protected virtual void OnCheckMusicToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkMusic.Active,"-nomusic");
	}

	protected virtual void OnCheckSoundToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkSound.Active,"-nosound");
	}

	protected virtual void OnCheckDualScanLinesToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkDualScanLines.Active,"-dualscanlines");
	}

	protected virtual void OnCheckTargetInfoToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkTargetInfo.Active,"-targetinfo");
	}

	protected virtual void OnCheckOrbRadarToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkOrbRadar.Active,"-orbradar");
	}

	protected virtual void OnCheckBallisticGaugeToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkBallisticGauge.Active,"-ballistic_gauge");
	}

	protected virtual void OnCheckRearmTimerToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkRearmTimer.Active,"-rearm_timer");
	}

	protected virtual void OnCheckBabylonToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkBabylon.Active,"-tbp");
	}

	protected virtual void OnCheckWIngCommanderToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkWIngCommander.Active,"-wcsaga");
	}

	protected virtual void OnCheckAmbientToggled (object sender, System.EventArgs e)
	{
		spinAmbient.Sensitive = checkAmbient.Active;
		checkbox_handler(checkAmbient.Active,"-ambient_factor",spinAmbient.Value.ToString());
	}

	protected virtual void OnSpinAmbientValueChanged (object sender, System.EventArgs e)
	{
		checkbox_handler(checkAmbient.Active,"-ambient_factor",spinAmbient.Value.ToString());
	}

	protected virtual void OnCheckEmissiveLightToggled (object sender, System.EventArgs e)
	{ 
		checkbox_handler(checkEmissiveLight.Active,"-no_emissive_light");
	}

	protected virtual void OnCheckSpecExpToggled (object sender, System.EventArgs e)
	{
		spinSpecExp.Sensitive = checkSpecExp.Active;
		checkbox_handler(checkSpecExp.Active,"-spec_exp",spinSpecExp.Value.ToString());
	}

	protected virtual void OnSpinSpecExpValueChanged (object sender, System.EventArgs e)
	{
		checkbox_handler(checkSpecExp.Active,"-spec_exp",spinSpecExp.Value.ToString());
	}

	protected virtual void OnCheckSpecPointToggled (object sender, System.EventArgs e)
	{
		spinSpecPoint.Sensitive = checkSpecPoint.Active;
		checkbox_handler(checkSpecPoint.Active,"-spec_point",spinSpecPoint.Value.ToString());
	}

	protected virtual void OnSpinSpecPointValueChanged (object sender, System.EventArgs e)
	{
		checkbox_handler(checkSpecPoint.Active,"-spec_point",spinSpecPoint.Value.ToString());
	}

	protected virtual void OnCheckSpecStaticToggled (object sender, System.EventArgs e)
	{
		spinSpecStatic.Sensitive = checkSpecStatic.Active;
		checkbox_handler(checkSpecStatic.Active,"-spec_static",spinSpecStatic.Value.ToString());
	}

	protected virtual void OnSpinSpecStaticValueChanged (object sender, System.EventArgs e)
	{
		checkbox_handler(checkSpecStatic.Active,"-spec_static",spinSpecStatic.Value.ToString());
	}

	protected virtual void OnCheckSpecTubeToggled (object sender, System.EventArgs e)
	{
		spinSpecTube.Sensitive = checkSpecTube.Active;
		checkbox_handler(checkSpecTube.Active,"-spec_tube",spinSpecTube.Value.ToString());
	}

	protected virtual void OnSpinSpecTubeValueChanged (object sender, System.EventArgs e)
	{
		checkbox_handler(checkSpecTube.Active,"-spec_tube",spinSpecTube.Value.ToString());
	}

	protected virtual void OnCheckOGLSpecToggled (object sender, System.EventArgs e)
	{
		spinOGLSpec.Sensitive = checkOGLSpec.Active;
		checkbox_handler(checkOGLSpec.Active,"-ogl_spec",spinOGLSpec.Value.ToString());
	}

	protected virtual void OnSpinOGLSpecValueChanged (object sender, System.EventArgs e)
	{
		checkbox_handler(checkOGLSpec.Active,"-ogl_spec",spinOGLSpec.Value.ToString());
	}

	protected virtual void OnCheckClipDistToggled (object sender, System.EventArgs e)
	{
		spinClipDist.Sensitive = checkClipDist.Active;
		checkbox_handler(checkClipDist.Active,"-clipdist",spinClipDist.Value.ToString());
	}

	protected virtual void OnSpinClipDistValueChanged (object sender, System.EventArgs e)
	{
		checkbox_handler(checkClipDist.Active,"-clipdist",spinClipDist.Value.ToString());
	}

	protected virtual void OnCheckFOVToggled (object sender, System.EventArgs e)
	{
		spinFOV.Sensitive = checkFOV.Active;
		checkbox_handler(checkFOV.Active,"-fov",spinFOV.Value.ToString());
	}

	protected virtual void OnSpinFOVValueChanged (object sender, System.EventArgs e)
	{
		checkbox_handler(checkFOV.Active,"-fov",spinFOV.Value.ToString());
	}

	protected virtual void OnCheckStandaloneToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkStandalone.Active,"-standalone");
	}

	protected virtual void OnCheckStartServerToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkStartServer.Active,"-startgame");
	}

	protected virtual void OnCheckClosedToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkClosed.Active,"-closed");
	}

	protected virtual void OnCheckRestrictedToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkRestricted.Active,"-restricted");
	}

	protected virtual void OnCheckMultilogToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkMultilog.Active,"-multilog");
	}

	protected virtual void OnCheckMPNoReturnToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkMPNoReturn.Active,"-mpnoreturn");
	}

	protected virtual void OnCheckGameNameToggled (object sender, System.EventArgs e)
	{
		entryGameName.Sensitive = checkGameName.Active;
		checkbox_handler(checkGameName.Active,"-gamename","\"" + entryGameName.Text.Trim('"','\'') + "\"");
	}

	protected virtual void OnEntryGameNameChanged (object sender, System.EventArgs e)
	{
		checkbox_handler(checkGameName.Active,"-gamename","\"" + entryGameName.Text.Trim('"','\'') + "\"");
	}

	protected virtual void OnCheckPasswordToggled (object sender, System.EventArgs e)
	{
		entryPassword.Sensitive = checkPassword.Active;
		checkbox_handler(checkPassword.Active,"-password","\"" + entryPassword.Text.Trim('"','\'') + "\"");
	}

	protected virtual void OnEntryPasswordChanged (object sender, System.EventArgs e)
	{
		checkbox_handler(checkPassword.Active,"-password","\"" + entryPassword.Text.Trim('"','\'') + "\"");
	}

	protected virtual void OnCheckAllowAboveToggled (object sender, System.EventArgs e)
	{
		spinAllowAbove.Sensitive = checkAllowAbove.Active;
		checkbox_handler(checkAllowAbove.Active,"-allowabove",spinAllowAbove.Value.ToString());
	}

	protected virtual void OnSpinAllowAboveValueChanged (object sender, System.EventArgs e)
	{
		checkbox_handler(checkAllowAbove.Active,"-allowabove",spinAllowAbove.Value.ToString());
	}

	protected virtual void OnCheckAllowBelowToggled (object sender, System.EventArgs e)
	{
		spinAllowBelow.Sensitive = checkAllowBelow.Active;
		checkbox_handler(checkAllowBelow.Active,"-allowbelow",spinAllowBelow.Value.ToString());
	}

	protected virtual void OnSpinAllowBelowValueChanged (object sender, System.EventArgs e)
	{
		checkbox_handler(checkAllowBelow.Active,"-allowbelow",spinAllowBelow.Value.ToString());
	}

	protected virtual void OnCheckPortToggled (object sender, System.EventArgs e)
	{
		spinPort.Sensitive = checkPort.Active;
		checkbox_handler(checkPort.Active,"-port",spinPort.Value.ToString());
	}

	protected virtual void OnSpinPortValueChanged (object sender, System.EventArgs e)
	{
		checkbox_handler(checkPort.Active,"-port",spinPort.Value.ToString());
	}

	protected virtual void OnCheckTimeoutToggled (object sender, System.EventArgs e)
	{
		spinTimeout.Sensitive = checkTimeout.Active;
		checkbox_handler(checkTimeout.Active,"-timeout",spinTimeout.Value.ToString());
	}

	protected virtual void OnSpinTimeoutValueChanged (object sender, System.EventArgs e)
	{
		checkbox_handler(checkTimeout.Active,"-timeout",spinTimeout.Value.ToString());
	}
	
	protected virtual void OnCheckCapObjectUpdateToggled (object sender, System.EventArgs e)
	{
		spinCapObjectUpdate.Sensitive = checkCapObjectUpdate.Active;
		checkbox_handler(checkCapObjectUpdate.Active,"-cap_object_update",spinCapObjectUpdate.Value.ToString());
	}

	protected virtual void OnSpinCapObjectUpdateValueChanged (object sender, System.EventArgs e)
	{
		checkbox_handler(checkCapObjectUpdate.Active,"-cap_object_update",spinCapObjectUpdate.Value.ToString());
	}

	protected virtual void OnCheckPosToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkPos.Active,"-pos");
	}

	protected virtual void OnCheckCoordsToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkCoords.Active,"-coords");
	}

	protected virtual void OnCheckTimerBarToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkTimerBar.Active,"-timerbar");
	}

	protected virtual void OnCheckStatsToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkStats.Active,"-stats");
	}

	protected virtual void OnCheckShowMumUsageToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkShowMumUsage.Active,"-show_mem_usage");
	}

	protected virtual void OnCheckPofSpewToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkPofSpew.Active,"-pofspew");
	}

	protected virtual void OnCheckTableCRCsToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkTableCRCs.Active,"-tablecrcs");
	}

	protected virtual void OnCheckMissionCRCsToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkMissionCRCs.Active,"-missioncrcs");
	}

	protected virtual void OnCheckDisableCollisionsToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkDisableCollisions.Active,"-dis_collisions");
	}

	protected virtual void OnCheckDisableWeaponsToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkDisableWeapons.Active,"-dis_weapons");
	}

	protected virtual void OnCheckOutputSEXPsToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkOutputSEXPs.Active,"-output_sexps");
	}

	protected virtual void OnCheckOutputScriptingToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkOutputScripting.Active,"-output_scripting");
	}

	protected virtual void OnCheckImgToDDSToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkImgToDDS.Active,"-img2dds");
	}

	protected virtual void OnCheckCacheBitmapsToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkCacheBitmaps.Active,"-cache_bitmaps");
	}

	protected virtual void OnCheckNoAPInterruptToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkNoAPInterrupt.Active,"-no_ap_interrupt");
	}

	protected virtual void OnCheckNoParseErrorsToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkNoParseErrors.Active,"-noparseerrors");
	}

	protected virtual void OnCheckSafeLoadingToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkSafeLoading.Active,"-safeloading");
	}

	protected virtual void OnCheckNoVBOToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkNoVBO.Active,"-novbo");
	}

	protected virtual void OnCheckNoIBXToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkNoIBX.Active,"-noibx");
	}

	protected virtual void OnCheckLoadAllWepsToggled (object sender, System.EventArgs e)
	{
		checkbox_handler(checkLoadAllWeps.Active,"-loadallweps");
	}

	protected virtual void OnNothingSpecialButtonClicked (object sender, System.EventArgs e)
	{
		comboMod.Active = 0;
		checkSpec.Active = false;
		checkEnvironment.Active = false;
		checkGlow.Active = false;
		checkMissileLighting.Active = false;
		checkShockwave.Active = false;
		checkWarp.Active = false;
		checkShipChoice.Active = false;
		checkWeaponChoice.Active = false;
		checkNormal.Active = false;
		checkHeight.Active = false;
		checkDebris.Active = false;
		checkSoundPreload.Active = false;
		checkDualScanLines.Active = false;
		checkTargetInfo.Active = false;
		checkRearmTimer.Active = false;
	}

	protected virtual void OnAllFeaturesButtonClicked (object sender, System.EventArgs e)
	{
		if(comboResolution.Active==0)
			comboResolution.Active = 2;
		checkSpec.Active = true;
		checkEnvironment.Active = true;
		checkGlow.Active = true;
		checkMissileLighting.Active = true;
		checkShockwave.Active = true;
		checkWarp.Active = true;
		checkShipChoice.Active = true;
		checkWeaponChoice.Active = true;
		checkNormal.Active = true;
		checkHeight.Active = true;
		checkDebris.Active = true;
		checkSoundPreload.Active = true;
		checkDualScanLines.Active = true;
		checkTargetInfo.Active = true;
		checkRearmTimer.Active = true;
		int original_mod = comboMod.Active;
		if(original_mod==0) {
			for(int i = 0;i<mods.count();i++) {
				comboMod.Active = i;
				if(comboMod.ActiveText=="mediavps")
					break;
				comboMod.Active = original_mod;
			}
		}
	}

}