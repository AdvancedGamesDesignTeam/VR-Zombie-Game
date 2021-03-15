using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.BehaviourTree;

namespace AI.ZombieBehaviour
{
    public class IsHealthZero : Node
    {
        private ZombieAI zombie;
        private Animator anim;
        private float health;
        private float deathThreshold = 0;

        public IsHealthZero(ZombieAI zombie)
        {
            this.zombie = zombie;
            anim = zombie.Anim;
        }

        public override NodeState Evaluate()
        {
            health = zombie.Health;
            anim.SetInteger("health", (int)health);

            if (health <= deathThreshold)
            {
                state = NodeState.SUCCESS;
            }

            else
            {
                state = NodeState.FAILURE;
            }

            return state;
        }
    }
}