using System;
using Xwt;
using Xwt.Drawing;

namespace Samples
{
	public class MainWindow: Window
	{
		TreeView samplesTree;
		TreeStore store;
		Image icon;
		VBox sampleBox;
		Label title;
		Widget currentSample;
		
		DataField<string> nameCol = new DataField<string> ();
		DataField<Sample> widgetCol = new DataField<Sample> ();
		DataField<Image> iconCol = new DataField<Image> ();
		
		StatusIcon statusIcon;
		
		public MainWindow ()
		{
			Title = "NPlot.Xwt Samples Application";
			Width = 600;
			Height = 500;

			Menu menu = new Menu ();
			
			var file = new MenuItem ("File");
			file.SubMenu = new Menu ();
			file.SubMenu.Items.Add (new MenuItem ("Open"));
			file.SubMenu.Items.Add (new MenuItem ("New"));
			MenuItem mi = new MenuItem ("Close");
			mi.Clicked += delegate {
				Application.Exit();
			};
			file.SubMenu.Items.Add (mi);
			menu.Items.Add (file);
			
			var edit = new MenuItem ("Edit");
			edit.SubMenu = new Menu ();
			edit.SubMenu.Items.Add (new MenuItem ("Copy"));
			edit.SubMenu.Items.Add (new MenuItem ("Cut"));
			edit.SubMenu.Items.Add (new MenuItem ("Paste"));
			menu.Items.Add (edit);
			
			MainMenu = menu;
			
			
			HPaned box = new HPaned ();
			
			icon = Image.FromResource (typeof(App), "class.png");
			
			store = new TreeStore (nameCol, iconCol, widgetCol);
			samplesTree = new TreeView ();
			samplesTree.Columns.Add ("Name", iconCol, nameCol);
			
			//AddSample (null, "Boxes", typeof(Boxes));
			//AddSample (null, "Buttons", typeof(ButtonSample));
			//AddSample (null, "CheckBox", typeof(Checkboxes));
			//AddSample (null, "Clipboard", typeof(ClipboardSample));
			//AddSample (null, "ColorSelector", typeof(ColorSelectorSample));
			//AddSample (null, "ComboBox", typeof(ComboBoxes));

			var plots = AddSample (null, "PlotSamples", null);
			//AddSample (plots, "Chart", typeof (ChartSample));
			//AddSample (plots, "Colors", typeof(ColorsSample));

			var tests = AddSample (null, "Tests", null);
			AddSample (tests, "AxisTests", typeof (AxisTests));
			//AddSample (tests, "Colors", typeof(ColorsSample));

			samplesTree.DataSource = store;
			
			box.Panel1.Content = samplesTree;
			
			sampleBox = new VBox ();
			title = new Label ("Sample:");
			sampleBox.PackStart (title, BoxMode.None);
			
			box.Panel2.Content = sampleBox;
			box.Panel2.Resize = true;
			box.Position = 160;
			
			Content = box;
			
			samplesTree.SelectionChanged += HandleSamplesTreeSelectionChanged;

			CloseRequested += HandleCloseRequested;
		}

		void HandleCloseRequested (object sender, CloseRequestedEventArgs args)
		{
			args.Handled = !MessageDialog.Confirm ("Samples will be closed", Command.Ok);
		}
		
		protected override void Dispose (bool disposing)
		{
			base.Dispose (disposing);
			
			if (statusIcon != null) {
				statusIcon.Dispose ();
			}
		}

		void HandleSamplesTreeSelectionChanged (object sender, EventArgs e)
		{
			if (samplesTree.SelectedRow != null) {
				if (currentSample != null)
					sampleBox.Remove (currentSample);
				Sample s = store.GetNavigatorAt (samplesTree.SelectedRow).GetValue (widgetCol);
				if (s.Type != null) {
					if (s.Widget == null)
						s.Widget = (Widget)Activator.CreateInstance (s.Type);
					sampleBox.PackStart (s.Widget, BoxMode.FillAndExpand);
				}
				
//				string txt = System.Xaml.XamlServices.Save (s);
				currentSample = s.Widget;
				Dump (currentSample, 0);
			}
		}
		
		void Dump (IWidgetSurface w, int ind)
		{
			if (w == null)
				return;
			Console.WriteLine (new string (' ', ind * 2) + " " + w.GetType ().Name + " " + w.GetPreferredWidth () + " " + w.GetPreferredHeight ());
			foreach (var c in w.Children)
				Dump (c, ind + 1);
		}
		
		TreePosition AddSample (TreePosition pos, string name, Type sampleType)
		{
			//if (page != null)
			//	page.Margin.SetAll (5);
			return store.AddNode (pos).SetValue (nameCol, name).SetValue (iconCol, icon).SetValue (widgetCol, new Sample (sampleType)).CurrentPosition;
		}
	}
	
	class Sample
	{
		public Sample (Type type)
		{
			Type = type;
		}

		public Type Type;
		public Widget Widget;
	}
}

