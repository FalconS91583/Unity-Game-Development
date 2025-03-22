using BramaBadura.Combat;
using BramaBadura.Core;
using BramaBadura.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BramaBadura.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private float chaseDistance = 5f;
        [SerializeField] private float suspicionTime = 3f;
        [SerializeField] private float aggroCooldownTime = 3f;
        [SerializeField] private PatrolPath patrolPath;
        [SerializeField] private float waypointTolerance = 1f;
        [SerializeField] private float waypointDwelltime = 2f;
        [Range(0,1)]
        [SerializeField] private float patrolSpeedFraction = 0.2f;

        [SerializeField] private float shoutDistance = 5f;

        private Fighter fighter;
        private GameObject player;
        private Health health;
        private Mover mover;

        private Vector3 guardLocation;
        private float timeSinceLastSawPlayer = Mathf.Infinity;
        private float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        private float timeSinceAggro = Mathf.Infinity;
        private int currentWaypointIndex = 0;

        private void Awake()
        {
            fighter = GetComponent<Fighter>();
            player = GameObject.FindWithTag("Player");
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
        }

        private void Start()
        {
            guardLocation = transform.position;
        }

        private void Update()
        {
            if (health.IsDead()) return;

            GameObject player = GameObject.FindWithTag("Player");
            if (InAttackRange() && fighter.CanAttack(player))
            {
                timeSinceLastSawPlayer = 0;
                AttackBehavior(player);
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspictionBehavior();
            }
            else
            {
                GuardBehavior();
            }

            UpdateTimers();
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
            timeSinceAggro += Time.deltaTime;
        }

        private void GuardBehavior()
        {
            Vector3 nextPos = guardLocation;

            if(patrolPath != null)
            {
                if (AtWaypoint())
                {
                    timeSinceArrivedAtWaypoint = 0f;
                    CycleWaypoint();
                }
                nextPos = GetCurrentWaypoint();
            }

            if(timeSinceArrivedAtWaypoint > waypointDwelltime)
            {
                mover.StartMoveAction(nextPos, patrolSpeedFraction);
            }
        }

        public void AggroEnemy()
        {
            timeSinceAggro = 0;
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;

        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }
        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private void SuspictionBehavior()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehavior(GameObject player)
        {
            fighter.Attack(player);

            AggroNearbyEnemies();
        }

        private void AggroNearbyEnemies()
        {
            RaycastHit[] hits =  Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);

            foreach(RaycastHit hit in hits)
            {
                AIController ai = hit.collider.GetComponent<AIController>();
                if (ai == null) continue;

                ai.AggroEnemy();
            }
        }

        private bool InAttackRange()
        {
            return Vector3.Distance(player.transform.position, transform.position) < chaseDistance
                || timeSinceAggro < aggroCooldownTime;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}


