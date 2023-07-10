namespace Scripts.BehaviorTrees
{
    public abstract class Node
    {
        public NodeState nodeState;
        
        public abstract NodeState Evaluate();
    }

    public enum NodeState
    {
        RUNNING,
        SUCCESS,
        FAILURE
    }
    
}