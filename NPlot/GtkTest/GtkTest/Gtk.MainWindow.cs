using System;
using Gtk;

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
        

        public MainWindow ()
        {
            Title = "NPlot.Gtk Samples Application";
            Width = 500;
            Height = 400;

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
            
            icon = Image.LoadFromResource ("class.png");    //HWT chk
            
            store = new TreeStore (nameCol, iconCol, widgetCol);
            samplesTree = new TreeView ();
            samplesTree.Columns.Add ("Name", iconCol, nameCol);
            
            AddSample (null, "Boxes", typeof(Boxes));
            AddSample (null, "Buttons", typeof(ButtonSample));
            AddSample (null, "CheckBox", typeof(Checkboxes));
            AddSample (null, "Clipboard", typeof(ClipboardSample));
            AddSample (null, "ColorSelector", typeof(ColorSelectorSample));
            AddSample (null, "ComboBox", typeof(ComboBoxes));
            AddSample (null, "Drag & Drop", typeof(DragDrop));
            AddSample (null, "Expander", typeof (ExpanderSample));
            AddSample (null, "Progress bars", typeof(ProgressBarSample));
            AddSample (null, "Frames", typeof(Frames));
            AddSample (null, "Images", typeof(Images));
            AddSample (null, "Labels", typeof(Labels));
            AddSample (null, "ListBox", typeof(ListBoxSample));
            AddSample (null, "LinkLabels", typeof(LinkLabels));
            AddSample (null, "ListView", typeof(ListView1));
            AddSample (null, "Markdown", typeof (MarkDownSample));

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
                
            }
        }
        

        TreePosition AddSample (TreePosition pos, string name, Type sampleType)
        {
            //if (page != null)
            //  page.Margin.SetAll (5);
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

