using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class WalkToPoints : Node
{
    private ZombieAI zombie;
    private NavMeshAgent agent;
    private Animator anim;
    private List<Transform> points;
    private float maxDistanceToPoint;
    private float health;
    private float deathThreshold = 0;

    public WalkToPoints(ZombieAI zombie)
    {
        this.zombie = zombie;
        agent = zombie.Agent;
        anim = zombie.Anim;
        maxDistanceToPoint = zombie.MaxDistanceToPoint;
        points = zombie.Points.GetComponentsInChildren<Transform>().ToList();
        points.RemoveAt(0);
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
            anim.SetInteger("moveValue", 0);
            agent.isStopped = true;
            state = NodeState.FAILURE;
            return state;
        }

        float distance = Vector3.Distance(zombie.transform.position, points[zombie.PatrolIndex].position);

        if (distance >= maxDistanceToPoint)
        {
            agent.isStopped = false;
            zombie.IsIdle = false;
            zombie.IsPatrolling = true;
            agent.SetDestination(points[zombie.PatrolIndex].position);
            anim.SetInteger("moveValue", 1);
            anim.SetBool("isIdle", false);
            anim.SetBool("IsPatrolling", true);
            state = NodeState.RUNNING;
        }

        else
        {
            agent.isStopped = true;
            zombie.IsIdle = true;
            zombie.IsPatrolling = false;
            anim.SetBool("isIdle", true);
            anim.SetBool("IsPatrolling", false);
            anim.SetInteger("moveValue", 0);

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Agonizing") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !anim.IsInTransition(0))
            {
                zombie.PatrolIndex += 1;

                if (zombie.PatrolIndex > points.Count - 1)
                {
                    zombie.PatrolIndex = 0;
                }
            }

            state = NodeState.SUCCESS;
        }

        return state;
    }
}
