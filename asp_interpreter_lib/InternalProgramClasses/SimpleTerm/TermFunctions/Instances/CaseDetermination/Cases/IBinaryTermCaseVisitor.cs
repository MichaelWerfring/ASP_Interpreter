namespace Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.CaseDetermination.Cases;

public interface IBinaryTermCaseVisitor
{
    public void Visit(IntegerIntegerCase binaryCase);

    public void Visit(IntegerStructureCase binaryCase);

    public void Visit(IntegerVariableCase binaryCase);

    public void Visit(StructureIntegerCase binaryCase);

    public void Visit(StructureStructureCase binaryCase);

    public void Visit(StructureVariableCase binaryCase);

    public void Visit(VariableIntegerCase binaryCase);

    public void Visit(VariableVariableCase binaryCase);

    public void Visit(VariableStructureCase binaryCase);
}

public interface IBinaryTermCaseVisitor<T>
{
    public T Visit(IntegerIntegerCase binaryCase);

    public T Visit(IntegerStructureCase binaryCase);

    public T Visit(IntegerVariableCase binaryCase);

    public T Visit(StructureIntegerCase binaryCase);

    public T Visit(StructureStructureCase binaryCase);

    public T Visit(StructureVariableCase binaryCase);

    public T Visit(VariableIntegerCase binaryCase);

    public T Visit(VariableVariableCase binaryCase);

    public T Visit(VariableStructureCase binaryCase);
}