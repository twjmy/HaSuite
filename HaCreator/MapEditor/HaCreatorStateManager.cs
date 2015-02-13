﻿/* Copyright (C) 2015 haha01haha01

* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using HaCreator.CustomControls;
using HaCreator.CustomControls.EditorPanels;
using HaCreator.GUI;
using HaCreator.GUI.InstanceEditor;
using MapleLib.Helpers;
using MapleLib.WzLib.WzStructure.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HaCreator.ThirdParty.TabPages;
using MapleLib.WzLib;

namespace HaCreator.MapEditor
{
    public class HaCreatorStateManager
    {
        MultiBoard multiBoard;
        HaRibbon ribbon;
        PageCollection tabs;
        TilePanel tilePanel;

        public HaCreatorStateManager(MultiBoard multiBoard, HaRibbon ribbon, PageCollection tabs)
        {
            this.multiBoard = multiBoard;
            this.ribbon = ribbon;
            this.tabs = tabs;

            this.ribbon.OpenClicked += ribbon_OpenClicked;
            this.ribbon.SaveClicked += ribbon_SaveClicked;
            this.ribbon.RepackClicked += ribbon_RepackClicked;
            this.ribbon.AboutClicked += ribbon_AboutClicked;
            this.ribbon.HelpClicked += ribbon_HelpClicked;
            this.ribbon.SettingsClicked += ribbon_SettingsClicked;
            this.ribbon.ExitClicked += ribbon_ExitClicked;
            this.ribbon.ViewToggled += ribbon_ViewToggled;
            this.ribbon.ShowVRToggled += ribbon_ShowVRToggled;
            this.ribbon.ShowMinimapToggled += ribbon_ShowMinimapToggled;
            this.ribbon.ParallaxToggled += ribbon_ParallaxToggled;
            this.ribbon.LayerViewChanged += ribbon_LayerViewChanged;
            this.ribbon.AllLayerToggled += ribbon_AllLayerToggled;
            this.ribbon.MapSimulationClicked += ribbon_MapSimulationClicked;
            this.ribbon.RegenerateMinimapClicked += ribbon_RegenerateMinimapClicked;
            this.ribbon.SnappingToggled += ribbon_SnappingToggled;
            this.ribbon.RandomTilesToggled += ribbon_RandomTilesToggled;
            this.ribbon.HaRepackerClicked += ribbon_HaRepackerClicked;

            this.tabs.CurrentPageChanged += tabs_CurrentPageChanged;

            this.multiBoard.OnBringToFrontClicked += multiBoard_OnBringToFrontClicked;
            this.multiBoard.OnEditBaseClicked += multiBoard_OnEditBaseClicked;
            this.multiBoard.OnEditInstanceClicked += multiBoard_OnEditInstanceClicked;
            this.multiBoard.OnLayerTSChanged += multiBoard_OnLayerTSChanged;
            this.multiBoard.OnSendToBackClicked += multiBoard_OnSendToBackClicked;
            this.multiBoard.ReturnToSelectionState += multiBoard_ReturnToSelectionState;
            this.multiBoard.SelectedItemChanged += multiBoard_SelectedItemChanged;
            this.multiBoard.MouseMoved += multiBoard_MouseMoved;

            multiBoard.Visible = false;
            ribbon.SetEnabled(false);
        }

        #region MultiBoard Events
        void multiBoard_MouseMoved(Board selectedBoard, Microsoft.Xna.Framework.Point oldPos, Microsoft.Xna.Framework.Point newPos, Microsoft.Xna.Framework.Point currPhysicalPos)
        {
            ribbon.SetMousePos(newPos.X, newPos.Y, currPhysicalPos.X, currPhysicalPos.Y);
        }

        void multiBoard_SelectedItemChanged(BoardItem selectedItem)
        {
            if (selectedItem != null)
            {
                ribbon.SetItemDesc(CreateItemDescription(selectedItem, "\n"));
            }
            else
            {
                ribbon.SetItemDesc("");
            }
        }

        void multiBoard_ReturnToSelectionState()
        {
            // No need to lock because SelectionMode() and ExitEditMode() are both thread-safe
            multiBoard.SelectedBoard.Mouse.SelectionMode();
            ExitEditMode();
            multiBoard.Focus();
        }

        void multiBoard_OnSendToBackClicked(BoardItem boardRefItem)
        {
            lock (multiBoard)
            {
                foreach (BoardItem item in boardRefItem.Board.SelectedItems)
                {
                    if (item.Z > 0)
                    {
                        item.Board.UndoRedoMan.AddUndoBatch(new List<UndoRedoAction> { UndoRedoManager.ItemZChanged(item, item.Z, 0) });
                        item.Z = 0;
                    }
                }
                boardRefItem.Board.BoardItems.Sort();
            }
            multiBoard.Focus();
        }

        void multiBoard_OnLayerTSChanged(Layer layer)
        {
            ribbon.SetLayer(layer);
        }

        void multiBoard_OnEditInstanceClicked(BoardItem item)
        {
            InputHandler.ClearBoundItems(multiBoard.SelectedBoard);
            switch (item.GetType().Name)
            {
                case "ObjectInstance":
                    new ObjectInstanceEditor((ObjectInstance)item).ShowDialog();
                    break;
                case "TileInstance":
                case "Chair":
                    new GeneralInstanceEditor(item).ShowDialog();
                    break;
                case "FootholdAnchor":
                    FootholdLine[] selectedFootholds = FootholdLine.GetSelectedFootholds(item.Board);
                    if (selectedFootholds.Length > 0)
                    {
                        new FootholdEditor(selectedFootholds).ShowDialog();
                    }
                    else
                    {
                        new GeneralInstanceEditor(item).ShowDialog();
                    }
                    break;
                case "RopeAnchor":
                    new RopeInstanceEditor((RopeAnchor)item).ShowDialog();
                    break;
                case "NPCInstance":
                case "MobInstance":
                    new LifeInstanceEditor((LifeInstance)item).ShowDialog();
                    break;
                case "ReactorInstance":
                    new ReactorInstanceEditor((ReactorInstance)item).ShowDialog();
                    break;
                case "BackgroundInstance":
                    new BackgroundInstanceEditor((BackgroundInstance)item).ShowDialog();
                    break;
                case "PortalInstance":
                    new PortalInstanceEditor((PortalInstance)item).ShowDialog();
                    break;
                case "ToolTip":
                    new TooltipInstanceEditor((ToolTip)item).ShowDialog();
                    break;
                default:
                    break;
            }
        }

        void multiBoard_OnEditBaseClicked(BoardItem item)
        {
            //TODO
        }

        void multiBoard_OnBringToFrontClicked(BoardItem boardRefItem)
        {
            lock (multiBoard)
            {
                foreach (BoardItem item in boardRefItem.Board.SelectedItems)
                {
                    int oldZ = item.Z;
                    if (item is BackgroundInstance)
                    {
                        IList list = ((BackgroundInstance)item).front ? multiBoard.SelectedBoard.BoardItems.FrontBackgrounds : multiBoard.SelectedBoard.BoardItems.BackBackgrounds;
                        int highestZ = 0;
                        foreach (BackgroundInstance bg in list)
                            if (bg.Z > highestZ)
                                highestZ = bg.Z;
                        item.Z = highestZ + 1;
                    }
                    else
                    {
                        int highestZ = 0;
                        foreach (LayeredItem layeredItem in multiBoard.SelectedBoard.BoardItems.TileObjs)
                            if (layeredItem.Z > highestZ) highestZ = layeredItem.Z;
                        item.Z = highestZ + 1;
                    }
                    if (item.Z != oldZ)
                        item.Board.UndoRedoMan.AddUndoBatch(new List<UndoRedoAction> { UndoRedoManager.ItemZChanged(item, oldZ, item.Z) });
                }
            }
            boardRefItem.Board.BoardItems.Sort();
        }
        #endregion

        #region Tab Events
        private void mapEditInfo(object sender, EventArgs e)
        {
            Board selectedBoard = (Board)((ToolStripMenuItem)sender).Tag;
            new InfoEditor(selectedBoard.MapInfo, multiBoard).ShowDialog();
        }

        void tabs_CurrentPageChanged(HaCreator.ThirdParty.TabPages.TabPage currentPage, HaCreator.ThirdParty.TabPages.TabPage previousPage)
        {
            lock (multiBoard)
            {
                if (previousPage != null)
                {
                    multiBoard_ReturnToSelectionState();
                }

                multiBoard.SelectedBoard = (Board)currentPage.Tag;
                ribbon.SetLayers(multiBoard.SelectedBoard.Layers);
                ApplicationSettings.lastDefaultLayer = multiBoard.SelectedBoard.SelectedLayerIndex;
                ribbon.SetSelectedLayer(multiBoard.SelectedBoard.SelectedLayerIndex);
                ParseVisibleEditedTypes();
                multiBoard.Focus();
            }
        }
        #endregion

        #region Ribbon Handlers
        void ribbon_HaRepackerClicked()
        {
            HaRepacker.Program.WzMan = new HaRepackerLib.WzFileManager();
            bool firstRun = HaRepacker.Program.PrepareApplication(false);
            HaRepacker.GUI.MainForm mf = new HaRepacker.GUI.MainForm(null, false, firstRun);
            mf.unloadAllToolStripMenuItem.Visible = false;
            mf.reloadAllToolStripMenuItem.Visible = false;
            foreach (DictionaryEntry entry in Program.WzManager.wzFiles)
                mf.Interop_AddLoadedWzFileToManager((WzFile)entry.Value);
            lock (multiBoard)
            {

                mf.ShowDialog();
            }
            HaRepacker.Program.EndApplication(false, false);
        }
        
        bool? getTypes(ItemTypes visibleTypes, ItemTypes editedTypes, ItemTypes type)
        {
            if ((editedTypes & type) == type)
            {
                return true;
            }
            else if ((visibleTypes & type) == type)
            {
                return (bool?)null;
            }
            else
            {
                return false;
            }
        }

        private void ParseVisibleEditedTypes()
        {
            ItemTypes visibleTypes = ApplicationSettings.theoreticalVisibleTypes = multiBoard.SelectedBoard.VisibleTypes;
            ItemTypes editedTypes = ApplicationSettings.theoreticalEditedTypes = multiBoard.SelectedBoard.EditedTypes;
            ribbon.SetVisibilityCheckboxes(getTypes(visibleTypes, editedTypes, ItemTypes.Tiles),
                                            getTypes(visibleTypes, editedTypes, ItemTypes.Objects),
                                            getTypes(visibleTypes, editedTypes, ItemTypes.NPCs),
                                            getTypes(visibleTypes, editedTypes, ItemTypes.Mobs),
                                            getTypes(visibleTypes, editedTypes, ItemTypes.Reactors),
                                            getTypes(visibleTypes, editedTypes, ItemTypes.Portals),
                                            getTypes(visibleTypes, editedTypes, ItemTypes.Footholds),
                                            getTypes(visibleTypes, editedTypes, ItemTypes.Ropes),
                                            getTypes(visibleTypes, editedTypes, ItemTypes.Chairs),
                                            getTypes(visibleTypes, editedTypes, ItemTypes.ToolTips),
                                            getTypes(visibleTypes, editedTypes, ItemTypes.Backgrounds),
                                            getTypes(visibleTypes, editedTypes, ItemTypes.Misc));
        }
        
        void ribbon_RandomTilesToggled(bool pressed)
        {
            ApplicationSettings.randomTiles = pressed;
            if (tilePanel != null)
                tilePanel.LoadTileSetList();
        }

        void ribbon_SnappingToggled(bool pressed)
        {
            UserSettings.useSnapping = pressed;
        }

        void ribbon_RegenerateMinimapClicked()
        {
            if (multiBoard.SelectedBoard.RegenerateMinimap())
                MessageBox.Show("Minimap regenerated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
                MessageBox.Show("An error occured during minimap regeneration. The error has been logged. If possible, save the map and send it to" + ApplicationSettings.AuthorEmail, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ErrorLogger.Log(ErrorLevel.Critical, "error regenning minimap for map " + multiBoard.SelectedBoard.MapInfo.id.ToString());
            }
        }

        void ribbon_MapSimulationClicked()
        {
            multiBoard.DeviceReady = false;
            MapSimulator.MapSimulator.CreateMapSimulator(multiBoard.SelectedBoard).ShowDialog();
            multiBoard.DeviceReady = true;
        }

        void ribbon_ParallaxToggled(bool pressed)
        {
            UserSettings.emulateParallax = pressed;
        }

        void ribbon_ShowMinimapToggled(bool pressed)
        {
            UserSettings.useMiniMap = pressed;
        }

        void ribbon_ShowVRToggled(bool pressed)
        {
            UserSettings.showVR = pressed;
        }

        void setTypes(ref ItemTypes newVisibleTypes, ref ItemTypes newEditedTypes, bool? x, ItemTypes type)
        {
            if (x.HasValue)
            {
                if (x.Value)
                {
                    newVisibleTypes ^= type;
                    newEditedTypes ^= type;
                }
            }
            else
            {
                newVisibleTypes ^= type;
            }
        }

        void ribbon_ViewToggled(bool? tiles, bool? objs, bool? npcs, bool? mobs, bool? reactors, bool? portals, bool? footholds, bool? ropes, bool? chairs, bool? tooltips, bool? backgrounds, bool? misc)
        {
            lock (multiBoard)
            {
                ItemTypes newVisibleTypes = 0;
                ItemTypes newEditedTypes = 0;
                setTypes(ref newVisibleTypes, ref newEditedTypes, tiles, ItemTypes.Tiles);
                setTypes(ref newVisibleTypes, ref newEditedTypes, objs, ItemTypes.Objects);
                setTypes(ref newVisibleTypes, ref newEditedTypes, npcs, ItemTypes.NPCs);
                setTypes(ref newVisibleTypes, ref newEditedTypes, mobs, ItemTypes.Mobs);
                setTypes(ref newVisibleTypes, ref newEditedTypes, reactors, ItemTypes.Reactors);
                setTypes(ref newVisibleTypes, ref newEditedTypes, portals, ItemTypes.Portals);
                setTypes(ref newVisibleTypes, ref newEditedTypes, footholds, ItemTypes.Footholds);
                setTypes(ref newVisibleTypes, ref newEditedTypes, ropes, ItemTypes.Ropes);
                setTypes(ref newVisibleTypes, ref newEditedTypes, chairs, ItemTypes.Chairs);
                setTypes(ref newVisibleTypes, ref newEditedTypes, tooltips, ItemTypes.ToolTips);
                setTypes(ref newVisibleTypes, ref newEditedTypes, backgrounds, ItemTypes.Backgrounds);
                setTypes(ref newVisibleTypes, ref newEditedTypes, misc, ItemTypes.Misc);
                ApplicationSettings.theoreticalVisibleTypes = newVisibleTypes;
                ApplicationSettings.theoreticalEditedTypes = newEditedTypes;
                if (multiBoard.SelectedBoard != null)
                {
                    InputHandler.ClearSelectedItems(multiBoard.SelectedBoard);
                    multiBoard.SelectedBoard.VisibleTypes = newVisibleTypes;
                    multiBoard.SelectedBoard.EditedTypes = newEditedTypes;
                }
            }
        }

        void ribbon_ExitClicked()
        {
            if (CloseRequested != null)
            {
                CloseRequested.Invoke();
            }
        }

        void ribbon_SettingsClicked()
        {
            new UserSettingsForm().ShowDialog();
        }

        void ribbon_HelpClicked()
        {
            string helpPath = Path.Combine(Application.StartupPath, "Help.htm");
            if (File.Exists(helpPath))
                Process.Start(helpPath);
            else
                HaRepackerLib.Warning.Error("Help could not be shown because the help file (HRHelp.htm) was not found");
        }

        void ribbon_AboutClicked()
        {
            new About().ShowDialog();
        }

        void ribbon_RepackClicked()
        {
        }

        void ribbon_SaveClicked()
        {
        }

        void ribbon_OpenClicked()
        {
            LoadMap();
        }

        public void LoadMap()
        {
            if (new Load(multiBoard, tabs, new EventHandler(mapEditInfo)).ShowDialog() == DialogResult.OK)
            {
                if (!multiBoard.DeviceReady)
                {
                    ribbon.SetEnabled(true);
                    ribbon.SetOptions(UserSettings.showVR, UserSettings.useMiniMap, UserSettings.emulateParallax, UserSettings.useSnapping, ApplicationSettings.randomTiles);
                    if (FirstMapLoaded != null)
                        FirstMapLoaded.Invoke();
                    multiBoard.Start();
                }
                ribbon.SetLayers(multiBoard.SelectedBoard.Layers);
                multiBoard.SelectedBoard.VisibleTypes = ApplicationSettings.theoreticalVisibleTypes;
                multiBoard.SelectedBoard.EditedTypes = ApplicationSettings.theoreticalEditedTypes;
                ParseVisibleEditedTypes();
                multiBoard.Focus();
            }
        }
        #endregion

        #region Ribbon Layer Boxes
        private void SetLayer(int currentLayer)
        {
            multiBoard.SelectedBoard.SelectedLayerIndex = currentLayer;
            ApplicationSettings.lastDefaultLayer = currentLayer;
        }

        void ribbon_AllLayerToggled(int layer)
        {
            if (multiBoard.SelectedBoard == null)
                return;
            SetLayer(layer);
            InputHandler.ClearSelectedItems(multiBoard.SelectedBoard);
        }

        private bool LayeredItemsSelected(out int layer)
        {
            foreach (BoardItem item in multiBoard.SelectedBoard.SelectedItems)
                if (item is LayeredItem)
                {
                    layer = ((LayeredItem)item).Layer.LayerNumber;
                    return true;
                }
            layer = 0;
            return false;
        }
        private bool LayerCapableOfHoldingSelectedItems(Layer layer)
        {
            if (layer.tS == null) return true;
            foreach (BoardItem item in multiBoard.SelectedBoard.SelectedItems)
                if (item is TileInstance && ((TileInfo)item.BaseInfo).tS != layer.tS) return false;
            return true;
        }

        void ribbon_LayerViewChanged(int layer)
        {
            if (multiBoard.SelectedBoard == null)
                return;
            int oldLayer;
            if (multiBoard.SelectedBoard.SelectedItems.Count > 0 && LayeredItemsSelected(out oldLayer))
            {
                if (UserSettings.suppressWarnings || MessageBox.Show("Are you sure you want to move these items from layer " + oldLayer.ToString() + " to " + layer.ToString() + "?\r\n", "Cross-Layer Operation", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes)
                {
                    InputHandler.ClearSelectedItems(multiBoard.SelectedBoard);
                    SetLayer(layer);
                    return;
                }
                Layer targetLayer = multiBoard.SelectedBoard.Layers[layer];
                if (!LayerCapableOfHoldingSelectedItems(targetLayer))
                {
                    MessageBox.Show("Error: Target layer cannot hold the selected items because they contain tiles with a tS different from the layer's", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                List<IContainsLayerInfo> items = new List<IContainsLayerInfo>();
                lock (multiBoard)
                {
                    foreach (BoardItem item in multiBoard.SelectedBoard.SelectedItems)
                    {
                        if (!(item is IContainsLayerInfo)) continue;
                        ((IContainsLayerInfo)item).LayerNumber = targetLayer.LayerNumber;
                        items.Add((IContainsLayerInfo)item);
                    }
                }
                if (items.Count > 0)
                    multiBoard.SelectedBoard.UndoRedoMan.AddUndoBatch(new List<UndoRedoAction>() { UndoRedoManager.ItemsLayerChanged(items, oldLayer, targetLayer.LayerNumber) });
                multiBoard.SelectedBoard.Layers[oldLayer].RecheckTileSet();
                targetLayer.RecheckTileSet();
            }
            SetLayer(layer);
        }
        #endregion

        public delegate void EmptyDelegate();

        public event EmptyDelegate CloseRequested;
        public event EmptyDelegate FirstMapLoaded;

        public static string CreateItemDescription(BoardItem item, string lineBreak)
        {
            switch (item.GetType().Name)
            {
                case "TileInstance":
                    return "Tile:" + lineBreak + ((TileInfo)item.BaseInfo).tS + @"\" + ((TileInfo)item.BaseInfo).u + @"\" + ((TileInfo)item.BaseInfo).no;
                case "ObjectInstance":
                    return "Object:" + lineBreak + ((ObjectInfo)item.BaseInfo).oS + @"\" + ((ObjectInfo)item.BaseInfo).l0 + @"\" + ((ObjectInfo)item.BaseInfo).l1 + @"\" + ((ObjectInfo)item.BaseInfo).l2;
                case "BackgroundInstance":
                    return "Background:" + lineBreak + ((BackgroundInfo)item.BaseInfo).bS + @"\" + (((BackgroundInfo)item.BaseInfo).ani ? "ani" : "back") + @"\" + ((BackgroundInfo)item.BaseInfo).no;
                case "PortalInstance":
                    return "Portal:" + lineBreak + "Name: " + ((PortalInstance)item).pn + lineBreak + "Type: " + Tables.PortalTypeNames[(int)((PortalInstance)item).pt];
                case "MobInstance":
                    return "Mob:" + lineBreak + "Name: " + ((MobInfo)item.BaseInfo).Name + lineBreak + "ID: " + ((MobInfo)item.BaseInfo).ID;
                case "NPCInstance":
                    return "Npc:" + lineBreak + "Name: " + ((NpcInfo)item.BaseInfo).Name + lineBreak + "ID: " + ((NpcInfo)item.BaseInfo).ID;
                case "ReactorInstance":
                    return "Reactor:" + lineBreak + "ID: " + ((ReactorInfo)item.BaseInfo).ID;
                case "FootholdAnchor":
                    return "Foothold";
                case "RopeAnchor":
                    return ((RopeAnchor)item).ParentRope.ladder ? "Ladder" : "Rope";
                case "Chair":
                    return "Chair";
                case "ToolTipDot":
                case "ToolTip":
                case "ToolTipChar":
                    return "Tooltip";
                default:
                    if (item is INamedMisc)
                    {
                        return ((INamedMisc)item).Name;
                    }
                    else
                    {
                        return "";
                    }
            }
        }

        public void SetTilePanel(TilePanel tp)
        {
            this.tilePanel = tp;
        }

        public void EnterEditMode(ItemTypes type)
        {
            multiBoard.SelectedBoard.EditedTypes = type;
            ribbon.SetEnabled(false);
        }

        public void ExitEditMode()
        {
            multiBoard.SelectedBoard.EditedTypes = ApplicationSettings.theoreticalEditedTypes;
            ribbon.SetEnabled(true);
        }

        public MultiBoard MultiBoard
        {
            get
            {
                return multiBoard;
            }
        }

        public HaRibbon Ribbon
        {
            get
            {
                return ribbon;
            }
        }
    }
}
