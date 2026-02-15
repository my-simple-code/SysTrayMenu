namespace SysTrayMenu
{
    internal class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new SysTrayMenu());
        }
    }
}
