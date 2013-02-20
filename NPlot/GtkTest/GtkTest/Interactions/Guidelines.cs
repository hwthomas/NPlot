using System;
using System.Drawing;

namespace NPlot
{
    public class Guidelines : Interaction
    {
        public Guidelines ()
        {
        }

        /// <summary>
        /// Horizontal and Vertical guidelines
        /// </summary>
            
        private bool vertical_ = true;
        private bool horizontal_ = true;
        private int xPos_ = -1;
        private int yPos_ = -1;
        private Point xStart_, xEnd_;
        private Point yStart_, yEnd_;
        private Color lineColor_;
            
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="horizontal">enable horizontal guideline</param>
        /// <param name="vertical">enable vertical guideline/param>
        public Guidelines (bool horizontal, bool vertical)
        {
            lineColor_ = Color.White;
            Vertical = vertical;
            Horizontal = horizontal;
        }
                
        /// <summary>
        /// LineColor property
        /// </summary>
        public Color LineColor
        {
            get { return lineColor_; }
            set { lineColor_ = value; }
        }

        /// <summary>
        /// Horizontal guideline property enable/disable
        /// </summary>
        public bool Horizontal
        {
            get { return horizontal_; }
            set { horizontal_ = value; }
        }

        /// <summary>
        /// Vertical guideline enable/disable
        /// </summary>
        public bool Vertical
        {
            get { return vertical_; }
            set { vertical_ = value; }
        }

            
        /// <summary>
        /// MouseMove method for Guidelines
        /// </summary>
        /// <param name="X">mouse X position</param>
        /// <param name="Y"> mouse Y position</param>
        /// <param name="keys"> mouse and keyboard modifiers</param>
        /// <param name="ps">the InteractivePlotSurface2D</param>
        public override bool DoMouseMove (int X, int Y, Modifier keys, PlotSurface2D ps)
        {
            Rectangle plotArea = ps.PlotAreaBoundingBoxCache;
            Rectangle lineExtent = Rectangle.Empty;

            if (vertical_) {
                if (xPos_ != -1) {
                    // set extents to erase current vertical guideline
                    //lineExtent = new Rectangle (yStart_.X, yStart_.Y, 1, (int)Math.Abs (yEnd_.Y - yStart_.Y));
                    ps.DrawOverlayLine(yStart_, yEnd_, lineColor_, false);
                }
                    
                // Only display guideline when mouse is within the plotArea
                if (plotArea.Contains(X,Y)) {
                    xPos_ = X;
                    yStart_ = new Point(X, plotArea.Top);
                    yEnd_ = new Point(X, plotArea.Bottom);
                    ps.DrawOverlayLine(yStart_, yEnd_, lineColor_, true);
                } 
                else {
                    xPos_ = -1;
                }

                //if (xPos_ != -1) {
                //    // Invalidate line extents, then DoDraw will request a new line to be drawn
                //    ps.QueueDraw (lineExtent);
                //}

            }
            if (horizontal_) {
                if (yPos_ != -1) {
                    // set extents to erase current vertical guideline
                    //lineExtent = new Rectangle (xStart_.X, xStart_.Y, (int)Math.Abs (xEnd_.X - xStart_.X), 1);
                    ps.DrawOverlayLine(xStart_, xEnd_, lineColor_, false);
                }
                // Only display guideline when mouse is within the plotArea
                if (plotArea.Contains(X,Y)) {
                    yPos_ = Y;
                    xStart_ = new Point(plotArea.Left,Y);
                    xEnd_ = new Point(plotArea.Right,Y);
                        ps.DrawOverlayLine(xStart_, xEnd_, lineColor_, true);
                }
                else {
                    yPos_ = -1; 
                }

                //if (yPos_ != -1) {
                //    // Invalidate line extents, then DoDraw will request a new line to be drawn
                //    ps.QueueDraw (lineExtent);
                //}
            }
            return false;
        }
            

        /// <summary>
        /// MouseLeave method for Guidelines
        /// </summary>
        /// <param name="ps">the InteractivePlotSurface2D</param>
        public override bool DoMouseLeave (InteractivePlotSurface2D ps)
        {
            Rectangle lineExtent = Rectangle.Empty;

            if (vertical_) {
                if (xPos_ != -1) {
                    // set extents to erase current vertical guideline
                    lineExtent = new Rectangle (yStart_.X, yStart_.Y, 1, (int)Math.Abs (yEnd_.Y - yStart_.Y));
                    ps.DrawOverlayLine(yStart_, yEnd_, lineColor_, false);
                }
                xPos_ = -1;
            }
            if (horizontal_) {
                if (yPos_ != -1) {
                    // set extents to erase current vertical guideline
                    lineExtent = new Rectangle (xStart_.X, xStart_.Y, (int)Math.Abs (xEnd_.X - xStart_.X), 1);
                    ps.DrawOverlayLine(xStart_, xEnd_, lineColor_, false);
                }
                yPos_ = -1;
            }
            return false;
        }


        public override void DoDraw (Graphics g, Rectangle dirtyRect)
        {
        }

    } // Guidelines
 
}

