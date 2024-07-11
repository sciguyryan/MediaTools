using System.Runtime.InteropServices;

namespace MediaTools
{
    internal partial class Interop
    {
        #region DLL Imports

        public const int SW_HIDE = 0;
        public const int SW_SHOW = 5;
        private const int STD_OUTPUT_HANDLE = -11;
        private const int ENABLE_VIRTUAL_TERMINAL_PROCESSING = 4;

        [LibraryImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool AllocConsole();

        [LibraryImport("kernel32.dll")]
        public static partial IntPtr GetConsoleWindow();

        [LibraryImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]

        public static partial bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [LibraryImport("kernel32.dll", SetLastError = true)]
        public static partial IntPtr GetStdHandle(int nStdHandle);

        [LibraryImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [LibraryImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        #endregion

        public static void SetConsoleMode()
        {
            var handle = GetStdHandle(STD_OUTPUT_HANDLE);

            if (!GetConsoleMode(handle, out var mode))
            {
                Console.Error.WriteLine("Error: failed to get console mode");
                return;
            }

            mode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING;
            if (!SetConsoleMode(handle, mode))
            {
                Console.Error.WriteLine("Failed to set console mode");
            }
        }
    }
}
