using System;
using System.Collections.Generic;
using Gtk;
namespace fsosharplauncher {
	public class Resolutions: ListStore {
		public void Add(OpenTK.DisplayResolution new_res) {
			foreach(Resolution res in this) {
				if (res.Height == new_res.Height && res.Width == new_res.Width) {
					res.Depths.Add (new_res.BitsPerPixel);
					return;
				}
			}
			base.AppendValues(new Resolution(new_res.Width,new_res.Height,new_res.BitsPerPixel));
		}
	}

	public class Resolution {
		public int Height { get; protected set; }
		public int Width { get; protected set; }
		public List<int> Depths = new List<int> ();

		public Resolution (int width, int height, int depth) {
			this.Height = height;
			this.Width = width;
			Depths.Add (depth);
		}

		public override string ToString () {
			return string.Format ("{0}x{1}",Width, Height);
		}

	}
}

