// <copyright file="FunctorTableRecord.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.FunctorNaming;

/// <summary>
/// A class that contains functor and identifier strings that various parts of the solver need to handle in a special way.
/// </summary>
public record FunctorTableRecord
{
    private readonly string arithmeticEvaluation = "#is";
    private readonly string arithmeticEvaluationNegated = "#isnt";
    private readonly string addition = "+";
    private readonly string subtraction = "-";
    private readonly string multiplication = "*";
    private readonly string division = "/";
    private readonly string power = "**";
    private readonly string parenthesis = "#()";
    private readonly string anonymousVariable = "#AnonVar";
    private readonly string internalVariable = "#Var";
    private readonly string greaterOrEqualThan = ">=";
    private readonly string greaterThan = ">";
    private readonly string lessOrEqualThan = "=<";
    private readonly string lessThan = "<";
    private readonly string unification = "=";
    private readonly string disunification = "\\=";
    private readonly string negationAsFailure = "not";
    private readonly string classicalNegation = "-";
    private readonly string list = "#cons";
    private readonly string nil = "#nil";
    private readonly string forall = "#forall";

    /// <summary>
    /// Gets the functor for an arithmetic evaluation structure.
    /// </summary>
    public string ArithmeticEvaluation
    {
        get
        {
            return this.arithmeticEvaluation;
        }

        init
        {
            ArgumentNullException.ThrowIfNull(value);
            this.arithmeticEvaluation = value;
        }
    }

    /// <summary>
    /// Gets the functor for a negated arithmetic evaluation structure.
    /// </summary>
    public string ArithmeticEvaluationNegated
    {
        get
        {
            return this.arithmeticEvaluationNegated;
        }

        init
        {
            ArgumentNullException.ThrowIfNull(value);
            this.arithmeticEvaluationNegated = value;
        }
    }

    /// <summary>
    /// Gets the functor for an arithmetic addition structure.
    /// </summary>
    public string Addition
    {
        get
        {
            return this.addition;
        }

        init
        {
            ArgumentNullException.ThrowIfNull(value);
            this.addition = value;
        }
    }

    /// <summary>
    /// Gets the functor for an arithmetic sutraction structure.
    /// </summary>
    public string Subtraction
    {
        get
        {
            return this.subtraction;
        }

        init
        {
            ArgumentNullException.ThrowIfNull(value);
            this.subtraction = value;
        }
    }

    /// <summary>
    /// Gets the functor for an arithmetic multiplication structure.
    /// </summary>
    public string Multiplication
    {
        get
        {
            return this.multiplication;
        }

        init
        {
            ArgumentNullException.ThrowIfNull(value);
            this.multiplication = value;
        }
    }

    /// <summary>
    /// Gets the functor for an arithmetic division structure.
    /// </summary>
    public string Division
    {
        get
        {
            return this.division;
        }

        init
        {
            ArgumentNullException.ThrowIfNull(value);
            this.division = value;
        }
    }

    /// <summary>
    /// Gets the functor for an arithmetic power-to structure.
    /// </summary>
    public string Power
    {
        get
        {
            return this.power;
        }

        init
        {
            ArgumentNullException.ThrowIfNull(value);
            this.power = value;
        }
    }

    /// <summary>
    /// Gets the functor for a arithmetic parenthesis structure.
    /// </summary>
    public string Parenthesis
    {
        get
        {
            return this.parenthesis;
        }

        init
        {
            ArgumentNullException.ThrowIfNull(value);
            this.parenthesis = value;
        }
    }

    /// <summary>
    /// Gets the identifier for an anonympus variable.
    /// </summary>
    public string AnonymousVariable
    {
        get
        {
            return this.anonymousVariable;
        }

        init
        {
            ArgumentNullException.ThrowIfNull(value);
            this.anonymousVariable = value;
        }
    }

    /// <summary>
    /// Gets the identifier for an internal variable.
    /// </summary>
    public string InternalVariable
    {
        get
        {
            return this.internalVariable;
        }

        init
        {
            ArgumentNullException.ThrowIfNull(value);
            this.internalVariable = value;
        }
    }

    /// <summary>
    /// Gets the functor for a greater-than-or-equal comparison structure.
    /// </summary>
    public string GreaterOrEqualThan
    {
        get
        {
            return this.greaterOrEqualThan;
        }

        init
        {
            ArgumentNullException.ThrowIfNull(value);
            this.greaterOrEqualThan = value;
        }
    }

    /// <summary>
    /// Gets the functor for a greater-than comparison structure.
    /// </summary>
    public string GreaterThan
    {
        get
        {
            return this.greaterThan;
        }

        init
        {
            ArgumentNullException.ThrowIfNull(value);
            this.greaterThan = value;
        }
    }

    /// <summary>
    /// Gets the functor for a less-or-equal-than comparison structure.
    /// </summary>
    public string LessOrEqualThan
    {
        get
        {
            return this.lessOrEqualThan;
        }

        init
        {
            ArgumentNullException.ThrowIfNull(value);
            this.lessOrEqualThan = value;
        }
    }

    /// <summary>
    /// Gets the functor for a less-than comparison structure.
    /// </summary>
    public string LessThan
    {
        get
        {
            return this.lessThan;
        }

        init
        {
            ArgumentNullException.ThrowIfNull(value);
            this.lessThan = value;
        }
    }

    /// <summary>
    /// Gets the functor for a unification structure.
    /// </summary>
    public string Unification
    {
        get
        {
            return this.unification;
        }

        init
        {
            ArgumentNullException.ThrowIfNull(value);
            this.unification = value;
        }
    }

    /// <summary>
    /// Gets the functor for a disunification structure.
    /// </summary>
    public string Disunification
    {
        get
        {
            return this.disunification;
        }

        init
        {
            ArgumentNullException.ThrowIfNull(value);
            this.disunification = value;
        }
    }

    /// <summary>
    /// Gets the functor for a negation-as-failure structure.
    /// </summary>
    public string NegationAsFailure
    {
        get
        {
            return this.negationAsFailure;
        }

        init
        {
            ArgumentNullException.ThrowIfNull(value);
            this.negationAsFailure = value;
        }
    }

    /// <summary>
    /// Gets the functor for a classical-negation structure.
    /// </summary>
    public string ClassicalNegation
    {
        get
        {
            return this.classicalNegation;
        }

        init
        {
            ArgumentNullException.ThrowIfNull(value);
            this.classicalNegation = value;
        }
    }

    /// <summary>
    /// Gets the functor for a list structure.
    /// </summary>
    public string List
    {
        get
        {
            return this.list;
        }

        init
        {
            ArgumentNullException.ThrowIfNull(value);
            this.list = value;
        }
    }

    /// <summary>
    /// Gets the functor for an empty list structure.
    /// </summary>
    public string Nil
    {
        get
        {
            return this.nil;
        }

        init
        {
            ArgumentNullException.ThrowIfNull(value);
            this.nil = value;
        }
    }

    /// <summary>
    /// Gets the functor for a forall structure.
    /// </summary>
    public string Forall
    {
        get
        {
            return this.forall;
        }

        init
        {
            ArgumentNullException.ThrowIfNull(value);
            this.forall = value;
        }
    }
}