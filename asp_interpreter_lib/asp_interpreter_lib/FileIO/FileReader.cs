using System.Net;

namespace asp_interpreter_lib.FileIO;

public static class FileReader
{
    public static FileReadResult ReadFile(string path)
    {
        FileReadResult result;

        try
        {
            var text = File.ReadAllText(path);
            result = new FileReadResult(text, true, "Success");
        }
        catch (FileNotFoundException)
        {
            result = new FileReadResult(
                string.Empty,
                false, 
                "File not found: " + path);
        }
        catch (UnauthorizedAccessException)
        {
            result = new FileReadResult(
                string.Empty,
                false, 
                "Access denied: " + path);
        }
        catch (IOException e)
        {
            result = new FileReadResult(
                string.Empty,
                false, 
                "Error reading file: " + e.Message);
        }
        
        return result; 
    }
}