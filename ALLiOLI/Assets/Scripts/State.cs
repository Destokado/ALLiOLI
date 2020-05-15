public abstract class State
{
    public abstract void StartState();
    public abstract void UpdateState(float deltaTime);
    public abstract State GetCurrentState(); //Returns the next Phase if this should be ended
    public abstract void EndState();
}