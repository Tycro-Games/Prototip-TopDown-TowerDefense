using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public Sequence[] sequences;
    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }
    IEnumerator SpawnEnemies()
    {

        for (int i = 0; i < sequences.Length; i++)
        {

            for (int j = 0; j < sequences[i].enemies.Length; j++)
            {
                Enemies enemy = sequences[i].enemies[j];//have a single variable for short writing
                Spawn(enemy.Enemy, transform.position);
                yield return new WaitForSeconds(enemy.DelayForSpawn);
            }
        }
    }
    void Spawn(GameObject enemy, Vector3 pos)
    {
        Instantiate(enemy, pos, Quaternion.identity, transform);
    }
}
