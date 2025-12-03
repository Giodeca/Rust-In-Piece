using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class EnemyPatrol : EnemyState
{
    private Vector3 targetPos;
    private Vector3 direction;
    private float rayLenght = 2f;
    private LayerMask maskObstacle;
    private EnemyFlyMelee enemyMelee;
    public EnemyPatrol(Enemy enemy, EnemyStateMachine enemyStateMachine, EnemyFlyMelee enemyMelee) : base(enemy, enemyStateMachine)
    {
        maskObstacle = 1 << 6;
        this.enemyMelee = enemyMelee;
    }

    public override void EnterState()
    {
        base.EnterState();
        targetPos = GetRandomPointInCircle();

        AudioManager.instance.PlaySFX(24, enemy.transform);
    }

    public override void ExitState()
    {
        base.ExitState();
        AudioManager.instance.StopSFX(24);

        //Debug.Log(enemyMelee.rb.velocity);
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        direction = (targetPos - enemyMelee.transform.position).normalized;

        enemyMelee.MoveEnemy(direction * enemyMelee.RandomMovementSpeed);

        if ((enemyMelee.transform.position - targetPos).sqrMagnitude < 0.1f)
        {
            targetPos = GetRandomPointInCircle();
        }

        if (enemyMelee.isAggroed)
            enemyMelee.stateMachine.ChangeState(enemyMelee.chase);
        else if (!enemyMelee.isInArea)
            enemyMelee.stateMachine.ChangeState(enemyMelee.idle);

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    //private Vector3 GetRandomPointInCircle() => enemy.transform.position + (Vector3)UnityEngine.Random.insideUnitCircle * enemy.RandomMovementRange;
    private Vector3 GetRandomPointInCircle()
    {
        List<Vector2> validDirections = CheckCollistionAvoidance();

        if (validDirections.Count > 0)
        {
            Vector2 direction = validDirections[UnityEngine.Random.Range(0, validDirections.Count)];
            return enemyMelee.transform.position + (Vector3)direction * enemyMelee.RandomMovementRange;
        }
        else
            return enemyMelee.transform.position;
    }

    private List<Vector2> CheckCollistionAvoidance()
    {
        List<Vector2> freedirection = new List<Vector2>();

        Vector2 vectorOrigin = enemyMelee.transform.position;

        Vector2[] directionRay = { Vector2.left, Vector2.right, Vector2.up, Vector2.down };

        foreach (Vector2 direction in directionRay)
        {
            RaycastHit2D ray = Physics2D.Raycast(vectorOrigin, direction, rayLenght, maskObstacle);

            Debug.DrawRay(vectorOrigin, direction * rayLenght, ray.collider ? Color.red : Color.green);

            if (ray.collider != null)
                Debug.Log("AvoidDirection");
            else
                freedirection.Add(direction);
        }

        return freedirection;
    }

}
