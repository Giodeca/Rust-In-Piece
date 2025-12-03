using UnityEngine;
public class EGM_Chase : EnemyState
{
    private EnemyGroundMelee enemyMelee;
    private Vector2 actualDirection;
    private Vector2 downPointer;
    public EGM_Chase(Enemy enemy, EnemyStateMachine enemyStateMachine, EnemyGroundMelee enemyMelee) : base(enemy, enemyStateMachine)
    {
        this.enemyMelee = enemyMelee;
    }

    public override void EnterState()
    {
        base.EnterState();
        AudioManager.instance.PlaySFX(24, enemy.transform);

    }

    public override void ExitState()
    {
        base.ExitState();
        AudioManager.instance.StopSFX(24);

    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        Vector2 moveDirection = (PlayerManager.instance.player.transform.position - enemyMelee.transform.position).normalized;
        enemyMelee.MoveEnemy(new Vector2(moveDirection.x, 0) * enemyMelee.chaseSpeed);

        if (enemyMelee.isInSight)
            enemyMelee.stateMachine.ChangeState(enemyMelee.attack);
        if (!enemyMelee.isAggroed)
            enemyMelee.stateMachine.ChangeState(enemyMelee.patrol);

        if (!enemyMelee.IsGroundDetected())
        {
            enemyMelee.rb.velocity = Vector2.zero;
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }



}
