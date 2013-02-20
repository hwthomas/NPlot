using System;

namespace NPlot
{
    /// <summary>
    /// CursorTypes for NPlot that are useful for Interactions
    /// </summary>
    public enum CursorType
    {
        None,           // no cursor displayed
        LeftPointer,    // standard mouse pointer
        RightPointer,   // same but pointing right
        CrossHair,      // for accurate coordinates
        Hand,           // use for dragging in plot
        LeftRight,      // expanding horizontal axis
        UpDown,         // expanding vertical axis
        Zoom            // expanding both axes
    }

}

