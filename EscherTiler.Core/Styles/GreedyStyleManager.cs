using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;

namespace EscherTiler.Styles
{
    public class GreedyStyleManager : StyleManager
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="GreedyStyleManager" /> class.
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
        ///     Initializes a new instance of the <see cref="GreedyStyleManager" /> class.
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

        /// <summary>
        ///     Gets the parameter a.
        /// </summary>
        /// <value>
        ///     The parameter a.
        /// </value>
        public int ParamA { get; }

        /// <summary>
        ///     Gets the parameter b.
        /// </summary>
        /// <value>
        ///     The parameter b.
        /// </value>
        public int ParamB { get; }

        /// <summary>
        ///     Gets the parameter c.
        /// </summary>
        /// <value>
        ///     The parameter c.
        /// </value>
        public int ParamC { get; }

        /// <summary>
        ///     Gets the style for the given tile.
        /// </summary>
        /// <param name="tile">The tile.</param>
        /// <param name="styles">The styles to choose from.</param>
        /// <param name="state">The style state associated with the tile.</param>
        /// <returns></returns>
        protected override IStyle GetStyle(TileBase tile, IStyle[] styles, ref object state)
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            TileBase[] neighbours = tile.AdjacentTiles.Values.Distinct().ToArray();

            State st = GetTileState(ref state);

            IStyle style = null;

            int offset = st.Offset;
            for (int i = 0; i < styles.Length; i++)
            {
                style = styles[offset % styles.Length];
                if (neighbours.All(n => n.Style != style))
                    break;

                style = null;
                offset += ParamC;
            }

            if (style == null)
            {
                style = styles[offset % styles.Length];
                offset += ParamB;
                st.Offset = offset;
            }

            foreach (TileBase neighbour in neighbours)
            {
                Debug.Assert(neighbour != null, "neighbour != null");

                GetTileState(neighbour).Offset = offset;
                offset += ParamA;
            }

            return style;
        }

        /// <summary>
        ///     Gets the state of the tile.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns></returns>
        [NotNull]
        private State GetTileState(ref object state)
        {
            State s = state as State;
            if (s == null)
                state = s = new State();
            return s;
        }

        /// <summary>
        ///     Gets the state of the tile.
        /// </summary>
        /// <param name="tile">The tile.</param>
        /// <returns></returns>
        [NotNull]
        private State GetTileState([NotNull] TileBase tile)
        {
            State state = GetState(tile) as State;
            if (state == null)
            {
                state = new State();
                SetState(tile, state);
            }
            return state;
        }

        private class State
        {
            public int Offset;
        }
    }
}