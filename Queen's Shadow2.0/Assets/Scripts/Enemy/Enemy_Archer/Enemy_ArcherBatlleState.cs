using UnityEngine;

public class Enemy_ArcherBatlleState : Enemy_BattleState
{
    private bool canFlip;
    private bool rechaedDeadEnd;
    public Enemy_ArcherBatlleState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        stateTimer -= Time.deltaTime;
        UpdateAnimationParameters();

        if(enemy.groundDetected == false || enemy.wallDetected)
            rechaedDeadEnd = true;

        if (enemy.PlayerDetection() == true)
        {
            UpdateTargetIfNeeded();
            UpdateBattleTimer();
        }

        if (BattleTimeIsOver())
            stateMachine.ChangeState(enemy.idleState);

        if (CanAttack())
        {
            if(enemy.PlayerDetection() == false && canFlip)
            {
                enemy.HandleFlip(DirectionToPlayer());
                canFlip = false;
            }

            enemy.SetVelocity(0, rb.linearVelocity.y);

            if (WithinAttackRange() && enemy.PlayerDetection() == true)
            {
                lastTimeAttacked = Time.time;
                canFlip = true; 
                stateMachine.ChangeState(enemy.attackState);
            }
        }
        else
        {
            bool shouldWalkAway = rechaedDeadEnd == false && DistanceToPlayer() < (enemy.attackDistance * 0.85f);

            if(shouldWalkAway)
            {
                enemy.SetVelocity((enemy.GetBattleMoveSpeed()  * -1) * DirectionToPlayer(), rb.linearVelocity.y);
            }
            else
            {
                enemy.SetVelocity(0, rb.linearVelocity.y);

                if (enemy.PlayerDetection() == false)
                    enemy.HandleFlip(DirectionToPlayer());
            }
        }
    }
}
