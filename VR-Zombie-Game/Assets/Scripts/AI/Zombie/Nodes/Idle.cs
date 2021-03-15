using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.BehaviourTree;

namespace AI.ZombieBehaviour
{
    public class Idle : Node
    {
        private ZombieAI zombie;
        private Animator anim;
        private float health;
        private float deathThreshold = 0;

        public Idle(ZombieAI zombie)
        {
            this.zombie = zombie;
            anim = zombie.Anim;
        }

        public override NodeState Evaluate()
        {
            health = zombie.Health;

            if (zombie.IsPlayerFound == true || health <= deathThreshold)
            {
                zombie.IsIdle = false;
                zombie.IsPatrolling = false;
                anim.SetBool("isIdle", false);
                anim.SetBool("IsPatrolling", false);
                state = NodeState.FAILURE;
                return state;
            }

            zombie.IsIdle = true;
            zombie.IsPatrolling = false;
            anim.SetBool("isIdle", true);
            anim.SetBool("IsPatrolling", false);
            state = NodeState.SUCCESS;
            return state;
        }
    }
}