using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
namespace Scripts.BehaviorTrees.Test.Nodes
{
    public class ShootNode : Node
    {
        public NavMeshAgent agent;
        public EnemyAI ai;
        
        public ShootNode(NavMeshAgent agent, EnemyAI ai)
        {
            this.agent = agent;
            this.ai = ai;
        }
        
        public override NodeState Evaluate()
        {
            agent.isStopped = true;
            ai.SetColor(Color.green);
            return NodeState.RUNNING;
        }
    }
}