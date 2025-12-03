using UnityEngine;
public class EnemyIdle : EnemyState
{
    private EnemyFlyMelee enemyMelee;
    public EnemyIdle(Enemy enemy, EnemyStateMachine enemyStateMachine, EnemyFlyMelee enemyMelee) : base(enemy, enemyStateMachine)
    {
        this.enemyMelee = enemyMelee;
    }

    public override void EnterState()
    {
        base.EnterState();
        enemyMelee.rb.velocity = Vector2.zero;

    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if (enemyMelee.isInArea)
            enemyMelee.stateMachine.ChangeState(enemyMelee.patrol);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
