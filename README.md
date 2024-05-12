# ASP_Interpreter
This Poject is based on the s(ASP) system https://sourceforge.net/projects/sasp-system/ and is completely implemented in C# and .NET 8.0.

## Usage
To use the Interpreter only the executeable is needed and any text file containing a valid ASP program. 

### Examples
- `interpreter.exe -l 0 -p /path/to/file.txt --interactive`
- `interpreter.exe --log-level 1 --path /path/to/file.txt --help`

### Command Line Arguments
Some Configurations can be made by supplying Command Line Arguments to the interpreter.

#### Path
- `-p`, `--path`: Specifies the path for the operation.
  - Example: `-p /path/to/file`, `--path /path/to/file`

#### Log Level
- `-l`, `--log-level`: Sets the logging level for the operation.
  - Example: `-l 0`, `--log-level 4`

#### Timestamp
- `-t`, `--timestamp`: Adds timestamps to the logging messages.
  - Example: `-t`, `--timestamp`

#### Help
- `-h`, `--help`: Displays the help message.
  - Example: `-h`, `--help`

#### Interactive Mode
- `-i`, `--interactive`: Enables interactive query window.
  - Example: `-i`, `--interactive`

### Log Levels

Log levels determine the severity or verbosity of the log messages. They are represented by the following enum values:

- `Trace (0)`: Detailed diagnostic information.
- `Debug (1)`: Debugging information, less detailed than trace.
- `Info (2)`: Informational messages.
- `Error (3)`: Indicates error conditions.
- `None (4)`: Indicates that no logging should be performed.

When setting the log level, provide the corresponding integer value to indicate the desired severity of log messages. For example, setting the log level to `2` will log only info and more severe messages.

### Interactive Mode
When starting the Interpreter in Interactive Mode, a console window will open up and ask the developer to supply a query to solve the program. 
#### Commands 
-`exit` closes the console window

-`clear` clears the console window

-`reload` reloads the specified file

### Developer Mode 
The developer mode aims to simplify the Debugging process. When supplying the command `interpreter.exe /path/to/file.txt` it will be interpreted as `interpreter.exe --path /path/to/file.txt --log-level 1 --interactive` to automatically start in `Interactive Mode` and set the Log Level to `Debug`