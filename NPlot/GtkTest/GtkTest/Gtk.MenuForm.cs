/*
 * NPlot - A charting library for .NET
 * 
 * Gtk.MenuForm.cs
 * 
 * This is NPlot.MenuForm with interactive features added and
 * implemented using Gtk# Graphics (Gtk.MenuForm) (HWT 2011)
 * 
 * Copyright (C) 2003-2009 Matt Howlett and others.
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without modification,
 * are permitted provided that the following conditions are met:
 * 
 * 1. Redistributions of source code must retain the above copyright notice, this
 *    list of conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright notice,
 *    this list of conditions and the following disclaimer in the documentation
 *    and/or other materials provided with the distribution.
 * 3. Neither the name of NPlot nor the names of its contributors may
 *    be used to endorse or promote products derived from this software without
 *    specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
 * IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT,
 * INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING,
 * BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
 * LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE
 * OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED
 * OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Threading;

using Gtk;

namespace NPlotDemo
{
	/// <summary>
	/// Summary description for MenuForm.
	/// </summary>
	public class MenuForm : Gtk.Window
	{
		private Gtk.Label demosLabel;
		private Gtk.Button plotSurface2DDemoButton;
		private Gtk.Button multiPlotDemoButton;
		private Gtk.Label testsLabel;
		private Gtk.ComboBox TestSelectComboBox;
		private Gtk.Button RunTestButton;
		private Gtk.Button quitButton;
		private Gtk.Window displayForm_ = null;

		public MenuForm() : base ("Gtk# Demo")
		{
			InitializeComponent();
		}

		#region Gtk Menu Window generation
		/// <summary>
		/// Replaces Windows Form Designer code
		/// </summary>
		private void InitializeComponent()
		{
			SetSizeRequest( 192, 240 );		// Define MenuForm size
			//
			// Define an 8x6 table on which to lay out the test buttons, etc
			//
			Gtk.Table layout = new Gtk.Table ( 8, 6, true );
			layout.BorderWidth = 8;
			layout.ColumnSpacing = 2;
			layout.RowSpacing = 2;
			Add( layout );
			
			// Create menu components, then add to layout
			// 
			// DemosLabel
			//
			demosLabel = new Gtk.Label( "Demos" );
			layout.Attach( demosLabel, 0,2, 0,1 );
			// 
			// plotSurface2DDemoButton
			//
			plotSurface2DDemoButton = new Gtk.Button( "PlotSurface2D Demo" );
			plotSurface2DDemoButton.Clicked += new System.EventHandler(this.plotSurface2DDemoButton_Click);
			layout.Attach( plotSurface2DDemoButton, 0,6, 1,2 );
			// 
			// multiPlotDemoButton
			//
			multiPlotDemoButton = new Gtk.Button( "Multi Plot Demo" );
			multiPlotDemoButton.Clicked += new System.EventHandler(this.runDemoButton_Click);
			layout.Attach( multiPlotDemoButton, 0,6, 2,3 );
			// 
			// testsLabel
			// 
			testsLabel = new Gtk.Label( "Tests" );
			layout.Attach ( testsLabel, 0,2, 3,4 );
			// 
			// TestSelectComboBox
			// 
			//
			TestSelectComboBox = ComboBox.NewText ();
			TestSelectComboBox.AppendText ("Axis Test");
			TestSelectComboBox.AppendText ("PlotSurface2D");
			layout.Attach ( TestSelectComboBox, 0,6, 4,5 );
			// 
			// RunTestButton
			// 
			RunTestButton = new Gtk.Button( "Run Selected Test" );
			RunTestButton.Clicked += new System.EventHandler(this.RunTestButton_Click);
			layout.Attach ( RunTestButton, 0,6, 5,6 );
			// 
			// quitButton
			// 
			quitButton = new Gtk.Button ( "Quit" );
			quitButton.Clicked += new System.EventHandler(this.quitButton_Click);
			layout.Attach ( quitButton, 2,4, 7,8, 0,0,0,0 );
			
		}
		#endregion

        [STAThread]
		static void Main() 
		{
			Application.Init();				// Initialise Gtk
			MenuForm mf = new MenuForm ();
			mf.ShowAll();
			Application.Run();
		}
		
		private void runDemoButton_Click(object sender, System.EventArgs e)
		{
           	displayForm_ = new FinancialDemo();
			displayForm_.ShowAll();
        }

        private void plotSurface2DDemoButton_Click(object sender, System.EventArgs e)
		{
 			displayForm_ = new PlotSurface2DDemo();
			displayForm_.ShowAll();
		}


		private void quitButton_Click(object sender, System.EventArgs e)
		{
			Application.Quit();
		}

		private void RunTestButton_Click(object sender, System.EventArgs e)
		{
			TreeIter iter;
			bool activeIter;
			string str;

			activeIter = TestSelectComboBox.GetActiveIter (out iter);
			if (activeIter)
			{
				str = (string)TestSelectComboBox.Model.GetValue (iter, 0);
				if (str.Equals("Axis Test"))
				{
				    displayForm_ = new AxisTestsForm();
					displayForm_.ShowAll();
				}
				if (str.Equals("PlotSurface2D"))
				{
				    displayForm_ = new PlotSurface2DDemo();
					displayForm_.ShowAll();
				}
			}
		}


	}
}
