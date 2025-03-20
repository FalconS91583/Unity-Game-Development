using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    [SerializeField] private Tower balistaPrefab;
    public bool isPlaceable;

    private void OnMouseDown()
    {
        if (isPlaceable)
        {
            bool isPlaced = balistaPrefab.CreateTower(balistaPrefab, transform.position);
            isPlaceable = !isPlaced;
        }
    }
}
