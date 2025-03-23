using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{
    private List<NodeClass> path = new List<NodeClass>();
    [Range(1f, 5f)]
    [SerializeField] private float speed = 1f;

    private PathFinder pathFinder;
    private GridManager gridManager;

    private Enemy enemy;
    void OnEnable()
    {
        ReturnToStart();
        FindPath(true);
    }
    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        gridManager = FindObjectOfType<GridManager>();
        pathFinder = FindObjectOfType<PathFinder>();

    }
    private void FindPath(bool resetPath)
    {
        Vector2Int coordinates;

        if (resetPath)
        {
            coordinates = pathFinder.StartCoordinates;
            transform.position = gridManager.GetPositionFromCoordinates(coordinates); 
        }
        else
        {
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position); 
        }

        StopAllCoroutines();
        path.Clear();
        path = pathFinder.GetNewPath(coordinates); 
        StartCoroutine(FollowPath());
    }


    private void ReturnToStart()
    {
        transform.position = gridManager.GetPositionFromCoordinates(pathFinder.StartCoordinates);
    }

    private IEnumerator FollowPath()
    {
        for(int i=1; i< path.Count; i++)
        {
            Vector3 startPos = transform.position;
            Vector3 endPos = gridManager.GetPositionFromCoordinates(path[i].coordinates);
            float travelPrecent = 0f;

            transform.LookAt(endPos);

            while(travelPrecent < 1f)
            {
                travelPrecent += Time.deltaTime * speed;
                transform.position = Vector3.Lerp(startPos, endPos, travelPrecent);
                yield return new WaitForEndOfFrame();
            }
        }
        FinishPath();
    }

    private void FinishPath()
    {

        enemy.StealGold();
        gameObject.SetActive(false);
    }

}
