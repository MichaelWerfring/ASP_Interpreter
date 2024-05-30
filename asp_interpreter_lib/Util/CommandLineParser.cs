//-----------------------------------------------------------------------
// <copyright file="CommandLineParser.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Util
{
    /// <summary>
    /// Provides utility for parsing command line arguments.
    /// </summary>
    public class CommandLineParser
    {
        private readonly Dictionary<string, Func<int, ProgramConfig, string[], ProgramConfig>> actions;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineParser"/> class.
        /// </summary>
        /// <param name="actions">The arguments string representation mapped to a function consuming one argument from the given list and returning the given program configuration.</param>
        /// <exception cref="ArgumentNullException">Is thrown if the actions are null.</exception>
        public CommandLineParser(Dictionary<string, Func<int, ProgramConfig, string[], ProgramConfig>> actions)
        {
            // Function should take current position in args, current program config and args and return updated config
            this.actions = actions ??
                throw new ArgumentNullException(nameof(actions), "The given argument must not be null!");
        }

        /// <summary>
        /// Parses the given command line arguments and returns the corresponding program configuration.
        /// </summary>
        /// <param name="args">The arguments to be parsed.</param>
        /// <returns>The corresponding program configuration.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the args are null.</exception>
        public ProgramConfig Parse(string[] args)
        {
            ArgumentNullException.ThrowIfNull(args);

            ProgramConfig conf = new ProgramConfig();
            for (int i = 0; i < args.Length; i++)
            {
                var arg = args[i];

                Func<int, ProgramConfig, string[], ProgramConfig> action;
                if (!this.actions.TryGetValue(arg, out action))
                {
                    // just skip unknown for simplicity
                    continue;
                }

                action.Invoke(i, conf, args);
            }

            return conf;
        }
    }
}