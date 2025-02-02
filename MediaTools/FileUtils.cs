using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;

namespace MediaTools
{
    internal class FileUtils(string basePath)
    {
        #region DLL Imports

        private const uint FO_MOVE = 0x0001; // Move a file.
        private const uint FO_DELETE = 0x0003; // Trash a file.
        private const uint FOF_NOCONFIRMATION = 0x0010; // Show no confirmation dialog box to the user.
        private const uint FOF_ALLOWUNDO = 0x0040; // Preserve undo information, if possible.
        private const uint FOF_RESTORE = 0x1000; // Restore a file from the trash.

        // Struct which contains information that the SHFileOperation
        // function uses to perform file operations.
        // See: https://learn.microsoft.com/en-us/windows/win32/api/shellapi/ns-shellapi-shfileopstructw
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct SHFILEOPSTRUCTW
        {
            public IntPtr hWnd;
            [MarshalAs(UnmanagedType.U4)]
            public uint wFunc;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pFrom;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pTo;
            [MarshalAs(UnmanagedType.U4)]
            public uint fFlags;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fAnyOperationsAborted;
            public IntPtr hNameMappings;
            public string lpszProgressTitle;
        }

        // Struct which contains information that the SHQueryRecycleBinW
        // function uses to perform recycling bin queries.
        // See: https://learn.microsoft.com/en-us/windows/win32/api/shellapi/nf-shellapi-shqueryrecyclebinw
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct LPSHQUERYRBINFO
        {
            [MarshalAs(UnmanagedType.U4)]
            public uint cbSize;
            [MarshalAs(UnmanagedType.U8)]
            public ulong i64Size;
            [MarshalAs(UnmanagedType.U8)]
            public ulong i64NumItems;
        }

