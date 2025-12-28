using UnityEngine;

public class Enemy_MageQuickRetreatState : EnemyState
{
    private Enemy_Mage enemyMage;
    private Vector3 startPosition;

    private Transform player;
    public Enemy_MageQuickRetreatState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        enemyMage = enemy as Enemy_Mage;    
    }

    public override void Enter()
    {
        base.Enter();
        if (player == null)
            player = enemy.GetPlayerReference();

        startPosition = enemy.transform.position;

        rb.linearVelocity = new Vector2(enemyMage.retreatSpeed * -DirectionToPlayer(), 0);
        enemy.HandleFlip(DirectionToPlayer());


        enemy.MakeUntargetable(true);
        enemy.entity_VFX.DoImageEchoEffect(1);
    }

    public override void Update()
    {
        base.Update();

        bool reachedMaxDistance = Vector2.Distance(enemy.transform.position, startPosition) > enemyMage.retreatMaxDistance;

        if (reachedMaxDistance || enemyMage.CantMoveBackwards())
        {
            stateMachine.ChangeState(enemyMage.spellCastState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        enemy.entity_VFX.StopImageEchoEffect();
        enemy.MakeUntargetable(false);  
    }

    protected int DirectionToPlayer()
    {
        if (player == null)
            return 0;

        return player.position.x > enemy.transform.position.x ? 1 : -1;
    }


}
