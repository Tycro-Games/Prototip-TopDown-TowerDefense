using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceChecker : MonoBehaviour
{
    [HideInInspector] public bool canPlace = true;
    [HideInInspector] public bool inRange = true;
    public Color CanPlaceColor;
    public Color CanNotPlaceColor;
    Renderer render;
    public LayerMask m_LayerMask;
    private void Start()
    {
        render = GetComponent<Renderer>();
    }
    void Update()
    {
        //check if something is in range
        Collider[] InBox = Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity, m_LayerMask);
        if (InBox.Length == 1)
        {
            canPlace = true;
        }
        else
        {
            canPlace = false;
        }
        ChangingColors();


    }
    public void ChangingColors()
    {
        if (canPlace && inRange)
        {
            render.material.SetColor("_BaseColor", CanPlaceColor);
        }
        else
        {
            render.material.SetColor("_BaseColor", CanNotPlaceColor);
        }
    }
    public void Place(Material mat)
    {
        render.material = mat;
        gameObject.AddComponent<BoxCollider>();
        StaticEventToBuild.BuildNavMesh();
        //activate the script for this tower
        Destroy(this);
    }

}
