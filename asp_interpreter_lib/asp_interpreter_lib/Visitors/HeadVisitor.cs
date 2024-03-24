﻿using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using Antlr4.Runtime.Tree;
using asp_interpreter_lib.Types;

namespace asp_interpreter_lib.Visitors;

public class HeadVisitor : ASPBaseVisitor<Head>
{
    public override Head VisitHead(ASPParser.HeadContext context)
    {
        var literalVisitor = new ClassicalLiteralVisitor();
        List<ClassicalLiteral> literals = [];
        
        foreach (var literal in context.disjunction().children)
        {
            //In a disjunction we only expect classical literals 
            //due to the grammar definition
            var c = literal.Accept(literalVisitor);
            if (c != null)
            {
                literals.Add(c);   
            }
        }

        return new Head(literals);
    }
}