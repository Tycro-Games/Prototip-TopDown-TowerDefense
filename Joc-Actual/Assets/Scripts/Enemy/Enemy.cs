using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{
    public int Health = 100;
    public int damage;
    public float rangeToHit = 2f;
    public float timeAnimation;
    public int damageToObjective = 1;
    NavMeshAgent agent;
    Transform Objective;
    public delegate IEnumerator OnDamageTake(int damage);
    public static event OnDamageTake damageTake;
    private Coroutine atack = null;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Objective = GameObject.FindGameObjectWithTag("Finish").transform;
    }
    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
            Die();
    }
    void Update()
    {

        agent.SetDestination(Objective.position);

        if (Vector3.Distance(transform.position, Objective.position) <= agent.stoppingDistance && Objective.tag == "Finish")
        {
            HealthCount.TakeEnemies(damageToObjective);
            Destroy(gameObject);
        }
        if (Vector3.Distance(transform.position, Objective.position) <= agent.stoppingDistance && Objective.tag == "Player" && atack == null)
        {
            atack = StartCoroutine(Atack(damage));
        }
    }
    void Die()
    {
        //play death animation
        Destroy(gameObject);


    }

    IEnumerator Atack(int damage)
    {

        yield return new WaitForSeconds(timeAnimation);//can atack after animation
        if (damageTake != null && !Player.atacked)
            StartCoroutine(damageTake(damage));
        atack = null;
        yield return null;
    }
}
