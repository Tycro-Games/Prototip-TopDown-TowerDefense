using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class StaticEventToBuild : MonoBehaviour
{
    static NavMeshSurface nav;
    private void Start()
    {
        nav = GetComponent<NavMeshSurface>();
    }
    public static void BuildNavMesh()
    {
        nav.BuildNavMesh();
    }
}
