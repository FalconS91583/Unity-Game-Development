using BramaBadura.Combat;
using BramaBadura.Core;
using BramaBadura.Movement;
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace BramaBadura.Control
{
    public class PlayerController : MonoBehaviour
    {
        private Fighter fighter;
        private Health health;

        [SerializeField] private float raycastRadius = 1f;

        [System.Serializable]
        private struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] private CursorMapping[] cursorMappings = null;
        [SerializeField] private float maxNavMesshProjectionDistance = 1f;

        private void Start()
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
        }

        private void Update()
        {
            if (InteractWithUI()) return;

            if (health.IsDead())
            {
                SetCursor(CursorType.None);
                return;
            }

            if (InteractWithComponent()) return;

            if(InteractWithMovement()) return;

            SetCursor(CursorType.None);
        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = RayccastAllSorted();

            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (IRaycastable raycastable in raycastables)
                {
                   if(raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }
            return false;
        }

        RaycastHit[] RayccastAllSorted()
        {
            RaycastHit[] hits = Physics.SphereCastAll(GetMouseRay(), raycastRadius);

            float[] distances = new float[hits.Length];
            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }

            Array.Sort(distances, hits);

            return hits;
        }

        private bool InteractWithUI()
        {
            if(EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(CursorType.UI);
                return true;
            }
            return false;
        }

        private void SetCursor(CursorType cursorType)
        {
            CursorMapping mapping = GetCursorMapping(cursorType);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type) 
        { 
            foreach(CursorMapping mapping in cursorMappings)
            {
                if (mapping.type == type)
                {
                    return mapping;
                }
            }
            return cursorMappings[0];
        }

        private bool InteractWithMovement()
        {
            Vector3 target;
            bool hasHit = RayCastNavMesh(out target);
            if (hasHit)
            {
                if (!GetComponent<Mover>().CanMoveTo(target)) return false;

                if (Input.GetMouseButtonDown(0))
                {
                    Mover mover = GetComponent<Mover>();
                    if (mover != null)  
                    {
                        mover.StartMoveAction(target, 1f);
                    }
                    else
                    {
                        Debug.LogError("Mover component is missing on " + gameObject.name);
                    }
                }
                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }

        private bool RayCastNavMesh(out Vector3 target)
        {
            target = new Vector3();

            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (!hasHit) return false;

            NavMeshHit navMeshHit;
            bool hasCastToNavMesh =
                NavMesh.SamplePosition(hit.point, out navMeshHit, maxNavMesshProjectionDistance, NavMesh.AllAreas);

            if (!hasCastToNavMesh) return false;

            target = navMeshHit.position;


            return true;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}

