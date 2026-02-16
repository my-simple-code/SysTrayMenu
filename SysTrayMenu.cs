namespace SysTrayMenu
{
    public class SysTrayMenu : ApplicationContext
    {
        private string paramFolderPath = WinShell.DesktopFolder;
        private StockIconId paramAppIconId = StockIconId.Folder;
        private DirectoryItem mainDirectoryItem;
        private ContextMenuStrip mainMenuStrip;
        private NotifyIcon trayIcon;

        public SysTrayMenu()
        {
            ReadParameters();
            mainDirectoryItem = DirectoryItem.Folder(paramFolderPath);

            mainMenuStrip = new ContextMenuStrip();
            DynamicMenu.ConfigMenu(mainMenuStrip, mainDirectoryItem);

            trayIcon = new NotifyIcon();
            ConfigTrayIcon(trayIcon);

            trayIcon.ContextMenuStrip = new ContextMenuStrip();
            ConfigContextMenu(trayIcon.ContextMenuStrip);

            trayIcon.Visible = true;
        }

        private void ReadParameters()
        {
            var args = Environment.GetCommandLineArgs();
            if (args.Length > 1 && Directory.Exists(args[1]))
            {
                paramFolderPath = args[1];
            }

            if (args.Length > 2 && int.TryParse(args[2], out _) && Enum.IsDefined(typeof(StockIconId), int.Parse(args[2])))
            {
                paramAppIconId = (StockIconId) int.Parse(args[2]);
            }
        }

        private void ConfigTrayIcon(NotifyIcon trayIcon)
        {
            trayIcon.Text = mainDirectoryItem.Name;
            trayIcon.Icon = SystemIcons.GetStockIcon(paramAppIconId);
            trayIcon.MouseClick += TrayIcon_MouseClick;
        }

        private void ConfigContextMenu(ContextMenuStrip contextMenu)
        {
            if (mainDirectoryItem.Path != WinShell.DesktopFolder)
            {
                var directoryItem = DirectoryItem.Folder(WinShell.DesktopFolder);
                var menuItem = new ToolStripMenuItem(directoryItem.Name, SystemIcons.GetStockIcon(StockIconId.DesktopPC).ToBitmap());
                DynamicMenu.ConfigMenu((ToolStripDropDownMenu) menuItem.DropDown, directoryItem);

                contextMenu.Items.Add(menuItem);
                contextMenu.Items.Add($"Open {mainDirectoryItem.Name} in Explorer", WinShell.FolderImage, (s, e) => WinShell.OpenInExplorer(mainDirectoryItem.Path));
            }
            
            contextMenu.Items.Add(new ToolStripMenuItem("Exit", SystemIcons.GetStockIcon(StockIconId.Delete).ToBitmap(), Exit));
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
                WinShell.SetForeground(mainMenuStrip);
                mainMenuStrip.Show(Cursor.Position);
            }
        }
    }
}
