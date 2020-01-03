using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class StandardBullet : MonoBehaviour
{
    public int Damage;
    public float speed;
    Rigidbody rg;
    private void Start()
    {
        rg = GetComponent<Rigidbody>();
        Destroy(gameObject, 10f);
    }
    private void FixedUpdate()
    {
        rg.MovePosition(transform.position + transform.forward * speed * Time.fixedDeltaTime); //move the bullet
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Enemy")
        {
            other.gameObject.GetComponent<Enemy>().TakeDamage(Damage);
            //some effect

        }
        else
        {
            //some other effect
        }
        Destroy(gameObject);

    }
}
