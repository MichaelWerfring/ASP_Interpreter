namespace Asp_interpreter_lib.Util
{
    using Asp_interpreter_lib.Util.ErrorHandling.Either;

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
}