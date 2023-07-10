using System.Collections.Generic;

namespace Scripts.BehaviorTrees
{
    public class Inverter : Node
    {
        public Node child;

        public Inverter(Node child)
        {
            this.child = child;
        }

        public override NodeState Evaluate()
        {
            switch (child.Evaluate())
            {
                case NodeState.RUNNING:
                    nodeState = NodeState.RUNNING;
                    break;
                case NodeState.SUCCESS:
                    nodeState = NodeState.FAILURE;
                    break;
                case NodeState.FAILURE:
                    nodeState = NodeState.SUCCESS;
                    break;
            }
            return nodeState;
        }
    }
}