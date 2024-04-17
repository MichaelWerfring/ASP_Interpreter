using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.InternalProgramClasses.InternalProgram;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;
using asp_interpreter_lib.Types;
using asp_interpreter_lib.Types.BinaryOperations;
using asp_interpreter_lib.Types.TypeVisitors;

namespace asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram;

public class ProgramConverter : TypeBaseVisitor<ISimpleTerm>
{
    private TermConverter  _termConverter = new TermConverter();

    public InternalAspProgram Convert(AspProgram prog)
    {
        var clauses = prog.Statements.Select(ConvertStatement);

        var query = ConvertLiteral(prog.Query.ClassicalLiteral);

        return new InternalAspProgram(clauses, [query]);
    }

    public override IOption<ISimpleTerm> Visit(Forall forall)
    {

        var var = _termConverter.Convert(forall.VariableTerm);

        return new Some<ISimpleTerm>(new Structure("forall", [var, forall.Goal.Accept(this).GetValueOrThrow()], false));
    }

    public override IOption<ISimpleTerm> Visit(Literal literal)
    {
        return new Some<ISimpleTerm>(ConvertLiteral(literal));
    }

    public override IOption<ISimpleTerm> Visit(BinaryOperation binaryOperation)
    {
        throw new NotImplementedException();
    }

    private ISimpleTerm ConvertLiteral(Literal literal)
    {
        var terms = literal.Terms.Select(_termConverter.Convert);

        var term = new Structure(literal.Identifier, terms, literal.HasStrongNegation);
        if (literal.HasNafNegation)
        {
            term = new Structure("not", [term], false);
        }

        return term;
    }

    private IEnumerable<ISimpleTerm> ConvertStatement(Statement statement)
    {
        var list = new List<ISimpleTerm>();

        if (statement.HasHead)
        {
            list.Add(ConvertLiteral(statement.Head.GetValueOrThrow()));
        }

        foreach (var item in statement.Body)
        {
            list.Add(item.Accept(this).GetValueOrThrow());
        }

        return list;
    }
}