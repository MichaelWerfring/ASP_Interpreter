using asp_interpreter_lib.Util.ErrorHandling;
using asp_interpreter_lib.Util.ErrorHandling.Either;

namespace asp_interpreter_lib.Util;

public static class FileReader
{
    public static IEither<string, string> ReadFile(string path)
    {
        ArgumentException.ThrowIfNullOrEmpty(path, nameof(path));

        try
        {
            return new Right<string, string>(File.ReadAllText(path));
        }
        catch (FileNotFoundException)
        {
            return new Left<string, string>("File not Found: " + path);
        }
        catch (UnauthorizedAccessException)
        {
            return new Left<string, string>("Access denied: " + path);
        }
        catch (IOException e)
        {
            return new Left<string, string>("Unable to read file: " + e.Message);
        }
    }
}