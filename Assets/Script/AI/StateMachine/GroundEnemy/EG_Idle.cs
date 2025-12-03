public class EG_Idle : EnemyState
{
    protected EnemyGroundScrool enemyGround;
    public EG_Idle(Enemy enemy, EnemyStateMachine enemyStateMachine, EnemyGroundScrool enemyGround) : base(enemy, enemyStateMachine)
    {
        this.enemyGround = enemyGround;
    }

    public override void EnterState()
    {
        base.EnterState();
        enemyGround.rb.velocity = UnityEngine.Vector2.zero;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if (enemyGround.isInArea)
        {
            enemyGround.stateMachine.ChangeState(enemyGround.patrol);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
