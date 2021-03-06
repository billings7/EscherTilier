﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using EscherTiler.Controllers;
using EscherTiler.Styles;
using JetBrains.Annotations;

namespace EscherTiler
{
    // This part of the class deals with editing the tile styles.
    public partial class Main
    {
        [NotNull]
        private readonly Random _random = new Random();

        [NotNull]
        private Control[] _styleManagerPanels;

        /// <summary>
        ///     Initializes the style manager related UI elements.
        /// </summary>
        private void InitializeStyleManager()
        {
            _styleManagerPanels = new Control[]
            {
                _randomStyleMangerPnl,
                _greedyStyleManagerPnl
            };

            _styleManagerTypeCmb.Items.Add(new ComboBoxValue<Type>("Random", typeof(RandomStyleManager)));
            _styleManagerTypeCmb.Items.Add(new ComboBoxValue<Type>("Random Greedy", typeof(RandomGreedyStyleManager)));
            _styleManagerTypeCmb.Items.Add(new ComboBoxValue<Type>("Greedy", typeof(GreedyStyleManager)));
            _styleManagerTypeCmb.Items.Add(new ComboBoxValue<Type>("Simple", typeof(SimpleStyleManager)));
        }

        /// <summary>
        ///     Updates the style manager that the UI controls.
        /// </summary>
        /// <param name="manager">The manager.</param>
        private void UpdateStyleManager([NotNull] StyleManager manager)
        {
            Debug.Assert(manager != null, "manager != null");

            _styleManagerTypeCmb.Enabled = true;
            _lineStyleGroup.Enabled = true;
            _fillStylesGroup.Enabled = true;

            _lineWidthTrack.Value = (int) (manager.LineStyle.Width * 10);
            _lineStyleControl.Style = manager.LineStyle.Style;

            RandomStyleManager randomStyleManager = manager as RandomStyleManager;
            GreedyStyleManager greedyStyleManager;
            if (randomStyleManager != null)
            {
                _randomStyleMangerPnl.Visible = true;
                _seedNum.Value = randomStyleManager.Seed;
            }
            else if ((greedyStyleManager = manager as GreedyStyleManager) != null)
            {
                _greedyStyleManagerPnl.Visible = true;

                _greedyParamATrack.Maximum = manager.Styles.Count - 1;
                _greedyParamBTrack.Maximum = manager.Styles.Count - 1;
                _greedyParamCTrack.Maximum = manager.Styles.Count - 1;

                _greedyParamATrack.Value = greedyStyleManager.ParamA % manager.Styles.Count;
                _greedyParamBTrack.Value = greedyStyleManager.ParamB % manager.Styles.Count;
                _greedyParamCTrack.Value = greedyStyleManager.ParamC % manager.Styles.Count;
            }
            else
            {
                foreach (Control panel in _styleManagerPanels)
                {
                    Debug.Assert(panel != null, "panel != null");
                    panel.Visible = false;
                }
            }

            _styleList.SetStyles(manager.Styles, _lineStyleControl.ResourceManager);
        }

        /// <summary>
        ///     Handles the StylesChanged event of the _styleList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void _styleList_StylesChanged(object sender, EventArgs e)
        {
            TilingController controller = _tilingController;
            if (controller == null) return;
            lock (_lock)
            {
                controller = _tilingController;
                if (controller == null) return;

                controller.Tiling.UpdateStyles(controller.Tiles);
            }
        }

        /// <summary>
        ///     Handles the SelectedIndexChanged event of the _styleManagerTypeCmb control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        /// <exception cref="System.InvalidOperationException"></exception>
        private void _styleManagerTypeCmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBoxValue<Type> val = (ComboBoxValue<Type>) _styleManagerTypeCmb.SelectedItem;

