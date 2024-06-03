//-----------------------------------------------------------------------
// <copyright file="ProgramConfig.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Util
{
    using Asp_interpreter_lib.Util.ErrorHandling;

    /// <summary>
    /// Represents the configuration for the program.
    /// </summary>
    public class ProgramConfig
    {
        private string filePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgramConfig"/> class.
        /// </summary>
        public ProgramConfig()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgramConfig"/> class.
        /// </summary>
        /// <param name="filePath">A path providing the program file.</param>
        /// <param name="showExplanation">A boolean value indicating whether the program should be explained textually.</param>
        /// <param name="runInteractive">A boolean value indicating whether the interpreter should be run in interactive mode.</param>
        /// <param name="logTimestamp">A boolean value indicating whether time should be logged with every log message.</param>
        /// <param name="displayHelp">A boolean value indicating whether help should be displayed for the user.</param>
        /// <param name="logLevel">The log level to initialize the interpreter with.</param>
        /// <exception cref="ArgumentException">Is thrown if the file path is null or empty.</exception>
        public ProgramConfig(
            string filePath,
            bool showExplanation,
            bool runInteractive,
            bool logTimestamp,
            bool displayHelp,
            LogLevels logLevel = LogLevels.Error)
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

        /// <summary>
        /// Gets or sets the file path to provide the program file.
        /// </summary>
        /// <exception cref="ArgumentException">Is thrown if the file path is null or empty.</exception>
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

        /// <summary>
        /// Gets or sets a value indicating whether the program should be explained textually.
        /// </summary>
        public bool DisplayExplanation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether time should be logged with every log message.
        /// </summary>
        public bool LogTimestamp { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether help should be displayed for the user.
        /// </summary>
        public bool DisplayHelp { get; set; }

        /// <summary>
        /// Gets or sets the log level for the interpreter.
        /// </summary>
        public LogLevels LogLevel { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the interpreter should be run in interactive mode.
        /// </summary>
        public bool RunInteractive { get; set; }
    }
}