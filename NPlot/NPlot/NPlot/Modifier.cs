using System;

namespace NPlot
{
	/// <summary>
	/// Common set of button/key flags that NPlot will respond to for Interactions
	/// </summary>
	[System.Flags]
	public enum Modifier
	{
		None	= 0x00000,	// no keys
		Alt		= 0x00001,	// the Alt key
		Control = 0x00002,	// the Control key
		Shift	= 0x00004,	// the Shift key
		Command = 0x00008,	// the Command key
		Button1 = 0x00010,	// the first (left) mouse button
		Button2 = 0x00020,	// the second (middle) mouse button
		Button3 = 0x00040,	// the third (right) mouse button
		Spare1	= 0x00080,
		Home	= 0x00100,	// a restricted set of keyboard keys
		End		= 0x00200,	// that NPlot will respond to
		Left	= 0x00400,
		Up		= 0x00800,
		Right	= 0x01000,
		Down	= 0x02000,
		PageUp	= 0x02000,
		PageDn	= 0x04000,
		Plus	= 0x08000,
		Minus	= 0x10000
	}
}

