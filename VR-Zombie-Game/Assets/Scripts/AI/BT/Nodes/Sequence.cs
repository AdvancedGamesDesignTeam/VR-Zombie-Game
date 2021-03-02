using System.Collections.Generic;

namespace AI.BehaviourTree
{
    /// <summary>
    /// Behaviour Tree Sequence Node
    /// </summary>
    public class Sequence : Node
    {
        protected List<Node> nodes = new List<Node>();

        public Sequence(List<Node> nodes)
        {
            this.nodes = nodes;
        }

        public override NodeState Evaluate()
        {
            bool isAnyNodeRunning = false;

            foreach (var node in nodes)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        state = NodeState.FAILURE;
                        return state;
                    case NodeState.SUCCESS:
                        break;
                    case NodeState.RUNNING:
                        isAnyNodeRunning = true;
                        break;
                    default:
                        break;
                }
            };

            state = isAnyNodeRunning ? NodeState.RUNNING : NodeState.SUCCESS;
            return state;
        }
    }
}