using Asp_interpreter_lib.Util.ErrorHandling;

namespace Asp_interpreter_lib.Util;

public class ProgramConfig
{
    private string path;

    public ProgramConfig()
    {
    }

    public ProgramConfig(
        string path, 
        bool explain,
        bool interactive,
        bool timestamp,
        bool help = false,
        LogLevel logLevel = LogLevel.Error)
    {
        if (string.IsNullOrEmpty(path))
        {
            throw new ArgumentException($"'{nameof(path)}' cannot be null or empty.", nameof(path));
        }

        Path = path;
        Explain = explain;
        Interactive = interactive;
        Timestamp = timestamp;
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

    public bool Explain { get; set; }
    public bool Timestamp { get; set; }
    public bool Help { get; set; }

    public LogLevel LogLevel { get; set; }

    public bool Interactive { get; set; }
}