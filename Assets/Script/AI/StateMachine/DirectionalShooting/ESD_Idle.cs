public class ESD_Idle : EnemyState
{
    EnemyShootingDirectional enemyShooting;
    public ESD_Idle(Enemy enemy, EnemyStateMachine enemyStateMachine, EnemyShootingDirectional enemyShooting) : base(enemy, enemyStateMachine)
    {
        this.enemyShooting = enemyShooting;
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if (enemyShooting.isInArea)
            enemyShooting.stateMachine.ChangeState(enemyShooting.shooting);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
