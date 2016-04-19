using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using EscherTilier.Graphics;
using EscherTilier.Graphics.Resources;
using EscherTilier.Utilities;
using JetBrains.Annotations;

namespace EscherTilier.Styles
{
    public class SimpleStyleManager : StyleManager
    {
        public SimpleStyleManager(
            [CanBeNull] IResourceManager resourceManager)
            : base(resourceManager)
        {
        }

        protected override IStyle GetStyle(TileBase tile, IStyle[] styles)
        {
            Debug.Assert(styles.Length == 1);
            return styles[0];
        }
    }
}