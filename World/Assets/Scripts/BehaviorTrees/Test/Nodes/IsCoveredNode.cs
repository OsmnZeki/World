using UnityEngine;
namespace Scripts.BehaviorTrees.Test.Nodes
{
    public class IsCoveredNode : Node
    {
        public Transform target;
        public Transform origin;
        
        public IsCoveredNode(Transform target, Transform origin)
        {
            this.target = target;
            this.origin = origin;
        }
        public override NodeState Evaluate()
        {
            RaycastHit hit;
            if (Physics.Raycast(origin.position, target.position - origin.position, out hit))
            {
                if (hit.transform != target)
                {
                    return NodeState.SUCCESS;
                }
            }
            return NodeState.FAILURE;
        }
    }
}