using asp_interpreter_exe;
using asp_interpreter_lib.Util.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace asp_interpreter_lib.Util
{
    public class CommandLineParser(Dictionary<string, Func<int, ProgramConfig, string[], ProgramConfig>> actions)
    {
        //Function should take current position in args, current program config and args and return updated config
        private Dictionary<string, Func<int, ProgramConfig, string[], ProgramConfig>> _actions = actions ??
            throw new ArgumentNullException(nameof(actions),"The given argument must not be null!");

        public ProgramConfig Parse(string[] args)
        {
            ProgramConfig config = new();
            for (int i = 0; i < args.Length; i++)
            {
                var arg = args[i];

                Func<int, ProgramConfig, string[], ProgramConfig> action;
                if (!_actions.TryGetValue(arg, out action))
                {
                    //just skip unknown for simplicity
                    continue;
                }
                
                action.Invoke(i,config, args);
            }

            return config;
        }

        
    }
}
