using System.Diagnostics;
using EscherTilier.Graphics.Resources;
using JetBrains.Annotations;

namespace EscherTilier.Styles
{
    public class SimpleStyleManager : StyleManager
    {
        public SimpleStyleManager(
            [CanBeNull] IResourceManager resourceManager)
            : base(resourceManager) { }

        protected override IStyle GetStyle(TileBase tile, IStyle[] styles)
        {
            Debug.Assert(styles.Length == 1);
            return styles[0];
        }
    }
}