using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : LivingEntity
{
    public enum States { Chasing_Target, Chasing_Player, Atacking_Player }
    States currentState;
    public int damage;
    public float rangeToHit = 2f;
    public float RangeToSpot = 5f;
    public float timeBetweenAtacks;
    float nextAtackTime;
    public int damageToObjective = 1;
    NavMeshAgent agent;
    Transform Objective;
    Transform player;
    Transform target;
    public delegate IEnumerator OnDamageTake(int damage);
    public static event OnDamageTake damageTake;
    public override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        Objective = GameObject.FindGameObjectWithTag("Finish").transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        target = Objective;

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

                    if (SqrtDist(player, rangeToHit))
                    {
                        nextAtackTime = Time.time + timeBetweenAtacks;
                        currentState = States.Atacking_Player;
                        StartCoroutine(Atack(damage));

                    }

                }
            }
        }
    }
    IEnumerator UpdatePath()
    {
        float refreshRate = .25f;
        if (target == Objective)
        {
            print(SqrtDist(Objective, rangeToHit));
            currentState = States.Chasing_Target;
            if (SqrtDist(Objective, rangeToHit))
            {
                HealthCount.TakeObjectiveDamage(damageToObjective);
                Die();
            }
        }
        if (target == null)
            target = Objective;
        while (target != null)
        {
            Vector3 newTargetPosition = new Vector3(target.position.x, 0, target.position.z);
            agent.SetDestination(newTargetPosition);
            yield return new WaitForSeconds(refreshRate);
        }


    }


    IEnumerator Atack(int damage)
    {
        //animation
        if (damageTake != null && !Player.atacked)
        {
            StartCoroutine(damageTake(damage));

        }
        yield return null;
    }
}
