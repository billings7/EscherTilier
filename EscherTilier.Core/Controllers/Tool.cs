﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using EscherTilier.Graphics;
using JetBrains.Annotations;

namespace EscherTilier.Controllers
{
    /// <summary>
    ///     Base class for tools.
    /// </summary>
    public abstract class Tool : IDrawable
    {
        [NotNull]
        private IEnumerable<Option> _options;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Tool" /> class.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="name">The name of the tool.</param>
        /// <remarks>If no name is specified, the full name of the type will be used.</remarks>
        protected Tool([NotNull] Controller controller, [CanBeNull] string name = null)
            : this(controller, name, Enumerable.Empty<Option>()) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Tool" /> class.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="options">The options.</param>
        /// <remarks>If no name is specified, the full name of the type will be used.</remarks>
        protected Tool([NotNull] Controller controller, [CanBeNull] params Option[] options)
            : this(controller, null, options) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Tool" /> class.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="name">The name of the tool.</param>
        /// <param name="options">The options.</param>
        /// <remarks>If no name is specified, the full name of the type will be used.</remarks>
        protected Tool([NotNull] Controller controller, [CanBeNull] string name, [CanBeNull] params Option[] options)
            : this(controller, name, (IEnumerable<Option>) options) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Tool" /> class.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="name">The name of the tool.</param>
        /// <param name="options">The options.</param>
        /// <remarks>If no name is specified, the full name of the type will be used.</remarks>
        protected Tool(
            [NotNull] Controller controller,
            [CanBeNull] string name,
            [CanBeNull] IEnumerable<Option> options)
        {
            if (controller == null) throw new ArgumentNullException(nameof(controller));
            if (string.IsNullOrWhiteSpace(name))
                name = GetType().FullName;
            Controller = controller;
            Name = name;
            _options = options ?? Enumerable.Empty<Option>();
        }

        /// <summary>
        ///     Gets the controller the tool belongs to.
        /// </summary>
        /// <value>
        ///     The controller.
        /// </value>
        [NotNull]
        public Controller Controller { get; }

        /// <summary>
        ///     Gets the name of the tool.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        [NotNull]
        public string Name { get; }

        /// <summary>
        ///     Occurs when the value of the <see cref="Options" /> property changes.
        /// </summary>
        public event EventHandler OptionsChanged;

        /// <summary>
        ///     Gets or sets the options for this tool.
        /// </summary>
        /// <value>
        ///     The options.
        /// </value>
        [NotNull]
        [ItemNotNull]
        public IEnumerable<Option> Options
        {
            get { return _options; }
            protected set
            {
                if (ReferenceEquals(_options, value)) return;
                _options = value;
                OnOptionsChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Called when this tool is selected as the current tool.
        /// </summary>
        public virtual void Selected() { }

        /// <summary>
        ///     Called when this tool is deselected as the current tool.
        /// </summary>
        public virtual void Deselected() { }

        /// <summary>
        /// Called when the highlighted location (ie the cursor location) changes.
        /// </summary>
        /// <param name="rawLocation">
        ///     The raw location.
        ///     Should be transformed by the <see cref="IView.InverseViewMatrix" /> for the
        ///     <see cref="Controller">Controllers</see> <see cref="EscherTilier.Controllers.Controller.View" /> to get the
        ///     location in the tiling itself.
        /// </param>
        public virtual void UpdateLocation(Vector2 rawLocation) { }

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
        [CanBeNull]
        public abstract Action StartAction(Vector2 rawLocation);

        /// <summary>
        ///     Raises the <see cref="E:OptionsChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected virtual void OnOptionsChanged(EventArgs e) => OptionsChanged?.Invoke(this, e);

        /// <summary>
        ///     Draws this object to the <see cref="IGraphics" /> provided.
        /// </summary>
        /// <param name="graphics">The graphics object to use to draw this object.</param>
        public virtual void Draw(IGraphics graphics) { }
    }

    /// <summary>
    ///     Base class for an action perfomed by a <see cref="Tool" />.
    /// </summary>
    public abstract class Action
    {
        /// <summary>
        ///     An instance that represents an action that has no extra steps to perform.
        /// </summary>
        public static readonly InstantAction Instant = InstantAction.Instance;
    }

    /// <summary>
    ///     Represents an action that happens instantly when started and has no extra steps.
    /// </summary>
    public sealed class InstantAction : Action
    {
        /// <summary>
        ///     The instance of this class.
        /// </summary>
        [NotNull]
        public static readonly InstantAction Instance = new InstantAction();

        /// <summary>
        ///     Prevents a default instance of the <see cref="InstantAction" /> class from being created.
        /// </summary>
        private InstantAction() { }
    }

    /// <summary>
    ///     Base class for an action that can be dragged around after it has started.
    /// </summary>
    public abstract class DragAction : Action
    {
        /// <summary>
        ///     Updates the location of the action.
        /// </summary>
        /// <param name="rawLocation">
        ///     The raw location that the action has been dragged to.
        ///     Should be transformed by the <see cref="IView.InverseViewMatrix" /> for the
        ///     <see cref="EscherTilier.Controllers.Controller.View" /> to get the location in 1the tiling itself.
        /// </param>
        public abstract void Update(Vector2 rawLocation);

        /// <summary>
        ///     Cancels this action.
        /// </summary>
        public abstract void Cancel();

        /// <summary>
        ///     Applies this action.
        /// </summary>
        public abstract void Apply();
    }

    public class Option
    {
        public string Name { get; }

        public IOptionType Type { get; }

        public object Value { get; }
    }

    public interface IOptionType { }
}