using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField] private List<WayPoint> path = new List<WayPoint>();
    [Range(1f, 5f)]
    [SerializeField] private float speed = 1f;

    private Enemy enemy;
    void OnEnable()
    {
        FindPath();
        ReturnToStart();
        StartCoroutine(FollowPath());
    }
    private void Start()
    {
        enemy = GetComponent<Enemy>();
    }
    private void FindPath()
    {
        path.Clear();

        GameObject parent = GameObject.FindGameObjectWithTag("Path");

        foreach(Transform child in parent.transform)
        {
            WayPoint waypoint = child.GetComponent<WayPoint>();
            if(waypoint != null)
            {
                path.Add(waypoint);
            }
        }
    }

    private void ReturnToStart()
    {
        transform.position = path[0].transform.position;
    }

    private IEnumerator FollowPath()
    {
        foreach(WayPoint waypoints in path)
        {
            Vector3 startPos = transform.position;
            Vector3 endPos = waypoints.transform.position;
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
