//-----------------------------------------------------------------------
// <copyright file="LogLevels.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Util.ErrorHandling
{
    /// <summary>
    /// Represents the different log levels.
    /// </summary>
    public enum LogLevels
    {
        /// <summary>
        /// Log level to display all the log messages.
        /// </summary>
        Trace = 0,

        /// <summary>
        /// Log level to display messages for debugging the interpreter.
        /// </summary>
        Debug = 1,

        /// <summary>
        /// Log level to display information and current state of the interpreter.
        /// </summary>
        Info = 2,

        /// <summary>
        /// Log level to display error messages regarding input programs.
        /// </summary>
        Error = 3,

        /// <summary>
        /// Log level to display no messages at all.
        /// </summary>
        None = 4,
    }
}