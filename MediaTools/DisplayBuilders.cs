namespace MediaTools
{
    internal class DisplayBuilders
    {
        #region Base Strings

        private const string Success = "Success:";
        private const string SuccessMediaListTail = " media list successfully reloaded!";
        private const string SuccessConfigWriteTail = " download config file successfully written!";
        private const string SuccessMoveDownloadsTail = " downloaded files successfully moved!";
        private const string SuccessDownloadTail = " [0] [1] successfully downloaded!";
        private const string SuccessTrashFileTail = " file '[0]' has been sent to the trash!";
        private const string SuccessDeleteFileTail = " file '[0]' has been deleted!";

        private const string Error = "Error:";
        private const string ErrorNoValidUrlsTail = " no valid target download URLs specified.";
        private const string ErrorTrashFileTail = " failed to send file '[0]' to the trash!";
        private const string ErrorDeleteFileTail = " failed to delete file '[0]'!";

        private const string AttemptWriteConfigText = "Attempting to write download config file...";
        private const string AttemptingDownloadText = "Downloading [0] [1] of [2]...";
        private const string AttemptingMoveDownloadsText = "Attempting to move files to specified folder...";
        private const string ReloadingMediaFilesText = "Reloading media file list...";

        public const string MediaInfoDurationTitle = "Total Media Duration";
        public const string MediaInfoDurationText =
            "There are a total of [0] files. The average duration of a file is [1] and a total length of [2].";

        #endregion

        #region Builders

        public static readonly OutputFormatBuilder SuccessReload = new OutputFormatBuilder()
            .Foreground(ConsoleColour.Green)
            .Text(Success)
            .ResetForeground()
            .Text(SuccessMediaListTail);
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

        public static readonly OutputFormatBuilder AttemptWriteConfig = new OutputFormatBuilder()
            .Text(AttemptWriteConfigText);
        public static readonly OutputFormatBuilder AttemptingDownload = new OutputFormatBuilder()
            .Text(AttemptingDownloadText);
        public static readonly OutputFormatBuilder AttemptingMoveDownloads = new OutputFormatBuilder()
            .Text(AttemptingMoveDownloadsText);
        public static readonly OutputFormatBuilder ReloadingMediaFiles = new OutputFormatBuilder()
            .Text(ReloadingMediaFilesText);

        public static readonly OutputFormatBuilder MediaInfoDuration = new OutputFormatBuilder()
            .Text(MediaInfoDurationText);

        #endregion
    }
}
