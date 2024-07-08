namespace MediaTools
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            var folderIndex = Array.FindIndex(args, x => x is "-f");
            var folder = (folderIndex > -1 && folderIndex < args.Length) ? args[folderIndex] : ".\\";
            var path = Path.GetFullPath(folder);
            if (!Directory.Exists(path))
            {
                Console.WriteLine(@"The specified folder doesn't exist.");
                return;
            }

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1(path));
        }
    }
}