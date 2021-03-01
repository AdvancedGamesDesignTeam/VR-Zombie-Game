/// <summary>
/// Behaviour Tree Inverter Node
/// </summary>
public class Inverter : Node
{
    protected Node node;

    public Inverter(Node node)
    {
        this.node = node;
    }

    public override NodeState Evaluate()
    {
        switch (node.Evaluate())
        {
            case NodeState.FAILURE:
                state = NodeState.SUCCESS;
                break;
            case NodeState.SUCCESS:
                state = NodeState.FAILURE;
                break;
            case NodeState.RUNNING:
                state = NodeState.RUNNING;
                break;
            default:
                break;
        }

        return state;
    }
}
