//-----------------------------------------------------------------------
// <copyright file="HeadAtomEliminator.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Preprocessing.DualRules
{
    using Asp_interpreter_lib.Preprocessing;
    using Asp_interpreter_lib.Types;
    using Asp_interpreter_lib.Types.BinaryOperations;
    using Asp_interpreter_lib.Types.Terms;
    using Asp_interpreter_lib.Types.TypeVisitors;
    using Asp_interpreter_lib.Util.ErrorHandling;

    /// <summary>
    /// Contains utility methods to remove atoms form the head of a statement.
    /// </summary>
    public class HeadAtomEliminator : TypeBaseVisitor<(ITerm TermToReplace, List<Goal> GoalsForBody)>
    {
        private readonly PrefixOptions options;

        private readonly HashSet<string> variables;

        private int counter;

        /// <summary>
        /// Initializes a new instance of the <see cref="HeadAtomEliminator"/> class.
        /// </summary>
        /// <param name="options">The prefixes to append to generated variables.</param>
        /// <exception cref="ArgumentNullException">Is thrown if the options are null.</exception>
        public HeadAtomEliminator(PrefixOptions options)
        {
            ArgumentNullException.ThrowIfNull(options);
            this.options = options;
            this.variables = new HashSet<string>();
            this.counter = 1;
        }

        /// <summary>
        /// Rewrites the head of a statement by removing atoms in the head and appending them to the body if necessary.
        /// </summary>
        /// <param name="statement">The statement to rewrite.</param>
        /// <returns>Returns the converted statement.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the statement is null.</exception>
        public Statement Rewrite(Statement statement)
        {
            ArgumentNullException.ThrowIfNull(statement);

            if (!statement.HasHead)
            {
                return statement;
            }

            var head = statement.Head.GetValueOrThrow("Cannot rewrite headless statement!");

            var generatedGoals = new List<Goal>();
            for (var index = 0; index < head.Terms.Count; index++)
            {
                var term = head.Terms[index];
                var rewrite = term.Accept(this).GetValueOrThrow("Unable to parse head!");
                head.Terms[index] = rewrite.TermToReplace;

                // statement.Body.InsertRange(0, rewrite.Item2);
                generatedGoals.AddRange(rewrite.GoalsForBody);
            }

            statement.Body.InsertRange(0, generatedGoals);

            return statement;
        }

        /// <summary>
        /// Visits a basic term.
        /// </summary>
        /// <param name="term">The term to remove from the head.</param>
        /// <returns>Returns the term to be used at the current position and goals to be added to the body.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the term is null.</exception>
        public override IOption<(ITerm TermToReplace, List<Goal> GoalsForBody)> Visit(BasicTerm term)
        {
            ArgumentNullException.ThrowIfNull(term);

            var newVariable = new VariableTerm(this.options.VariablePrefix + this.counter++);
            var body = new BinaryOperation(newVariable, new Equality(), term);

            return new Some<(ITerm, List<Goal>)>((newVariable, [body]));
        }

        /// <summary>
        /// Visits an anonymous variable term.
        /// </summary>
        /// <param name="term">The term to remove from the head.</param>
        /// <returns>Returns the term to be used at the current position and goals to be added to the body.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the term is null.</exception>
        public override IOption<(ITerm TermToReplace, List<Goal> GoalsForBody)> Visit(AnonymousVariableTerm term)
        {
            ArgumentNullException.ThrowIfNull(term);
            return new Some<(ITerm, List<Goal>)>((term, new List<Goal>()));
        }

        /// <summary>
        /// Visits a variable term.
        /// </summary>
        /// <param name="term">The term to remove from the head.</param>
        /// <returns>Returns the term to be used at the current position and goals to be added to the body.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the term is null.</exception>
        public override IOption<(ITerm TermToReplace, List<Goal> GoalsForBody)> Visit(VariableTerm term)
        {
            ArgumentNullException.ThrowIfNull(term);

            if (this.variables.Add(term.Identifier))
            {
                // If the variable only occurs once in the head, we can just return it again
                return new Some<(ITerm, List<Goal>)>((term, new List<Goal>()));
            }

            var newVariable = new VariableTerm(this.options.VariablePrefix + this.counter++);
            var body = new BinaryOperation(newVariable, new Equality(), term);

            return new Some<(ITerm, List<Goal>)>((newVariable, [body]));
        }

        /// <summary>
        /// Visits an arithmetic operation.
        /// </summary>
        /// <param name="term">The term to remove from the head.</param>
        /// <returns>Returns the term to be used at the current position and goals to be added to the body.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the term is null.</exception>
        public override IOption<(ITerm TermToReplace, List<Goal> GoalsForBody)> Visit(ArithmeticOperationTerm term)
        {
            ArgumentNullException.ThrowIfNull(term);
            return new Some<(ITerm, List<Goal>)>((term, new List<Goal>()));
        }

        /// <summary>
        /// Visits string term.
        /// </summary>
        /// <param name="term">The term to remove from the head.</param>
        /// <returns>Returns the term to be used at the current position and goals to be added to the body.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the term is null.</exception>
        public override IOption<(ITerm TermToReplace, List<Goal> GoalsForBody)> Visit(StringTerm term)
        {
            ArgumentNullException.ThrowIfNull(term);

            var newVariable = new VariableTerm(this.options.VariablePrefix + this.counter++);
            var body = new BinaryOperation(newVariable, new Equality(), term);

            return new Some<(ITerm, List<Goal>)>((newVariable, [body]));
        }

        /// <summary>
        /// Visits a number term.
        /// </summary>
        /// <param name="term">The term to remove from the head.</param>
        /// <returns>Returns the term to be used at the current position and goals to be added to the body.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the term is null.</exception>
        public override IOption<(ITerm TermToReplace, List<Goal> GoalsForBody)> Visit(NumberTerm term)
        {
            ArgumentNullException.ThrowIfNull(term);

            var newVariable = new VariableTerm(this.options.VariablePrefix + this.counter++);
            var body = new BinaryOperation(newVariable, new Equality(), term);

            return new Some<(ITerm, List<Goal>)>((newVariable, [body]));
        }

        /// <summary>
        /// Visits a negated term.
        /// </summary>
        /// <param name="term">The term to remove from the head.</param>
        /// <returns>Returns the term to be used at the current position and goals to be added to the body.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the term is null.</exception>
        public override IOption<(ITerm TermToReplace, List<Goal> GoalsForBody)> Visit(NegatedTerm term)
        {
            ArgumentNullException.ThrowIfNull(term);

            var newVariable = new VariableTerm(this.options.VariablePrefix + this.counter++);

            var number =
                term.Term.Accept(new TermToNumberConverter()).GetValueOrThrow("Cannot convert term to number!");

            var body = new BinaryOperation(
                newVariable, new Equality(), new NumberTerm(-number));

            return new Some<(ITerm, List<Goal>)>((newVariable, [body]));
        }

        /// <summary>
        /// Visits a parenthesized term.
        /// </summary>
        /// <param name="term">The term to remove from the head.</param>
        /// <returns>Returns the term to be used at the current position and goals to be added to the body.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the term is null.</exception>
        public override IOption<(ITerm TermToReplace, List<Goal> GoalsForBody)> Visit(ParenthesizedTerm term)
        {
            ArgumentNullException.ThrowIfNull(term);
            return term.Term.Accept(this);
        }

        /// <summary>
        /// Visits a recursive list term.
        /// </summary>
        /// <param name="term">The term to remove from the head.</param>
        /// <returns>Returns the term to be used at the current position and goals to be added to the body.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the term is null.</exception>
        public override IOption<(ITerm TermToReplace, List<Goal> GoalsForBody)> Visit(RecursiveList term)
        {
            ArgumentNullException.ThrowIfNull(term);

            var newVariable = new VariableTerm(this.options.VariablePrefix + this.counter++);
            var body = new BinaryOperation(newVariable, new Equality(), term);

            return new Some<(ITerm, List<Goal>)>((newVariable, [body]));
        }

        /// <summary>
        /// Visits a conventional list term.
        /// </summary>
        /// <param name="term">The term to remove from the head.</param>
        /// <returns>Returns the term to be used at the current position and goals to be added to the body.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the term is null.</exception>
        public override IOption<(ITerm TermToReplace, List<Goal> GoalsForBody)> Visit(ConventionalList term)
        {
            ArgumentNullException.ThrowIfNull(term);

            var newVariable = new VariableTerm(this.options.VariablePrefix + this.counter++);
            var body = new BinaryOperation(newVariable, new Equality(), term);

            return new Some<(ITerm, List<Goal>)>((newVariable, [body]));
        }
    }
}