namespace asp_interpreter_lib.FileIO;

public class FileReadResult
{
    private string? _content;
    private string _message;

    public FileReadResult(string content, bool success, string message)
    {
        Content = content;
        Success = success;
        Message = message;
    }

    public string? Content
    {
        get => _content;
        private set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Value must not be null or empty!",nameof(value));
            }

            _content = value;
        }
    }

    public bool Success { get; private set; }

    public string Message
    {
        get => _message;
        private set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Value must not be null or empty!",nameof(value));
            }

            _message = value;
        }
    }
}