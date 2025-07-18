using UnityEngine;
using UnityEngine.SceneManagement;

public class Object_Waypoint : MonoBehaviour
{
    [SerializeField] private string transferToScene;
    [Space]
    [SerializeField] private RespawnType waypointType;
    [SerializeField] private RespawnType connectedWaypoint;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private bool canBeTriggerd = true;

    public RespawnType GetWaypointType() => waypointType;
    public Vector3 Getposition()
    {
        canBeTriggerd = false;  
        return respawnPoint == null ? transform.position : respawnPoint.position;   
    }

    private void OnValidate()
    {
        gameObject.name = "Object Waypoint - " + waypointType.ToString() + " - " + transferToScene;

        if(waypointType == RespawnType.Enter)
            connectedWaypoint = RespawnType.Exit;

        if(waypointType == RespawnType.Exit)
            connectedWaypoint = RespawnType.Enter;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canBeTriggerd == false)
            return;


        GameManager.instance.ChangeScene(transferToScene, connectedWaypoint);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canBeTriggerd = true;
    }
}
