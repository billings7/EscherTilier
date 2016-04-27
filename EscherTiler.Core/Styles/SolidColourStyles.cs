using System.Collections.Generic;
using JetBrains.Annotations;

namespace EscherTiler.Styles
{
    public partial class SolidColourStyle
    {
        [NotNull]
        private static readonly Dictionary<string, SolidColourStyle> _styles = new Dictionary<string, SolidColourStyle>();

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color Transparent that has an ARGB value of #00FFFFFF.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle Transparent
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("Transparent", out style)) return style;
                _styles["Transparent"] = style = new SolidColourStyle(Colour.Transparent);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color AliceBlue that has an ARGB value of #FFF0F8FF.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle AliceBlue
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("AliceBlue", out style)) return style;
                _styles["AliceBlue"] = style = new SolidColourStyle(Colour.AliceBlue);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color AntiqueWhite that has an ARGB value of #FFFAEBD7.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle AntiqueWhite
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("AntiqueWhite", out style)) return style;
                _styles["AntiqueWhite"] = style = new SolidColourStyle(Colour.AntiqueWhite);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color Aqua that has an ARGB value of #FF00FFFF.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle Aqua
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("Aqua", out style)) return style;
                _styles["Aqua"] = style = new SolidColourStyle(Colour.Aqua);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color Aquamarine that has an ARGB value of #FF7FFFD4.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle Aquamarine
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("Aquamarine", out style)) return style;
                _styles["Aquamarine"] = style = new SolidColourStyle(Colour.Aquamarine);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color Azure that has an ARGB value of #FFF0FFFF.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle Azure
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("Azure", out style)) return style;
                _styles["Azure"] = style = new SolidColourStyle(Colour.Azure);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color Beige that has an ARGB value of #FFF5F5DC.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle Beige
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("Beige", out style)) return style;
                _styles["Beige"] = style = new SolidColourStyle(Colour.Beige);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color Bisque that has an ARGB value of #FFFFE4C4.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle Bisque
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("Bisque", out style)) return style;
                _styles["Bisque"] = style = new SolidColourStyle(Colour.Bisque);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color Black that has an ARGB value of #FF000000.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle Black
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("Black", out style)) return style;
                _styles["Black"] = style = new SolidColourStyle(Colour.Black);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color BlanchedAlmond that has an ARGB value of #FFFFEBCD.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle BlanchedAlmond
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("BlanchedAlmond", out style)) return style;
                _styles["BlanchedAlmond"] = style = new SolidColourStyle(Colour.BlanchedAlmond);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color Blue that has an ARGB value of #FF0000FF.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle Blue
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("Blue", out style)) return style;
                _styles["Blue"] = style = new SolidColourStyle(Colour.Blue);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color BlueViolet that has an ARGB value of #FF8A2BE2.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle BlueViolet
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("BlueViolet", out style)) return style;
                _styles["BlueViolet"] = style = new SolidColourStyle(Colour.BlueViolet);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color Brown that has an ARGB value of #FFA52A2A.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle Brown
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("Brown", out style)) return style;
                _styles["Brown"] = style = new SolidColourStyle(Colour.Brown);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color BurlyWood that has an ARGB value of #FFDEB887.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle BurlyWood
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("BurlyWood", out style)) return style;
                _styles["BurlyWood"] = style = new SolidColourStyle(Colour.BurlyWood);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color CadetBlue that has an ARGB value of #FF5F9EA0.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle CadetBlue
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("CadetBlue", out style)) return style;
                _styles["CadetBlue"] = style = new SolidColourStyle(Colour.CadetBlue);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color Chartreuse that has an ARGB value of #FF7FFF00.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle Chartreuse
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("Chartreuse", out style)) return style;
                _styles["Chartreuse"] = style = new SolidColourStyle(Colour.Chartreuse);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color Chocolate that has an ARGB value of #FFD2691E.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle Chocolate
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("Chocolate", out style)) return style;
                _styles["Chocolate"] = style = new SolidColourStyle(Colour.Chocolate);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color Coral that has an ARGB value of #FFFF7F50.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle Coral
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("Coral", out style)) return style;
                _styles["Coral"] = style = new SolidColourStyle(Colour.Coral);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color CornflowerBlue that has an ARGB value of #FF6495ED.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle CornflowerBlue
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("CornflowerBlue", out style)) return style;
                _styles["CornflowerBlue"] = style = new SolidColourStyle(Colour.CornflowerBlue);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color Cornsilk that has an ARGB value of #FFFFF8DC.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle Cornsilk
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("Cornsilk", out style)) return style;
                _styles["Cornsilk"] = style = new SolidColourStyle(Colour.Cornsilk);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color Crimson that has an ARGB value of #FFDC143C.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle Crimson
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("Crimson", out style)) return style;
                _styles["Crimson"] = style = new SolidColourStyle(Colour.Crimson);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color Cyan that has an ARGB value of #FF00FFFF.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle Cyan
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("Cyan", out style)) return style;
                _styles["Cyan"] = style = new SolidColourStyle(Colour.Cyan);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color DarkBlue that has an ARGB value of #FF00008B.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle DarkBlue
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("DarkBlue", out style)) return style;
                _styles["DarkBlue"] = style = new SolidColourStyle(Colour.DarkBlue);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color DarkCyan that has an ARGB value of #FF008B8B.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle DarkCyan
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("DarkCyan", out style)) return style;
                _styles["DarkCyan"] = style = new SolidColourStyle(Colour.DarkCyan);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color DarkGoldenrod that has an ARGB value of #FFB8860B.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle DarkGoldenrod
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("DarkGoldenrod", out style)) return style;
                _styles["DarkGoldenrod"] = style = new SolidColourStyle(Colour.DarkGoldenrod);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color DarkGray that has an ARGB value of #FFA9A9A9.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle DarkGray
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("DarkGray", out style)) return style;
                _styles["DarkGray"] = style = new SolidColourStyle(Colour.DarkGray);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color DarkGreen that has an ARGB value of #FF006400.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle DarkGreen
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("DarkGreen", out style)) return style;
                _styles["DarkGreen"] = style = new SolidColourStyle(Colour.DarkGreen);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color DarkKhaki that has an ARGB value of #FFBDB76B.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle DarkKhaki
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("DarkKhaki", out style)) return style;
                _styles["DarkKhaki"] = style = new SolidColourStyle(Colour.DarkKhaki);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color DarkMagenta that has an ARGB value of #FF8B008B.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle DarkMagenta
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("DarkMagenta", out style)) return style;
                _styles["DarkMagenta"] = style = new SolidColourStyle(Colour.DarkMagenta);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color DarkOliveGreen that has an ARGB value of #FF556B2F.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle DarkOliveGreen
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("DarkOliveGreen", out style)) return style;
                _styles["DarkOliveGreen"] = style = new SolidColourStyle(Colour.DarkOliveGreen);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color DarkOrange that has an ARGB value of #FFFF8C00.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle DarkOrange
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("DarkOrange", out style)) return style;
                _styles["DarkOrange"] = style = new SolidColourStyle(Colour.DarkOrange);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color DarkOrchid that has an ARGB value of #FF9932CC.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle DarkOrchid
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("DarkOrchid", out style)) return style;
                _styles["DarkOrchid"] = style = new SolidColourStyle(Colour.DarkOrchid);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color DarkRed that has an ARGB value of #FF8B0000.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle DarkRed
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("DarkRed", out style)) return style;
                _styles["DarkRed"] = style = new SolidColourStyle(Colour.DarkRed);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color DarkSalmon that has an ARGB value of #FFE9967A.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle DarkSalmon
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("DarkSalmon", out style)) return style;
                _styles["DarkSalmon"] = style = new SolidColourStyle(Colour.DarkSalmon);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color DarkSeaGreen that has an ARGB value of #FF8FBC8B.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle DarkSeaGreen
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("DarkSeaGreen", out style)) return style;
                _styles["DarkSeaGreen"] = style = new SolidColourStyle(Colour.DarkSeaGreen);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color DarkSlateBlue that has an ARGB value of #FF483D8B.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle DarkSlateBlue
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("DarkSlateBlue", out style)) return style;
                _styles["DarkSlateBlue"] = style = new SolidColourStyle(Colour.DarkSlateBlue);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color DarkSlateGray that has an ARGB value of #FF2F4F4F.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle DarkSlateGray
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("DarkSlateGray", out style)) return style;
                _styles["DarkSlateGray"] = style = new SolidColourStyle(Colour.DarkSlateGray);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color DarkTurquoise that has an ARGB value of #FF00CED1.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle DarkTurquoise
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("DarkTurquoise", out style)) return style;
                _styles["DarkTurquoise"] = style = new SolidColourStyle(Colour.DarkTurquoise);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color DarkViolet that has an ARGB value of #FF9400D3.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle DarkViolet
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("DarkViolet", out style)) return style;
                _styles["DarkViolet"] = style = new SolidColourStyle(Colour.DarkViolet);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color DeepPink that has an ARGB value of #FFFF1493.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle DeepPink
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("DeepPink", out style)) return style;
                _styles["DeepPink"] = style = new SolidColourStyle(Colour.DeepPink);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color DeepSkyBlue that has an ARGB value of #FF00BFFF.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle DeepSkyBlue
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("DeepSkyBlue", out style)) return style;
                _styles["DeepSkyBlue"] = style = new SolidColourStyle(Colour.DeepSkyBlue);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color DimGray that has an ARGB value of #FF696969.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle DimGray
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("DimGray", out style)) return style;
                _styles["DimGray"] = style = new SolidColourStyle(Colour.DimGray);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color DodgerBlue that has an ARGB value of #FF1E90FF.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle DodgerBlue
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("DodgerBlue", out style)) return style;
                _styles["DodgerBlue"] = style = new SolidColourStyle(Colour.DodgerBlue);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color Firebrick that has an ARGB value of #FFB22222.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle Firebrick
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("Firebrick", out style)) return style;
                _styles["Firebrick"] = style = new SolidColourStyle(Colour.Firebrick);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color FloralWhite that has an ARGB value of #FFFFFAF0.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle FloralWhite
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("FloralWhite", out style)) return style;
                _styles["FloralWhite"] = style = new SolidColourStyle(Colour.FloralWhite);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color ForestGreen that has an ARGB value of #FF228B22.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle ForestGreen
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("ForestGreen", out style)) return style;
                _styles["ForestGreen"] = style = new SolidColourStyle(Colour.ForestGreen);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color Fuchsia that has an ARGB value of #FFFF00FF.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle Fuchsia
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("Fuchsia", out style)) return style;
                _styles["Fuchsia"] = style = new SolidColourStyle(Colour.Fuchsia);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color Gainsboro that has an ARGB value of #FFDCDCDC.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle Gainsboro
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("Gainsboro", out style)) return style;
                _styles["Gainsboro"] = style = new SolidColourStyle(Colour.Gainsboro);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color GhostWhite that has an ARGB value of #FFF8F8FF.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle GhostWhite
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("GhostWhite", out style)) return style;
                _styles["GhostWhite"] = style = new SolidColourStyle(Colour.GhostWhite);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color Gold that has an ARGB value of #FFFFD700.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle Gold
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("Gold", out style)) return style;
                _styles["Gold"] = style = new SolidColourStyle(Colour.Gold);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color Goldenrod that has an ARGB value of #FFDAA520.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle Goldenrod
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("Goldenrod", out style)) return style;
                _styles["Goldenrod"] = style = new SolidColourStyle(Colour.Goldenrod);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color Gray that has an ARGB value of #FF808080.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle Gray
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("Gray", out style)) return style;
                _styles["Gray"] = style = new SolidColourStyle(Colour.Gray);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color Green that has an ARGB value of #FF008000.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle Green
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("Green", out style)) return style;
                _styles["Green"] = style = new SolidColourStyle(Colour.Green);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color GreenYellow that has an ARGB value of #FFADFF2F.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle GreenYellow
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("GreenYellow", out style)) return style;
                _styles["GreenYellow"] = style = new SolidColourStyle(Colour.GreenYellow);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color Honeydew that has an ARGB value of #FFF0FFF0.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle Honeydew
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("Honeydew", out style)) return style;
                _styles["Honeydew"] = style = new SolidColourStyle(Colour.Honeydew);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color HotPink that has an ARGB value of #FFFF69B4.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle HotPink
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("HotPink", out style)) return style;
                _styles["HotPink"] = style = new SolidColourStyle(Colour.HotPink);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color IndianRed that has an ARGB value of #FFCD5C5C.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle IndianRed
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("IndianRed", out style)) return style;
                _styles["IndianRed"] = style = new SolidColourStyle(Colour.IndianRed);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color Indigo that has an ARGB value of #FF4B0082.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle Indigo
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("Indigo", out style)) return style;
                _styles["Indigo"] = style = new SolidColourStyle(Colour.Indigo);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color Ivory that has an ARGB value of #FFFFFFF0.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle Ivory
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("Ivory", out style)) return style;
                _styles["Ivory"] = style = new SolidColourStyle(Colour.Ivory);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color Khaki that has an ARGB value of #FFF0E68C.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle Khaki
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("Khaki", out style)) return style;
                _styles["Khaki"] = style = new SolidColourStyle(Colour.Khaki);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color Lavender that has an ARGB value of #FFE6E6FA.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle Lavender
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("Lavender", out style)) return style;
                _styles["Lavender"] = style = new SolidColourStyle(Colour.Lavender);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color LavenderBlush that has an ARGB value of #FFFFF0F5.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle LavenderBlush
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("LavenderBlush", out style)) return style;
                _styles["LavenderBlush"] = style = new SolidColourStyle(Colour.LavenderBlush);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color LawnGreen that has an ARGB value of #FF7CFC00.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle LawnGreen
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("LawnGreen", out style)) return style;
                _styles["LawnGreen"] = style = new SolidColourStyle(Colour.LawnGreen);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color LemonChiffon that has an ARGB value of #FFFFFACD.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle LemonChiffon
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("LemonChiffon", out style)) return style;
                _styles["LemonChiffon"] = style = new SolidColourStyle(Colour.LemonChiffon);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color LightBlue that has an ARGB value of #FFADD8E6.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle LightBlue
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("LightBlue", out style)) return style;
                _styles["LightBlue"] = style = new SolidColourStyle(Colour.LightBlue);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color LightCoral that has an ARGB value of #FFF08080.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle LightCoral
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("LightCoral", out style)) return style;
                _styles["LightCoral"] = style = new SolidColourStyle(Colour.LightCoral);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color LightCyan that has an ARGB value of #FFE0FFFF.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle LightCyan
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("LightCyan", out style)) return style;
                _styles["LightCyan"] = style = new SolidColourStyle(Colour.LightCyan);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color LightGoldenrodYellow that has an ARGB value of #FFFAFAD2.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle LightGoldenrodYellow
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("LightGoldenrodYellow", out style)) return style;
                _styles["LightGoldenrodYellow"] = style = new SolidColourStyle(Colour.LightGoldenrodYellow);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color LightGreen that has an ARGB value of #FF90EE90.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle LightGreen
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("LightGreen", out style)) return style;
                _styles["LightGreen"] = style = new SolidColourStyle(Colour.LightGreen);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color LightGray that has an ARGB value of #FFD3D3D3.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle LightGray
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("LightGray", out style)) return style;
                _styles["LightGray"] = style = new SolidColourStyle(Colour.LightGray);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color LightPink that has an ARGB value of #FFFFB6C1.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle LightPink
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("LightPink", out style)) return style;
                _styles["LightPink"] = style = new SolidColourStyle(Colour.LightPink);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color LightSalmon that has an ARGB value of #FFFFA07A.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle LightSalmon
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("LightSalmon", out style)) return style;
                _styles["LightSalmon"] = style = new SolidColourStyle(Colour.LightSalmon);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color LightSeaGreen that has an ARGB value of #FF20B2AA.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle LightSeaGreen
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("LightSeaGreen", out style)) return style;
                _styles["LightSeaGreen"] = style = new SolidColourStyle(Colour.LightSeaGreen);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color LightSkyBlue that has an ARGB value of #FF87CEFA.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle LightSkyBlue
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("LightSkyBlue", out style)) return style;
                _styles["LightSkyBlue"] = style = new SolidColourStyle(Colour.LightSkyBlue);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color LightSlateGray that has an ARGB value of #FF778899.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle LightSlateGray
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("LightSlateGray", out style)) return style;
                _styles["LightSlateGray"] = style = new SolidColourStyle(Colour.LightSlateGray);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color LightSteelBlue that has an ARGB value of #FFB0C4DE.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle LightSteelBlue
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("LightSteelBlue", out style)) return style;
                _styles["LightSteelBlue"] = style = new SolidColourStyle(Colour.LightSteelBlue);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color LightYellow that has an ARGB value of #FFFFFFE0.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle LightYellow
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("LightYellow", out style)) return style;
                _styles["LightYellow"] = style = new SolidColourStyle(Colour.LightYellow);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color Lime that has an ARGB value of #FF00FF00.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle Lime
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("Lime", out style)) return style;
                _styles["Lime"] = style = new SolidColourStyle(Colour.Lime);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color LimeGreen that has an ARGB value of #FF32CD32.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle LimeGreen
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("LimeGreen", out style)) return style;
                _styles["LimeGreen"] = style = new SolidColourStyle(Colour.LimeGreen);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color Linen that has an ARGB value of #FFFAF0E6.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle Linen
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("Linen", out style)) return style;
                _styles["Linen"] = style = new SolidColourStyle(Colour.Linen);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color Magenta that has an ARGB value of #FFFF00FF.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle Magenta
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("Magenta", out style)) return style;
                _styles["Magenta"] = style = new SolidColourStyle(Colour.Magenta);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color Maroon that has an ARGB value of #FF800000.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle Maroon
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("Maroon", out style)) return style;
                _styles["Maroon"] = style = new SolidColourStyle(Colour.Maroon);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color MediumAquamarine that has an ARGB value of #FF66CDAA.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle MediumAquamarine
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("MediumAquamarine", out style)) return style;
                _styles["MediumAquamarine"] = style = new SolidColourStyle(Colour.MediumAquamarine);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color MediumBlue that has an ARGB value of #FF0000CD.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle MediumBlue
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("MediumBlue", out style)) return style;
                _styles["MediumBlue"] = style = new SolidColourStyle(Colour.MediumBlue);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color MediumOrchid that has an ARGB value of #FFBA55D3.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle MediumOrchid
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("MediumOrchid", out style)) return style;
                _styles["MediumOrchid"] = style = new SolidColourStyle(Colour.MediumOrchid);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color MediumPurple that has an ARGB value of #FF9370DB.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle MediumPurple
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("MediumPurple", out style)) return style;
                _styles["MediumPurple"] = style = new SolidColourStyle(Colour.MediumPurple);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color MediumSeaGreen that has an ARGB value of #FF3CB371.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle MediumSeaGreen
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("MediumSeaGreen", out style)) return style;
                _styles["MediumSeaGreen"] = style = new SolidColourStyle(Colour.MediumSeaGreen);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color MediumSlateBlue that has an ARGB value of #FF7B68EE.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle MediumSlateBlue
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("MediumSlateBlue", out style)) return style;
                _styles["MediumSlateBlue"] = style = new SolidColourStyle(Colour.MediumSlateBlue);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color MediumSpringGreen that has an ARGB value of #FF00FA9A.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle MediumSpringGreen
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("MediumSpringGreen", out style)) return style;
                _styles["MediumSpringGreen"] = style = new SolidColourStyle(Colour.MediumSpringGreen);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color MediumTurquoise that has an ARGB value of #FF48D1CC.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle MediumTurquoise
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("MediumTurquoise", out style)) return style;
                _styles["MediumTurquoise"] = style = new SolidColourStyle(Colour.MediumTurquoise);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color MediumVioletRed that has an ARGB value of #FFC71585.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle MediumVioletRed
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("MediumVioletRed", out style)) return style;
                _styles["MediumVioletRed"] = style = new SolidColourStyle(Colour.MediumVioletRed);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color MidnightBlue that has an ARGB value of #FF191970.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle MidnightBlue
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("MidnightBlue", out style)) return style;
                _styles["MidnightBlue"] = style = new SolidColourStyle(Colour.MidnightBlue);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color MintCream that has an ARGB value of #FFF5FFFA.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle MintCream
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("MintCream", out style)) return style;
                _styles["MintCream"] = style = new SolidColourStyle(Colour.MintCream);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color MistyRose that has an ARGB value of #FFFFE4E1.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle MistyRose
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("MistyRose", out style)) return style;
                _styles["MistyRose"] = style = new SolidColourStyle(Colour.MistyRose);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color Moccasin that has an ARGB value of #FFFFE4B5.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle Moccasin
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("Moccasin", out style)) return style;
                _styles["Moccasin"] = style = new SolidColourStyle(Colour.Moccasin);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color NavajoWhite that has an ARGB value of #FFFFDEAD.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle NavajoWhite
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("NavajoWhite", out style)) return style;
                _styles["NavajoWhite"] = style = new SolidColourStyle(Colour.NavajoWhite);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color Navy that has an ARGB value of #FF000080.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle Navy
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("Navy", out style)) return style;
                _styles["Navy"] = style = new SolidColourStyle(Colour.Navy);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color OldLace that has an ARGB value of #FFFDF5E6.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle OldLace
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("OldLace", out style)) return style;
                _styles["OldLace"] = style = new SolidColourStyle(Colour.OldLace);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color Olive that has an ARGB value of #FF808000.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle Olive
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("Olive", out style)) return style;
                _styles["Olive"] = style = new SolidColourStyle(Colour.Olive);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color OliveDrab that has an ARGB value of #FF6B8E23.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle OliveDrab
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("OliveDrab", out style)) return style;
                _styles["OliveDrab"] = style = new SolidColourStyle(Colour.OliveDrab);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color Orange that has an ARGB value of #FFFFA500.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle Orange
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("Orange", out style)) return style;
                _styles["Orange"] = style = new SolidColourStyle(Colour.Orange);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color OrangeRed that has an ARGB value of #FFFF4500.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle OrangeRed
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("OrangeRed", out style)) return style;
                _styles["OrangeRed"] = style = new SolidColourStyle(Colour.OrangeRed);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color Orchid that has an ARGB value of #FFDA70D6.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle Orchid
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("Orchid", out style)) return style;
                _styles["Orchid"] = style = new SolidColourStyle(Colour.Orchid);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color PaleGoldenrod that has an ARGB value of #FFEEE8AA.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle PaleGoldenrod
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("PaleGoldenrod", out style)) return style;
                _styles["PaleGoldenrod"] = style = new SolidColourStyle(Colour.PaleGoldenrod);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color PaleGreen that has an ARGB value of #FF98FB98.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle PaleGreen
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("PaleGreen", out style)) return style;
                _styles["PaleGreen"] = style = new SolidColourStyle(Colour.PaleGreen);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color PaleTurquoise that has an ARGB value of #FFAFEEEE.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle PaleTurquoise
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("PaleTurquoise", out style)) return style;
                _styles["PaleTurquoise"] = style = new SolidColourStyle(Colour.PaleTurquoise);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color PaleVioletRed that has an ARGB value of #FFDB7093.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle PaleVioletRed
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("PaleVioletRed", out style)) return style;
                _styles["PaleVioletRed"] = style = new SolidColourStyle(Colour.PaleVioletRed);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color PapayaWhip that has an ARGB value of #FFFFEFD5.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle PapayaWhip
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("PapayaWhip", out style)) return style;
                _styles["PapayaWhip"] = style = new SolidColourStyle(Colour.PapayaWhip);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color PeachPuff that has an ARGB value of #FFFFDAB9.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle PeachPuff
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("PeachPuff", out style)) return style;
                _styles["PeachPuff"] = style = new SolidColourStyle(Colour.PeachPuff);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color Peru that has an ARGB value of #FFCD853F.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle Peru
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("Peru", out style)) return style;
                _styles["Peru"] = style = new SolidColourStyle(Colour.Peru);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color Pink that has an ARGB value of #FFFFC0CB.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle Pink
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("Pink", out style)) return style;
                _styles["Pink"] = style = new SolidColourStyle(Colour.Pink);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color Plum that has an ARGB value of #FFDDA0DD.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle Plum
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("Plum", out style)) return style;
                _styles["Plum"] = style = new SolidColourStyle(Colour.Plum);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color PowderBlue that has an ARGB value of #FFB0E0E6.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle PowderBlue
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("PowderBlue", out style)) return style;
                _styles["PowderBlue"] = style = new SolidColourStyle(Colour.PowderBlue);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color Purple that has an ARGB value of #FF800080.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle Purple
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("Purple", out style)) return style;
                _styles["Purple"] = style = new SolidColourStyle(Colour.Purple);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color Red that has an ARGB value of #FFFF0000.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle Red
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("Red", out style)) return style;
                _styles["Red"] = style = new SolidColourStyle(Colour.Red);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color RosyBrown that has an ARGB value of #FFBC8F8F.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle RosyBrown
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("RosyBrown", out style)) return style;
                _styles["RosyBrown"] = style = new SolidColourStyle(Colour.RosyBrown);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color RoyalBlue that has an ARGB value of #FF4169E1.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle RoyalBlue
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("RoyalBlue", out style)) return style;
                _styles["RoyalBlue"] = style = new SolidColourStyle(Colour.RoyalBlue);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color SaddleBrown that has an ARGB value of #FF8B4513.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle SaddleBrown
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("SaddleBrown", out style)) return style;
                _styles["SaddleBrown"] = style = new SolidColourStyle(Colour.SaddleBrown);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color Salmon that has an ARGB value of #FFFA8072.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle Salmon
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("Salmon", out style)) return style;
                _styles["Salmon"] = style = new SolidColourStyle(Colour.Salmon);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color SandyBrown that has an ARGB value of #FFF4A460.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle SandyBrown
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("SandyBrown", out style)) return style;
                _styles["SandyBrown"] = style = new SolidColourStyle(Colour.SandyBrown);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color SeaGreen that has an ARGB value of #FF2E8B57.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle SeaGreen
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("SeaGreen", out style)) return style;
                _styles["SeaGreen"] = style = new SolidColourStyle(Colour.SeaGreen);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color SeaShell that has an ARGB value of #FFFFF5EE.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle SeaShell
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("SeaShell", out style)) return style;
                _styles["SeaShell"] = style = new SolidColourStyle(Colour.SeaShell);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color Sienna that has an ARGB value of #FFA0522D.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle Sienna
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("Sienna", out style)) return style;
                _styles["Sienna"] = style = new SolidColourStyle(Colour.Sienna);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color Silver that has an ARGB value of #FFC0C0C0.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle Silver
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("Silver", out style)) return style;
                _styles["Silver"] = style = new SolidColourStyle(Colour.Silver);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color SkyBlue that has an ARGB value of #FF87CEEB.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle SkyBlue
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("SkyBlue", out style)) return style;
                _styles["SkyBlue"] = style = new SolidColourStyle(Colour.SkyBlue);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color SlateBlue that has an ARGB value of #FF6A5ACD.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle SlateBlue
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("SlateBlue", out style)) return style;
                _styles["SlateBlue"] = style = new SolidColourStyle(Colour.SlateBlue);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color SlateGray that has an ARGB value of #FF708090.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle SlateGray
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("SlateGray", out style)) return style;
                _styles["SlateGray"] = style = new SolidColourStyle(Colour.SlateGray);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color Snow that has an ARGB value of #FFFFFAFA.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle Snow
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("Snow", out style)) return style;
                _styles["Snow"] = style = new SolidColourStyle(Colour.Snow);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color SpringGreen that has an ARGB value of #FF00FF7F.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle SpringGreen
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("SpringGreen", out style)) return style;
                _styles["SpringGreen"] = style = new SolidColourStyle(Colour.SpringGreen);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color SteelBlue that has an ARGB value of #FF4682B4.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle SteelBlue
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("SteelBlue", out style)) return style;
                _styles["SteelBlue"] = style = new SolidColourStyle(Colour.SteelBlue);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color Tan that has an ARGB value of #FFD2B48C.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle Tan
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("Tan", out style)) return style;
                _styles["Tan"] = style = new SolidColourStyle(Colour.Tan);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color Teal that has an ARGB value of #FF008080.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle Teal
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("Teal", out style)) return style;
                _styles["Teal"] = style = new SolidColourStyle(Colour.Teal);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color Thistle that has an ARGB value of #FFD8BFD8.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle Thistle
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("Thistle", out style)) return style;
                _styles["Thistle"] = style = new SolidColourStyle(Colour.Thistle);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color Tomato that has an ARGB value of #FFFF6347.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle Tomato
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("Tomato", out style)) return style;
                _styles["Tomato"] = style = new SolidColourStyle(Colour.Tomato);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color Turquoise that has an ARGB value of #FF40E0D0.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle Turquoise
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("Turquoise", out style)) return style;
                _styles["Turquoise"] = style = new SolidColourStyle(Colour.Turquoise);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color Violet that has an ARGB value of #FFEE82EE.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle Violet
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("Violet", out style)) return style;
                _styles["Violet"] = style = new SolidColourStyle(Colour.Violet);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color Wheat that has an ARGB value of #FFF5DEB3.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle Wheat
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("Wheat", out style)) return style;
                _styles["Wheat"] = style = new SolidColourStyle(Colour.Wheat);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color White that has an ARGB value of #FFFFFFFF.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle White
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("White", out style)) return style;
                _styles["White"] = style = new SolidColourStyle(Colour.White);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color WhiteSmoke that has an ARGB value of #FFF5F5F5.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle WhiteSmoke
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("WhiteSmoke", out style)) return style;
                _styles["WhiteSmoke"] = style = new SolidColourStyle(Colour.WhiteSmoke);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color Yellow that has an ARGB value of #FFFFFF00.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle Yellow
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("Yellow", out style)) return style;
                _styles["Yellow"] = style = new SolidColourStyle(Colour.Yellow);
                return style;
            }
        }

        /// <summary>
        ///     Gets the <see cref="SolidColourStyle"/> for the system-defined color YellowGreen that has an ARGB value of #FF9ACD32.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public static SolidColourStyle YellowGreen
        {
            get
            {
                SolidColourStyle style;
                if (_styles.TryGetValue("YellowGreen", out style)) return style;
                _styles["YellowGreen"] = style = new SolidColourStyle(Colour.YellowGreen);
                return style;
            }
        }

    }
}