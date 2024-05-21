
namespace asp_interpreter_lib.Unification.Constructive.CaseDetermination.Cases;

public interface IBinaryTermCaseVisitor
{
    public void Visit(VariableVariableCase unficiationCase);

    public void Visit(VariableStructureCase unficiationCase);

    public void Visit(StructureStructure unificationCase);

    public void Visit(StructureVariableCase unificationCase);
}