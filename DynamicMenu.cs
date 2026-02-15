namespace SysTrayMenu
{
    internal static class DynamicMenu
    {
        internal static void ConfigMenu(ToolStripDropDownMenu? menu, DirectoryItem directoryItem)
        {
            if (menu != null)
            {
                menu.Items.Add(new ToolStripMenuItem());
                menu.Tag = directoryItem;
                menu.Closing += Closing;
                menu.Opening += Opening;
                menu.MouseHover += MouseHover;
            }
        }
        internal static void SetMenuItems(ToolStripDropDownMenu? menuItem)
        {
            if (menuItem != null)
            {
                DirectoryItem? fileItem = menuItem.Tag as DirectoryItem;
                if (fileItem != null)
                {
                    menuItem.Items.Clear();
                    menuItem.Items.AddRange(DirectoryReader.GetDirectoryItems(fileItem.Path).Select(x => CreateMenuItem(x)).ToArray());
                    if (menuItem.Items.Count == 0)
                    {
                        menuItem.Items.Add(new ToolStripMenuItem("Empty", null));
                    }
                }
            }
        }

        internal static ToolStripMenuItem CreateMenuItem(DirectoryItem directoryItem)
        {
            var image = directoryItem.IsFile ? Icon.ExtractAssociatedIcon(directoryItem.Path)?.ToBitmap() : WinShell.FolderImage;
            var menuItem = new ToolStripMenuItem(directoryItem.Name, image);
            menuItem.Tag = directoryItem;
            menuItem.MouseDown += MenuItem_MouseDown;
            if (!directoryItem.IsFile)
            {
                ConfigMenu(menuItem.DropDown as ToolStripDropDownMenu, directoryItem);
            }
            return menuItem;
        }
        internal static void SetForegroundTopParent(Control? control)
        {
            if (control != null)
            {
                while (control.Parent != null)
                {
                    control = control.Parent;
                }
                WinShell.SetForeground(control);
            }
        }

        internal static void Opening(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            ToolStripDropDownMenu? clickedItem = sender as ToolStripDropDownMenu;
            SetMenuItems(clickedItem);
        }

        internal static void Closing(object? sender, ToolStripDropDownClosingEventArgs e)
        { 
            if ((Control.ModifierKeys & Keys.Control) != 0)
            {
                e.Cancel = true;
                SetForegroundTopParent(sender as Control);
            }
        }

        private static void MouseHover(object? sender, EventArgs e)
        {
            SetForegroundTopParent(sender as Control);
        }

        internal static void MenuItem_MouseDown(object? sender, MouseEventArgs e)
        {
            var menuItem = sender as ToolStripMenuItem;
            if (menuItem != null)
            {
                var directoryItem = menuItem.Tag as DirectoryItem;
                if (directoryItem != null)
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        WinShell.ViewInExplorer(directoryItem.Path);
                    }
                    else if (e.Button == MouseButtons.Left && directoryItem.IsFile)
                    {
                         WinShell.Execute(directoryItem.Path);
                    }
                }
            }
        }
    }
}
