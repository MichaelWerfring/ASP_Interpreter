using Asp_interpreter_lib.Types.Terms;
using Asp_interpreter_lib.Types.TypeVisitors;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asp_interpreter_lib.Types
{
    public class Explanation
    {
        private readonly List<string> _textParts;
        private readonly HashSet<int> _variablesAt;
        private readonly Literal _literal;

        public Explanation(List<string> textParts, HashSet<int> variablesAt, Literal literal)
        {
            _textParts = textParts ?? throw new ArgumentNullException(nameof(textParts));
            _variablesAt = variablesAt ?? throw new ArgumentNullException(nameof(variablesAt));
            _literal = literal ?? throw new ArgumentNullException(nameof(literal));

            if (variablesAt.Any(n => n > textParts.Count || n < 0))
            {
                throw new InvalidOperationException(
                    "The indices of the variables must be between than the text parts length and 0!");
            }
        }

        public List<string> TextParts { get => _textParts; } 
        
        public HashSet<int> VariablesAt { get => _variablesAt; }

        public Literal Literal { get => _literal; }
    }
}
