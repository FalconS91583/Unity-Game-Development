using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent (typeof(MovementToPositionEvent))]
[DisallowMultipleComponent]

public class MovementToPosition : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;
    private MovementToPositionEvent movementToPositionEvent;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        movementToPositionEvent = GetComponent<MovementToPositionEvent>();
    }

    private void OnEnable()
    {
        movementToPositionEvent.OnMovementToPosition += MovementTopositionEvent_OnMovementToPosition;
    }
    private void OnDisable()
    {
        movementToPositionEvent.OnMovementToPosition -= MovementTopositionEvent_OnMovementToPosition;
    }

    private void MovementTopositionEvent_OnMovementToPosition(MovementToPositionEvent movementToPositionEvent, MovementToPositionArgs
        movementToPosotionArgs)
    {
        MoveRigidbody(movementToPosotionArgs.movePosition, movementToPosotionArgs.currentPosition, movementToPosotionArgs.moveSpeed);
    }

    private void MoveRigidbody(Vector3 movePosition, Vector3 currentPosition, float moveSpeed)
    {
        Vector2 unitvector = Vector3.Normalize(movePosition - currentPosition);

        rigidbody2D.MovePosition(rigidbody2D.position + (unitvector * moveSpeed * Time.fixedDeltaTime));
    }
}
