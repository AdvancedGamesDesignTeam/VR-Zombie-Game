using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using AI.BehaviourTree;

namespace AI.ZombieBehaviour
{
    public enum ZombieType
    {
        GRUNT,
        SPITTER
    }

    public class ZombieAI : MonoBehaviour
    {
        
        private Node topNode;
        private float fovAngle = 110f;
        [SerializeField] ZombieType zombieType;
        [SerializeField] private float health = 100;
        [SerializeField] private GameObject player;
        [SerializeField] private float soundPerceptionRadius = 30f;
        [SerializeField] private float fovRadius = 20f;
        [SerializeField] private float attackingDistance = 3.5f;
        [SerializeField] private float maxDistanceToPoint = 3.5f;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Animator anim;
        [SerializeField] private Transform points;
        [SerializeField] private float maxWaitTime;
        public Transform Points { get { return points; } }
        public bool IsPlayerFound { get; set; }
        public bool HasWalkedToPlayer { get; set; }
        public bool IsIdle { get; set; } = true;
        public bool IsPatrolling { get; set; }
        public int PatrolIndex { get; set; }
        public float Health { get { return health; } }
        public GameObject Player { get { return player; } }
        public float SoundPerceptionRadius { get { return soundPerceptionRadius; } }
        public float FOVRadius { get { return fovRadius; } }
        public float FOVAngle { get { return fovAngle; } }
        public Animator Anim { get { return anim; } }
        public NavMeshAgent Agent { get { return agent; } }
        public float AttackingDistance { get { return attackingDistance; } }
        public float MaxDistanceToPoint { get { return maxDistanceToPoint; } }


        private void Start()
        {
            ConstructBehaviourTree();
        }

        private void Update()
        {
            topNode.Evaluate();
        }

        private void ConstructBehaviourTree()
        {
            IsPlayerFound isPlayerFoundNode = new IsPlayerFound(this);
            WalkTowardsPlayer walkTowardsPlayerNode = new WalkTowardsPlayer(this);
            AttackPlayer attackPlayerNode = new AttackPlayer(this);
            Sequence attackSequenceNode = new Sequence(new List<Node> { isPlayerFoundNode, walkTowardsPlayerNode, attackPlayerNode });
            Idle idleNode = new Idle(this);
            WalkToPoints walkToPointsNode = new WalkToPoints(this);
            Sequence patrolSequenceNode = new Sequence(new List<Node> { idleNode, walkToPointsNode });
            IsHealthZero isHealthZeroNode = new IsHealthZero(this);
            Die dieNode = new Die(this);
            Sequence deathSequenceNode = new Sequence(new List<Node> { isHealthZeroNode, dieNode });
            topNode = new Selector(new List<Node> { attackSequenceNode, deathSequenceNode, patrolSequenceNode });
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, soundPerceptionRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, fovRadius);
            Gizmos.color = Color.green;
        }
    }
}