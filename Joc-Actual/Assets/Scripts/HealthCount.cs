using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCount : MonoBehaviour
{
    static int RemainingHpoints;
    public int startingHealth = 15;

    // Start is called before the first frame update
    void Start()
    {
        RemainingHpoints = startingHealth;
    }
    public static void TakeEnemies(int damage)
    {
        RemainingHpoints -= damage;
    }
}
