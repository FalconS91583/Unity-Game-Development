using StarterAssets;
using UnityEngine;
using UnityEngine.AI;

public class Robot : MonoBehaviour
{
    private NavMeshAgent agent;
    private FirstPersonController player;

    const string PLAYER_STRING = "Player";

    [SerializeField] private Transform target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        player = FindFirstObjectByType<FirstPersonController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!player) return;

        agent.SetDestination(player.transform.position);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag(PLAYER_STRING))
        {
            EnemyHealth enemyHealth = GetComponent<EnemyHealth>();
            enemyHealth.SelfDestruct();
        }
    }
}
