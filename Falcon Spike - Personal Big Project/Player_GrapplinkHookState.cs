using UnityEngine;

public class Player_GrapplinkHookState : PlayerState
{

    private enum GrapplePhase { Shooting, Pulling }
    private GrapplePhase currentPhase;

    private Vector3 currentGrappleVelocity;
    private Vector3 visualHookPosition; 
    private float hookShootSpeed = 60f; 

    public Player_GrapplinkHookState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        Ray ray = new Ray(player.cameraTransform.position, player.cameraTransform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, player.maxHookLength, player.validLayerMask))
        {
            player.isGrapling = true;
            player.grapplePoint = hit.point;

            
            currentPhase = GrapplePhase.Shooting;


            visualHookPosition = player.grapplingPosition.position;

            if (player.hook != null)
            {
                player.hook.enabled = true;
                player.hook.SetPosition(0, player.grapplingPosition.position);
                player.hook.SetPosition(1, visualHookPosition); 
            }

            currentGrappleVelocity = Vector3.zero;
        }
        else
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void Update()
    {
        base.Update();

        if (!player.isGrapling) return;


        if (player.hook != null)
        {
            player.hook.SetPosition(0, player.grapplingPosition.position);
        }

       
        if (currentPhase == GrapplePhase.Shooting)
        {
            HandleShootingPhase();
        }
        else if (currentPhase == GrapplePhase.Pulling)
        {
            HandlePullingPhase();
        }
    }

    private void HandleShootingPhase()
    {

        visualHookPosition = Vector3.MoveTowards(visualHookPosition, player.grapplePoint, hookShootSpeed * Time.deltaTime);

        if (player.hook != null)
        {
            player.hook.SetPosition(1, visualHookPosition);
        }

      
        if (Vector3.Distance(visualHookPosition, player.grapplePoint) < 0.1f)
        {

            currentPhase = GrapplePhase.Pulling;

           
            player.SetSlideCamera(true, 0f);
        }
    }

    private void HandlePullingPhase()
    {

        Vector3 directionToPoint = player.grapplePoint - player.transform.position;
        float distance = directionToPoint.magnitude;

        Vector3 pullForce = directionToPoint.normalized * player.grappligSpeed;
        pullForce.y += 3f;

        currentGrappleVelocity = Vector3.Lerp(currentGrappleVelocity, pullForce, Time.deltaTime * 5f);
        player.characterController.Move(currentGrappleVelocity * Time.deltaTime);


        if (distance < 2.5f)
        {
            player.SetMomentum(currentGrappleVelocity);
            stateMachine.ChangeState(player.fallState);
        }

        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.SetMomentum(currentGrappleVelocity * 1.5f + Vector3.up * player.jumpForce);
            stateMachine.ChangeState(player.fallState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        player.isGrapling = false;

        if (player.hook != null)
            player.hook.enabled = false;

        player.SetSlideCamera(false);
    }
}