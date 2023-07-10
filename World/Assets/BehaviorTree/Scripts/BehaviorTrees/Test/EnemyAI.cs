using System;
using System.Collections.Generic;
using Scripts.BehaviorTrees.Test.Nodes;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts.BehaviorTrees.Test
{
    public class EnemyAI : MonoBehaviour
    {

        public float startingHealth;
        
        public float lowHealthThreshold;
        public float healthRestoreRate;

        public float currentHealth;
        public float CurrentHealth
        {
            get => currentHealth;
            set => currentHealth = Mathf.Clamp(value, 0, startingHealth);
        }

        public float chasingRange;
        public float shootingRange;

        public Transform playerTransform;

        public Cover[] availableCovers;
        [NonSerialized] public Transform bestCoverSpot;
        [NonSerialized] public NavMeshAgent agent;
        
        [NonSerialized] public Material material;
        public Node topNode;

        void Awake()
        {
            material = GetComponent<MeshRenderer>().material;
            agent = GetComponent<NavMeshAgent>();
        }

        void Start()
        {
            CurrentHealth = startingHealth;
            ConstructBehaviorTree();
        }
        
        public void Update()
        {
            topNode.Evaluate();
            if (topNode.nodeState == NodeState.FAILURE)
            {
                SetColor(Color.magenta);
                agent.isStopped = true;
            }
            
            CurrentHealth += healthRestoreRate * Time.deltaTime;
        }

        void OnMouseDown()
        {
            CurrentHealth -= 10;
        }

        void ConstructBehaviorTree()
        {
            IsCoverAvailableNode isCoverAvailableNode = new IsCoverAvailableNode(availableCovers, playerTransform, this);
            GoToCoverNode goToCoverNode = new GoToCoverNode(agent, this);
            HealthNode healthNode = new HealthNode(this, lowHealthThreshold);
            IsCoveredNode isCoveredNode = new IsCoveredNode(playerTransform, transform);
            ChaseNode chaseNode = new ChaseNode(playerTransform, agent, this);
            RangeNode chasingRangeNode = new RangeNode(chasingRange,playerTransform, transform);
            RangeNode shootingRangeNode = new RangeNode(shootingRange,playerTransform, transform);
            ShootNode shootNode = new ShootNode(agent,this);
            
            Sequence chaseSequence = new Sequence(new List<Node>(){ chasingRangeNode, chaseNode});
            Sequence shootRangeSequence = new Sequence(new List<Node>(){ shootingRangeNode, shootNode});
            Sequence goToCoverSequence = new Sequence(new List<Node>(){ isCoverAvailableNode, goToCoverNode});
            Selector findCoverSelector = new Selector(new List<Node>(){ goToCoverSequence, chaseSequence});
            Selector tryToTakeCoverNode = new Selector(new List<Node>(){ isCoveredNode, findCoverSelector});
            Sequence mainCoverSequence = new Sequence(new List<Node>(){ healthNode, tryToTakeCoverNode});

            topNode = new Selector(new List<Node>() {mainCoverSequence, shootRangeSequence, chaseSequence});
        }

        public void SetColor(Color color)
        {
            material.color = color;
        }
        
        public void SetBestCoverSpot(Transform bestSpot)
        {
            bestCoverSpot = bestSpot;
        }
    }

}

