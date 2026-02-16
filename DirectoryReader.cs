using System.Linq;

namespace SysTrayMenu
{
    internal class DirectoryItem
    {
        private List<string> hiddenExtensions = [".lnk", ".url"];
        private  DirectoryItem(string path, bool isFile)
        {
            var fileInfo = new FileInfo(path);
            Path = path;
            Name = isFile && hiddenExtensions.Contains(fileInfo.Extension) ? System.IO.Path.GetFileNameWithoutExtension(path) : fileInfo.Name;
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
