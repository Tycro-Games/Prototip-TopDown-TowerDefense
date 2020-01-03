using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("speeds")]
    public float MovementSpeed;
    private float speed;
    public float RotationSpeed;
    public LayerMask GroundLayer;
    [Header("PlayerShooting")]
    public Transform ShootingPosition;
    public Weapon currentWeapon;
    private float currentShot = 0;
    [Header("Building")]
    public GameObject currentBuilding;

    // Update is called once per frame
    void Update()
    {
        Ray point = Camera.main.ScreenPointToRay(Input.mousePosition);
        Rotate(point);
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Build(point);
        }
        Move();
        CheckFirerate();


    }
    public void Move()
    {
        speed = MovementSpeed;
        //speed modifier if the player has a weapon
        if (currentWeapon != null)
        {
            speed -= currentWeapon.Weight;
        }
        //Getting the input
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        //moving the player
        transform.Translate(new Vector3(h, 0, v) * speed * Time.deltaTime, Space.World);
    }
    void Rotate(Ray point)
    {
        //making a ray from cam to the ground 
        RaycastHit hit;


        if (Physics.Raycast(point, out hit, 50, GroundLayer)) //50 is max units
        {
            //rotating to face the hit point
            Vector3 Dir = hit.point - transform.position;
            //ensuring that the point is not in other place than 0
            Dir.y = 0;
            //making the new rotation
            Quaternion desiredRot = Quaternion.LookRotation(Dir);
            //applying it
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRot, Time.deltaTime * RotationSpeed);
        }


    }
    void Build(Ray point)
    {
        Debug.Log("Builds");
    }
    void CheckFirerate()
    {
        //firerate
        if (Time.time >= currentShot)
        {
            if (Input.GetMouseButton(0))
            {
                Shoot();
                currentShot = Time.time + 1 / currentWeapon.FireRatePerSecond; //how many shots per second
            }
        }
    }
    void Shoot()
    {
        GameObject projectile = Instantiate(currentWeapon.projectile, ShootingPosition.position, transform.rotation);
    }
}
