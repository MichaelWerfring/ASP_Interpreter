//-----------------------------------------------------------------------
// <copyright file="AnonymousVariableReplacer.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Types.TypeVisitors
{
    using Asp_interpreter_lib.Preprocessing;
    using Asp_interpreter_lib.Types.Terms;
    using Asp_interpreter_lib.Util.ErrorHandling;

    /// <summary>
    /// Provides utility methods for replacing anonymous variables.
    /// </summary>
    public class AnonymousVariableReplacer : TypeBaseVisitor<ITerm>
    {
        private readonly PrefixOptions prefixOptions;

        private int replacementCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnonymousVariableReplacer"/> class.
        /// </summary>
        /// <param name="prefixOptions">The prefixes to be appended to generated variables.</param>
        /// <exception cref="ArgumentNullException">Is thrown if the prefix options are null.</exception>
        public AnonymousVariableReplacer(PrefixOptions prefixOptions)
        {
            ArgumentNullException.ThrowIfNull(prefixOptions);
            this.prefixOptions = prefixOptions;
            this.replacementCount = 0;
        }

        /// <summary>
        /// Replaces anonymous variables in the given statement.
        /// </summary>
        /// <param name="statement">The statement to replace the variables in.</param>
        /// <returns>The statement without anonymous variables.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the given statement is null.</exception>
        public Statement Replace(Statement statement)
        {
            ArgumentNullException.ThrowIfNull(statement);

            this.replacementCount = 0;
            statement.Accept(this);
            this.replacementCount = 0;
            return statement;
        }

        /// <summary>
        /// Replaces anonymous variables in the given statement, if using this method indexes of generated
        /// variables will not be consistent. Use <see cref="Replace(Statement)"/> instead.
        /// </summary>
        /// <param name="statement">The statement to be visited.</param>
        /// <returns>An instance of <see cref="None{T}"/> class.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the given statement is null.</exception>
        public override IOption<ITerm> Visit(Statement statement)
        {
            ArgumentNullException.ThrowIfNull(statement);

            if (statement.HasHead)
            {
                statement.Head.GetValueOrThrow().Accept(this);
            }

            if (statement.HasBody)
            {
                foreach (var goal in statement.Body)
                {
                    goal.Accept(this);
                }
            }

            return new None<ITerm>();
        }

        /// <summary>
        /// Replaces anonymous variables in the given literal.
        /// </summary>
        /// <param name="literal">The literal to be visited.</param>
        /// <returns>An instance of <see cref="None{T}"/> class.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the given literal is null.</exception>
        public override IOption<ITerm> Visit(Literal literal)
        {
            ArgumentNullException.ThrowIfNull(literal);
            List<ITerm> terms = new List<ITerm>();
            for (var i = 0; i < literal.Terms.Count; i++)
            {
                var term = literal.Terms[i];
                literal.Terms[i] = term.Accept(this).GetValueOrThrow();
            }

            return new None<ITerm>();
        }

        public override IOption<ITerm> Visit(BinaryOperation binOp)
        {
            ArgumentNullException.ThrowIfNull(binOp);

            binOp.Left = binOp.Left.Accept(this).GetValueOrThrow();
            binOp.Right = binOp.Right.Accept(this).GetValueOrThrow();

            return new None<ITerm>();
        }

        public override IOption<ITerm> Visit(AnonymousVariableTerm variable)
        {
            ArgumentNullException.ThrowIfNull(variable);
            string newName = this.prefixOptions.VariablePrefix + this.replacementCount++ + "_";
            return new Some<ITerm>(new VariableTerm(newName));
        }

        public override IOption<ITerm> Visit(VariableTerm term)
        {
            ArgumentNullException.ThrowIfNull(term);
            return new Some<ITerm>(term);
        }

        public override IOption<ITerm> Visit(ArithmeticOperationTerm term)
        {
            ArgumentNullException.ThrowIfNull(term);

            var left = term.Left.Accept(this).GetValueOrThrow();
            var right = term.Right.Accept(this).GetValueOrThrow();

            return new Some<ITerm>(new ArithmeticOperationTerm(left, term.Operation, right));
        }

        public override IOption<ITerm> Visit(BasicTerm basicTerm)
        {
            ArgumentNullException.ThrowIfNull(basicTerm);
            List<ITerm> terms = new List<ITerm>();

            for (var i = 0; i < basicTerm.Terms.Count; i++)
            {
                var term = basicTerm.Terms[i];
                basicTerm.Terms[i] = term.Accept(this).
                    GetValueOrThrow();
            }

            return new Some<ITerm>(basicTerm);
        }

        public override IOption<ITerm> Visit(StringTerm term)
        {
            return new Some<ITerm>(term);
        }

        public override IOption<ITerm> Visit(NumberTerm term)
        {
            return new Some<ITerm>(term);
        }

        public override IOption<ITerm> Visit(NegatedTerm term)
        {
            ArgumentNullException.ThrowIfNull(term);

            var newTerm = term.Term.Accept(this).GetValueOrThrow();

            return new Some<ITerm>(new NegatedTerm(newTerm));
        }

        public override IOption<ITerm> Visit(ParenthesizedTerm term)
        {
            ArgumentNullException.ThrowIfNull(term);
            var newTerm = term.Term.Accept(this).GetValueOrThrow();

            return new Some<ITerm>(new ParenthesizedTerm(newTerm));
        }

        public override IOption<ITerm> Visit(RecursiveList list)
        {
            ArgumentNullException.ThrowIfNull(list);
            List<ITerm> terms = new List<ITerm>();

            var head = list.Head.Accept(this).GetValueOrThrow();
            var tail = list.Tail.Accept(this).GetValueOrThrow();

            return new Some<ITerm>(new RecursiveList(head, tail));
        }

        public override IOption<ITerm> Visit(ConventionalList list)
        {
            ArgumentNullException.ThrowIfNull(list);

            for (var i = 0; i < list.Terms.Count; i++)
            {
                var term = list.Terms[i];
                list.Terms[i] = term.Accept(this).GetValueOrThrow();
            }

            return new Some<ITerm>(list);
        }
    }
}