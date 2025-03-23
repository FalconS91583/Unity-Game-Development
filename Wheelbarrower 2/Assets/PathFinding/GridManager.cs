using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Vector2Int gridSize;
    [SerializeField] private int worldGridSize = 10;
    public int WorldGridSize { get { return worldGridSize; } }

    private Dictionary<Vector2Int, NodeClass> grid = new Dictionary<Vector2Int, NodeClass>();
    public Dictionary<Vector2Int, NodeClass> Grid { get { return grid;  } }

    private void Awake()
    {
        CreateGrid();
    }

    public NodeClass GetNode(Vector2Int cooridinates)
    {
        if (grid.ContainsKey(cooridinates))
        {
            return grid[cooridinates];
        }

        return null;
    }

    public void BlockNode(Vector2Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            grid[coordinates].isWalkable = false;
        }
    }

    public void ResetNode()
    {
        foreach(KeyValuePair<Vector2Int, NodeClass> entry in grid)
        {
            entry.Value.connectedTo = null;
            entry.Value.isExplode = false;
            entry.Value.isPath = false;
        }
    }

    public Vector2Int GetCoordinatesFromPosition(Vector3 position)
    {
        Vector2Int coordinates = new Vector2Int();

        coordinates.x = Mathf.RoundToInt(position.x / worldGridSize);
        coordinates.y = Mathf.RoundToInt(position.z / worldGridSize);

        return coordinates;
    }

    public Vector3 GetPositionFromCoordinates(Vector2Int coordinates)
    {
        Vector3 position = new Vector3();

        position.x = coordinates.x * worldGridSize;
        position.z = coordinates.y * worldGridSize;

        return position;
    }

    private void CreateGrid()
    {
        for(int x=0; x < gridSize.x; x++)
        {
            for(int y=0; y < gridSize.y; y++)
            {
                Vector2Int coordinates = new Vector2Int(x, y);
                grid.Add(coordinates, new NodeClass(coordinates, true));
            }
        }
    }

}
