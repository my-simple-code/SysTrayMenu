using System.Diagnostics;
using System.Runtime.InteropServices;


namespace SysTrayMenu
{
    public static class WinShell
    {
        private static Icon? folderIcon;
        private static Image? folderImage;

        public static Icon FolderIcon
        {
            get { return folderIcon != null ? folderIcon : folderIcon = System.Drawing.SystemIcons.GetStockIcon(StockIconId.Folder); }
        }

        public static Image FolderImage
        {
            get { return folderImage != null ? folderImage : folderImage = FolderIcon.ToBitmap(); }
        }

        public static string DesktopFolder
        {
            get { return Environment.GetFolderPath(Environment.SpecialFolder.Desktop); }
        }

        public static void SetForeground(Control control)
        {
            SetForegroundWindow(new HandleRef(control, control.Handle));
        }

        public static void ViewInExplorer(string path)
        {
            if (path != null)
            {
                if (Directory.Exists(path))
                {
                    Process.Start("explorer.exe", $"\"{ path }\"");
                }
                else if (File.Exists(path))
                {
                    Process.Start("explorer.exe", $"/select, \"{ path }\"");
                }
            }
        }

        public static void Execute(string path)
        {
            if (path != null && File.Exists(path))
            {
                Process process = new Process() { StartInfo = new ProcessStartInfo() { UseShellExecute = true, FileName = path } };
                try
                {
                    process.Start();
                }
                catch (Exception) { }
            }
        }

        [DllImport("User32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        private static extern bool SetForegroundWindow(HandleRef hWnd);

    }
}
