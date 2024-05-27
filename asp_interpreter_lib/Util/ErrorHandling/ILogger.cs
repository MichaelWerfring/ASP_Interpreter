//-----------------------------------------------------------------------
// <copyright file="ILogger.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Util.ErrorHandling
{
    using Antlr4.Runtime;

    public interface ILogger
    {
        void LogTrace(string message);

        void LogDebug(string message);

        void LogInfo(string message);

        void LogError(string message);

        void LogError(string message, ParserRuleContext context);

        ILogger GetDummy();
    }
}