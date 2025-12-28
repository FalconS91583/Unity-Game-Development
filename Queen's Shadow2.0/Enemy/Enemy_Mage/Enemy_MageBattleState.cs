using UnityEngine;

public class Enemy_MageBattleState : Enemy_BattleState
{
    private float lastTimeUsedRetreat = float.NegativeInfinity;
    protected Enemy_Mage enemyMage;
    public Enemy_MageBattleState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        enemyMage = enemy as Enemy_Mage;    
    }

    public override void Enter()
    {
        base.Enter();

        if (ShouldRetread())
        {
            if (CanuseRetreatAbility())
                Retreat();
            else
                ShortRetreat();
        }
    }

    private void Retreat()
    {
        lastTimeUsedRetreat = Time.time;
        stateMachine.ChangeState(enemyMage.quickRetreatState);

    }
    private bool CanuseRetreatAbility() => Time.time > lastTimeUsedRetreat + enemyMage.retreatCooldown;

    
}
