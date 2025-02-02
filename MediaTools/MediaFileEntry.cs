namespace MediaTools
{
    internal class MediaFileEntry(
        int rawDuration,
        DateTime lastModified,
        FileInfo fileInfo)
    {
        private FileInfo FileInfo { get; } = fileInfo;

        public int RawDuration { get; set; } = rawDuration;

        private string? _duration;
        public string Duration => _duration ??= Utils.SecondsToDuration(RawDuration, false);

        public DateTime LastModified => lastModified;

        private string? _folder;
        public string Folder => _folder ??= Utils.TruncateString(FileInfo.Directory!.Name);

        public string Title => Path.GetFileNameWithoutExtension(FileInfo.FullName);

        public string FullPath => FileInfo.FullName;

        private string? _hash;
        public string Hash => _hash ??= Utils.ComputeMd5Hash(FileInfo.FullName);
    }
}
