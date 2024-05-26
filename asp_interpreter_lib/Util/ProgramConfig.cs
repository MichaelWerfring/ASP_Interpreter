namespace Asp_interpreter_lib.Util
{
    using Asp_interpreter_lib.Util.ErrorHandling;

    public class ProgramConfig
    {
        private string filePath;

        public ProgramConfig()
        {
        }

        public ProgramConfig(
            string filePath,
            bool showExplanation,
            bool runInteractive,
            bool logTimestamp,
            bool displayHelp = false,
            LogLevel logLevel = LogLevel.Error)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException($"'{nameof(filePath)}' cannot be null or empty.", nameof(filePath));
            }

            this.FilePath = filePath;
            this.DisplayExplanation = showExplanation;
            this.RunInteractive = runInteractive;
            this.LogTimestamp = logTimestamp;
            this.DisplayHelp = displayHelp;
            this.LogLevel = logLevel;
        }

        public string FilePath
        {
            get
            {
                return this.filePath;
            }

            set
            {
                ArgumentException.ThrowIfNullOrEmpty(nameof(value));
                this.filePath = value;
            }
        }

        public bool DisplayExplanation { get; set; }

        public bool LogTimestamp { get; set; }

        public bool DisplayHelp { get; set; }

        public LogLevel LogLevel { get; set; }

        public bool RunInteractive { get; set; }
    }
}