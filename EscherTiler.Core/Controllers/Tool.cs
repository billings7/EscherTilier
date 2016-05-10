using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using EscherTiler.Graphics;
using JetBrains.Annotations;

namespace EscherTiler.Controllers
{
    /// <summary>
    ///     Base class for tools.
    /// </summary>
    public abstract class Tool : IDrawable
    {
        [NotNull]
        private readonly Dictionary<string, Option> _options = new Dictionary<string, Option>();

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

            if (options != null)
            {
                foreach (Option option in options)
                {
                    Debug.Assert(option != null, "option != null");
                    _options.Add(option.Name, option);
                }
            }
        }

        /// <summary>
        ///     Gets or sets the controller the tool belongs to.
        /// </summary>
        /// <value>
        ///     The controller.
        /// </value>
        [NotNull]
        public Controller Controller { get; set; }

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
        public IReadOnlyCollection<Option> Options => _options.Values;

        /// <summary>
        ///     Adds an option.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        protected void AddOption([NotNull] Option option)
        {
            if (option == null) throw new ArgumentNullException(nameof(option));
            if (!_options.ContainsKey(option.Name))
            {
                _options.Add(option.Name, option);
                OnOptionsChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Removes an option.
        /// </summary>
        /// <param name="option">The option.</param>
        protected bool RemoveOption([NotNull] Option option)
        {
            if (option == null) throw new ArgumentNullException(nameof(option));
            bool removed = _options.Remove(option.Name);
            if (removed)
                OnOptionsChanged(EventArgs.Empty);
            return removed;
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
        ///     Called when the highlighted location (ie the cursor location) changes.
        /// </summary>
        /// <param name="rawLocation">
        ///     The raw location.
        ///     Should be transformed by the <see cref="IView.InverseViewMatrix" /> for the
        ///     <see cref="Controller">Controllers</see> <see cref="EscherTiler.Controllers.Controller.View" /> to get the
        ///     location in the tiling itself.
        /// </param>
        public virtual void UpdateLocation(Vector2 rawLocation) { }

        /// <summary>
        ///     Starts the action associated with this tool at the location given.
        /// </summary>
        /// <param name="rawLocation">
        ///     The raw location to start the action.
        ///     Should be transformed by the <see cref="IView.InverseViewMatrix" /> for the
        ///     <see cref="Controller">Controllers</see> <see cref="EscherTiler.Controllers.Controller.View" /> to get the
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
        ///     Gets a value indicating whether this action changes any data that would need to be saved.
        /// </summary>
        /// <value>
        ///     <see langword="true" /> if the action changes data; otherwise, <see langword="false" />.
        /// </value>
        public abstract bool ChangesData { get; }
    }

    /// <summary>
    ///     Represents an action that happens instantly when started and has no extra steps.
    /// </summary>
    public sealed class InstantAction : Action
    {
        /// <summary>
        ///     An instance of this class with <see cref="ChangesData" /> equal to <see langword="false" />.
        /// </summary>
        [NotNull]
        public static readonly InstantAction PureInstance = new InstantAction(false);

        /// <summary>
        ///     An instance of this class with <see cref="ChangesData" /> equal to <see langword="true" />.
        /// </summary>
        [NotNull]
        public static readonly InstantAction DestructiveInstance = new InstantAction(true);

        /// <summary>
        ///     Prevents a default instance of the <see cref="InstantAction" /> class from being created.
        /// </summary>
        /// <param name="changesData">if set to <see langword="true" /> [changes data].</param>
        private InstantAction(bool changesData)
        {
            ChangesData = changesData;
        }

        /// <summary>
        ///     Gets a value indicating whether this action changes any data that would need to be saved.
        /// </summary>
        /// <value>
        ///     <see langword="true" /> if the action changes data; otherwise, <see langword="false" />.
        /// </value>
        public override bool ChangesData { get; }
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
        ///     <see cref="EscherTiler.Controllers.Controller.View" /> to get the location in 1the tiling itself.
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

    /// <summary>
    ///     Defines an option for a tool.
    /// </summary>
    public class Option
    {
        private object _value;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Option" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="initialValue">The initial value.</param>
        public Option([NotNull] string name, [CanBeNull] object initialValue = null)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            Name = name;
            Value = initialValue;
        }

        /// <summary>
        ///     Gets the name of the option.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        [NotNull]
        public string Name { get; }

        /// <summary>
        ///     Gets the value.
        /// </summary>
        /// <value>
        ///     The value.
        /// </value>
        public object Value
        {
            get { return _value; }
            set
            {
                if (Equals(value, _value)) return;
                _value = value;
                ValueChanged?.Invoke(value);
            }
        }

        /// <summary>
        ///     Occurs when the value of the <see cref="Value" /> property changes.
        /// </summary>
        public event Action<object> ValueChanged;
    }
}