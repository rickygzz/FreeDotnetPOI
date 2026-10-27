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

namespace NPOI.DDF;

/**
* The following enum specifies values that indicate special procedural properties that
* are used to modify the color components of another color. These values are combined with
* those of the {@link SysIndexSource} enum or with a user-specified color.
* The first six values are mutually exclusive.
*/
public class SysIndexProcedure
{
    internal static SysIndexProcedure[] Values()
    {
        return [
            SysIndexProcedure.DARKEN_COLOR,
            SysIndexProcedure.LIGHTEN_COLOR,
            SysIndexProcedure.ADD_GRAY_LEVEL,
            SysIndexProcedure.SUB_GRAY_LEVEL,
            SysIndexProcedure.REVERSE_GRAY_LEVEL,
            SysIndexProcedure.THRESHOLD,
            SysIndexProcedure.INVERT_AFTER,
            SysIndexProcedure.INVERT_HIGHBIT_AFTER
        ];
    }
    /*
    * Darken the color by the value that is specified in the blue field.
    * A blue value of 0xFF specifies that the color is to be left unChanged,
    * whereas a blue value of 0x00 specifies that the color is to be completely darkened.
    */
    public static SysIndexProcedure DARKEN_COLOR = new(0x01),

    /*
    * Lighten the color by the value that is specified in the blue field.
    * A blue value of 0xFF specifies that the color is to be left unChanged,
    * whereas a blue value of 0x00 specifies that the color is to be completely lightened.
    */
    LIGHTEN_COLOR = new(0x02),

    /*
    * Add a gray level RGB value. The blue field Contains the gray level to Add:
    * NewColor = SourceColor + gray
    */
    ADD_GRAY_LEVEL = new(0x03),

    /*
    * Subtract a gray level RGB value. The blue field Contains the gray level to subtract:
    * NewColor = SourceColor - gray
    */
    SUB_GRAY_LEVEL = new(0x04),

    /*
    * Reverse-subtract a gray level RGB value. The blue field Contains the gray level from
    * which to subtract:
    * NewColor = gray - SourceColor
    */
    REVERSE_GRAY_LEVEL = new(0x05),

    /*
    * If the color component being modified is less than the parameter Contained in the blue
    * field, Set it to the minimum intensity. If the color component being modified is greater
    * than or equal to the parameter, Set it to the maximum intensity.
    */
    THRESHOLD = new(0x06),

    /*
    * After making other modifications, invert the color.
    * This enum value is only for documentation and won't be directly returned.
    */
    INVERT_AFTER = new(0x20),

    /*
    * After making other modifications, invert the color by toggling just the high bit of each
    * color channel.
    * This enum value is only for documentation and won't be directly returned.
    */
    INVERT_HIGHBIT_AFTER = new(0x40);
    internal BitField mask;
    internal SysIndexProcedure(int mask)
    {
        this.mask = new BitField(mask);
    }
}
