using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.Util;

public static class FileReader
{
    public static string? ReadFile(string path, ILogger logger)
    {
        ArgumentException.ThrowIfNullOrEmpty(path, nameof(path));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        try
        {
            return File.ReadAllText(path);
        }
        catch (FileNotFoundException)
        {
            logger.LogError("File not Found: " + path);
        }
        catch (UnauthorizedAccessException)
        {
            logger.LogError("Access denied: " + path);
        }
        catch (IOException e)
        {
            logger.LogError("Unable to read file: " + e.Message);
        }
        
        return null; 
    }
}