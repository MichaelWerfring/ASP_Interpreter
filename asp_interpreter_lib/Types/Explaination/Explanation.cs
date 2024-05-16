using System.Drawing;
using System.Text;

namespace asp_interpreter_lib.Types.Explaination;

public class Explanation
{
    private readonly ExplainationText _text;

    public Explanation(ExplainationText text)
    {
        _text = text;
    }

    public string Explain(Statement statement)
    {
        StringBuilder sb = new();
        return sb.ToString();
    }
}