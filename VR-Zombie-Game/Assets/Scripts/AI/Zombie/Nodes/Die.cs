using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.BehaviourTree;

namespace AI.ZombieBehaviour
{
    public class Die : Node
    {
        private ZombieAI zombie;
        private Animator anim;

        public Die(ZombieAI zombie)
        {
            this.zombie = zombie;
            anim = zombie.Anim;
        }

        public override NodeState Evaluate()
        {
            anim.SetInteger("health", 0);
            Object.Destroy(zombie.gameObject, 5f);
            return state;
        }
    }
}