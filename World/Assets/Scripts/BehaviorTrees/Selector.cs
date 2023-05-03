using System.Collections.Generic;

namespace Scripts.BehaviorTrees
{
    public class Selector : Node
    {
        public List<Node> childs = new List<Node>();
        
        public Selector(List<Node> childs)
        {
            this.childs = childs;
        }
        
        public override NodeState Evaluate()
        {
            foreach (var node in childs)
            {
                switch (node.Evaluate())
                {
                    case NodeState.RUNNING:
                        nodeState = NodeState.RUNNING;
                        return nodeState;
                    case NodeState.SUCCESS:
                        nodeState = NodeState.SUCCESS;
                        return nodeState;
                    case NodeState.FAILURE:
                        break;
                }
            }

            nodeState = NodeState.FAILURE;
            return nodeState;
        }
    }
}