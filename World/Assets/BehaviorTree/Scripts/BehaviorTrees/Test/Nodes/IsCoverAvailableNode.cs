using UnityEngine;
namespace Scripts.BehaviorTrees.Test.Nodes
{
    public class IsCoverAvailableNode : Node
    {
        public Cover[] availableCovers;
        public Transform target;
        public EnemyAI ai;

        public IsCoverAvailableNode(Cover[] availableCovers, Transform target, EnemyAI ai)
        {
            this.availableCovers = availableCovers;
            this.target = target;
            this.ai = ai;
        }

        public override NodeState Evaluate()
        {
            Transform bestSpot = FindBestCoverSpot();
            ai.SetBestCoverSpot(bestSpot);
            return bestSpot != null ? NodeState.SUCCESS : NodeState.FAILURE;
        }

        Transform FindBestCoverSpot()
        {
            if (ai.bestCoverSpot != null)
            {
                if (CheckSpotIsValid(ai.bestCoverSpot))
                {
                    return ai.bestCoverSpot;
                }
            }
            
            float minAngle = 90;
            Transform bestSpot = null;
            for (int i = 0; i < availableCovers.Length; i++)
            {
                Transform bestSpotInCover = FindBestSpotInCover(availableCovers[i], ref minAngle);
                if (bestSpotInCover != null)
                {
                    bestSpot = bestSpotInCover;
                }
            }
            return bestSpot;
        }

        Transform FindBestSpotInCover(Cover cover, ref float minAngle)
        {
            Transform[] availableSpots = cover.coverSpots;
            Transform bestSpot = null;

            for (int i = 0; i < availableSpots.Length; i++)
            {
                Vector3 direction = target.position - availableSpots[i].position;
                if (CheckSpotIsValid(availableSpots[i]))
                {
                    float angle = Vector3.Angle(availableSpots[i].forward, direction);
                    if (angle < minAngle)
                    {
                        minAngle = angle;
                        bestSpot = availableSpots[i];
                    }
                }
            }

            return bestSpot;
        }

        bool CheckSpotIsValid(Transform availableSpot)
        {
            RaycastHit hit;
            if (Physics.Raycast(availableSpot.position, target.position - availableSpot.position, out hit))
            {
                if (hit.transform != target)
                {
                    return true;
                }
            }
            return false;
        }
    }
}