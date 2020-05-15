public abstract class MatchPhase : State
{
    public abstract string informativeText { get; protected set; }
    public abstract bool showTrapsCounter { get; protected set; }
    public abstract bool showReadiness { get; protected set; }
    public abstract bool showMatchTimer { get; protected set; }
}