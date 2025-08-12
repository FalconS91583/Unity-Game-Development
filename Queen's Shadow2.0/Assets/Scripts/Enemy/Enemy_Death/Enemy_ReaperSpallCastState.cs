using UnityEngine;

public class Enemy_ReaperSpallCastState : EnemyState
{
    private Enemy_Reaper enemyReaper;
    public Enemy_ReaperSpallCastState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        enemyReaper = enemy as Enemy_Reaper;            
    }

    public override void Enter()
    {
        base.Enter();
        enemyReaper.SetVelocity(0,0); 
        enemyReaper.SetSpellCastPerformed(false);
        enemyReaper.SetSpellCastOnCooldown();
    }

    public override void Update()
    {
        base.Update();
        if(enemyReaper.spellCastPerformed)
        {
            anim.SetBool("spellCastPerformed", true); 
        }

        if (triggerCalled)
        {
            if (enemyReaper.ShouldTeleport())
                stateMachine.ChangeState(enemyReaper.reaperTeleportState);
            else
                stateMachine.ChangeState(enemyReaper.reaperBattleState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        anim.SetBool("spellCastPerformed", false);
    }
}
