using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using JetBrains.Annotations;

namespace EscherTiler
{
    /// <summary>
    ///     A pair of buttons for up/down functions.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Control" />
    public class UpDownButtons : Control
    {
        [NotNull]
        private readonly UpDownButton _upButton;

        [NotNull]
        private readonly UpDownButton _downButton;

        /// <summary>
        ///     Occurs when the up button is clicked.
        /// </summary>
        public event EventHandler UpClick
        {
            add { _upButton.Click += value; }
            remove { _upButton.Click -= value; }
        }

        /// <summary>
        ///     Occurs when the down button is clicked.
        /// </summary>
        public event EventHandler DownClick
        {
            add { _downButton.Click += value; }
            remove { _downButton.Click -= value; }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="UpDownButtons" /> class.
        /// </summary>
        public UpDownButtons()
        {
            Name = "UpDownButtons";
            Size = new Size(16, 24);

            _upButton = new UpDownButton
            {
                IsUp = true,
                Width = Width,
                Height = Height / 2,
                Location = Point.Empty
            };
            _downButton = new UpDownButton
            {
                IsUp = false,
                Width = Width,
                Height = Height / 2,
                Location = new Point(0, Height / 2)
            };

            Layout += OnLayout;

            Controls.Add(_upButton);
            Controls.Add(_downButton);
        }

        private void OnLayout(object sender, LayoutEventArgs layoutEventArgs)
        {
            _upButton.Width = Width;
            _downButton.Width = Width;

            int halfHeight = Height / 2;

            _upButton.Height = halfHeight;
            _downButton.Height = halfHeight;

            _downButton.Top = halfHeight;
        }

        /// <summary>
        /// An up/down button.
        /// </summary>
        /// <seealso cref="System.Windows.Forms.Button" />
        private class UpDownButton : Button
        {
            private bool _isUp;

            /// <summary>
            /// Gets or sets a value indicating whether button is an up or down button.
            /// </summary>
            /// <value>
            /// <see langword="true" /> if this is an up button; otherwise this is a down button.
            /// </value>
            public bool IsUp
            {
                get { return _isUp; }
                set
                {
                    _isUp = value;
                    Invalidate();
                }
            }

            private bool _pushed;
            private bool _captured;

            protected override void OnMouseDown(MouseEventArgs e)
            {
                _pushed = _captured = true;
                base.OnMouseDown(e);
            }

            protected override void OnMouseMove(MouseEventArgs e)
            {
                _captured = true;
                base.OnMouseMove(e);
            }

            protected override void OnMouseUp(MouseEventArgs e)
            {
                _pushed = false;
                base.OnMouseUp(e);
            }

            protected override void OnMouseLeave(EventArgs e)
            {
                _captured = false;
                Invalidate();
                base.OnMouseLeave(e);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                if (Application.RenderWithVisualStyles)
                {
                    VisualStyleRenderer vsr;

                    // ReSharper disable AssignNullToNotNullAttribute
                    if (IsUp)
                    {
                        vsr = new VisualStyleRenderer(
                            _captured
                                ? VisualStyleElement.Spin.Up.Hot
                                : VisualStyleElement.Spin.Up.Normal);

                        if (!Enabled)
                            vsr.SetParameters(VisualStyleElement.Spin.Up.Disabled);
                        else if (_pushed)
                            vsr.SetParameters(VisualStyleElement.Spin.Up.Pressed);
                    }
                    else
                    {
                        vsr = new VisualStyleRenderer(
                            _captured
                                ? VisualStyleElement.Spin.Down.Hot
                                : VisualStyleElement.Spin.Down.Normal);

                        if (!Enabled)
                            vsr.SetParameters(VisualStyleElement.Spin.Down.Disabled);
                        else if (_pushed)
                            vsr.SetParameters(VisualStyleElement.Spin.Down.Pressed);
                    }
                    // ReSharper restore AssignNullToNotNullAttribute

                    vsr.DrawBackground(e.Graphics, new Rectangle(0, 0, Width, Height));
                }
                else
                {
                    ControlPaint.DrawScrollButton(
                        e.Graphics,
                        new Rectangle(0, 0, Width, Height),
                        IsUp ? ScrollButton.Up : ScrollButton.Down,
                        _pushed
                            ? ButtonState.Pushed
                            : (Enabled
                                ? ButtonState.Normal
                                : ButtonState.Inactive));
                }
            }
        }
    }
}