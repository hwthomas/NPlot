//
// NPlot - A charting library for .NET
// 
// Swf.PlotSurface2DActiveDemo.cs
// 
// This is PlotSurface2DDemo with interactive features
// added and implemented using Swf Graphics (HWT 2011)
// 
// Copyright (C) 2003-2009 Matt Howlett and others.
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
// 
// 1. Redistributions of source code must retain the above copyright notice, this
//    list of conditions and the following disclaimer.
// 2. Redistributions in binary form must reproduce the above copyright notice,
//    this list of conditions and the following disclaimer in the documentation
//    and/or other materials provided with the distribution.
// 3. Neither the name of NPlot nor the names of its contributors may
//    be used to endorse or promote products derived from this software without
//    specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
// IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT,
// INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING,
// BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
// LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE
// OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED
// OF THE POSSIBILITY OF SUCH DAMAGE.
//

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Data;
using System.IO;
using System.Reflection;

using NPlot;
using SwfSamples;
using System.Windows.Forms;


namespace NPlotDemo
{
    /// <summary>
    /// The main demo window.
    /// </summary>
	public class PlotSurface2DDemo : System.Windows.Forms.Form
	{
		private int currentPlot = 0;
		private int id = 1;

		private Type [] sampleTypes;
		private Type currentType;
		private PlotSample currentSample;

		private System.Windows.Forms.Control plotCanvas;

        private System.Windows.Forms.Button quitButton;
        private System.Windows.Forms.Button nextPlotButton;
        private System.Windows.Forms.Button prevPlotButton;
        private System.Windows.Forms.Button printButton;
        private System.ComponentModel.IContainer components;
        private PrintDocument printDocument;
        
		private System.Windows.Forms.Label exampleNumberLabel;

        private TextBox infoBox;

		public PlotSurface2DDemo()
		{
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

			// Define array of PlotSamples classes
			sampleTypes = new Type []
			{
				typeof (PlotWave),
				typeof (PlotDataset),
				typeof (PlotMockup),
				typeof (PlotImage),
				typeof (PlotQE),
				typeof (PlotMarkers),
				typeof (PlotLogLin),
				typeof (PlotLogLog),
				typeof (PlotParticles), 
				typeof (PlotWavelet), 
				typeof (PlotSinc), 
				typeof (PlotGaussian),
				typeof (PlotLabelAxis),
				typeof (PlotCircular),
				typeof (PlotCandle),
				typeof (PlotABC)
			};


            // setup resize handler that takes care of placement of buttons, and sizing of
            // plotsurface2D when window is resized.
            Resize += new System.EventHandler (this.ResizeHandler);

            // set up printer
            printDocument = new PrintDocument();
            printDocument.PrintPage += new PrintPageEventHandler(pd_PrintPage);

			// draw the first plot sample
			currentPlot = 0;
			id = currentPlot + 1;
			exampleNumberLabel.Text = "Plot " + id.ToString("0") + "/" + sampleTypes.Length.ToString("0");
			ShowSample (currentPlot);

		}


        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            if (disposing) {
                if (components != null) {
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
			quitButton = new System.Windows.Forms.Button();
			nextPlotButton = new System.Windows.Forms.Button();
			prevPlotButton = new System.Windows.Forms.Button();
			printButton = new System.Windows.Forms.Button();
			exampleNumberLabel = new System.Windows.Forms.Label();

			infoBox = new System.Windows.Forms.TextBox();
			plotCanvas = new System.Windows.Forms.Control ();
			components = new System.ComponentModel.Container();

			SuspendLayout();
// 
// plotCanvas
// 
			plotCanvas.BackColor = System.Drawing.SystemColors.Control;
			plotCanvas.Location = new System.Drawing.Point(8, 8);
			plotCanvas.Size = new System.Drawing.Size(616, 360);
// 
// quitButton
// 
			quitButton.Anchor = 
				AnchorStyles.Bottom | AnchorStyles.Left;
			quitButton.Location = new System.Drawing.Point(248, 386);
			quitButton.Name = "quitButton";
			quitButton.TabIndex = 14;
			quitButton.Text = "Close";
			quitButton.Click += new System.EventHandler(this.quitButton_Click);
// 
// nextPlotButton
//
			nextPlotButton.Anchor =
				AnchorStyles.Bottom | AnchorStyles.Left;
			nextPlotButton.Location = new System.Drawing.Point(88, 386);
			nextPlotButton.Name = "nextPlotButton";
			nextPlotButton.TabIndex = 17;
			nextPlotButton.Text = "Next";
			nextPlotButton.Click += new System.EventHandler(this.nextPlotButton_Click);
// 
// prevPlotButton
// 
			prevPlotButton.Anchor =
				AnchorStyles.Bottom | AnchorStyles.Left;
			prevPlotButton.Location = new System.Drawing.Point(8, 386);
			prevPlotButton.Name = "prevPlotButton";
			prevPlotButton.TabIndex = 15;
			prevPlotButton.Text = "Prev";
			prevPlotButton.Click += new System.EventHandler(this.prevPlotButton_Click);

// 
// printButton
// 
			printButton.Anchor =
				AnchorStyles.Bottom | AnchorStyles.Left;
			printButton.Location = new System.Drawing.Point(166, 386);
			printButton.Name = "printButton";
			printButton.TabIndex = 9;
			printButton.Text = "Print";
			printButton.Click += new System.EventHandler(this.printButton_Click);
// 
// exampleNumberLabel
// 
			exampleNumberLabel.Anchor =
				AnchorStyles.Bottom | AnchorStyles.Left;
			exampleNumberLabel.Location = new System.Drawing.Point(336, 390);
			exampleNumberLabel.Name = "exampleNumberLabel";
			exampleNumberLabel.Size = new System.Drawing.Size(72, 23);
			exampleNumberLabel.TabIndex = 16;
// 
// infoBox
// 
			infoBox.Anchor =
				AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			infoBox.AutoSize = false;
			infoBox.Location = new System.Drawing.Point(13, 416);
			infoBox.Multiline = true;
			infoBox.Name = "infoBox";
			infoBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			infoBox.Size = new System.Drawing.Size(611, 92);
			infoBox.TabIndex = 18;
// 
// PlotSurface2DDemo
// 
			AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			ClientSize = new System.Drawing.Size(631, 520);
			Controls.Add (infoBox);
			Controls.Add (plotCanvas);
			Controls.Add (quitButton);
			Controls.Add (printButton);
			Controls.Add (prevPlotButton);
			Controls.Add (exampleNumberLabel);
			Controls.Add (nextPlotButton);
			Name = "PlotSurface2DDemo";
			Text = "NPlot C# Demo";

			ResumeLayout (false);

        }
        #endregion

		/// <summary>
		/// Creates and shows samplePlot [index]
		/// </summary>
		private void ShowSample (int index)
		{
			Point canvasOrigin = plotCanvas.Location;
			Size canvasSize = plotCanvas.Size;

			SuspendLayout ();
			Controls.Remove (plotCanvas);		// remove previous sample

			currentType = sampleTypes [index];
			currentSample = (PlotSample)Activator.CreateInstance (currentType);

			plotCanvas = currentSample.Canvas;
			plotCanvas.Location = canvasOrigin;
			plotCanvas.Size = canvasSize;

			infoBox.Text = currentSample.InfoText;	// update info Text
			Controls.Add (plotCanvas);				// Add new sample
			ResumeLayout ();
		}


        // The PrintPage event is raised for each page to be printed.
        private void pd_PrintPage(object sender, PrintPageEventArgs ev) 
        {
			//plotSurface.Draw( ev.Graphics, ev.MarginBounds );
			//ev.HasMorePages = false;
        }
    

        /// <summary>
        /// callback for quit button click
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="e">unused</param>
        private void quitButton_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }


