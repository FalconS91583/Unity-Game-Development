using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    [SerializeField] private Tower balistaPrefab;
    public bool isPlaceable;

    GridManager gridManager;
    PathFinder pathFinder;
    Vector2Int coordinates = new Vector2Int();
    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        pathFinder = FindObjectOfType<PathFinder>();
    }

    private void Start()
    {
        if(gridManager != null)
        {
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);

            if (!isPlaceable)
            {
                gridManager.BlockNode(coordinates);
            }
        }
    }

    private void OnMouseDown()
    {
        if (gridManager.GetNode(coordinates).isWalkable && !pathFinder.WillBlockPath(coordinates))
        {
            bool isPlaced = balistaPrefab.CreateTower(balistaPrefab, transform.position);
            if (isPlaced)
            {
                gridManager.BlockNode(coordinates);
                pathFinder.NotifyReceiver();
            }
        }
    }
}
