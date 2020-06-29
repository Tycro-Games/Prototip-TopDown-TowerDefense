using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "new sequence", menuName = "Create/ new sequence", order = 3)]
public class Sequence : ScriptableObject
{
    public Enemies[] enemies;
}
[System.Serializable]
public struct Enemies
{
    public GameObject Enemy;
    public float DelayForSpawn;
    public Enemies(GameObject enemy, float delayForSpawn)
    {
        this.Enemy = enemy;
        this.DelayForSpawn = delayForSpawn;
    }
}