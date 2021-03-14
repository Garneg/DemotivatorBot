using System;
using System.Collections.Generic;
using System.Text;
using SixLabors.Fonts;

namespace DemotivatorBot
{
    public class GlyphRenderClass : IGlyphRenderer
    {
        void IGlyphRenderer.BeginFigure() { }
        bool IGlyphRenderer.BeginGlyph(FontRectangle bounds, GlyphRendererParameters paramaters) { return false; }
        void IGlyphRenderer.BeginText(FontRectangle bounds) { }
        void IGlyphRenderer.CubicBezierTo(System.Numerics.Vector2 secondControlPoint, System.Numerics.Vector2 thirdControlPoint, System.Numerics.Vector2 point) { }
        void IGlyphRenderer.EndFigure() { }
        void IGlyphRenderer.EndGlyph() { }
        void IGlyphRenderer.EndText() { }
        void IGlyphRenderer.LineTo(System.Numerics.Vector2 point) { }
        void IGlyphRenderer.MoveTo(System.Numerics.Vector2 point) { }
        void IGlyphRenderer.QuadraticBezierTo(System.Numerics.Vector2 secondControlPoint, System.Numerics.Vector2 point) { }

    }
}