            TilingController controller = _tilingController;
            if (controller == null) return;
            lock (_lock)
            {
                controller = _tilingController;
                if (controller == null) return;

                StyleManager currManager = controller.StyleManager;
                if (currManager.GetType() == val.Value) return;

                StyleManager newManager;

                if (val.Value == typeof(RandomStyleManager))
                    newManager = new RandomStyleManager((int) _seedNum.Value, currManager.LineStyle, currManager.Styles);
                else if (val.Value == typeof(RandomGreedyStyleManager))
                {
                    newManager = new RandomGreedyStyleManager(
                        (int) _seedNum.Value,
                        currManager.LineStyle,
                        currManager.Styles);
                }
                else if (val.Value == typeof(GreedyStyleManager))
                    newManager = new GreedyStyleManager(1, 1, 1, currManager.LineStyle, currManager.Styles);
                else if (val.Value == typeof(SimpleStyleManager))
                {
                    // TODO filter styles properly when multiple shapes are supported
                    newManager = new SimpleStyleManager(currManager.LineStyle, currManager.Styles.First());
                }
                else
                    throw new InvalidOperationException();

                Debug.Assert(newManager.GetType() == val.Value);

                controller.Tiling.SetStyleManager(newManager, controller.Tiles);
                UpdateStyleManager(newManager);
            }
        }

        /// <summary>
        ///     Handles the VisibleChanged event of the manager panel controls.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void StyleManagerPanel_VisibleChanged(object sender, EventArgs e)
        {
            Control control = (Control) sender;
            if (!control.Visible) return;

            foreach (Control panel in _styleManagerPanels)
            {
                if (panel == control) continue;
                panel.Visible = false;
            }
        }

        /// <summary>
        ///     Handles the ValueChanged event of the _seedNum control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void _seedNum_ValueChanged(object sender, EventArgs e)
        {
            RandomStyleManager manager = _tilingController?.StyleManager as RandomStyleManager;
            if (manager == null) return;

            manager.Seed = (int) _seedNum.Value;
        }

        /// <summary>
        ///     Handles the Click event of the _randomSeedBtn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void _randomSeedBtn_Click(object sender, EventArgs e)
        {
            _seedNum.Value = _random.Next();
        }

        /// <summary>
        ///     Handles the ValueChanged event of the _greedyParamATrack control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void _greedyParamATrack_ValueChanged(object sender, EventArgs e)
        {
            GreedyStyleManager manager = _tilingController?.StyleManager as GreedyStyleManager;
            if (manager == null) return;

            manager.ParamA = _greedyParamATrack.Value;
        }

        /// <summary>
        ///     Handles the ValueChanged event of the _greedyParamBTrack control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void _greedyParamBTrack_ValueChanged(object sender, EventArgs e)
        {
            GreedyStyleManager manager = _tilingController?.StyleManager as GreedyStyleManager;
            if (manager == null) return;

            manager.ParamB = _greedyParamBTrack.Value;
        }

        /// <summary>
        ///     Handles the ValueChanged event of the _greedyParamCTrack control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void _greedyParamCTrack_ValueChanged(object sender, EventArgs e)
        {
            GreedyStyleManager manager = _tilingController?.StyleManager as GreedyStyleManager;
            if (manager == null) return;

            manager.ParamC = _greedyParamCTrack.Value;
        }

        /// <summary>
        ///     Handles the StyleChanged event of the _lineStyleControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void _lineStyleControl_StyleChanged(object sender, EventArgs e)
        {
            if (_lineStyleControl.Style == null) return;

            TilingController controller = _tilingController;
            if (controller == null) return;
            lock (_lock)
            {
                controller = _tilingController;
                if (controller == null) return;

                controller.StyleManager.LineStyle =
                    controller.StyleManager.LineStyle.WithStyle((SolidColourStyle) _lineStyleControl.Style);
            }
        }

        /// <summary>
        ///     Handles the ValueChanged event of the _lineWidthTrack control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void _lineWidthTrack_ValueChanged(object sender, EventArgs e)
        {
            if (_lineWidthTrack.Value < 1)
            {
                _lineWidthTrack.Value = 1;
                return;
            }

            TilingController controller = _tilingController;
            if (controller == null) return;
            lock (_lock)
            {
                controller = _tilingController;
                if (controller == null) return;

                controller.StyleManager.LineStyle =
                    controller.StyleManager.LineStyle.WithWidth(_lineWidthTrack.Value / 10f);
            }
        }
    }
}