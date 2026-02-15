namespace SysTrayMenu
{
    public class SysTrayMenu : ApplicationContext
    {
        private DirectoryItem rootDirectoryItem;
        private ContextMenuStrip mainMenu;
        private NotifyIcon trayIcon;

        public SysTrayMenu()
        {
            var args = Environment.GetCommandLineArgs();
            var rootPath = args.Length > 1 && Directory.Exists(args[1]) ? args[1] : WinShell.DesktopFolder;
            rootDirectoryItem = DirectoryItem.Folder(rootPath);

            mainMenu = new ContextMenuStrip();
            DynamicMenu.ConfigMenu(mainMenu, rootDirectoryItem);

            trayIcon = new NotifyIcon();
            ConfigTrayIcon(trayIcon);

            trayIcon.ContextMenuStrip = new ContextMenuStrip();
            ConfigContextMenu(trayIcon.ContextMenuStrip);

            trayIcon.Visible = true;
        }

        private void ConfigTrayIcon(NotifyIcon trayIcon)
        {
            trayIcon.Text = rootDirectoryItem.Name;
            trayIcon.Icon = WinShell.FolderIcon;
            trayIcon.MouseClick += TrayIcon_MouseClick;
        }

        private void ConfigContextMenu(ContextMenuStrip contextMenu)
        {
            if (rootDirectoryItem.Path != WinShell.DesktopFolder)
            {
                var directoryItem = DirectoryItem.Folder(WinShell.DesktopFolder);
                var menuItem = new ToolStripMenuItem(directoryItem.Name, WinShell.FolderImage);
                DynamicMenu.ConfigMenu((ToolStripDropDownMenu) menuItem.DropDown, directoryItem);

                contextMenu.Items.Add(menuItem);
                contextMenu.Items.Add($"Open {rootDirectoryItem.Name}", null, (s, e) => WinShell.ViewInExplorer(rootDirectoryItem.Path));
            }
            
            contextMenu.Items.Add(new ToolStripMenuItem("Exit", null, Exit));
        }

        private void Exit(object? sender, EventArgs e)
        {
            trayIcon.Visible = false;
            Application.Exit();
        }
        private void TrayIcon_MouseClick(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                WinShell.SetForeground(mainMenu);
                mainMenu.Show(Cursor.Position);
            }
        }
    }
}
