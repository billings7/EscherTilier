using System.Numerics;
using System.Windows.Forms;
using EscherTilier.Controllers;
using JetBrains.Annotations;
using DragAction = EscherTilier.Controllers.DragAction;

namespace EscherTilier
{
    public partial class Main
    {
        /// <summary>
        ///     Tool used to pan around the tiling.
        /// </summary>
        /// <seealso cref="EscherTilier.Controllers.Tool" />
        private class PanTool : Tool
        {
            [NotNull]
            private readonly Main _form;

            private Cursor _lastCursor;

            /// <summary>
            ///     Initializes a new instance of the <see cref="PanTool" /> class.
            /// </summary>
            /// <param name="controller">The controller.</param>
            /// <param name="form">The form.</param>
            public PanTool([NotNull] Controller controller, [NotNull] Main form)
                : base(controller)
            {
                _form = form;
            }

            /// <summary>
            ///     Called when this tool is selected as the current tool.
            /// </summary>
            public override void Selected()
            {
                base.Selected();
                _lastCursor = _form._renderControl.Cursor;
                _form._renderControl.Cursor = Cursors.SizeAll;
            }

            /// <summary>
            ///     Called when this tool is deselected as the current tool.
            /// </summary>
            public override void Deselected()
            {
                base.Deselected();
                _form._renderControl.Cursor = _lastCursor;
            }

            /// <summary>
            ///     Starts the action associated with this tool at the location given.
            /// </summary>
            /// <param name="rawLocation">
            ///     The raw location to start the action.
            ///     Should be transformed by the <see cref="IView.InverseViewMatrix" /> for the
            ///     <see cref="Controller">Controllers</see> <see cref="EscherTilier.Controllers.Controller.View" /> to get the
            ///     location in the tiling itself.
            /// </param>
            /// <returns>
            ///     The action that was performed, or null if no action was performed.
            /// </returns>
            public override Controllers.Action StartAction(Vector2 rawLocation)
            {
                return new PanAction(rawLocation, _form);
            }

            /// <summary>
            ///     Action used for panning.
            /// </summary>
            private class PanAction : DragAction
            {
                private Vector2 _last;

                [NotNull]
                private readonly Main _form;

                private readonly Matrix3x2 _translate;
                private readonly Matrix3x2 _invTranslate;

                /// <summary>
                ///     Initializes a new instance of the <see cref="PanAction" /> class.
                /// </summary>
                /// <param name="start">The start point of the action.</param>
                /// <param name="form">The form.</param>
                public PanAction(Vector2 start, [NotNull] Main form)
                {
                    _last = start;
                    _form = form;

                    _translate = form._translate;
                    _invTranslate = form._invTranslate;
                }

                /// <summary>
                ///     Updates the location of the action.
                /// </summary>
                /// <param name="rawLocation">
                ///     The raw location that the action has been dragged to.
                ///     Should be transformed by the <see cref="P:EscherTilier.IView.InverseViewMatrix" /> for the
                ///     <see cref="P:EscherTilier.Controllers.Controller.View" /> to get the location in 1the tiling itself.
                /// </param>
                public override void Update(Vector2 rawLocation)
                {
                    _form.UpdateTranslation(rawLocation - _last);
                    _last = rawLocation;
                }

                /// <summary>
                ///     Cancels this action.
                /// </summary>
                public override void Cancel()
                {
                    _form._translate = _translate;
                    _form._invTranslate = _invTranslate;
                    _form.UpdateViewBounds();
                }

                /// <summary>
                ///     Applies this action.
                /// </summary>
                public override void Apply() { }
            }
        }
    }
}