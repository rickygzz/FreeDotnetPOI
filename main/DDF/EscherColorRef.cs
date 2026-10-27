/* ====================================================================
    Licensed to the Apache Software Foundation (ASF) under one or more
    contributor license agreements.  See the NOTICE file distributed with
    this work for Additional information regarding copyright ownership.
    The ASF licenses this file to You under the Apache License, Version 2.0
    (the "License"); you may not use this file except in compliance with
    the License.  You may obtain a copy of the License at

        http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.
==================================================================== */

using NPOI.Util;
using System.Diagnostics;

namespace NPOI.DDF;

/**
* An OfficeArtCOLORREF structure entry which also handles color extension opid data
*/
public class EscherColorRef
{
    readonly int opid = -1;
    int colorRef = 0;

    /*
    * A bit that specifies whether the system color scheme will be used to determine the color.
    * A value of 0x1 specifies that green and red will be treated as an unsigned 16-bit index
    * into the system color table. Values less than 0x00F0 map directly to system colors.
    */
    private static readonly BitField FLAG_SYS_INDEX = new(0x10000000);

    /*
    * A bit that specifies whether the current application-defined color scheme will be used
    * to determine the color. A value of 0x1 specifies that red will be treated as an index
    * into the current color scheme table. If this value is 0x1, green and blue MUST be 0x00.
    */
    private static readonly BitField FLAG_SCHEME_INDEX = new(0x08000000);

    /*
    * A bit that specifies whether the color is a standard RGB color.
    * 0x0 : The RGB color MAY use halftone dithering to display.
    * 0x1 : The color MUST be a solid color.
    */
    private static readonly BitField FLAG_SYSTEM_RGB = new(0x04000000);

    /*
    * A bit that specifies whether the current palette will be used to determine the color.
    * A value of 0x1 specifies that red, green, and blue contain an RGB value that will be
    * matched in the current color palette. This color MUST be solid.
    */
    private static readonly BitField FLAG_PALETTE_RGB = new(0x02000000);

    /*
    * A bit that specifies whether the current palette will be used to determine the color.
    * A value of 0x1 specifies that green and red will be treated as an unsigned 16-bit index into
    * the current color palette. This color MAY be dithered. If this value is 0x1, blue MUST be 0x00.
    */
    private static readonly BitField FLAG_PALETTE_INDEX = new(0x01000000);

    /*
    * An unsigned integer that specifies the intensity of the blue color channel. A value
    * of 0x00 has the minimum blue intensity. A value of 0xFF has the maximum blue intensity.
    */
    private static readonly BitField FLAG_BLUE = new(0x00FF0000);

    /*
    * An unsigned integer that specifies the intensity of the green color channel. A value
    * of 0x00 has the minimum green intensity. A value of 0xFF has the maximum green intensity.
    */
    private static readonly BitField FLAG_GREEN = new(0x0000FF00);

    /*
    * An unsigned integer that specifies the intensity of the red color channel. A value
    * of 0x00 has the minimum red intensity. A value of 0xFF has the maximum red intensity.
    */
    private static readonly BitField FLAG_RED = new(0x000000FF);

    public EscherColorRef(int colorRef)
    {
        this.colorRef = colorRef;
    }

    public EscherColorRef(byte[] source, int start, int len)
    {
        Debug.Assert(len == 4 || len == 6);

        int offset = start;
        if (len == 6)
        {
            opid = LittleEndian.GetUShort(source, offset);
            offset += 2;
        }
        colorRef = LittleEndian.GetInt(source, offset);
    }

    public bool HasSysIndexFlag()
    {
        return FLAG_SYS_INDEX.IsSet(colorRef);
    }

    public void SetSysIndexFlag(bool flag)
    {
        colorRef = FLAG_SYS_INDEX.SetBoolean(colorRef, flag);
    }

    public bool HasSchemeIndexFlag()
    {
        return FLAG_SCHEME_INDEX.IsSet(colorRef);
    }

