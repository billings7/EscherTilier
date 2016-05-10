using System;
using System.Reflection;
using System.Windows.Forms;
using JetBrains.Annotations;

namespace EscherTiler
{
    /// <summary>
    ///     General extension methods for the UI.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        ///     Just used to get the actual name for the <see cref="Control.DoubleBuffered" /> property.
        /// </summary>
        private class Ctrl : Control
        {
            public const string DoubleBufferedName = nameof(DoubleBuffered);
        }

        /// <summary>
        ///     Sets the value of the <see cref="Control.DoubleBuffered" /> property for the given control.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="doubleBuffered">if set to <see langword="true" /> the control should be double buffered.</param>
        public static void SetDoubleBuffered([NotNull] this Control control, bool doubleBuffered = true)
        {
            if (control == null) throw new ArgumentNullException(nameof(control));

            PropertyInfo prop = control.GetType()
                .GetProperty(
                    Ctrl.DoubleBufferedName,
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            prop.SetValue(control, doubleBuffered);
        }
    }
}