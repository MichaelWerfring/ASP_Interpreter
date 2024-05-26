namespace Asp_interpreter_lib.Util
{
    public class CommandLineParser
    {
        public CommandLineParser(Dictionary<string, Func<int, ProgramConfig, string[], ProgramConfig>> actions)
        {
            // Function should take current position in args, current program config and args and return updated config
            this.actions = actions ??
                throw new ArgumentNullException(nameof(actions), "The given argument must not be null!");
        }

        private Dictionary<string, Func<int, ProgramConfig, string[], ProgramConfig>> actions;

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