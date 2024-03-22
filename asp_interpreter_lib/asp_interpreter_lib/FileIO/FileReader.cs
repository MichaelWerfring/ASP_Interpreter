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
        catch (Exception e)
        {
            result = new FileReadResult(
                string.Empty,
                false, 
                "Error reading file: " + e.Message);
        }
        
        return result; 
    }
}