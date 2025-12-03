using UnityEngine;

public class EnemyAttack : EnemyState
{
    private bool canAttack = true;
    private float elapseTime;
    private float TimeToAttack = 1;
    private EnemyFlyMelee enemyMelee;
    public EnemyAttack(Enemy enemy, EnemyStateMachine enemyStateMachine, EnemyFlyMelee enemyFlyMelee) : base(enemy, enemyStateMachine)
    {
        this.enemyMelee = enemyFlyMelee;
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
        if (!enemyMelee.isInSight)
            enemyMelee.stateMachine.ChangeState(enemyMelee.patrol);

        if (canAttack)
        {
            elapseTime += Time.deltaTime;
            if (TimeToAttack <= elapseTime)
            {
                elapseTime = 0;
                Debug.Log("Attack");
            }
        }



    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
