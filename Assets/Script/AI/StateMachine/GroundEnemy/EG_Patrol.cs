using UnityEngine;

public class EG_Patrol : EnemyState
{
    protected EnemyGroundScrool enemyGround;
    private Vector2 actualDirection;
    public EG_Patrol(Enemy enemy, EnemyStateMachine enemyStateMachine, EnemyGroundScrool enemyGround) : base(enemy, enemyStateMachine)
    {
        this.enemyGround = enemyGround;
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


        enemyGround.rb.velocity = actualDirection * enemyGround.patrolSpeed;

        SetRotation();

        if (!enemyGround.isInArea)
            enemyGround.stateMachine.ChangeState(enemyGround.idle);
    }

    public void SetRotation()
    {
        if ((enemyGround.transform.position - enemyGround.checkPointPatrol[enemyGround.indexPatrolPoint]).sqrMagnitude < 0.1)
        {
            CicleList();
            Debug.Log(enemyGround.indexPatrolPoint);
            actualDirection = GetNewDirection();
            if (actualDirection.x > 0)
            {
                Vector3 newRotation = new Vector3(enemyGround.transform.rotation.x, 0, enemyGround.transform.rotation.z);
                enemyGround.transform.rotation = Quaternion.Euler(newRotation);
                enemyGround.isFacingRight = true;
            }
            else
            {
                Vector3 newRotation = new Vector3(enemyGround.transform.rotation.x, 180, enemyGround.transform.rotation.z);
                enemyGround.transform.rotation = Quaternion.Euler(newRotation);
                enemyGround.isFacingRight = false;
            }
        }
    }
    private void CicleList()
    {
        enemyGround.indexPatrolPoint = (enemyGround.indexPatrolPoint + 1) % enemyGround.checkPointPatrol.Count;
    }
    private Vector2 GetNewDirection()
    {
        Vector2 difference = enemyGround.checkPointPatrol[enemyGround.indexPatrolPoint] - enemyGround.transform.position;
        difference.Normalize();
        return difference;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
