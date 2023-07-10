using System.Collections.Generic;

namespace Scripts.BehaviorTrees
{
    public class Sequence : Node
    {
        public List<Node> childs = new List<Node>();
        
        public Sequence(List<Node> childs)
        {
            this.childs = childs;
        }
        
        public override NodeState Evaluate()
        {
            bool isAnyNodeRunning = false;
            foreach (var child in childs)
            {
                switch (child.Evaluate())
                {
                    case NodeState.RUNNING:
                        isAnyNodeRunning = true;
                        break;
                    case NodeState.SUCCESS:
                        break;
                    case NodeState.FAILURE:
                        nodeState = NodeState.FAILURE;
                        return nodeState;
                }
            }
            
            nodeState = isAnyNodeRunning ? NodeState.RUNNING : NodeState.SUCCESS;
            return nodeState;
        }
    }
}


