using UnityEngine;
using UnityEngine.AI;
namespace Scripts.BehaviorTrees.Test.Nodes
{
    public class GoToCoverNode : Node
    {
        public NavMeshAgent agent;
        public EnemyAI ai;
        
        public GoToCoverNode(NavMeshAgent agent, EnemyAI ai)
        {
            this.agent = agent;
            this.ai = ai;
        }
        
        public override NodeState Evaluate()
        {
            Transform coverSpot = ai.bestCoverSpot;
            if(coverSpot == null)
                return NodeState.FAILURE;
            
            ai.SetColor(Color.yellow);
            float distance = Vector3.Distance(coverSpot.position, agent.transform.position);
            if (distance > .2f)
            {
                agent.isStopped = false;
                agent.SetDestination(coverSpot.position);
                return NodeState.RUNNING;
            }
            else
            {
                agent.isStopped = true;
                return NodeState.SUCCESS;
            }
        }
    }
}