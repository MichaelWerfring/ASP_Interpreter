using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_exe;

public class ProgramConfig
{
    private string path;

    public ProgramConfig()
    {
    }

    public ProgramConfig(
        string path, 
        bool interactive,
        bool help = false,
        LogLevel logLevel = LogLevel.Error)
    {
        if (string.IsNullOrEmpty(path))
        {
            throw new ArgumentException($"'{nameof(path)}' cannot be null or empty.", nameof(path));
        }

        Path = path;
        Help = help;
        LogLevel = logLevel;
    }

    public string Path
    {
        get
        {
            return path;
        }

        set
        {
            ArgumentException.ThrowIfNullOrEmpty(nameof(value));
            path = value;
        }
    }

    public bool Help { get; set; }

    public LogLevel LogLevel { get; set; }

    public bool Interactive { get; set; }
}