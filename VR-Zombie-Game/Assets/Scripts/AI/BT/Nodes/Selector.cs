using System.Collections.Generic;

namespace AI.BehaviourTree
{
    /// <summary>
    /// Behaviour Tree Selector Node
    /// </summary>
    public class Selector : Node
    {
        protected List<Node> nodes = new List<Node>();

        public Selector(List<Node> nodes)
        {
            this.nodes = nodes;
        }

        public override NodeState Evaluate()
        {
            foreach (var node in nodes)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        break;
                    case NodeState.SUCCESS:
                        state = NodeState.SUCCESS;
                        return state;
                    case NodeState.RUNNING:
                        state = NodeState.RUNNING;
                        return state;
                    default:
                        break;
                }
            };

            state = NodeState.FAILURE;
            return state;
        }
    }
}