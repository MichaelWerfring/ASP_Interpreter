namespace asp_interpreter_lib.FunctorNaming;

public record FunctorTableRecord
{
    // arithmetic
    public string ArithmeticEvaluation { get; init; } = "#is";
    public string ArithmeticEvaluationNegated { get; init; } = "#isnt";
    public string Addition { get; init; } = "+";
    public string Subtraction { get; init; } = "-";
    public string Multiplication { get; init; } = "*";
    public string Division { get; init; } = "/";
    public string Power { get; init; } = "**";
    public string Parenthesis { get; init; } = "#()";

    // variables 
    public string AnonymusVariable { get; init; } = "#AnonVar";

    public string InternalVariable { get; init; } = "#Var";

    // number comparison
    public string GreaterOrEqualThan { get; init; } = ">=";
    public string GreaterThan { get; init; } = ">";
    public string LessOrEqualThan { get; init; } = "=<";
    public string LessThan { get; init; } = "<";

    //unification
    public string Unification { get; init; } = "=";
    public string Disunification { get; init; } = "\\=";

    // negation
    public string NegationAsFailure { get; init; } = "#not";
    public string ClassicalNegation { get; init; } = "~";

    // list
    public string List { get; init; } = "#cons";
    public string Nil { get; init; } = "#nil";

    // sasp
    public string Forall { get; init; } = "#forall";

    public string LessOrEqualTerm { get; init; } = "@<";
}