    public void SetSchemeIndexFlag(bool flag)
    {
        colorRef = FLAG_SCHEME_INDEX.SetBoolean(colorRef, flag);
    }

    public bool HasSystemRGBFlag()
    {
        return FLAG_SYSTEM_RGB.IsSet(colorRef);
    }

    public void SetSystemRGBFlag(bool flag)
    {
        colorRef = FLAG_SYSTEM_RGB.SetBoolean(colorRef, flag);
    }

    public bool HasPaletteRGBFlag()
    {
        return FLAG_PALETTE_RGB.IsSet(colorRef);
    }

    public void SetPaletteRGBFlag(bool flag)
    {
        colorRef = FLAG_PALETTE_RGB.SetBoolean(colorRef, flag);
    }

    public bool HasPaletteIndexFlag()
    {
        return FLAG_PALETTE_INDEX.IsSet(colorRef);
    }

    public void SetPaletteIndexFlag(bool flag)
    {
        colorRef = FLAG_PALETTE_INDEX.SetBoolean(colorRef, flag);
    }

    public int[] GetRGB()
    {
        int[] rgb = [
            FLAG_RED.GetValue(colorRef),
            FLAG_GREEN.GetValue(colorRef),
            FLAG_BLUE.GetValue(colorRef)
        ];

        return rgb;
    }

    /**
    * @return {@link SysIndexSource} if {@link #hasSysIndexFlag()} is {@code true}, otherwise null
    */
    public SysIndexSource GetSysIndexSource()
    {
        if (!HasSysIndexFlag())
        {
            return null;
        }
        int val = FLAG_RED.GetValue(colorRef);
        foreach (SysIndexSource sis in SysIndexSource.Values())
        {
            if (sis.value == val)
            {
                return sis;
            }
        }
        return null;
    }

    /**
    * Return the {@link SysIndexProcedure} - for invert flag use {@link #getSysIndexInvert()}
    * @return {@link SysIndexProcedure} if {@link #hasSysIndexFlag()} is {@code true}, otherwise null
    */
    public SysIndexProcedure GetSysIndexProcedure()
    {
        if (!HasSysIndexFlag())
        {
            return null;
        }
        int val = FLAG_RED.GetValue(colorRef);
        foreach (SysIndexProcedure sip in SysIndexProcedure.Values())
        {
            if (sip == SysIndexProcedure.INVERT_AFTER || sip == SysIndexProcedure.INVERT_HIGHBIT_AFTER)
            {
                continue;
            }
            if (sip.mask.IsSet(val))
            {
                return sip;
            }
        }
        return null;
    }

    /**
    * @return 0 for no invert flag, 1 for {@link SysIndexProcedure#INVERT_AFTER} and
    * 2 for {@link SysIndexProcedure#INVERT_HIGHBIT_AFTER}
    */
    public int GetSysIndexInvert()
    {
        if (!HasSysIndexFlag())
        {
            return 0;
        }
        int val = FLAG_GREEN.GetValue(colorRef);
        if ((SysIndexProcedure.INVERT_AFTER.mask.IsSet(val)))
        {
            return 1;
        }
        if ((SysIndexProcedure.INVERT_HIGHBIT_AFTER.mask.IsSet(val)))
        {
            return 2;
        }
        return 0;
    }

    /**
    * @return index of the scheme color or -1 if {@link #hasSchemeIndexFlag()} is {@code false}
    *
    * @see NPOI.HSLF.Record.ColorSchemeAtom#getColor(int)
    */
    public int GetSchemeIndex()
    {
        if (!HasSchemeIndexFlag())
        {
            return -1;
        }
        return FLAG_RED.GetValue(colorRef);
    }

    /**
    * @return index of current palette (color) or -1 if {@link #hasPaletteIndexFlag()} is {@code false}
    */
    public int GetPaletteIndex()
    {
        if (!HasPaletteIndexFlag())
        {
            return -1;
        }
        return (FLAG_GREEN.GetValue(colorRef) << 8) & FLAG_RED.GetValue(colorRef);
    }
}
