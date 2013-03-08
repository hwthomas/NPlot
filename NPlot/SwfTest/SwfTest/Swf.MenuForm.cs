/*
 * NPlot - A charting library for .NET
 * 
 * Swf.MenuForm.cs
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
using System.Windows.Forms;
using System.Threading;

namespace NPlotDemo
{
	/// <summary>
	/// Summary description for MenuForm.
	/// </summary>
	public class MenuForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button multiPlotDemoButton;
		private System.Windows.Forms.Button plotSurface2DDemoButton;
		private System.Windows.Forms.Button quitButton;
		private System.Windows.Forms.Label DemosLabel;
		private System.Windows.Forms.Label TestsLabel;
		private System.Windows.Forms.ComboBox TestSelectComboBox;

		private ArrayList testItems;
		private System.Windows.Forms.Button RunTestButton;

		public class TestItem
		{

			/// <summary>
			/// Constructor
			/// </summary>
			/// <param name="name">The name of the test</param>
			/// <param name="form">The form that contains the test</param>
			public TestItem( string name, Form form )
			{
				form_ = form;
				name_ = name;
			}

			/// <summary>
			/// Name of the test.
			/// </summary>
			public string Name
			{
				get
				{
					return name_;
				}
			}
			private string name_;

			/// <summary>
			/// The form that contains the test.
			/// </summary>
			public Form Form
			{
				get
				{
					return form_;
				}
			}
			private Form form_;

		}

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MenuForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();		

			testItems = new ArrayList();
			testItems.Add( new TestItem( "Axis Test", new AxisTestsForm() ) );

			this.TestSelectComboBox.DataSource = testItems;
			this.TestSelectComboBox.DisplayMember = "Name";
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.multiPlotDemoButton = new System.Windows.Forms.Button();
			this.plotSurface2DDemoButton = new System.Windows.Forms.Button();
			this.quitButton = new System.Windows.Forms.Button();
			this.DemosLabel = new System.Windows.Forms.Label();
			this.TestsLabel = new System.Windows.Forms.Label();
			this.TestSelectComboBox = new System.Windows.Forms.ComboBox();
			this.RunTestButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// multiPlotDemoButton
			// 
			this.multiPlotDemoButton.Location = new System.Drawing.Point(8, 56);
			this.multiPlotDemoButton.Name = "multiPlotDemoButton";
			this.multiPlotDemoButton.Size = new System.Drawing.Size(136, 23);
			this.multiPlotDemoButton.TabIndex = 2;
			this.multiPlotDemoButton.Text = "Multi Plot Demo";
			this.multiPlotDemoButton.Click += new System.EventHandler(this.runDemoButton_Click);
			// 
			// plotSurface2DDemoButton
			// 
			this.plotSurface2DDemoButton.Location = new System.Drawing.Point(8, 32);
			this.plotSurface2DDemoButton.Name = "plotSurface2DDemoButton";
			this.plotSurface2DDemoButton.Size = new System.Drawing.Size(136, 23);
			this.plotSurface2DDemoButton.TabIndex = 3;
			this.plotSurface2DDemoButton.Text = "PlotSurface2D Demo";
			this.plotSurface2DDemoButton.Click += new System.EventHandler(this.plotSurface2DDemoButton_Click);
			// 
			// quitButton
			// 
			this.quitButton.Location = new System.Drawing.Point(40, 200);
			this.quitButton.Name = "quitButton";
			this.quitButton.TabIndex = 8;
			this.quitButton.Text = "Quit";
			this.quitButton.Click += new System.EventHandler(this.quitButton_Click);
			// 
			// DemosLabel
			// 
			this.DemosLabel.Location = new System.Drawing.Point(8, 8);
			this.DemosLabel.Name = "DemosLabel";
			this.DemosLabel.TabIndex = 9;
			this.DemosLabel.Text = "Demos";
			// 
			// TestsLabel
			// 
			this.TestsLabel.Location = new System.Drawing.Point(8, 96);
			this.TestsLabel.Name = "TestsLabel";
			this.TestsLabel.TabIndex = 10;
			this.TestsLabel.Text = "Tests";
			// 
			// TestSelectComboBox
			// 
			this.TestSelectComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.TestSelectComboBox.Location = new System.Drawing.Point(8, 120);
			this.TestSelectComboBox.Name = "TestSelectComboBox";
			this.TestSelectComboBox.Size = new System.Drawing.Size(136, 21);
			this.TestSelectComboBox.TabIndex = 11;
			// 
			// RunTestButton
			// 
			this.RunTestButton.Location = new System.Drawing.Point(8, 152);
			this.RunTestButton.Name = "RunTestButton";
			this.RunTestButton.Size = new System.Drawing.Size(136, 23);
			this.RunTestButton.TabIndex = 12;
			this.RunTestButton.Text = "Run Selected Test";
			this.RunTestButton.Click += new System.EventHandler(this.RunTestButton_Click);
			// 
			// MenuForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(152, 238);
			this.Controls.Add(this.RunTestButton);
			this.Controls.Add(this.TestSelectComboBox);
			this.Controls.Add(this.TestsLabel);
			this.Controls.Add(this.DemosLabel);
			this.Controls.Add(this.quitButton);
			this.Controls.Add(this.plotSurface2DDemoButton);
			this.Controls.Add(this.multiPlotDemoButton);
			this.Name = "MenuForm";
			this.Text = "NPlot Demo";
			this.ResumeLayout(false);

		}
		#endregion

        [STAThread]
		static void Main() 
		{
			Application.Run(new MenuForm());
		}

		private System.Windows.Forms.Form displayForm = null;
		private void WindowThread()
		{
			displayForm.ShowDialog();
		}

		private void runDemoButton_Click(object sender, System.EventArgs e)
		{
 			displayForm = new FinancialDemo();
 			displayForm.ShowDialog();
 		/*
			System.Threading.Thread t = new Thread( new ThreadStart(WindowThread) );
            //t.ApartmentState = ApartmentState.STA;
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
          */  
        }

        private void plotSurface2DDemoButton_Click(object sender, System.EventArgs e)
		{
			displayForm = new PlotSurface2DDemo();
			displayForm.ShowDialog();
			/*
			System.Threading.Thread t = new Thread( new ThreadStart(WindowThread) );
            //t.SetApartmentState( ApartmentState.STA ); // necessary for copy to clipboard to work.
            t.ApartmentState = ApartmentState.STA;
            t.Start();
			*/
		}


		private void quitButton_Click(object sender, System.EventArgs e)
		{
			Application.Exit();
		}

		private void RunTestButton_Click(object sender, System.EventArgs e)
		{
			TestItem ti = (TestItem)this.TestSelectComboBox.SelectedItem;
		
			displayForm = ti.Form;
			System.Threading.Thread t = new Thread( new ThreadStart(WindowThread) );
			t.SetApartmentState( ApartmentState.STA ); // necessary for copy to clipboard to work.
			//t.ApartmentState = ApartmentState.STA;
			t.Start();
			
		}


	}
}
