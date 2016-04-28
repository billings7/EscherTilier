using System.Collections.Generic;
using EscherTiler.Graphics.Resources;
using JetBrains.Annotations;

namespace EscherTiler.Styles
{
    public class GreedyStyleManager : StyleManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GreedyStyleManager"/> class.
        /// </summary>
        /// <param name="paramA">The parameter a.</param>
        /// <param name="paramB">The parameter b.</param>
        /// <param name="paramC">The parameter c.</param>
        /// <param name="lineStyle">The line style.</param>
        /// <param name="styles">The styles.</param>
        public GreedyStyleManager(
            int paramA,
            int paramB,
            int paramC,
            [NotNull] LineStyle lineStyle,
            [CanBeNull] IReadOnlyList<TileStyle> styles)
            : base(lineStyle, styles)
        {
            ParamA = paramA;
            ParamB = paramB;
            ParamC = paramC;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GreedyStyleManager"/> class.
        /// </summary>
        /// <param name="paramA">The parameter a.</param>
        /// <param name="paramB">The parameter b.</param>
        /// <param name="paramC">The parameter c.</param>
        /// <param name="lineStyle">The line style.</param>
        /// <param name="styles">The styles.</param>
        public GreedyStyleManager(
            int paramA,
            int paramB,
            int paramC,
            [NotNull] LineStyle lineStyle,
            [CanBeNull] params TileStyle[] styles)
            : base(lineStyle, styles)
        {
            ParamA = paramA;
            ParamB = paramB;
            ParamC = paramC;
        }

        public int ParamA { get; }

        public int ParamB { get; }

        public int ParamC { get; }

        protected override IStyle GetStyle(TileBase tile, IStyle[] styles)
        {
            throw new System.NotImplementedException();
        }
    }
}