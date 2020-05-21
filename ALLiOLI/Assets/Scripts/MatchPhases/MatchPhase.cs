using Mirror;

public abstract class MatchPhase : State
{
    public abstract string informativeText { get; protected set; }
    public abstract bool showTrapsCounter { get; protected set; }
    public abstract bool showReadiness { get; protected set; }
    public abstract bool showMatchTimer { get; protected set; }
    [Server]
    public abstract void ServerStartState();

    public int Id()
    {
        return MatchPhaseManager.GetPhaseId(this);
    }
}