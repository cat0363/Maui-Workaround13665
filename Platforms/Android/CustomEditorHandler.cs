﻿using Android.Text.Method;
using Android.Views;
using AndroidX.AppCompat.Widget;
using Microsoft.Maui.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Android.Resource;

namespace Maui_Workaround13665.Platforms.Android 
{

    public class CustomEditorHandler : EditorHandler 
    {
        /// <summary>
        /// Key Listener
        /// </summary>
        public IKeyListener KeyListener { get; set; }
        /// <summary>
        /// Custom Selection Action Mode Callback
        /// </summary>
        public ActionMode.ICallback CustomSelectionActionModeCallback { get; set; }
        /// <summary>
        /// Custom Mapper
        /// </summary>
        public static PropertyMapper<Editor, CustomEditorHandler> CustomMapper = new PropertyMapper<Editor, CustomEditorHandler>() 
        {
            [nameof(Editor.IsReadOnly)] = MapIsReadOnly,
        };

        /// <summary>
        /// Is ReadOnly Map
        /// </summary>
        /// <param name="handler">Handler</param>
        /// <param name="virtualView">Virtual View</param>
        static void MapIsReadOnly(EditorHandler handler, Editor virtualView) 
        {
            // Get Platform View
            var platformView = handler.PlatformView;

            if (virtualView.IsReadOnly) 
            {
                // Initialize Key Listener
                platformView.KeyListener = null;
                // Set Custom Selection Action Mode Callback
                platformView.CustomSelectionActionModeCallback = new CustomSelectionActionModeCallback();
            }
            else 
            {
                // Resetting Key Listener
                platformView.KeyListener = ((CustomEditorHandler)virtualView.Handler).KeyListener;
                // Resetting Custom Selection Action Mode Callback
                platformView.CustomSelectionActionModeCallback = ((CustomEditorHandler)virtualView.Handler).CustomSelectionActionModeCallback;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public CustomEditorHandler() : base(CustomMapper) 
        {

        }

        /// <summary>
        /// Create Platform View
        /// </summary>
        /// <returns>Platform View</returns>
        protected override AppCompatEditText CreatePlatformView() 
        {
            // Create Platform View
            var platformView = base.CreatePlatformView();
            // Evacuates Key Listener (Default)
            this.KeyListener = platformView.KeyListener;
            // Evacuates Custom Selection Action Mode Callback (Default)
            this.CustomSelectionActionModeCallback = platformView.CustomSelectionActionModeCallback;
            // Set Text
            platformView.Text = VirtualView.Text;
            // Set Text Selectable
            platformView.SetTextIsSelectable(true);

            return platformView;
        }
    }

    public class CustomSelectionActionModeCallback : ActionMode.Callback2 
    {
        /// <summary>
        /// Action Item Clicked
        /// </summary>
        /// <param name="mode">Action Mode</param>
        /// <param name="item">Menu Item</param>
        /// <returns></returns>
        public override bool OnActionItemClicked(ActionMode mode, IMenuItem item) 
        {
            return false;
        }

        /// <summary>
        /// Prepare Action Mode
        /// </summary>
        /// <param name="mode">Action Mode</param>
        /// <param name="menu">Menu</param>
        /// <returns>Result</returns>
        public override bool OnPrepareActionMode(ActionMode mode, IMenu menu) 
        {
            try 
            {
                // Search Copy Menu Item
                IMenuItem copyItem = menu.FindItem(Id.Copy);
                // Search Select ALL Menu Item
                IMenuItem selectAllItem = menu.FindItem(Id.SelectAll);
                // Menu Items Clear
                menu.Clear();
                // Add Copy Menu Item
                menu.Add(0, Id.Copy, 0, copyItem.ToString());
                // Add Select All Menu Item
                menu.Add(0, Id.SelectAll, 1, selectAllItem.ToString());
            }
            catch (Exception) 
            {
                ;
            }
            return true;
        }

        /// <summary>
        /// Create Action Mode
        /// </summary>
        /// <param name="mode">Action Mode</param>
        /// <param name="menu">Menu</param>
        /// <returns>Result</returns>
        public override bool OnCreateActionMode(ActionMode mode, IMenu menu) 
        {
            return true;
        }

        /// <summary>
        /// Destroy Action Mode
        /// </summary>
        /// <param name="mode">Action Mode</param>
        public override void OnDestroyActionMode(ActionMode mode) 
        {

        }
    }
}
