using System;
using System.Collections.Generic;
using System.Windows.Forms;
using EscherTiler.Graphics.GDI;
using EscherTiler.Styles;
using JetBrains.Annotations;

namespace EscherTiler
{
    public partial class StyleListView : UserControl
    {
        public event EventHandler StylesChanged;

        public StyleListView()
        {
            InitializeComponent();
        }

        public void AddStyle([NotNull] StyleControl control)
        {
            if (control == null) throw new ArgumentNullException(nameof(control));

            StyleUpDown sud = new StyleUpDown(this, control)
            {
                Dock = DockStyle.Top,
            };

            Controls.Add(sud);
        }

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

        private class StyleUpDown : Control
        {
            private readonly UpDownButtons _upDownButtons;

            [NotNull]
            private readonly StyleListView _list;

            [NotNull]
            public StyleControl StyleControl;

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
