using Mirror;
using UnityEngine;

public abstract class MatchPhase : State
{
    public abstract string informativeText { get; protected set; }
    public abstract bool showTrapsCounter { get; protected set; }
    public abstract bool showReadiness { get; protected set; }
    public abstract bool showMatchTimer { get; protected set; }
    public abstract bool inGamingMode { get; protected set; } // Should the cursor be locked and invisible

    public int Id()
    {
        return MatchPhaseManager.GetPhaseId(this);
    }
    
    public virtual void ServerStartState()
    {
        Debug.Log($"Server start of phase '{this.GetType().Name}' ({MatchPhaseManager.GetPhaseId(this)}).");
    }

    public override void StartState()
    {
        Debug.Log($"Starting phase '{this.GetType().Name}' ({MatchPhaseManager.GetPhaseId(this)}).");
    }
    
    public override void EndState()
    {
        Debug.Log($"Ending phase '{this.GetType().Name}' ({MatchPhaseManager.GetPhaseId(this)}).");
    }

    public override void UpdateState(float deltaTime) { }
}