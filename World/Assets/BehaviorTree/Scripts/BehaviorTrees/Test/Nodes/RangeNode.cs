using UnityEngine;
namespace Scripts.BehaviorTrees.Test.Nodes
{
    public class RangeNode : Node
    {
        public float range;
        public Transform target;
        public Transform origin;
        
        public RangeNode(float range, Transform target, Transform origin)
        {
            this.range = range;
            this.target = target;
            this.origin = origin;
        }
        public override NodeState Evaluate()
        {
            float distance = Vector3.Distance(target.position, origin.position);
            return distance <= range ? NodeState.SUCCESS : NodeState.FAILURE;
        }
    }
}