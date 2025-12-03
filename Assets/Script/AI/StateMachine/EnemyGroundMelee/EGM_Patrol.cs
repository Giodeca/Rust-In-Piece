using UnityEngine;
public class EGM_Patrol : EnemyState
{
    private EnemyGroundMelee enemyMelee;
    private Vector2 actualDirection;
    public EGM_Patrol(Enemy enemy, EnemyStateMachine enemyStateMachine, EnemyGroundMelee enemyMelee) : base(enemy, enemyStateMachine)
    {
        this.enemyMelee = enemyMelee;
    }

    public override void EnterState()
    {
        base.EnterState();
        actualDirection = GetNewDirection();
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
        enemyMelee.MoveEnemy(actualDirection * enemyMelee.patrolSpeed);
        //enemyMelee.rb.velocity = actualDirection * enemyMelee.patrolSpeed;
        SetRotation();

        if (enemyMelee.isAggroed)
            enemyMelee.stateMachine.ChangeState(enemyMelee.chase);
        else if (!enemyMelee.isInArea)
            enemyMelee.stateMachine.ChangeState(enemyMelee.idle);


    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public void SetRotation()
    {
        if ((enemyMelee.transform.position - enemyMelee.checkPointPatrol[enemyMelee.indexPatrolPoint]).sqrMagnitude < 0.1)
        {
            CicleList();
            actualDirection = GetNewDirection();
            if (actualDirection.x > 0)
            {
                Vector3 newRotation = new Vector3(enemyMelee.transform.rotation.x, 0, enemyMelee.transform.rotation.z);
                enemyMelee.transform.rotation = Quaternion.Euler(newRotation);
                enemyMelee.isFacingRight = true;
            }
            else
            {
                Vector3 newRotation = new Vector3(enemyMelee.transform.rotation.x, 180, enemyMelee.transform.rotation.z);
                enemyMelee.transform.rotation = Quaternion.Euler(newRotation);
                enemyMelee.isFacingRight = false;
            }
        }
    }
    private void CicleList()
    {
        enemyMelee.indexPatrolPoint = (enemyMelee.indexPatrolPoint + 1) % enemyMelee.checkPointPatrol.Count;
    }
    private Vector2 GetNewDirection()
    {
        Vector2 difference = enemyMelee.checkPointPatrol[enemyMelee.indexPatrolPoint] - enemyMelee.transform.position;
        difference.Normalize();
        return difference;
    }
}
