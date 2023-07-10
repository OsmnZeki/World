using UnityEngine;
using UnityEngine.AI;
namespace Scripts.BehaviorTrees.Test.Nodes
{
    public class ChaseNode : Node
    {
        public Transform target;
        public NavMeshAgent agent;
        public EnemyAI ai;
        
        public ChaseNode(Transform target, NavMeshAgent agent, EnemyAI ai)
        {
            this.target = target;
            this.agent = agent;
            this.ai = ai;
        }
        
        public override NodeState Evaluate()
        {
            ai.SetColor(Color.yellow);
            float distance = Vector3.Distance(target.position, agent.transform.position);
            if (distance > .2f)
            {
                agent.isStopped = false;
                agent.SetDestination(target.position);
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