using System;
using System.Collections.Generic;
using System.Windows.Forms;
using EscherTiler.Graphics.GDI;
using EscherTiler.Styles;
using JetBrains.Annotations;

namespace EscherTiler
{
    /// <summary>
    ///     Displays an orderable list of <see cref="StyleControl" />.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    public partial class StyleListView : UserControl
    {
        public event EventHandler StylesChanged;

        /// <summary>
        ///     Initializes a new instance of the <see cref="StyleListView" /> class.
        /// </summary>
        public StyleListView()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Adds a style control.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public void AddStyle([NotNull] StyleControl control)
        {
            if (control == null) throw new ArgumentNullException(nameof(control));

            StyleUpDown sud = new StyleUpDown(this, control)
            {
                Dock = DockStyle.Top
            };

            Controls.Add(sud);
        }

        /// <summary>
        ///     Sets the styles this list displays.
        /// </summary>
        /// <param name="styles">The styles.</param>
        /// <param name="resourceManager">The resource manager.</param>
        public void SetStyles([NotNull] IEnumerable<TileStyle> styles, [NotNull] GDIResourceManager resourceManager)
        {
            Controls.Clear();

            foreach (TileStyle style in styles)
            {
                StyleControl styleControl = new StyleControl(style.Style, resourceManager);
                styleControl.StyleChanged += (sender, args) =>
                {
                    if (styleControl.Style == null) return;
                    style.Style = styleControl.Style;
                    StylesChanged?.Invoke(sender, args);
                };
                AddStyle(styleControl);
            }
        }

        /// <summary>
        ///     Control for moving a style up and down the list/
        /// </summary>
        /// <seealso cref="System.Windows.Forms.Control" />
        private class StyleUpDown : Control
        {
            private readonly UpDownButtons _upDownButtons;

            [NotNull]
            private readonly StyleListView _list;

            [NotNull]
            public readonly StyleControl StyleControl;

            /// <summary>
            ///     Initializes a new instance of the <see cref="StyleUpDown" /> class.
            /// </summary>
            /// <param name="list">The list.</param>
            /// <param name="styleControl">The style control.</param>
            /// <exception cref="System.ArgumentNullException">
            /// </exception>
            public StyleUpDown([NotNull] StyleListView list, [NotNull] StyleControl styleControl)
            {
                if (list == null) throw new ArgumentNullException(nameof(list));
                if (styleControl == null) throw new ArgumentNullException(nameof(styleControl));

                Width = list.Width;
                Height = styleControl.Height;

                _list = list;

                StyleControl = styleControl;
                StyleControl.Dock = DockStyle.Fill;

                _upDownButtons = new UpDownButtons { Dock = DockStyle.Left };

                Controls.Add(StyleControl);
                // TODO temporarily not using these buttons Controls.Add(_upDownButtons);
            }
        }
    }
}