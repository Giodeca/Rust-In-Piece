public class ES_Idle : EnemyState
{
    private EnemyShooting enemyShooting;
    public ES_Idle(Enemy enemy, EnemyStateMachine enemyStateMachine, EnemyShooting enemyShooting) : base(enemy, enemyStateMachine)
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
