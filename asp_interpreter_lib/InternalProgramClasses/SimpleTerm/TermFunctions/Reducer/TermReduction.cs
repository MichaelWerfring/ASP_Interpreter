using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;

namespace asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Reducer
{
    public class TermReduction
    {
        public TermReduction(IEnumerable<ISimpleTerm> lefts, IEnumerable<ISimpleTerm> rights)
        {
            ArgumentNullException.ThrowIfNull(lefts);
            ArgumentNullException.ThrowIfNull(rights);
            if (lefts.Count() != rights.Count())
            {
                throw new ArgumentException("Must have equal term count");
            }

            Lefts = lefts;
            Rights = rights;
        }

        public IEnumerable<ISimpleTerm> Lefts { get; }

        public IEnumerable<ISimpleTerm> Rights { get; }
    }
}