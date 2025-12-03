public class ESD_Shooting : EnemyState
{
    EnemyShootingDirectional enemyShooting;
    public ESD_Shooting(Enemy enemy, EnemyStateMachine enemyStateMachine, EnemyShootingDirectional enemyShootingDirectional) : base(enemy, enemyStateMachine)
    {
        enemyShooting = enemyShootingDirectional;
    }

    public override void EnterState()
    {
        base.EnterState();
        AudioManager.instance.PlaySFX(25, null);
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        enemyShooting.Shoot();
        if (!enemyShooting.isInArea)
            enemyShooting.stateMachine.ChangeState(enemyShooting.idle);

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
