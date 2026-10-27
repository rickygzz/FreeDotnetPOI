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

namespace NPOI.DDF;

public class SysIndexSource
{
    private readonly int opid = -1;
    private readonly int colorRef = 0;
    internal static SysIndexSource[] Values()
    {
        return [
            SysIndexSource.FILL_COLOR,
            SysIndexSource.LINE_OR_FILL_COLOR,
            SysIndexSource.SHADOW_COLOR,
            SysIndexSource.CURRENT_OR_LAST_COLOR,
            SysIndexSource.FILL_BACKGROUND_COLOR,
            SysIndexSource.LINE_BACKGROUND_COLOR,
            SysIndexSource.FILL_OR_LINE_COLOR
        ];
    }
    /* Use the fill color of the shape. */
    public static SysIndexSource FILL_COLOR = new(0xF0),
    /* If the shape Contains a line, use the line color of the shape. Otherwise, use the fill color. */
    LINE_OR_FILL_COLOR = new(0xF1),

    // Use the line color of the shape.
    LINE_COLOR = new(0xF2),

    // Use the shadow color of the shape
    SHADOW_COLOR = new(0xF3),

    // Use the current, or last-used, color.
    CURRENT_OR_LAST_COLOR = new(0xF4),

    // Use the fill background color of the shape.
    FILL_BACKGROUND_COLOR = new(0xF5),

    // Use the line background color of the shape.
    LINE_BACKGROUND_COLOR = new(0xF6),

    // If the shape Contains a Fill, use the fill color of the shape. Otherwise, use the line color.
    FILL_OR_LINE_COLOR = new(0xF7);

    internal int value;
    internal SysIndexSource(int value) { this.value = value; }
}
