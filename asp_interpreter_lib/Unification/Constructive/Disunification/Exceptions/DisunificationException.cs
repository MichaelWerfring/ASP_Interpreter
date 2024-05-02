using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asp_interpreter_lib.Unification.Constructive.Disunification.Exceptions;

public abstract class DisunificationException : Exception
{
    public DisunificationException(string message) : base(message)
    { }
}
