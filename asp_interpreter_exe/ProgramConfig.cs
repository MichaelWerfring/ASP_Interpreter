namespace asp_interpreter_exe;

public class ProgramConfig
{
    public ProgramConfig(
        string path, 
        bool help = false,
        int logLevel = 3)
    {
        if (string.IsNullOrEmpty(path))
        {
            throw new ArgumentException($"'{nameof(path)}' cannot be null or empty.", nameof(path));
        }

        Path = path;
        Help = help;
        LogLevel = logLevel;
    }

    public string Path { get; private set; }

    public bool Help { get; private set; }

    public int LogLevel { get; private set; }
}