namespace Asp_interpreter_lib.Util
{
    using Antlr4.Runtime;
    using Asp_interpreter_lib.Preprocessing;
    using Asp_interpreter_lib.Types;
    using Asp_interpreter_lib.Types.Terms;
    using Asp_interpreter_lib.Types.TypeVisitors;
    using Asp_interpreter_lib.Types.TypeVisitors.Copy;
    using Asp_interpreter_lib.Util.ErrorHandling;
    using Asp_interpreter_lib.Visitors;
    using System.Text;

    /// <summary>
    /// Collection of extensions needed for the ASP interpreter.
    /// </summary>
    public static class AspExtensions
    {
        /// <summary>
        /// Returns a list of common prefixes used in the ASP program.
        /// </summary>
        public static PrefixOptions CommonPrefixes
        {
            get
            {
                return new("fa_", "eh", "chk_", "not_", "V");
            }
        }

        /// <summary>
        /// Reads the given ASP code and returns the corresponding AspProgram object.
        /// </summary>
        /// <param name="code">String representation of the program.</param>
        /// <param name="logger">Logger to print out messages.</param>
        /// <returns>The code parsed to an asp program.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if any of the arguments is null.</exception>
        /// <exception cref="InvalidOperationException">Is thrown if the given code cannot be parsed correctly.</exception>"
        public static AspProgram GetProgram(string code, ILogger logger)
        {
            ArgumentNullException.ThrowIfNull(code);
            ArgumentNullException.ThrowIfNull(logger);

            var inputStream = new AntlrInputStream(code);
            var lexer = new ASPLexer(inputStream);
            var commonTokenStream = new CommonTokenStream(lexer);
            var parser = new ASPParser(commonTokenStream);
            var context = parser.program();
            var visitor = new ProgramVisitor(logger);
            var program = visitor.VisitProgram(context);

            if (!program.HasValue)
            {
                throw new InvalidOperationException("The given code cannot be parsed!");
            }

            return program.GetValueOrThrow();
        }

        /// <summary>
        /// Returns a copy of the given string.
        /// </summary>
        /// <param name="input">The string to be copied.</param>
        /// <returns>A copy of the given string.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the given string is null.</exception>"
        public static string GetCopy(this string input)
        {
            ArgumentNullException.ThrowIfNull(input);
            string copy = string.Empty;

            for (int i = 0; i < input.Length; i++)
            {
                copy += input[i];
            }

            return copy;
        }

        /// <summary>
        /// Converts the given list to a string, by separating them with commas.
        /// </summary>
        /// <typeparam name="T">Type of the element contained in the list.</typeparam>
        /// <param name="list">List to be converted to a string.</param>
        /// <returns>The given list in a string representation.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the given argument is null.</exception>
        public static string ListToString<T>(this List<T> list)
        {
            ArgumentNullException.ThrowIfNull(list);

            if (list.Count == 0)
            {
                return string.Empty;
            }

            var sb = new StringBuilder();

            for (int i = 0; i < list.Count - 1; i++)
            {
                sb.Append($"{list[i].ToString()}, ");
            }

            sb.Append($"{list[^1].ToString()}");

            return sb.ToString();
        }

        /// <summary>
        /// Returns a copy of the given statements.
        /// </summary>
        /// <param name="statements">The statements to be duplicated.</param>
        /// <returns>A copy of the given statements.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the given argument is null.</exception>"
        public static List<Statement> Duplicate(this List<Statement> statements)
        {
            ArgumentNullException.ThrowIfNull(statements);

            var newStatements = new List<Statement>();
            var visitor = new StatementCopyVisitor();
            foreach (var statement in statements)
            {
                newStatements.Add(statement.Accept(visitor).GetValueOrThrow());
            }

            return newStatements;
        }


        /// <summary>
        /// Returns a copy of the given ASP program.
        /// </summary>
        /// <param name="program">The program to be copied.</param>
        /// <returns>A copy of the given program.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the given argument is null.</exception>
        public static AspProgram Duplicate(this AspProgram program)
        {
            ArgumentNullException.ThrowIfNull(program);

            var statements = program.Statements.Duplicate();

            if (!program.Query.HasValue)
            {
                return new AspProgram(statements, new None<Query>(), program.Explanations);
            }

            var queryCopy = new List<Goal>();
            var query = program.Query.GetValueOrThrow();
            var goalCopyVisitor = new GoalCopyVisitor(new TermCopyVisitor());

            foreach (var goal in query.Goals)
            {
                queryCopy.Add(goal.Accept(goalCopyVisitor).GetValueOrThrow());
            }

            return new AspProgram(statements, new Some<Query>(new Query(queryCopy)), program.Explanations);
        }

        /// <summary>
        /// Generates a list of variables with the given name and appends an index.
        /// </summary>
        /// <param name="numberOfVariables">The number of variables to be generated.</param>
        /// <param name="variableName">The name of the variables to be generated.</param>
        /// <returns>A list of variables.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Is thrown if the given number of variables is negative.</exception>
        /// <exception cref="ArgumentException">Is thrown if the given variable name is null or empty.</exception>
        public static List<ITerm> GenerateVariables(int numberOfVariables, string variableName)
        {
            ArgumentException.ThrowIfNullOrEmpty(variableName);
            ArgumentOutOfRangeException.ThrowIfNegative(numberOfVariables);

            List<ITerm> vars = [];

            for (int i = 0; i < numberOfVariables; i++)
            {
                vars.Add(new VariableTerm(variableName + (i + 1)));
            }

            return vars;
        }

        /// <summary>
        /// Compares the input goal with the given parameters.
        /// </summary>
        /// <param name="goal">The goal to compare.</param>
        /// <param name="naf">A value to indicate whether the goal has negation as failure.</param>
        /// <param name="neg">A value to indicate whether the goal has classical negation.</param>
        /// <param name="id">The identifier name of the goal.</param>
        /// <param name="terms">The expected string representation of the terms.</param>
        /// <returns>Returns a boolean value indicating whether the goal matches the given parameters.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if any of the arguments is null.</exception>"
        public static bool CompareGoal(Goal goal, bool naf, bool neg, string id, string[] terms)
        {
            ArgumentNullException.ThrowIfNull(id);
            ArgumentNullException.ThrowIfNull(terms);
            ArgumentNullException.ThrowIfNull(goal);

            var maybeLiteral = goal.Accept(new GoalToLiteralConverter());

            if (!maybeLiteral.HasValue)
            {
                return false;
            }

            var literal = maybeLiteral.GetValueOrThrow();

            if (literal.HasNafNegation != naf ||
                literal.HasStrongNegation != neg ||
                literal.Identifier != id)
            {
                return false;
            }

            if (terms.Length != literal.Terms.Count)
            {
                return false;
            }

            for (int i = 0; i < literal.Terms.Count; i++)
            {
                ITerm term = literal.Terms[i];

                if (term.ToString() != terms[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}