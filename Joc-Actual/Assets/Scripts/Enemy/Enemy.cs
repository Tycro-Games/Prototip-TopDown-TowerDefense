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
    NavMeshAgent agent;
    Transform player;
    public delegate IEnumerator OnDamageTake(int damage);
    public static event OnDamageTake damageTake;
    private Coroutine atack;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
            Die();
    }
    void Update()
    {
        agent.destination = player.position;
        if (Vector3.Distance(transform.position, player.position) <= rangeToHit && atack == null)
        {
            atack = StartCoroutine(Atack(damage));//player in range
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
