using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.BehaviourTree;

namespace AI.ZombieBehaviour
{
    public class IsPlayerFound : Node
    {
        private ZombieAI zombie;
        private GameObject player;
        private float soundPerceptionRadius;
        private float fovRadius;
        private float fovAngle;
        private Animator anim;
        private float health;
        private float deathThreshold = 0;

        public IsPlayerFound(ZombieAI zombie)
        {
            this.zombie = zombie;
            player = zombie.Player;
            soundPerceptionRadius = zombie.SoundPerceptionRadius;
            fovRadius = zombie.FOVRadius;
            fovAngle = zombie.FOVAngle;
            anim = zombie.Anim;
        }

        public override NodeState Evaluate()
        {
            health = zombie.Health;

            if (zombie.IsIdle || health <= deathThreshold)
            {
                zombie.IsPlayerFound = false;
                anim.SetBool("isPlayerFound", false);
                state = NodeState.FAILURE;
                return state;
            }

            if (zombie.IsPlayerFound == true)
            {
                state = NodeState.SUCCESS;
                anim.SetBool("isPlayerFound", true);
                zombie.IsPlayerFound = true;
                return state;
            }

            SoundPerception(zombie.transform.position, player.transform.position);

            VisualPerception(zombie.transform.position, player.transform.position);

            return state;
        }

        private void SoundPerception(Vector3 zombiePosition, Vector3 playerPosition)
        {
            float distanceToPlayer = Vector3.Distance(zombiePosition, playerPosition);

            if (distanceToPlayer <= soundPerceptionRadius)
            {
                //sound perception
                //if player makes noise by running, shooting
                Debug.Log("player in my sound perception radius");
            }

            else
            {
                state = NodeState.FAILURE;
            }
        }

        private void VisualPerception(Vector3 zombiePosition, Vector3 playerPosition)
        {
            float distanceToPlayer = Vector3.Distance(zombiePosition, playerPosition);

            if (distanceToPlayer <= fovRadius)
            {
                Vector3 direction = playerPosition - zombiePosition;
                float angle = Vector3.Angle(direction, zombie.transform.forward);

                if (angle <= fovAngle * 0.5f)
                {
                    RaycastHit hit;

                    if (Physics.Raycast(zombiePosition + zombie.transform.up, direction.normalized, out hit, fovRadius))
                    {
                        if (hit.transform.CompareTag("Player"))
                        {
                            state = NodeState.SUCCESS;
                            anim.SetBool("isPlayerFound", true);
                            zombie.IsPlayerFound = true;
                        }
                    }
                }
            }
        }
    }
}