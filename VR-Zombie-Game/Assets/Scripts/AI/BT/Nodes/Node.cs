/// <summary>
/// Behaviour Tree Main Node
/// </summary>
public abstract class Node
{
    protected NodeState state;
    public NodeState State { get { return state; } }
    public abstract NodeState Evaluate();
}

public enum NodeState
{
    FAILURE = 0,
    SUCCESS = 1,
    RUNNING = 2
}