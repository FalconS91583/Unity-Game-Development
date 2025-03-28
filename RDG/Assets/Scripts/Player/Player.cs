using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


[RequireComponent(typeof(MovementByVelocityEvent))]
[RequireComponent (typeof(MovementByVelocity))]
[RequireComponent(typeof(Health))]
[RequireComponent (typeof(PlayerControl))]
[RequireComponent(typeof(SortingGroup))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AnimatePlayer))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent (typeof(IdleEvent))]
[RequireComponent(typeof (Idle))]
[RequireComponent(typeof(AimWeaponEvent))]
[RequireComponent(typeof(AimWeapon))]
[RequireComponent (typeof(MovementToPositionEvent))]
[RequireComponent(typeof(MovementToPosition))]

[DisallowMultipleComponent]


public class Player : MonoBehaviour
{
    [HideInInspector] public PlayerDetailSO playerDetails;
    [HideInInspector] public Health health;
    [HideInInspector] public MovementByVelocityEvent movementByVelocityEvent;
    [HideInInspector] public MovementToPositionEvent movementToPositionEvent;
    [HideInInspector] public IdleEvent idleEvent;
    [HideInInspector] public AimWeaponEvent aimWeaponEvent;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public Animator animator;

    private void Awake()
    {
        health = GetComponent<Health>();
        movementByVelocityEvent = GetComponent<MovementByVelocityEvent>();
        movementToPositionEvent = GetComponent<MovementToPositionEvent>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        idleEvent = GetComponent<IdleEvent>();
        aimWeaponEvent = GetComponent<AimWeaponEvent>();
    }

    public void Initialize(PlayerDetailSO playerDetails)
    {
        this.playerDetails = playerDetails;
        SetPlayerHealt();
    }

    private void SetPlayerHealt()
    {
        health.SetStartingHealth(playerDetails.playerHealthAmout);
    }
}
