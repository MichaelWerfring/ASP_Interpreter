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

    // returns the term to be used at the occurring position (or same as before if no change)
    // and a goal to be added to the body (else its none)
    public class HeadAtomEliminator : TypeBaseVisitor<(ITerm, List<Goal>)>
    {
        private readonly PrefixOptions options;

        private readonly HashSet<string> variables;

        private int counter;

        public HeadAtomEliminator(PrefixOptions options, Statement statement)
        {
            ArgumentNullException.ThrowIfNull(options);
            ArgumentNullException.ThrowIfNull(statement);
            this.options = options;
            this.variables =[];
            this.counter = 1;
        }

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
                head.Terms[index] = rewrite.Item1;

                // statement.Body.InsertRange(0, rewrite.Item2);
                generatedGoals.AddRange(rewrite.Item2);
            }

            statement.Body.InsertRange(0, generatedGoals);

            return statement;
        }

        /// <inheritdoc/>
        public override IOption<(ITerm, List<Goal>)> Visit(BasicTerm term)
        {
            ArgumentNullException.ThrowIfNull(term);

            var newVariable = new VariableTerm(this.options.VariablePrefix + this.counter++);
            var body = new BinaryOperation(newVariable, new Equality(), term);

            return new Some<(ITerm, List<Goal>)>((newVariable,[body]));
        }

        /// <inheritdoc/>
        public override IOption<(ITerm, List<Goal>)> Visit(AnonymousVariableTerm term)
        {
            ArgumentNullException.ThrowIfNull(term);
            return new Some<(ITerm, List<Goal>)>((term,[]));
        }

        /// <inheritdoc/>
        public override IOption<(ITerm, List<Goal>)> Visit(VariableTerm term)
        {
            ArgumentNullException.ThrowIfNull(term);

            if (this.variables.Add(term.Identifier))
            {
                // If the variable only occurs once in the head, we can just return it again
                return new Some<(ITerm, List<Goal>)>((term,[]));
            }

            var newVariable = new VariableTerm(this.options.VariablePrefix + this.counter++);
            var body = new BinaryOperation(newVariable, new Equality(), term);

            return new Some<(ITerm, List<Goal>)>((newVariable,[body]));
        }

        /// <inheritdoc/>
        public override IOption<(ITerm, List<Goal>)> Visit(ArithmeticOperationTerm term)
        {
            ArgumentNullException.ThrowIfNull(term);
            return new Some<(ITerm, List<Goal>)>((term,[]));
        }

        /// <inheritdoc/>
        public override IOption<(ITerm, List<Goal>)> Visit(StringTerm term)
        {
            ArgumentNullException.ThrowIfNull(term);

            var newVariable = new VariableTerm(this.options.VariablePrefix + this.counter++);
            var body = new BinaryOperation(newVariable, new Equality(), term);

            return new Some<(ITerm, List<Goal>)>((newVariable,[body]));
        }

        /// <inheritdoc/>
        public override IOption<(ITerm, List<Goal>)> Visit(NumberTerm term)
        {
            ArgumentNullException.ThrowIfNull(term);

            var newVariable = new VariableTerm(this.options.VariablePrefix + this.counter++);
            var body = new BinaryOperation(newVariable, new Equality(), term);

            return new Some<(ITerm, List<Goal>)>((newVariable,[body]));
        }

        /// <inheritdoc/>
        public override IOption<(ITerm, List<Goal>)> Visit(NegatedTerm term)
        {
            ArgumentNullException.ThrowIfNull(term);

            var newVariable = new VariableTerm(this.options.VariablePrefix + this.counter++);

            var number =
                term.Term.Accept(new TermToNumberConverter()).GetValueOrThrow("Cannot convert term to number!");

            var body = new BinaryOperation(
                newVariable, new Equality(), new NumberTerm(-number));

            return new Some<(ITerm, List<Goal>)>((newVariable,[body]));
        }

        /// <inheritdoc/>
        public override IOption<(ITerm, List<Goal>)> Visit(ParenthesizedTerm term)
        {
            ArgumentNullException.ThrowIfNull(term);
            return term.Term.Accept(this);
        }

        /// <inheritdoc/>
        public override IOption<(ITerm, List<Goal>)> Visit(RecursiveList term)
        {
            ArgumentNullException.ThrowIfNull(term);

            var newVariable = new VariableTerm(this.options.VariablePrefix + this.counter++);
            var body = new BinaryOperation(newVariable, new Equality(), term);

            return new Some<(ITerm, List<Goal>)>((newVariable,[body]));
        }

        /// <inheritdoc/>
        public override IOption<(ITerm, List<Goal>)> Visit(ConventionalList term)
        {
            ArgumentNullException.ThrowIfNull(term);

            var newVariable = new VariableTerm(this.options.VariablePrefix + this.counter++);
            var body = new BinaryOperation(newVariable, new Equality(), term);

            return new Some<(ITerm, List<Goal>)>((newVariable,[body]));
        }
    }
}