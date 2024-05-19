namespace asp_interpreter_lib.FunctorNaming;

public record FunctorTableRecord
{
    // arithmetic
    public string ArithmeticEvaluation { get; init; } = "_is";
    public string Addition { get; init; } = "+";
    public string Subtraction { get; init; } = "-";
    public string Multiplication { get; init; } = "*";
    public string Division { get; init; } = "/";
    public string Power { get; init; } = "**";
    public string Parenthesis { get; init; } = "_";

    // variables 
    public string AnonymusVariable { get; init; } = "_AnonVar";

    public string InternalVariable { get; init; } = "#Var";

    // number comparison
    public string GreaterOrEqualThan { get; init; } = ">=";
    public string GreaterThan { get; init; } = ">";
    public string LessOrEqualThan { get; init; } = "<=";
    public string LessThan { get; init; } = "<";

    //unification
    public string Unification { get; init; } = "=";
    public string Disunification { get; init; } = "\\=";

    // negation
    public string NegationAsFailure { get; init; } = "not";
    public string ClassicalNegation { get; init; } = "_neg";

    // list
    public string List { get; init; } = "_cons";
    public string Nil { get; init; } = "_nil";

    // sasp
    public string Forall { get; init; } = "_forall";

    public string LessOrEqualTerm { get; init; } = "@<";
}
