public class ES_Shooting : EnemyState
{
    private EnemyShooting enemyShooting;

    public ES_Shooting(Enemy enemy, EnemyStateMachine enemyStateMachine, EnemyShooting enemyShooting) : base(enemy, enemyStateMachine)
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
        enemyShooting.Shoot();
        enemyShooting.Rotation();
        if (!enemyShooting.isInArea)
            enemyShooting.stateMachine.ChangeState(enemyShooting.idle);

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }


}
