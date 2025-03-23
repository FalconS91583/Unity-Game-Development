using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [SerializeField] private Vector2Int startCoordinates;
    public Vector2Int StartCoordinates { get { return startCoordinates; } }
    [SerializeField] private Vector2Int destinationCoordinates;
    public Vector2Int DestinationCoordinates { get { return destinationCoordinates; } }

    private NodeClass startNode;
    private NodeClass destinationNode;
    private NodeClass currentSearchNode;

    Queue<NodeClass> frontier = new Queue<NodeClass>();
    Dictionary<Vector2Int, NodeClass> reach = new Dictionary<Vector2Int, NodeClass>();

    Vector2Int[] directions = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };
    private GridManager gridManager;
    Dictionary<Vector2Int, NodeClass> grid = new Dictionary<Vector2Int, NodeClass>();

    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();

        if(gridManager != null)
        {
            grid = gridManager.Grid;
            startNode = grid[startCoordinates];
            destinationNode = grid[destinationCoordinates];
        }
    }

    private void Start()
    {
        GetNewPath();
    }

    public List<NodeClass> GetNewPath()
    {
        return GetNewPath(startCoordinates);
    }

    public List<NodeClass> GetNewPath(Vector2Int coordinates)
    {
        gridManager.ResetNode();
        PathSearch(coordinates);
        return BuildingPath();
    }

    private void ExploreNeighbors()
    {
        List<NodeClass> neighbours = new List<NodeClass>();

        foreach(Vector2Int direction in directions)
        {
            Vector2Int neighbourCoordinates = currentSearchNode.coordinates + direction;

            if (grid.ContainsKey(neighbourCoordinates))
            {
                neighbours.Add(grid[neighbourCoordinates]);
            }
        }

        foreach(NodeClass neighbor in neighbours)
        {
            if(!reach.ContainsKey(neighbor.coordinates) && neighbor.isWalkable)
            {
                neighbor.connectedTo = currentSearchNode;
                reach.Add(neighbor.coordinates, neighbor);
                frontier.Enqueue(neighbor);
            }
        }
    }

    private List<NodeClass> BuildingPath()
    {
        List<NodeClass> path = new List<NodeClass>();
        NodeClass currentNode = destinationNode;

        path.Add(currentNode);
        currentNode.isPath = true;

        while (currentNode.connectedTo != null)
        {
            currentNode = currentNode.connectedTo;
            path.Add(currentNode);
            currentNode.isPath = true;
        }

        path.Reverse();

        return path;
    }

    private void PathSearch(Vector2Int coordinates)
    {
        startNode.isWalkable = true;
        destinationNode.isWalkable = true;

        frontier.Clear();
        reach.Clear();

        bool isRunning = true;

        frontier.Enqueue(grid[coordinates]);
        reach.Add(coordinates, grid[coordinates]);

        while(frontier.Count > 0 && isRunning)
        {
            currentSearchNode = frontier.Dequeue();
            currentSearchNode.isExplode = true;
            ExploreNeighbors();
            if(currentSearchNode.coordinates == destinationCoordinates)
            {
                isRunning = false;
            }
        }
    }

    public bool WillBlockPath(Vector2Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            bool previousState = grid[coordinates].isWalkable;
            grid[coordinates].isWalkable = false;
            List<NodeClass> newPath = GetNewPath();
            grid[coordinates].isWalkable = previousState;

            if(newPath.Count <=1)
            {
                GetNewPath();
                return true;
            }  
        }

        return false;
    }

    public void NotifyReceiver()
    {
        BroadcastMessage("FindPath",false , SendMessageOptions.DontRequireReceiver);
    }
}
