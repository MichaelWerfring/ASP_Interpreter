using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types;
using asp_interpreter_lib.Types.ArithmeticOperations;
using asp_interpreter_lib.Types.BinaryOperations;
using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Types.TypeVisitors;

namespace asp_interpreter_lib.Solving;

//Due to performance reasons, a hash set is used to store the rule names
public class RuleNameFinder : TypeBaseVisitor<HashSet<string>>
{
    public override IOption<HashSet<string>> Visit(AspProgram program)
    {
        ArgumentNullException.ThrowIfNull(program);
        HashSet<string> ruleNames = [];
        foreach (var statement in program.Statements)
        {
            statement.Accept(this).IfHasValue(v =>
            {
                ruleNames.UnionWith(v);
            });
        }

        return new Some<HashSet<string>>(ruleNames);
    }

    public override IOption<HashSet<string>> Visit(Query query)
    {
        ArgumentNullException.ThrowIfNull(query);
        return query.ClassicalLiteral.Accept(this);
    }

    public override IOption<HashSet<string>> Visit(Statement statement)
    {
        ArgumentNullException.ThrowIfNull(statement);
        HashSet<string> ruleNames = [];

        if (statement.HasHead)
        {
            statement.Head.IfHasValue(h => h.Accept(this).
                IfHasValue(v => ruleNames.UnionWith(v)));
        }

        if (statement.HasBody)
        {
            foreach (var literal in statement.Body)
            {
                literal.Accept(this).IfHasValue(v =>
                {
                    ruleNames.UnionWith(v);
                });
            }
        }

        return new Some<HashSet<string>>(ruleNames);
    }

    public override IOption<HashSet<string>> Visit(Literal classicalLiteral)
    {
        ArgumentNullException.ThrowIfNull(classicalLiteral);
        HashSet<string> ruleNames = [ classicalLiteral.Identifier ];
        foreach (var term in classicalLiteral.Terms)
        {
            term.Accept(this).IfHasValue(v =>
            {
                ruleNames.UnionWith(v);
            });
        }

        return new Some<HashSet<string>>(ruleNames);
    }
    

    public override IOption<HashSet<string>> Visit(Plus _)
    {
        return new Some<HashSet<string>>([]);
    }
    

    public override IOption<HashSet<string>> Visit(Minus _)
    {
        return new Some<HashSet<string>>([]);
    }

    
    public override IOption<HashSet<string>> Visit(Multiply _)
    {
        return new Some<HashSet<string>>([]);
    }

    
    public override IOption<HashSet<string>> Visit(Divide _)
    {
        return new Some<HashSet<string>>([]);
    }

    
    public override IOption<HashSet<string>> Visit(BinaryOperation _)
    {
        return new Some<HashSet<string>>([]);
    }
    
    public override IOption<HashSet<string>> Visit(Disunification _)
    {
        return new Some<HashSet<string>>([]);
    }

    public override IOption<HashSet<string>> Visit(Equality _)
    {
        return new Some<HashSet<string>>([]);
    }

    public override IOption<HashSet<string>> Visit(GreaterOrEqualThan _)
    {
        return new Some<HashSet<string>>([]);
    }

    public override IOption<HashSet<string>> Visit(GreaterThan _)
    {
        return new Some<HashSet<string>>([]);
    }

    public override IOption<HashSet<string>> Visit(LessOrEqualThan _)
    {
        return new Some<HashSet<string>>([]);
    }

    public override IOption<HashSet<string>> Visit(LessThan _)
    {
        return new Some<HashSet<string>>([]);
    }

    public override IOption<HashSet<string>> Visit(Is _)
    {
        return new Some<HashSet<string>>([]);
    }

    public override IOption<HashSet<string>> Visit(AnonymusVariableTerm _)
    {
        return new Some<HashSet<string>>([]);
    }

    public override IOption<HashSet<string>> Visit(VariableTerm _)
    {
        return new Some<HashSet<string>>([]);
    }

    public override IOption<HashSet<string>> Visit(ArithmeticOperationTerm _)
    {
        return new Some<HashSet<string>>([]);
    }

    public override IOption<HashSet<string>> Visit(BasicTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);
        HashSet<string> ruleNames = [term.Identifier];

        foreach (var child in term.Terms)
        {
            child.Accept(this).IfHasValue(v =>
            {
                ruleNames.UnionWith(v);
            });
        }
        
        return new Some<HashSet<string>>(ruleNames);
    }

    public override IOption<HashSet<string>> Visit(StringTerm _)
    {
        return new Some<HashSet<string>>([]);
    }

    public override IOption<HashSet<string>> Visit(NumberTerm _)
    {
        return new Some<HashSet<string>>([]);
    }

    public override IOption<HashSet<string>> Visit(NegatedTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);
        HashSet<string> ruleNames = [];
        
        term.Term.Accept(this).IfHasValue(v =>
        {
            ruleNames.UnionWith(v);
        });
        
        return new Some<HashSet<string>>(ruleNames);
    }

    public override IOption<HashSet<string>> Visit(ParenthesizedTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);
        HashSet<string> ruleNames = [];
        
        term.Term.Accept(this).IfHasValue(v =>
        {
            ruleNames.UnionWith(v);
        });
        
        return new Some<HashSet<string>>(ruleNames);
    }

    public override IOption<HashSet<string>> Visit(RecursiveList list)
    {
        ArgumentNullException.ThrowIfNull(list);
        HashSet<string> ruleNames = [];

        list.Head.Accept(this).IfHasValue(h => ruleNames.UnionWith(h));
        list.Tail.Accept(this).IfHasValue(t => ruleNames.UnionWith(t));
        
        return new Some<HashSet<string>>(ruleNames);
    }

    public override IOption<HashSet<string>> Visit(ConventionalList list)
    {
        ArgumentNullException.ThrowIfNull(list);
        HashSet<string> ruleNames = [];
        
        foreach (var term in list.Terms)
        {
            term.Accept(this).IfHasValue(v =>
            {
                ruleNames.UnionWith(v);
            });
        }
        
        return new Some<HashSet<string>>(ruleNames);
    }

    public override IOption<HashSet<string>> Visit(Forall _)
    {
        return new Some<HashSet<string>>([]);
    }
}