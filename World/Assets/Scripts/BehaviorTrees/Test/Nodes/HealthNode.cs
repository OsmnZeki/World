namespace Scripts.BehaviorTrees.Test.Nodes
{
    public class HealthNode : Node
    {
        public EnemyAI ai;
        public float threshold;
        
        public HealthNode(EnemyAI ai, float threshold)
        {
            this.ai = ai;
            this.threshold = threshold;
        }
        
        public override NodeState Evaluate()
        {
            return ai.CurrentHealth <= threshold ? NodeState.SUCCESS : NodeState.FAILURE;
        }
    }
}