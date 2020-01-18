using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Projectile : MonoBehaviour
{
    public LayerMask collisionMask;
    public int Damage;
    public float speed = 10;
    float skinWidth = .1f;
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
    private void Update()
    {
        float moveDistance = speed * Time.deltaTime;
        CheckCollision(moveDistance);
        transform.Translate(Vector3.forward * moveDistance);
    }
    void CheckCollision(float moveDistance)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, moveDistance + skinWidth, collisionMask, QueryTriggerInteraction.Collide))
        {
            OnHitObject(hit);
        }
    }
    void OnHitObject(RaycastHit hit)
    {
        IDamageable damageable = hit.collider.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeHit(Damage, hit);
        }
        Destroy(gameObject);
    }
}
