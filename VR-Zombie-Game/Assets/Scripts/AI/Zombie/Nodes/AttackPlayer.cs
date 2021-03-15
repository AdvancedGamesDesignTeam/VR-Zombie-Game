using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.BehaviourTree;

namespace AI.ZombieBehaviour
{
    public class AttackPlayer : Node
    {
        private ZombieAI zombie;
        private GameObject player;
        private Animator anim;
        private float health;
        private float deathThreshold = 0;

        public AttackPlayer(ZombieAI zombie)
        {
            this.zombie = zombie;
            player = zombie.Player;
            anim = zombie.Anim;
        }

        public override NodeState Evaluate()
        {
            health = zombie.Health;

            if (health <= deathThreshold)
            {
                zombie.HasWalkedToPlayer = false;
                state = NodeState.FAILURE;
                return state;
            }

            //play attack animation
            if (zombie.HasWalkedToPlayer)
            {
                anim.SetBool("hasWalkedToPlayer", true);
                state = NodeState.SUCCESS;
            }

            else
            {
                anim.SetBool("hasWalkedToPlayer", false);
                state = NodeState.FAILURE;
            }

            return state;
        }
    }
}