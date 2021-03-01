using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkTowardsPlayer : Node
{
    private ZombieAI zombie;
    private Player player;
    private NavMeshAgent agent;
    private Animator anim;
    private float attackingDistance;
    private float health;
    private float deathThreshold = 0;

    public WalkTowardsPlayer(ZombieAI zombie)
    {
        this.zombie = zombie;
        player = zombie.Player;
        agent = zombie.Agent;
        anim = zombie.Anim;
        attackingDistance = zombie.AttackingDistance;
    }

    public override NodeState Evaluate()
    {
        health = zombie.Health;

        if (health <= deathThreshold)
        {
            agent.isStopped = true;
            anim.SetInteger("moveValue", 0);
            zombie.HasWalkedToPlayer = false;
            state = NodeState.FAILURE;
            return state;
        }

        float distance = Vector3.Distance(player.transform.position, agent.transform.position);

        if (distance > attackingDistance)
        {
            agent.isStopped = false;
            agent.SetDestination(player.transform.position);
            anim.SetInteger("moveValue", 1);
            anim.SetBool("hasWalkedToPlayer", false);
            zombie.HasWalkedToPlayer = false;
            state = NodeState.RUNNING;
        }

        else
        {
            agent.isStopped = true;
            anim.SetInteger("moveValue", 0);
            zombie.HasWalkedToPlayer = true;
            state = NodeState.SUCCESS;
        }
        

        return state;
    }
}
