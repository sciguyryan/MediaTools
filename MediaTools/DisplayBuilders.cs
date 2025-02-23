namespace MediaTools
{
    internal class DisplayBuilders
    {
        #region Base Strings

        private const string Success = "Success: ";
        private const string SuccessConfigWriteTail = "download config file successfully written!";
        private const string SuccessMoveDownloadsTail = "downloaded files successfully moved!";
        private const string SuccessDownloadTail = "[0] ([1] of [2]) successfully downloaded!";
        private const string SuccessTrashFileTail = "the file '[0]' has been sent to the trash!";
        private const string SuccessDeleteFileTail = "the file '[0]' has been deleted!";
        private const string SuccessRenameFileTail = "the file '[0]' has been renamed to '[1]'!";
        private const string SuccessRestoreFileTail = "the file '[0]' has been restored from the trash!";

        private const string Error = "Error: ";
        private const string ErrorNoValidUrlsTail = "no valid target download URLs specified.";
        private const string ErrorTrashFileTail = "failed to send the file '[0]' to the trash!";
        private const string ErrorDeleteFileTail = "failed to delete the file '[0]'!";
        private const string ErrorRenameFileTail = "failed to rename the the file '[0]'!";
        private const string ErrorSourceDirectoryMissingTail = "the source directory path '[0]' doesn't exist!";
        private const string ErrorFailedCreateDirectoryTail = "the destination path '[0]' could not be created!";
        private const string ErrorFileMoveFailedTail = "failed to move the file at '[0]' to '[1]'!";
        private const string ErrorDeleteDirectoryFailedTail = "failed to delete the directory at '[0]'!";
        private const string ErrorRestoreFileTail = "failed to restore the file '[0]' from the trash!";

        private const string Information = "Information: ";
        private const string InfoAttemptWriteConfigTail = "attempting to write download config file...";
        private const string InfoDownloadTail = "downloading [0] ([1] of [2])...";
        private const string InfoAttemptMoveDownloadsTail = "attempting to move files to specified folder...";
        private const string InfoReloadingMediaFilesTail = "reloading media file list...";
        private const string InfoMediaListReloadingTail = "media list successfully reloaded!";
        private const string InfoAbortDeleteFileTail =
            "deletion of the file '[0]' has been aborted.";
        private const string InfoShuttingDownComputerTail =
            "the computer will be shut down momentarily...";

        private const string MediaInfoDurationTitleText = "Total Media Duration";
        private const string MediaInfoDurationText =
            "There are a total of [0] files. The average duration of a file is [1] and a total length of [2].";

        private const string InvalidFileNameTitleText = "Invalid File Name";
        private const string InvalidFileNameTailText = "The file name contains one or more invalid characters!";
        private const string EmptyFileNameTailText = "The file name cannot be empty!";

        private const string ConfirmDeleteFileTitleText = "Delete File?";
        private const string ConfirmDeleteFileText =
            "Are you sure you wish to permenantly delete the file '[0]'?";

        private const string ErrorCacheFileOpenTitleText = "Cache File Already Open";
        private const string ErrorCacheFileOpenText =
            "the cache file is open in another application and can't be updated!";

        #endregion

        #region Builders

        public static readonly OutputFormatBuilder SuccessConfigWrite = new OutputFormatBuilder()
            .Foreground(ConsoleColour.Green)
            .Text(Success)
            .ResetForeground()
            .Text(SuccessConfigWriteTail);
        public static readonly OutputFormatBuilder SuccessMoveDownloads = new OutputFormatBuilder()
            .Foreground(ConsoleColour.Green)
            .Text(Success)
            .ResetForeground()
            .Text(SuccessMoveDownloadsTail);
        public static readonly OutputFormatBuilder SuccessDownload = new OutputFormatBuilder()
            .Foreground(ConsoleColour.Green)
            .Text(Success)
            .ResetForeground()
            .Text(SuccessDownloadTail);
        public static readonly OutputFormatBuilder SuccessTrashFile = new OutputFormatBuilder()
            .Foreground(ConsoleColour.Green)
            .Text(Success)
            .ResetForeground()
            .Text(SuccessTrashFileTail);
        public static readonly OutputFormatBuilder SuccessDeleteFile = new OutputFormatBuilder()
            .Foreground(ConsoleColour.Green)
            .Text(Success)
            .ResetForeground()
            .Text(SuccessDeleteFileTail);
        public static readonly OutputFormatBuilder SuccessRenameFile = new OutputFormatBuilder()
            .Foreground(ConsoleColour.Green)
            .Text(Success)
            .ResetForeground()
            .Text(SuccessRenameFileTail);
        public static readonly OutputFormatBuilder SuccessRestoreFile = new OutputFormatBuilder()
            .Foreground(ConsoleColour.Green)
            .Text(Success)
            .ResetForeground()
            .Text(SuccessRestoreFileTail);

        public static readonly OutputFormatBuilder ErrorNoValidUrls = new OutputFormatBuilder()
            .Foreground(ConsoleColour.Red)
            .Text(Error)
            .ResetForeground()
            .Text(ErrorNoValidUrlsTail);
        public static readonly OutputFormatBuilder ErrorTrashFile = new OutputFormatBuilder()
            .Foreground(ConsoleColour.Red)
            .Text(Error)
            .ResetForeground()
            .Text(ErrorTrashFileTail);
        public static readonly OutputFormatBuilder ErrorDeleteFile = new OutputFormatBuilder()
            .Foreground(ConsoleColour.Red)
            .Text(Error)
            .ResetForeground()
            .Text(ErrorDeleteFileTail);
        public static readonly OutputFormatBuilder ErrorRenameFile = new OutputFormatBuilder()
            .Foreground(ConsoleColour.Red)
            .Text(Error)
            .ResetForeground()
            .Text(ErrorRenameFileTail);
        public static readonly OutputFormatBuilder ErrorSourceDirectoryMissing = new OutputFormatBuilder()
            .Foreground(ConsoleColour.Red)
            .Text(Error)
            .ResetForeground()
            .Text(ErrorSourceDirectoryMissingTail);
        public static readonly OutputFormatBuilder ErrorFailedCreateDirectory = new OutputFormatBuilder()
            .Foreground(ConsoleColour.Red)
            .Text(Error)
            .ResetForeground()
            .Text(ErrorFailedCreateDirectoryTail);
        public static readonly OutputFormatBuilder ErrorFileMoveFailed = new OutputFormatBuilder()
            .Foreground(ConsoleColour.Red)
            .Text(Error)
            .ResetForeground()
            .Text(ErrorFileMoveFailedTail);
        public static readonly OutputFormatBuilder ErrorDeleteDirectoryFailed = new OutputFormatBuilder()
            .Foreground(ConsoleColour.Red)
            .Text(Error)
            .ResetForeground()
            .Text(ErrorDeleteDirectoryFailedTail);
        public static readonly OutputFormatBuilder ErrorRestoreFile = new OutputFormatBuilder()
            .Foreground(ConsoleColour.Red)
            .Text(Error)
            .ResetForeground()
            .Text(ErrorRestoreFileTail);

        public static readonly OutputFormatBuilder InfoMediaListReloading = new OutputFormatBuilder()
            .Foreground(ConsoleColour.Blue)
            .Text(Information)
            .ResetForeground()
            .Text(InfoMediaListReloadingTail);
        public static readonly OutputFormatBuilder InfoMediaListReloaded = new OutputFormatBuilder()
            .Foreground(ConsoleColour.Blue)
            .Text(Information)
            .ResetForeground()
            .Text(InfoReloadingMediaFilesTail);
        public static readonly OutputFormatBuilder InfoAttemptWriteConfig = new OutputFormatBuilder()
            .Foreground(ConsoleColour.Blue)
            .Text(Information)
            .ResetForeground()
            .Text(InfoAttemptWriteConfigTail);
        public static readonly OutputFormatBuilder InfoAttemptDownload = new OutputFormatBuilder()
            .Foreground(ConsoleColour.Blue)
            .Text(Information)
            .ResetForeground()
            .Text(InfoDownloadTail);
        public static readonly OutputFormatBuilder InfoAttemptMoveDownloads = new OutputFormatBuilder()
            .Foreground(ConsoleColour.Blue)
            .Text(Information)
            .ResetForeground()
            .Text(InfoAttemptMoveDownloadsTail);
        public static readonly OutputFormatBuilder InfoAbortDeleteFile = new OutputFormatBuilder()
            .Foreground(ConsoleColour.Blue)
            .Text(Information)
            .ResetForeground()
            .Text(InfoAbortDeleteFileTail);
        public static readonly OutputFormatBuilder InfoShuttingDownComputer = new OutputFormatBuilder()
            .Foreground(ConsoleColour.Blue)
            .Text(Information)
            .ResetForeground()
            .Text(InfoShuttingDownComputerTail);

        public static readonly OutputFormatBuilder MediaInfoDurationTitle = new OutputFormatBuilder()
            .Text(MediaInfoDurationTitleText);
        public static readonly OutputFormatBuilder MediaInfoDuration = new OutputFormatBuilder()
            .Text(MediaInfoDurationText);


        public static readonly OutputFormatBuilder InvalidFileNameTitle = new OutputFormatBuilder()
            .Text(InvalidFileNameTitleText);
        public static readonly OutputFormatBuilder InvalidFileName = new OutputFormatBuilder()
            .Foreground(ConsoleColour.Red)
            .Text(Error)
            .ResetForeground()
            .Text(InvalidFileNameTailText);
        public static readonly OutputFormatBuilder EmptyFileName = new OutputFormatBuilder()
            .Foreground(ConsoleColour.Red)
            .Text(Error)
            .ResetForeground()
            .Text(EmptyFileNameTailText);

        public static readonly OutputFormatBuilder ConfirmDeleteFileTitle = new OutputFormatBuilder()
            .Text(ConfirmDeleteFileTitleText);
        public static readonly OutputFormatBuilder ConfirmDeleteFile = new OutputFormatBuilder()
            .Text(ConfirmDeleteFileText);

        public static readonly OutputFormatBuilder ErrorCacheFileOpenTitle = new OutputFormatBuilder()
            .Text(ErrorCacheFileOpenTitleText);
        public static readonly OutputFormatBuilder ErrorCacheFileOpen = new OutputFormatBuilder()
            .Text(ErrorCacheFileOpenText);

        #endregion
    }
}
