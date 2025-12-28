using BramaBadura.Core;
using BramaBadura.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BramaBadura.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        private NavMeshAgent agent;
        private Health health;
        [SerializeField] private Transform targetPos;
        [SerializeField] private float maxSpeed = 6f;
        [SerializeField] private float maxPathLenght = 40f;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
        }
        void Start()
        {
            

        }

        void Update()
        {
            agent.enabled = !health.IsDead();

            UpadateAnimator();
        }

        public bool CanMoveTo(Vector3 destination)
        {
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);
            if (!hasPath) return false;
            if (path.status != NavMeshPathStatus.PathComplete) return false;
            if (GetPathLenght(path) > maxPathLenght) return false;

            return true;
        }

        private float GetPathLenght(NavMeshPath path)
        {
            float total = 0;
            if (path.corners.Length < 2) return total;
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                total += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }


            return total;
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);

        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            agent.SetDestination(destination);
            agent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            agent.isStopped = false;
        }

        public void Cancel()
        {
            agent.isStopped = true;
        }

        private void UpadateAnimator()
        {
            Vector3 velocity = agent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }

        public object CaptureState()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["position"] = new SerializableVector3(transform.position);
            data["rotation"] = new SerializableVector3(transform.eulerAngles);
            return data;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> data = (Dictionary<string, object>)state;

            if (agent == null) agent = GetComponent<NavMeshAgent>(); // Zabezpieczenie

            if (agent != null)
            {
                agent.enabled = false;
                transform.position = ((SerializableVector3)data["position"]).ToVector();
                transform.eulerAngles = ((SerializableVector3)data["rotation"]).ToVector();
                agent.enabled = true;
            }
            else
            {
                Debug.LogError("NavMeshAgent is null in Mover.RestoreState");
            }
        }

    }

}

