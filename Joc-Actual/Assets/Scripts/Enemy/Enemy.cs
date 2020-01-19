using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : LivingEntity
{
    public enum States { Chasing_Target, Chasing_Player, Atacking_Player }
    States currentState;
    public int damage;
    float rangeToHit = .5f;
    float RangeToSpot = 5f;
    public float timeBetweenAtacks;
    float nextAtackTime;
    public int damageToObjective = 1;
    NavMeshAgent agent;
    Transform Objective;
    Transform player;
    Transform target;
    Collider newCollider;
    float MycollisionRange;
    float PlayerCollisionRange;
    public delegate IEnumerator OnDamageTake(int damage);
    public static event OnDamageTake damageTake;

    public override void Start()
    {
        base.Start();

        MycollisionRange = GetComponent<CapsuleCollider>().radius;
        PlayerCollisionRange = GetComponent<CapsuleCollider>().radius;
        agent = GetComponent<NavMeshAgent>();
        Objective = GameObject.FindGameObjectWithTag("Finish").transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        target = Objective;
        agent.SetDestination(target.position);
        agent.stoppingDistance = MycollisionRange + PlayerCollisionRange + rangeToHit;
        StartCoroutine(UpdatePath());
    }
    bool SqrtDist(Transform newTarget, float range)
    {
        float sqrDstToTarget = (newTarget.position - transform.position).sqrMagnitude;
        if (sqrDstToTarget <= Mathf.Pow(range, 2))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    void Update()
    {
        if (player != null)
        {
            if (target == Objective)
            {
                currentState = States.Chasing_Target;
                if (SqrtDist(player, RangeToSpot))//if the player is close is spoted
                {
                    currentState = States.Chasing_Player;
                    target = player;
                }
            }
            if (target == player)
            {

                if (Time.time > nextAtackTime)//check if it can atack the player
                {

                    if (SqrtDist(player, MycollisionRange + PlayerCollisionRange + rangeToHit))
                    {
                        nextAtackTime = Time.time + timeBetweenAtacks;

                        GetComponent<Renderer>().material.SetColor("_BaseColor", Color.red);
                        StartCoroutine(Atack(damage));

                    }

                }
            }
        }
    }
    IEnumerator UpdatePath()
    {
        float refreshRate = .25f;
        if (target == null)
            target = Objective;

        while (target != null)
        {
            if (currentState != States.Atacking_Player)
            {
                if (target == player)
                {
                    Vector3 dir = (target.position - transform.position).normalized;
                    Vector3 newTargetPosition = target.position - dir * (MycollisionRange + PlayerCollisionRange + rangeToHit / 2);
                    agent.stoppingDistance = MycollisionRange + PlayerCollisionRange + rangeToHit / 2;
                    if (!dead)
                        agent.SetDestination(newTargetPosition);
                }
                else
                {
                    if (SqrtDist(target, 1))
                    {
                        HealthCount.TakeObjectiveDamage(damageToObjective);
                        Die();
                    }
                }

            }
            yield return new WaitForSeconds(refreshRate);
        }


    }


    IEnumerator Atack(int damage)
    {
        currentState = States.Atacking_Player;
        agent.enabled = false;

        //animation
        yield return new WaitForSeconds(1);
        if (damageTake != null && !Player.atacked)
        {
            StartCoroutine(damageTake(damage));
        }
        GetComponent<Renderer>().material.SetColor("_BaseColor", Color.white);
        currentState = States.Chasing_Player;
        agent.enabled = true;
        yield return null;
    }
}
