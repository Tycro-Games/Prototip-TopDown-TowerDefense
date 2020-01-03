using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "new weapon", menuName = "Create/ new weapon", order = 1)]
[ExecuteInEditMode]
public class Weapon : ScriptableObject
{
    public Image icon;
    public float FireRatePerSecond;
    public float Weight;
    public GameObject projectile;

}
