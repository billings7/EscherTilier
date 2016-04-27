using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace EscherTiler.Styles
{
    public partial struct Colour
    {
        /// <summary>
        ///     The system-defined color Transparent that has an ARGB value of #00FFFFFF.
        /// </summary>
        [PublicAPI]
        public static readonly Colour Transparent = new Colour(1f, 1f, 1f, 0f);

        /// <summary>
        ///     The system-defined color AliceBlue that has an ARGB value of #FFF0F8FF.
        /// </summary>
        [PublicAPI]
        public static readonly Colour AliceBlue = new Colour(0.9411765f, 0.972549f, 1f);

        /// <summary>
        ///     The system-defined color AntiqueWhite that has an ARGB value of #FFFAEBD7.
        /// </summary>
        [PublicAPI]
        public static readonly Colour AntiqueWhite = new Colour(0.980392158f, 0.921568632f, 0.843137264f);

        /// <summary>
        ///     The system-defined color Aqua that has an ARGB value of #FF00FFFF.
        /// </summary>
        [PublicAPI]
        public static readonly Colour Aqua = new Colour(0f, 1f, 1f);

        /// <summary>
        ///     The system-defined color Aquamarine that has an ARGB value of #FF7FFFD4.
        /// </summary>
        [PublicAPI]
        public static readonly Colour Aquamarine = new Colour(0.498039216f, 1f, 0.831372559f);

        /// <summary>
        ///     The system-defined color Azure that has an ARGB value of #FFF0FFFF.
        /// </summary>
        [PublicAPI]
        public static readonly Colour Azure = new Colour(0.9411765f, 1f, 1f);

        /// <summary>
        ///     The system-defined color Beige that has an ARGB value of #FFF5F5DC.
        /// </summary>
        [PublicAPI]
        public static readonly Colour Beige = new Colour(0.9607843f, 0.9607843f, 0.8627451f);

        /// <summary>
        ///     The system-defined color Bisque that has an ARGB value of #FFFFE4C4.
        /// </summary>
        [PublicAPI]
        public static readonly Colour Bisque = new Colour(1f, 0.894117653f, 0.768627465f);

        /// <summary>
        ///     The system-defined color Black that has an ARGB value of #FF000000.
        /// </summary>
        [PublicAPI]
        public static readonly Colour Black = new Colour(0f, 0f, 0f);

        /// <summary>
        ///     The system-defined color BlanchedAlmond that has an ARGB value of #FFFFEBCD.
        /// </summary>
        [PublicAPI]
        public static readonly Colour BlanchedAlmond = new Colour(1f, 0.921568632f, 0.8039216f);

        /// <summary>
        ///     The system-defined color Blue that has an ARGB value of #FF0000FF.
        /// </summary>
        [PublicAPI]
        public static readonly Colour Blue = new Colour(0f, 0f, 1f);

        /// <summary>
        ///     The system-defined color BlueViolet that has an ARGB value of #FF8A2BE2.
        /// </summary>
        [PublicAPI]
        public static readonly Colour BlueViolet = new Colour(0.5411765f, 0.168627456f, 0.8862745f);

        /// <summary>
        ///     The system-defined color Brown that has an ARGB value of #FFA52A2A.
        /// </summary>
        [PublicAPI]
        public static readonly Colour Brown = new Colour(0.647058845f, 0.164705887f, 0.164705887f);

        /// <summary>
        ///     The system-defined color BurlyWood that has an ARGB value of #FFDEB887.
        /// </summary>
        [PublicAPI]
        public static readonly Colour BurlyWood = new Colour(0.870588243f, 0.721568644f, 0.5294118f);

        /// <summary>
        ///     The system-defined color CadetBlue that has an ARGB value of #FF5F9EA0.
        /// </summary>
        [PublicAPI]
        public static readonly Colour CadetBlue = new Colour(0.372549027f, 0.619607866f, 0.627451f);

        /// <summary>
        ///     The system-defined color Chartreuse that has an ARGB value of #FF7FFF00.
        /// </summary>
        [PublicAPI]
        public static readonly Colour Chartreuse = new Colour(0.498039216f, 1f, 0f);

        /// <summary>
        ///     The system-defined color Chocolate that has an ARGB value of #FFD2691E.
        /// </summary>
        [PublicAPI]
        public static readonly Colour Chocolate = new Colour(0.8235294f, 0.4117647f, 0.117647059f);

        /// <summary>
        ///     The system-defined color Coral that has an ARGB value of #FFFF7F50.
        /// </summary>
        [PublicAPI]
        public static readonly Colour Coral = new Colour(1f, 0.498039216f, 0.3137255f);

        /// <summary>
        ///     The system-defined color CornflowerBlue that has an ARGB value of #FF6495ED.
        /// </summary>
        [PublicAPI]
        public static readonly Colour CornflowerBlue = new Colour(0.392156869f, 0.58431375f, 0.929411769f);

        /// <summary>
        ///     The system-defined color Cornsilk that has an ARGB value of #FFFFF8DC.
        /// </summary>
        [PublicAPI]
        public static readonly Colour Cornsilk = new Colour(1f, 0.972549f, 0.8627451f);

        /// <summary>
        ///     The system-defined color Crimson that has an ARGB value of #FFDC143C.
        /// </summary>
        [PublicAPI]
        public static readonly Colour Crimson = new Colour(0.8627451f, 0.0784313753f, 0.235294119f);

        /// <summary>
        ///     The system-defined color Cyan that has an ARGB value of #FF00FFFF.
        /// </summary>
        [PublicAPI]
        public static readonly Colour Cyan = new Colour(0f, 1f, 1f);

        /// <summary>
        ///     The system-defined color DarkBlue that has an ARGB value of #FF00008B.
        /// </summary>
        [PublicAPI]
        public static readonly Colour DarkBlue = new Colour(0f, 0f, 0.545098066f);

        /// <summary>
        ///     The system-defined color DarkCyan that has an ARGB value of #FF008B8B.
        /// </summary>
        [PublicAPI]
        public static readonly Colour DarkCyan = new Colour(0f, 0.545098066f, 0.545098066f);

        /// <summary>
        ///     The system-defined color DarkGoldenrod that has an ARGB value of #FFB8860B.
        /// </summary>
        [PublicAPI]
        public static readonly Colour DarkGoldenrod = new Colour(0.721568644f, 0.5254902f, 0.0431372561f);

        /// <summary>
        ///     The system-defined color DarkGray that has an ARGB value of #FFA9A9A9.
        /// </summary>
        [PublicAPI]
        public static readonly Colour DarkGray = new Colour(0.6627451f, 0.6627451f, 0.6627451f);

        /// <summary>
        ///     The system-defined color DarkGreen that has an ARGB value of #FF006400.
        /// </summary>
        [PublicAPI]
        public static readonly Colour DarkGreen = new Colour(0f, 0.392156869f, 0f);

        /// <summary>
        ///     The system-defined color DarkKhaki that has an ARGB value of #FFBDB76B.
        /// </summary>
        [PublicAPI]
        public static readonly Colour DarkKhaki = new Colour(0.7411765f, 0.7176471f, 0.419607848f);

        /// <summary>
        ///     The system-defined color DarkMagenta that has an ARGB value of #FF8B008B.
        /// </summary>
        [PublicAPI]
        public static readonly Colour DarkMagenta = new Colour(0.545098066f, 0f, 0.545098066f);

        /// <summary>
        ///     The system-defined color DarkOliveGreen that has an ARGB value of #FF556B2F.
        /// </summary>
        [PublicAPI]
        public static readonly Colour DarkOliveGreen = new Colour(0.333333343f, 0.419607848f, 0.184313729f);

        /// <summary>
        ///     The system-defined color DarkOrange that has an ARGB value of #FFFF8C00.
        /// </summary>
        [PublicAPI]
        public static readonly Colour DarkOrange = new Colour(1f, 0.549019635f, 0f);

        /// <summary>
        ///     The system-defined color DarkOrchid that has an ARGB value of #FF9932CC.
        /// </summary>
        [PublicAPI]
        public static readonly Colour DarkOrchid = new Colour(0.6f, 0.196078435f, 0.8f);

        /// <summary>
        ///     The system-defined color DarkRed that has an ARGB value of #FF8B0000.
        /// </summary>
        [PublicAPI]
        public static readonly Colour DarkRed = new Colour(0.545098066f, 0f, 0f);

        /// <summary>
        ///     The system-defined color DarkSalmon that has an ARGB value of #FFE9967A.
        /// </summary>
        [PublicAPI]
        public static readonly Colour DarkSalmon = new Colour(0.9137255f, 0.5882353f, 0.478431374f);

        /// <summary>
        ///     The system-defined color DarkSeaGreen that has an ARGB value of #FF8FBC8B.
        /// </summary>
        [PublicAPI]
        public static readonly Colour DarkSeaGreen = new Colour(0.56078434f, 0.7372549f, 0.545098066f);

        /// <summary>
        ///     The system-defined color DarkSlateBlue that has an ARGB value of #FF483D8B.
        /// </summary>
        [PublicAPI]
        public static readonly Colour DarkSlateBlue = new Colour(0.282352954f, 0.239215687f, 0.545098066f);

        /// <summary>
        ///     The system-defined color DarkSlateGray that has an ARGB value of #FF2F4F4F.
        /// </summary>
        [PublicAPI]
        public static readonly Colour DarkSlateGray = new Colour(0.184313729f, 0.309803933f, 0.309803933f);

        /// <summary>
        ///     The system-defined color DarkTurquoise that has an ARGB value of #FF00CED1.
        /// </summary>
        [PublicAPI]
        public static readonly Colour DarkTurquoise = new Colour(0f, 0.807843149f, 0.819607854f);

        /// <summary>
        ///     The system-defined color DarkViolet that has an ARGB value of #FF9400D3.
        /// </summary>
        [PublicAPI]
        public static readonly Colour DarkViolet = new Colour(0.5803922f, 0f, 0.827451f);

        /// <summary>
        ///     The system-defined color DeepPink that has an ARGB value of #FFFF1493.
        /// </summary>
        [PublicAPI]
        public static readonly Colour DeepPink = new Colour(1f, 0.0784313753f, 0.5764706f);

        /// <summary>
        ///     The system-defined color DeepSkyBlue that has an ARGB value of #FF00BFFF.
        /// </summary>
        [PublicAPI]
        public static readonly Colour DeepSkyBlue = new Colour(0f, 0.7490196f, 1f);

        /// <summary>
        ///     The system-defined color DimGray that has an ARGB value of #FF696969.
        /// </summary>
        [PublicAPI]
        public static readonly Colour DimGray = new Colour(0.4117647f, 0.4117647f, 0.4117647f);

        /// <summary>
        ///     The system-defined color DodgerBlue that has an ARGB value of #FF1E90FF.
        /// </summary>
        [PublicAPI]
        public static readonly Colour DodgerBlue = new Colour(0.117647059f, 0.5647059f, 1f);

        /// <summary>
        ///     The system-defined color Firebrick that has an ARGB value of #FFB22222.
        /// </summary>
        [PublicAPI]
        public static readonly Colour Firebrick = new Colour(0.698039234f, 0.13333334f, 0.13333334f);

        /// <summary>
        ///     The system-defined color FloralWhite that has an ARGB value of #FFFFFAF0.
        /// </summary>
        [PublicAPI]
        public static readonly Colour FloralWhite = new Colour(1f, 0.980392158f, 0.9411765f);

        /// <summary>
        ///     The system-defined color ForestGreen that has an ARGB value of #FF228B22.
        /// </summary>
        [PublicAPI]
        public static readonly Colour ForestGreen = new Colour(0.13333334f, 0.545098066f, 0.13333334f);

        /// <summary>
        ///     The system-defined color Fuchsia that has an ARGB value of #FFFF00FF.
        /// </summary>
        [PublicAPI]
        public static readonly Colour Fuchsia = new Colour(1f, 0f, 1f);

        /// <summary>
        ///     The system-defined color Gainsboro that has an ARGB value of #FFDCDCDC.
        /// </summary>
        [PublicAPI]
        public static readonly Colour Gainsboro = new Colour(0.8627451f, 0.8627451f, 0.8627451f);

        /// <summary>
        ///     The system-defined color GhostWhite that has an ARGB value of #FFF8F8FF.
        /// </summary>
        [PublicAPI]
        public static readonly Colour GhostWhite = new Colour(0.972549f, 0.972549f, 1f);

        /// <summary>
        ///     The system-defined color Gold that has an ARGB value of #FFFFD700.
        /// </summary>
        [PublicAPI]
        public static readonly Colour Gold = new Colour(1f, 0.843137264f, 0f);

        /// <summary>
        ///     The system-defined color Goldenrod that has an ARGB value of #FFDAA520.
        /// </summary>
        [PublicAPI]
        public static readonly Colour Goldenrod = new Colour(0.854901969f, 0.647058845f, 0.1254902f);

        /// <summary>
        ///     The system-defined color Gray that has an ARGB value of #FF808080.
        /// </summary>
        [PublicAPI]
        public static readonly Colour Gray = new Colour(0.5019608f, 0.5019608f, 0.5019608f);

        /// <summary>
        ///     The system-defined color Green that has an ARGB value of #FF008000.
        /// </summary>
        [PublicAPI]
        public static readonly Colour Green = new Colour(0f, 0.5019608f, 0f);

        /// <summary>
        ///     The system-defined color GreenYellow that has an ARGB value of #FFADFF2F.
        /// </summary>
        [PublicAPI]
        public static readonly Colour GreenYellow = new Colour(0.6784314f, 1f, 0.184313729f);

        /// <summary>
        ///     The system-defined color Honeydew that has an ARGB value of #FFF0FFF0.
        /// </summary>
        [PublicAPI]
        public static readonly Colour Honeydew = new Colour(0.9411765f, 1f, 0.9411765f);

        /// <summary>
        ///     The system-defined color HotPink that has an ARGB value of #FFFF69B4.
        /// </summary>
        [PublicAPI]
        public static readonly Colour HotPink = new Colour(1f, 0.4117647f, 0.7058824f);

        /// <summary>
        ///     The system-defined color IndianRed that has an ARGB value of #FFCD5C5C.
        /// </summary>
        [PublicAPI]
        public static readonly Colour IndianRed = new Colour(0.8039216f, 0.360784322f, 0.360784322f);

        /// <summary>
        ///     The system-defined color Indigo that has an ARGB value of #FF4B0082.
        /// </summary>
        [PublicAPI]
        public static readonly Colour Indigo = new Colour(0.294117659f, 0f, 0.509803951f);

        /// <summary>
        ///     The system-defined color Ivory that has an ARGB value of #FFFFFFF0.
        /// </summary>
        [PublicAPI]
        public static readonly Colour Ivory = new Colour(1f, 1f, 0.9411765f);

        /// <summary>
        ///     The system-defined color Khaki that has an ARGB value of #FFF0E68C.
        /// </summary>
        [PublicAPI]
        public static readonly Colour Khaki = new Colour(0.9411765f, 0.9019608f, 0.549019635f);

        /// <summary>
        ///     The system-defined color Lavender that has an ARGB value of #FFE6E6FA.
        /// </summary>
        [PublicAPI]
        public static readonly Colour Lavender = new Colour(0.9019608f, 0.9019608f, 0.980392158f);

        /// <summary>
        ///     The system-defined color LavenderBlush that has an ARGB value of #FFFFF0F5.
        /// </summary>
        [PublicAPI]
        public static readonly Colour LavenderBlush = new Colour(1f, 0.9411765f, 0.9607843f);

        /// <summary>
        ///     The system-defined color LawnGreen that has an ARGB value of #FF7CFC00.
        /// </summary>
        [PublicAPI]
        public static readonly Colour LawnGreen = new Colour(0.4862745f, 0.9882353f, 0f);

        /// <summary>
        ///     The system-defined color LemonChiffon that has an ARGB value of #FFFFFACD.
        /// </summary>
        [PublicAPI]
        public static readonly Colour LemonChiffon = new Colour(1f, 0.980392158f, 0.8039216f);

        /// <summary>
        ///     The system-defined color LightBlue that has an ARGB value of #FFADD8E6.
        /// </summary>
        [PublicAPI]
        public static readonly Colour LightBlue = new Colour(0.6784314f, 0.847058833f, 0.9019608f);

        /// <summary>
        ///     The system-defined color LightCoral that has an ARGB value of #FFF08080.
        /// </summary>
        [PublicAPI]
        public static readonly Colour LightCoral = new Colour(0.9411765f, 0.5019608f, 0.5019608f);

        /// <summary>
        ///     The system-defined color LightCyan that has an ARGB value of #FFE0FFFF.
        /// </summary>
        [PublicAPI]
        public static readonly Colour LightCyan = new Colour(0.8784314f, 1f, 1f);

        /// <summary>
        ///     The system-defined color LightGoldenrodYellow that has an ARGB value of #FFFAFAD2.
        /// </summary>
        [PublicAPI]
        public static readonly Colour LightGoldenrodYellow = new Colour(0.980392158f, 0.980392158f, 0.8235294f);

        /// <summary>
        ///     The system-defined color LightGreen that has an ARGB value of #FF90EE90.
        /// </summary>
        [PublicAPI]
        public static readonly Colour LightGreen = new Colour(0.5647059f, 0.933333337f, 0.5647059f);

        /// <summary>
        ///     The system-defined color LightGray that has an ARGB value of #FFD3D3D3.
        /// </summary>
        [PublicAPI]
        public static readonly Colour LightGray = new Colour(0.827451f, 0.827451f, 0.827451f);

        /// <summary>
        ///     The system-defined color LightPink that has an ARGB value of #FFFFB6C1.
        /// </summary>
        [PublicAPI]
        public static readonly Colour LightPink = new Colour(1f, 0.7137255f, 0.75686276f);

        /// <summary>
        ///     The system-defined color LightSalmon that has an ARGB value of #FFFFA07A.
        /// </summary>
        [PublicAPI]
        public static readonly Colour LightSalmon = new Colour(1f, 0.627451f, 0.478431374f);

        /// <summary>
        ///     The system-defined color LightSeaGreen that has an ARGB value of #FF20B2AA.
        /// </summary>
        [PublicAPI]
        public static readonly Colour LightSeaGreen = new Colour(0.1254902f, 0.698039234f, 0.6666667f);

        /// <summary>
        ///     The system-defined color LightSkyBlue that has an ARGB value of #FF87CEFA.
        /// </summary>
        [PublicAPI]
        public static readonly Colour LightSkyBlue = new Colour(0.5294118f, 0.807843149f, 0.980392158f);

        /// <summary>
        ///     The system-defined color LightSlateGray that has an ARGB value of #FF778899.
        /// </summary>
        [PublicAPI]
        public static readonly Colour LightSlateGray = new Colour(0.466666669f, 0.533333361f, 0.6f);

        /// <summary>
        ///     The system-defined color LightSteelBlue that has an ARGB value of #FFB0C4DE.
        /// </summary>
        [PublicAPI]
        public static readonly Colour LightSteelBlue = new Colour(0.6901961f, 0.768627465f, 0.870588243f);

        /// <summary>
        ///     The system-defined color LightYellow that has an ARGB value of #FFFFFFE0.
        /// </summary>
        [PublicAPI]
        public static readonly Colour LightYellow = new Colour(1f, 1f, 0.8784314f);

        /// <summary>
        ///     The system-defined color Lime that has an ARGB value of #FF00FF00.
        /// </summary>
        [PublicAPI]
        public static readonly Colour Lime = new Colour(0f, 1f, 0f);

        /// <summary>
        ///     The system-defined color LimeGreen that has an ARGB value of #FF32CD32.
        /// </summary>
        [PublicAPI]
        public static readonly Colour LimeGreen = new Colour(0.196078435f, 0.8039216f, 0.196078435f);

        /// <summary>
        ///     The system-defined color Linen that has an ARGB value of #FFFAF0E6.
        /// </summary>
        [PublicAPI]
        public static readonly Colour Linen = new Colour(0.980392158f, 0.9411765f, 0.9019608f);

        /// <summary>
        ///     The system-defined color Magenta that has an ARGB value of #FFFF00FF.
        /// </summary>
        [PublicAPI]
        public static readonly Colour Magenta = new Colour(1f, 0f, 1f);

        /// <summary>
        ///     The system-defined color Maroon that has an ARGB value of #FF800000.
        /// </summary>
        [PublicAPI]
        public static readonly Colour Maroon = new Colour(0.5019608f, 0f, 0f);

        /// <summary>
        ///     The system-defined color MediumAquamarine that has an ARGB value of #FF66CDAA.
        /// </summary>
        [PublicAPI]
        public static readonly Colour MediumAquamarine = new Colour(0.4f, 0.8039216f, 0.6666667f);

        /// <summary>
        ///     The system-defined color MediumBlue that has an ARGB value of #FF0000CD.
        /// </summary>
        [PublicAPI]
        public static readonly Colour MediumBlue = new Colour(0f, 0f, 0.8039216f);

        /// <summary>
        ///     The system-defined color MediumOrchid that has an ARGB value of #FFBA55D3.
        /// </summary>
        [PublicAPI]
        public static readonly Colour MediumOrchid = new Colour(0.7294118f, 0.333333343f, 0.827451f);

        /// <summary>
        ///     The system-defined color MediumPurple that has an ARGB value of #FF9370DB.
        /// </summary>
        [PublicAPI]
        public static readonly Colour MediumPurple = new Colour(0.5764706f, 0.4392157f, 0.858823538f);

        /// <summary>
        ///     The system-defined color MediumSeaGreen that has an ARGB value of #FF3CB371.
        /// </summary>
        [PublicAPI]
        public static readonly Colour MediumSeaGreen = new Colour(0.235294119f, 0.7019608f, 0.443137258f);

        /// <summary>
        ///     The system-defined color MediumSlateBlue that has an ARGB value of #FF7B68EE.
        /// </summary>
        [PublicAPI]
        public static readonly Colour MediumSlateBlue = new Colour(0.482352942f, 0.407843143f, 0.933333337f);

        /// <summary>
        ///     The system-defined color MediumSpringGreen that has an ARGB value of #FF00FA9A.
        /// </summary>
        [PublicAPI]
        public static readonly Colour MediumSpringGreen = new Colour(0f, 0.980392158f, 0.6039216f);

        /// <summary>
        ///     The system-defined color MediumTurquoise that has an ARGB value of #FF48D1CC.
        /// </summary>
        [PublicAPI]
        public static readonly Colour MediumTurquoise = new Colour(0.282352954f, 0.819607854f, 0.8f);

        /// <summary>
        ///     The system-defined color MediumVioletRed that has an ARGB value of #FFC71585.
        /// </summary>
        [PublicAPI]
        public static readonly Colour MediumVioletRed = new Colour(0.78039217f, 0.08235294f, 0.521568656f);

        /// <summary>
        ///     The system-defined color MidnightBlue that has an ARGB value of #FF191970.
        /// </summary>
        [PublicAPI]
        public static readonly Colour MidnightBlue = new Colour(0.09803922f, 0.09803922f, 0.4392157f);

        /// <summary>
        ///     The system-defined color MintCream that has an ARGB value of #FFF5FFFA.
        /// </summary>
        [PublicAPI]
        public static readonly Colour MintCream = new Colour(0.9607843f, 1f, 0.980392158f);

        /// <summary>
        ///     The system-defined color MistyRose that has an ARGB value of #FFFFE4E1.
        /// </summary>
        [PublicAPI]
        public static readonly Colour MistyRose = new Colour(1f, 0.894117653f, 0.882352948f);

        /// <summary>
        ///     The system-defined color Moccasin that has an ARGB value of #FFFFE4B5.
        /// </summary>
        [PublicAPI]
        public static readonly Colour Moccasin = new Colour(1f, 0.894117653f, 0.709803939f);

        /// <summary>
        ///     The system-defined color NavajoWhite that has an ARGB value of #FFFFDEAD.
        /// </summary>
        [PublicAPI]
        public static readonly Colour NavajoWhite = new Colour(1f, 0.870588243f, 0.6784314f);

        /// <summary>
        ///     The system-defined color Navy that has an ARGB value of #FF000080.
        /// </summary>
        [PublicAPI]
        public static readonly Colour Navy = new Colour(0f, 0f, 0.5019608f);

        /// <summary>
        ///     The system-defined color OldLace that has an ARGB value of #FFFDF5E6.
        /// </summary>
        [PublicAPI]
        public static readonly Colour OldLace = new Colour(0.992156863f, 0.9607843f, 0.9019608f);

        /// <summary>
        ///     The system-defined color Olive that has an ARGB value of #FF808000.
        /// </summary>
        [PublicAPI]
        public static readonly Colour Olive = new Colour(0.5019608f, 0.5019608f, 0f);

        /// <summary>
        ///     The system-defined color OliveDrab that has an ARGB value of #FF6B8E23.
        /// </summary>
        [PublicAPI]
        public static readonly Colour OliveDrab = new Colour(0.419607848f, 0.5568628f, 0.137254909f);

        /// <summary>
        ///     The system-defined color Orange that has an ARGB value of #FFFFA500.
        /// </summary>
        [PublicAPI]
        public static readonly Colour Orange = new Colour(1f, 0.647058845f, 0f);

        /// <summary>
        ///     The system-defined color OrangeRed that has an ARGB value of #FFFF4500.
        /// </summary>
        [PublicAPI]
        public static readonly Colour OrangeRed = new Colour(1f, 0.270588249f, 0f);

        /// <summary>
        ///     The system-defined color Orchid that has an ARGB value of #FFDA70D6.
        /// </summary>
        [PublicAPI]
        public static readonly Colour Orchid = new Colour(0.854901969f, 0.4392157f, 0.8392157f);

        /// <summary>
        ///     The system-defined color PaleGoldenrod that has an ARGB value of #FFEEE8AA.
        /// </summary>
        [PublicAPI]
        public static readonly Colour PaleGoldenrod = new Colour(0.933333337f, 0.9098039f, 0.6666667f);

        /// <summary>
        ///     The system-defined color PaleGreen that has an ARGB value of #FF98FB98.
        /// </summary>
        [PublicAPI]
        public static readonly Colour PaleGreen = new Colour(0.596078455f, 0.9843137f, 0.596078455f);

        /// <summary>
        ///     The system-defined color PaleTurquoise that has an ARGB value of #FFAFEEEE.
        /// </summary>
        [PublicAPI]
        public static readonly Colour PaleTurquoise = new Colour(0.6862745f, 0.933333337f, 0.933333337f);

        /// <summary>
        ///     The system-defined color PaleVioletRed that has an ARGB value of #FFDB7093.
        /// </summary>
        [PublicAPI]
        public static readonly Colour PaleVioletRed = new Colour(0.858823538f, 0.4392157f, 0.5764706f);

        /// <summary>
        ///     The system-defined color PapayaWhip that has an ARGB value of #FFFFEFD5.
        /// </summary>
        [PublicAPI]
        public static readonly Colour PapayaWhip = new Colour(1f, 0.9372549f, 0.8352941f);

        /// <summary>
        ///     The system-defined color PeachPuff that has an ARGB value of #FFFFDAB9.
        /// </summary>
        [PublicAPI]
        public static readonly Colour PeachPuff = new Colour(1f, 0.854901969f, 0.7254902f);

        /// <summary>
        ///     The system-defined color Peru that has an ARGB value of #FFCD853F.
        /// </summary>
        [PublicAPI]
        public static readonly Colour Peru = new Colour(0.8039216f, 0.521568656f, 0.247058824f);

        /// <summary>
        ///     The system-defined color Pink that has an ARGB value of #FFFFC0CB.
        /// </summary>
        [PublicAPI]
        public static readonly Colour Pink = new Colour(1f, 0.7529412f, 0.796078444f);

        /// <summary>
        ///     The system-defined color Plum that has an ARGB value of #FFDDA0DD.
        /// </summary>
        [PublicAPI]
        public static readonly Colour Plum = new Colour(0.8666667f, 0.627451f, 0.8666667f);

        /// <summary>
        ///     The system-defined color PowderBlue that has an ARGB value of #FFB0E0E6.
        /// </summary>
        [PublicAPI]
        public static readonly Colour PowderBlue = new Colour(0.6901961f, 0.8784314f, 0.9019608f);

        /// <summary>
        ///     The system-defined color Purple that has an ARGB value of #FF800080.
        /// </summary>
        [PublicAPI]
        public static readonly Colour Purple = new Colour(0.5019608f, 0f, 0.5019608f);

        /// <summary>
        ///     The system-defined color Red that has an ARGB value of #FFFF0000.
        /// </summary>
        [PublicAPI]
        public static readonly Colour Red = new Colour(1f, 0f, 0f);

        /// <summary>
        ///     The system-defined color RosyBrown that has an ARGB value of #FFBC8F8F.
        /// </summary>
        [PublicAPI]
        public static readonly Colour RosyBrown = new Colour(0.7372549f, 0.56078434f, 0.56078434f);

        /// <summary>
        ///     The system-defined color RoyalBlue that has an ARGB value of #FF4169E1.
        /// </summary>
        [PublicAPI]
        public static readonly Colour RoyalBlue = new Colour(0.254901975f, 0.4117647f, 0.882352948f);

        /// <summary>
        ///     The system-defined color SaddleBrown that has an ARGB value of #FF8B4513.
        /// </summary>
        [PublicAPI]
        public static readonly Colour SaddleBrown = new Colour(0.545098066f, 0.270588249f, 0.07450981f);

        /// <summary>
        ///     The system-defined color Salmon that has an ARGB value of #FFFA8072.
        /// </summary>
        [PublicAPI]
        public static readonly Colour Salmon = new Colour(0.980392158f, 0.5019608f, 0.447058827f);

        /// <summary>
        ///     The system-defined color SandyBrown that has an ARGB value of #FFF4A460.
        /// </summary>
        [PublicAPI]
        public static readonly Colour SandyBrown = new Colour(0.956862748f, 0.6431373f, 0.3764706f);

        /// <summary>
        ///     The system-defined color SeaGreen that has an ARGB value of #FF2E8B57.
        /// </summary>
        [PublicAPI]
        public static readonly Colour SeaGreen = new Colour(0.180392161f, 0.545098066f, 0.34117648f);

        /// <summary>
        ///     The system-defined color SeaShell that has an ARGB value of #FFFFF5EE.
        /// </summary>
        [PublicAPI]
        public static readonly Colour SeaShell = new Colour(1f, 0.9607843f, 0.933333337f);

        /// <summary>
        ///     The system-defined color Sienna that has an ARGB value of #FFA0522D.
        /// </summary>
        [PublicAPI]
        public static readonly Colour Sienna = new Colour(0.627451f, 0.321568638f, 0.1764706f);

        /// <summary>
        ///     The system-defined color Silver that has an ARGB value of #FFC0C0C0.
        /// </summary>
        [PublicAPI]
        public static readonly Colour Silver = new Colour(0.7529412f, 0.7529412f, 0.7529412f);

        /// <summary>
        ///     The system-defined color SkyBlue that has an ARGB value of #FF87CEEB.
        /// </summary>
        [PublicAPI]
        public static readonly Colour SkyBlue = new Colour(0.5294118f, 0.807843149f, 0.921568632f);

        /// <summary>
        ///     The system-defined color SlateBlue that has an ARGB value of #FF6A5ACD.
        /// </summary>
        [PublicAPI]
        public static readonly Colour SlateBlue = new Colour(0.41568628f, 0.3529412f, 0.8039216f);

        /// <summary>
        ///     The system-defined color SlateGray that has an ARGB value of #FF708090.
        /// </summary>
        [PublicAPI]
        public static readonly Colour SlateGray = new Colour(0.4392157f, 0.5019608f, 0.5647059f);

        /// <summary>
        ///     The system-defined color Snow that has an ARGB value of #FFFFFAFA.
        /// </summary>
        [PublicAPI]
        public static readonly Colour Snow = new Colour(1f, 0.980392158f, 0.980392158f);

        /// <summary>
        ///     The system-defined color SpringGreen that has an ARGB value of #FF00FF7F.
        /// </summary>
        [PublicAPI]
        public static readonly Colour SpringGreen = new Colour(0f, 1f, 0.498039216f);

        /// <summary>
        ///     The system-defined color SteelBlue that has an ARGB value of #FF4682B4.
        /// </summary>
        [PublicAPI]
        public static readonly Colour SteelBlue = new Colour(0.274509817f, 0.509803951f, 0.7058824f);

        /// <summary>
        ///     The system-defined color Tan that has an ARGB value of #FFD2B48C.
        /// </summary>
        [PublicAPI]
        public static readonly Colour Tan = new Colour(0.8235294f, 0.7058824f, 0.549019635f);

        /// <summary>
        ///     The system-defined color Teal that has an ARGB value of #FF008080.
        /// </summary>
        [PublicAPI]
        public static readonly Colour Teal = new Colour(0f, 0.5019608f, 0.5019608f);

        /// <summary>
        ///     The system-defined color Thistle that has an ARGB value of #FFD8BFD8.
        /// </summary>
        [PublicAPI]
        public static readonly Colour Thistle = new Colour(0.847058833f, 0.7490196f, 0.847058833f);

        /// <summary>
        ///     The system-defined color Tomato that has an ARGB value of #FFFF6347.
        /// </summary>
        [PublicAPI]
        public static readonly Colour Tomato = new Colour(1f, 0.3882353f, 0.2784314f);

        /// <summary>
        ///     The system-defined color Turquoise that has an ARGB value of #FF40E0D0.
        /// </summary>
        [PublicAPI]
        public static readonly Colour Turquoise = new Colour(0.2509804f, 0.8784314f, 0.8156863f);

        /// <summary>
        ///     The system-defined color Violet that has an ARGB value of #FFEE82EE.
        /// </summary>
        [PublicAPI]
        public static readonly Colour Violet = new Colour(0.933333337f, 0.509803951f, 0.933333337f);

        /// <summary>
        ///     The system-defined color Wheat that has an ARGB value of #FFF5DEB3.
        /// </summary>
        [PublicAPI]
        public static readonly Colour Wheat = new Colour(0.9607843f, 0.870588243f, 0.7019608f);

        /// <summary>
        ///     The system-defined color White that has an ARGB value of #FFFFFFFF.
        /// </summary>
        [PublicAPI]
        public static readonly Colour White = new Colour(1f, 1f, 1f);

        /// <summary>
        ///     The system-defined color WhiteSmoke that has an ARGB value of #FFF5F5F5.
        /// </summary>
        [PublicAPI]
        public static readonly Colour WhiteSmoke = new Colour(0.9607843f, 0.9607843f, 0.9607843f);

        /// <summary>
        ///     The system-defined color Yellow that has an ARGB value of #FFFFFF00.
        /// </summary>
        [PublicAPI]
        public static readonly Colour Yellow = new Colour(1f, 1f, 0f);

        /// <summary>
        ///     The system-defined color YellowGreen that has an ARGB value of #FF9ACD32.
        /// </summary>
        [PublicAPI]
        public static readonly Colour YellowGreen = new Colour(0.6039216f, 0.8039216f, 0.196078435f);

        [NotNull]
        private static readonly Dictionary<string, Colour> _knownColours = new Dictionary<string, Colour>(StringComparer.InvariantCultureIgnoreCase)
        {
            { "Transparent", Transparent },
            { "AliceBlue", AliceBlue },
            { "AntiqueWhite", AntiqueWhite },
            { "Aqua", Aqua },
            { "Aquamarine", Aquamarine },
            { "Azure", Azure },
            { "Beige", Beige },
            { "Bisque", Bisque },
            { "Black", Black },
            { "BlanchedAlmond", BlanchedAlmond },
            { "Blue", Blue },
            { "BlueViolet", BlueViolet },
            { "Brown", Brown },
            { "BurlyWood", BurlyWood },
            { "CadetBlue", CadetBlue },
            { "Chartreuse", Chartreuse },
            { "Chocolate", Chocolate },
            { "Coral", Coral },
            { "CornflowerBlue", CornflowerBlue },
            { "Cornsilk", Cornsilk },
            { "Crimson", Crimson },
            { "Cyan", Cyan },
            { "DarkBlue", DarkBlue },
            { "DarkCyan", DarkCyan },
            { "DarkGoldenrod", DarkGoldenrod },
            { "DarkGray", DarkGray },
            { "DarkGreen", DarkGreen },
            { "DarkKhaki", DarkKhaki },
            { "DarkMagenta", DarkMagenta },
            { "DarkOliveGreen", DarkOliveGreen },
            { "DarkOrange", DarkOrange },
            { "DarkOrchid", DarkOrchid },
            { "DarkRed", DarkRed },
            { "DarkSalmon", DarkSalmon },
            { "DarkSeaGreen", DarkSeaGreen },
            { "DarkSlateBlue", DarkSlateBlue },
            { "DarkSlateGray", DarkSlateGray },
            { "DarkTurquoise", DarkTurquoise },
            { "DarkViolet", DarkViolet },
            { "DeepPink", DeepPink },
            { "DeepSkyBlue", DeepSkyBlue },
            { "DimGray", DimGray },
            { "DodgerBlue", DodgerBlue },
            { "Firebrick", Firebrick },
            { "FloralWhite", FloralWhite },
            { "ForestGreen", ForestGreen },
            { "Fuchsia", Fuchsia },
            { "Gainsboro", Gainsboro },
            { "GhostWhite", GhostWhite },
            { "Gold", Gold },
            { "Goldenrod", Goldenrod },
            { "Gray", Gray },
            { "Green", Green },
            { "GreenYellow", GreenYellow },
            { "Honeydew", Honeydew },
            { "HotPink", HotPink },
            { "IndianRed", IndianRed },
            { "Indigo", Indigo },
            { "Ivory", Ivory },
            { "Khaki", Khaki },
            { "Lavender", Lavender },
            { "LavenderBlush", LavenderBlush },
            { "LawnGreen", LawnGreen },
            { "LemonChiffon", LemonChiffon },
            { "LightBlue", LightBlue },
            { "LightCoral", LightCoral },
            { "LightCyan", LightCyan },
            { "LightGoldenrodYellow", LightGoldenrodYellow },
            { "LightGreen", LightGreen },
            { "LightGray", LightGray },
            { "LightPink", LightPink },
            { "LightSalmon", LightSalmon },
            { "LightSeaGreen", LightSeaGreen },
            { "LightSkyBlue", LightSkyBlue },
            { "LightSlateGray", LightSlateGray },
            { "LightSteelBlue", LightSteelBlue },
            { "LightYellow", LightYellow },
            { "Lime", Lime },
            { "LimeGreen", LimeGreen },
            { "Linen", Linen },
            { "Magenta", Magenta },
            { "Maroon", Maroon },
            { "MediumAquamarine", MediumAquamarine },
            { "MediumBlue", MediumBlue },
            { "MediumOrchid", MediumOrchid },
            { "MediumPurple", MediumPurple },
            { "MediumSeaGreen", MediumSeaGreen },
            { "MediumSlateBlue", MediumSlateBlue },
            { "MediumSpringGreen", MediumSpringGreen },
            { "MediumTurquoise", MediumTurquoise },
            { "MediumVioletRed", MediumVioletRed },
            { "MidnightBlue", MidnightBlue },
            { "MintCream", MintCream },
            { "MistyRose", MistyRose },
            { "Moccasin", Moccasin },
            { "NavajoWhite", NavajoWhite },
            { "Navy", Navy },
            { "OldLace", OldLace },
            { "Olive", Olive },
            { "OliveDrab", OliveDrab },
            { "Orange", Orange },
            { "OrangeRed", OrangeRed },
            { "Orchid", Orchid },
            { "PaleGoldenrod", PaleGoldenrod },
            { "PaleGreen", PaleGreen },
            { "PaleTurquoise", PaleTurquoise },
            { "PaleVioletRed", PaleVioletRed },
            { "PapayaWhip", PapayaWhip },
            { "PeachPuff", PeachPuff },
            { "Peru", Peru },
            { "Pink", Pink },
            { "Plum", Plum },
            { "PowderBlue", PowderBlue },
            { "Purple", Purple },
            { "Red", Red },
            { "RosyBrown", RosyBrown },
            { "RoyalBlue", RoyalBlue },
            { "SaddleBrown", SaddleBrown },
            { "Salmon", Salmon },
            { "SandyBrown", SandyBrown },
            { "SeaGreen", SeaGreen },
            { "SeaShell", SeaShell },
            { "Sienna", Sienna },
            { "Silver", Silver },
            { "SkyBlue", SkyBlue },
            { "SlateBlue", SlateBlue },
            { "SlateGray", SlateGray },
            { "Snow", Snow },
            { "SpringGreen", SpringGreen },
            { "SteelBlue", SteelBlue },
            { "Tan", Tan },
            { "Teal", Teal },
            { "Thistle", Thistle },
            { "Tomato", Tomato },
            { "Turquoise", Turquoise },
            { "Violet", Violet },
            { "Wheat", Wheat },
            { "White", White },
            { "WhiteSmoke", WhiteSmoke },
            { "Yellow", Yellow },
            { "YellowGreen", YellowGreen },
        };

        /// <summary>
        /// Gets the known colours.
        /// </summary>
        /// <value>
        /// The known colours.
        /// </value> 
        [NotNull]
        public static IReadOnlyDictionary<string, Colour> KnownColours => _knownColours;
    }
}