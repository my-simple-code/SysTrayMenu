namespace SysTrayMenu
{
    internal class DirectoryItem
    {
        private  DirectoryItem(string path, bool isFile)
        {
            Path = path;
            Name = isFile && (path.ToLower().EndsWith(".lnk") || path.ToLower().EndsWith(".url")) ? System.IO.Path.GetFileNameWithoutExtension(path) : new FileInfo(path).Name;
            IsFile = isFile;
        }
        internal string Path { get; private set; }
        internal string Name { get; private set; }
        internal bool IsFile { get; private set; }
        internal static DirectoryItem Folder(string path) { return new DirectoryItem(path, false);  }
        internal static DirectoryItem File(string path) { return new DirectoryItem(path, true); }
    }
    internal static class DirectoryReader
    {
        internal static IList<DirectoryItem> GetDirectoryItems(string path)
        {
            var items = Directory.GetDirectories(path).Select(x => DirectoryItem.Folder(x)).ToList();
            items.AddRange(Directory.GetFiles(path).Select(x => DirectoryItem.File(x)).ToList());
            return items;
        }
    }
}