        /// <summary>
        /// callback for resize event.
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="e">unused</param>
        private void ResizeHandler(object sender, System.EventArgs e)
        {
            /*
             * plotSurface.Width = this.Width - 28;
             * plotSurface.Height = this.Height - 100;
             * nplotLinkLabel.Top = this.Height - 60;
             * nextPlotButton.Top = this.Height - 64;
             * prevPlotButton.Top = this.Height - 64;
             * printButton.Top = this.Height - 64;
             * quitButton.Top = this.Height - 64;
             * exampleNumberLabel.Top = this.Height - 60;
             */
        }

        /// <summary>
        /// callback for next button click
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="e">unused</param>
        private void nextPlotButton_Click(object sender, System.EventArgs e)
        {
            currentPlot += 1;
            if (currentPlot == sampleTypes.Length) {
                currentPlot = 0;
            }
            id = currentPlot+1;
            exampleNumberLabel.Text = "Plot " + id.ToString("0") + "/" + sampleTypes.Length.ToString("0");
			ShowSample (currentPlot);
        }

        /// <summary>
        /// Callback for prev button click.
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="e">unused</param>
        private void prevPlotButton_Click(object sender, System.EventArgs e)
        {
            currentPlot--;
            if ( currentPlot == -1 ) {
				currentPlot = sampleTypes.Length-1;
			}
            id = currentPlot + 1;
            exampleNumberLabel.Text = "Plot " + id.ToString("0") + "/" + sampleTypes.Length.ToString("0");
			ShowSample (currentPlot);
        }

        /// <summary>
        /// callback for print button click
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="e">unused</param>
        private void printButton_Click(object sender, System.EventArgs e)
        {
            PrintDialog dlg = new PrintDialog();
            dlg.Document = printDocument;
            if (dlg.ShowDialog() == DialogResult.OK) 
            {
                try
                {
                    printDocument.Print();
                }
                catch
                {
                    Console.WriteLine( "problem printing.\n" );
                }
            }   
		}


	}
}
