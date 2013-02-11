using System;
using Xwt;

namespace Samples
{
	public class App
	{
		public static void Run (ToolkitType type)
		{
			Application.Initialize (type);
			
			MainWindow w = new MainWindow ();
			w.Show ();
			
			Application.Run ();
			
			w.Dispose ();
		}
	}
}	

