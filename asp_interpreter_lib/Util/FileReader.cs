//-----------------------------------------------------------------------
// <copyright file="FileReader.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Util
{
    using Asp_interpreter_lib.Util.ErrorHandling.Either;

    /// <summary>
    /// Provides utility for reading files.
    /// </summary>
    public static class FileReader
    {
        /// <summary>
        /// Reads the file at the given path.
        /// </summary>
        /// <param name="path">The path to read the file from.</param>
        /// <returns>A <see cref="Left{TLeft, TRight}"/> with the given error message if loading fails and
        /// else an instance of <see cref="Right{TLeft, TRight}"/> with the files content.</returns>
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