        [DllImport("shell32.dll", EntryPoint = "SHFileOperationW", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int SHFileOperationW(ref SHFILEOPSTRUCTW lpFileOp);

        [DllImport("shell32.dll", EntryPoint = "SHQueryRecycleBinW", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int SHQueryRecycleBinW(string pszRootPath, ref LPSHQUERYRBINFO pSHQueryRBInfo);

        [DllImport("shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SHGetSpecialFolderPath(IntPtr hwnd, StringBuilder lpszPath, int nFolder, bool fCreate);

        #endregion

        private const string RecycleBinMetadataFilePrefix = "$I";
        private const string RecycleBinBackupFilePrefix = "$R";

        public static int TrashPath(string path)
        {
            // Note that the specification requires a double null termination here.
            // See: https://learn.microsoft.com/en-us/windows/win32/api/shellapi/ns-shellapi-shfileopstructa
            var fileOp = new SHFILEOPSTRUCTW
            {
                wFunc = FO_DELETE,
                pFrom = $"{path}\0\0",
                fFlags = FOF_ALLOWUNDO | FOF_NOCONFIRMATION
            };

            return SHFileOperationW(ref fileOp);
        }

        public static int TryUntrashPath(string path)
        {
            MessageBox.Show(path);

            var rbInfo = new LPSHQUERYRBINFO
            {
                cbSize = (uint)Marshal.SizeOf(typeof(LPSHQUERYRBINFO))
            };

            // Query the Recycle Bin for any files.
            var result = SHQueryRecycleBinW("", ref rbInfo);
            if (result != 0)
            {
                return result;
            }

            // Note that the specification requires a double null termination here.
            // See: https://learn.microsoft.com/en-us/windows/win32/api/shellapi/ns-shellapi-shfileopstructa
            var fileOp = new SHFILEOPSTRUCTW
            {
                wFunc = FO_MOVE,
                pFrom = $"{path}\0\0",
                pTo = $"{path}\0\0",
                fFlags = FOF_RESTORE | FOF_ALLOWUNDO
            };

            result = SHFileOperationW(ref fileOp);
            MessageBox.Show(result.ToString());

            return result;
        }

        public static string? GetRecyclePathForPath(string path)
        {
            // Get the recycling bin for the current user.
            var sid =
                WindowsIdentity.GetCurrent().Owner ?? throw new Exception("Could not retrieve SID for current user.");
            var recycleBinPath = Path.Combine(@"C:\$Recycle.Bin", sid.ToString());

            foreach (var entry in Directory.EnumerateFileSystemEntries(recycleBinPath, RecycleBinBackupFilePrefix + "*"))
            {
                var metaDataFileName = 
                    RecycleBinMetadataFilePrefix + 
                    Path.GetFileName(entry)[RecycleBinBackupFilePrefix.Length..];

                var metadataFilePath = Path.Combine(recycleBinPath, metaDataFileName);


            }
        }

        public void EnsureTempExists()
        {
            var tempDir = GetTempPath();
            if (!Directory.Exists(tempDir))
            {
                Directory.CreateDirectory(tempDir);
            }
        }

        public void MoveTempFiles(string? destFolder)
        {
            // Wait 1.5 seconds to ensure things are fully finished before attempting
            // to move the files. This prevents incomplete files being moved.
            Thread.Sleep(1500);

            var mediaDir = GetMediaPath();
            if (destFolder is not null)
            {
                mediaDir = Path.Combine(mediaDir, destFolder);
            }

            MoveDirectoryContents(GetTempPath(), mediaDir);
        }

        private bool MoveDirectoryContents(string sourceDir, string destDir)
        {
            if (!Directory.Exists(sourceDir))
            {
                var builder = DisplayBuilders.ErrorSourceDirectoryMissing;
                Console.WriteLine(builder.Build([sourceDir]));
                return false;
            }

            if (!Directory.Exists(destDir))
            {
                try
                {
                    Directory.CreateDirectory(destDir);
                }
                catch
                {
                    var builder = DisplayBuilders.ErrorFailedCreateDirectory;
                    Console.WriteLine(builder.Build([destDir]));
                    return false;
                }
            }

            // Move subdirectories recursively.
            var directories = Directory.GetDirectories(sourceDir);
            foreach (var directory in directories)
            {
                var dirName = Path.GetFileName(directory);
                var destDirectory = Path.Combine(destDir, dirName);
                if (!MoveDirectoryContents(directory, destDirectory))
                {
                    return false;
                }
            }

            // Move the individual files.
            var files = Directory.GetFiles(sourceDir);
            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);
                var destFile = Path.Combine(destDir, fileName);

                try
                {
                    File.Move(file, destFile, true);
                }
                catch
                {

                    var builder = DisplayBuilders.ErrorFileMoveFailed;
                    Console.WriteLine(builder.Build([file, destFile]));
                    return false;
                }
            }

            try
            {
                if (sourceDir != GetTempPath())
                {
                    Directory.Delete(sourceDir);
                }
            }
            catch
            {
                var builder = DisplayBuilders.ErrorDeleteDirectoryFailed;
                Console.WriteLine(builder.Build([sourceDir]));
                return false;
            }

            return true;
        }

        public string GetTempPath()
        {
            return Path.Combine(basePath, "temp");
        }

        public string GetMediaPath()
        {
            return Path.GetFullPath(Path.Combine(basePath, "..\\"));
        }

        public static void TruncateFile(string path)
        {
            using var f = File.Open(path, FileMode.Truncate);
        }

        public static bool IsFileLocked(string path)
        {
            if (!File.Exists(path))
            {
                // A file that doesn't exist can't be locked.
                return false;
            }

            FileStream? stream = null;

            try
            {
                stream = File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                stream?.Close();
            }

            return false;
        }

        public bool IsMediaInRoot(MediaFileEntry entry)
        {
            var entryDir = new FileInfo(entry.FullPath).DirectoryName!;

            var path1Trimmed =
                Path.GetFullPath(entryDir)
                    .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            var path2Trimmed =
                Path.GetFullPath(GetMediaPath())
                    .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            return string.Equals(path1Trimmed, path2Trimmed, StringComparison.OrdinalIgnoreCase);
        }
    }
}
