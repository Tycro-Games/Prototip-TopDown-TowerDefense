using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Player : LivingEntity
{
    public Transform FollowMouse;

    public float DamageDelay = .5f;
    public static bool atacked = false;
    [Header("speeds")]
    public float MovementSpeed;
    private float speed;
    public float RotationSpeed;
    public LayerMask GroundLayer;
    Rigidbody rb;
    Vector3 direction;
    [Header("PlayerShooting")]
    public Transform ShootingPosition;
    public Weapon currentWeapon;
    private float currentShot = 0;
    [Header("Building")]
    public float MaxRangeToPlace = 10;
    public LayerMask BuildingLayer;
    public Building currentBuilding;
    GameObject buildingToPlace;
    bool building = false;
    SpaceChecker spaceChecker;

    public override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        Enemy.damageTake += DamageTake;
    }
    private void OnDisable()
    {
        Enemy.damageTake -= DamageTake;
    }
    IEnumerator DamageTake(int dg)
    {

        atacked = true;
        yield return new WaitForSeconds(DamageDelay);//can't be hit in this interval
        TakeHit(dg);
        atacked = false;

    }
    void Update()
    {
        direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));//movement
        Rotate();
        if (!building)
        {
            CheckFirerate();
        }
        if (Input.GetKeyDown(KeyCode.Tab) && currentBuilding != null)
        {
            building = !building;
            Unbuild();
            if (building)
            {
                Build();
            }
        }
    }
    private void FixedUpdate()
    {
        if (building)
        {
            PlacingTower(buildingToPlace);
        }
        else
        {

            Move(direction);

        }
    }
    void Move(Vector3 dir)
    {

        speed = MovementSpeed;
        //speed modifier if the player has a weapon
        if (currentWeapon != null)
        {
            speed -= currentWeapon.Weight;
        }
        //moving the player
        rb.MovePosition(rb.position + dir.normalized * speed * Time.fixedDeltaTime);
    }
    void Rotate()
    {
        //making a ray from cam to the ground 
        RaycastHit hit;
        Ray point = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(point, out hit, 50, GroundLayer)) //50 is max units
        {
            //rotating to face the hit point
            Vector3 Dir = hit.point;
            //ensuring that the point is not in other place than 0
            Dir.y = transform.position.y;
            //making the new rotation
            //applying it

            transform.LookAt(Dir);

            FollowMouse.position = hit.point;
        }
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
    void Unbuild()
    {
        Destroy(buildingToPlace);
    }
    void Build()
    {
        RaycastHit hit;
        Ray point = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(point, out hit, 50, BuildingLayer)) //50 is max units
        {
            buildingToPlace = Instantiate(currentBuilding.building, hit.point, Quaternion.identity);
            spaceChecker = buildingToPlace.GetComponentInChildren<SpaceChecker>();
        }
    }
    void PlacingTower(GameObject ToPlace)
    {
        RaycastHit hit;
        Ray point = Camera.main.ScreenPointToRay(Input.mousePosition);
        //move it
        if (Physics.Raycast(point, out hit, 50, BuildingLayer)) //50 is max units
        {
            ToPlace.transform.position = hit.point;
        }
        if (Vector3.Distance(ToPlace.transform.position, transform.position) > MaxRangeToPlace)
        {
            spaceChecker.inRange = false;
        }
        else
        {
            spaceChecker.inRange = true;
        }
        //place if you press left click
        if (Input.GetMouseButtonDown(0) && spaceChecker.canPlace && spaceChecker.inRange)
        {
            //some trigger to actually build the tower from space checker
            spaceChecker.Place(currentBuilding.material);
            //some check for available towers
            //if you can place them then continue
            buildingToPlace = null;
            Build();
        }
    }
    public override void Die()
    {
        Application.Quit();
        base.Die();

    }

}
