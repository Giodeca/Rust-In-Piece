using UnityEngine;

public class EnemyChase : EnemyState
{
    private float rayLength = 2f;
    private LayerMask maskObstacle;
    private EnemyFlyMelee enemyMelee;
    public EnemyChase(Enemy enemy, EnemyStateMachine enemyStateMachine, EnemyFlyMelee enemyMelee) : base(enemy, enemyStateMachine)
    {
        maskObstacle = 1 << 6;

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
        Vector2 targetPosition = PlayerManager.instance.player.transform.position;
        Vector2 moveDirection = (targetPosition - (Vector2)enemyMelee.transform.position).normalized;

        //enemyMelee.moveDirection = moveDirection;

        // Controlla se c'è un ostacolo sulla traiettoria
        if (Physics2D.Raycast(enemyMelee.transform.position, moveDirection, rayLength, maskObstacle))
        {
            Debug.Log("Collision");
            moveDirection = GetAlternativeDirection(moveDirection);
            enemyMelee.CheckForLeftOrRightFacing(moveDirection);
        }


        // Muovi il nemico nella direzione finale
        enemyMelee.MoveEnemy(moveDirection * enemyMelee.movementSpeed);


        if (enemyMelee.isInSight)
            enemyMelee.stateMachine.ChangeState(enemyMelee.attack);
        else if (!enemyMelee.isAggroed)
            enemyMelee.stateMachine.ChangeState(enemyMelee.patrol);
    }

    /// <summary>
    /// Trova una direzione alternativa se c'è un ostacolo.
    /// </summary>
    private Vector2 GetAlternativeDirection(Vector2 originalDirection)
    {
        Vector2 pos = new Vector2(enemyMelee.transform.position.x, enemyMelee.transform.position.y);
        // Direzione verso sinistra rispetto a quella originale
        Vector2 leftDirection = new Vector2(-1f, 0);
        Vector2 boxSize = new Vector2(0.5f, 2);
        if (Physics2D.OverlapBox(pos + leftDirection, boxSize, maskObstacle))
        {
            return Vector2.right;
        }

        // Direzione verso destra rispetto a quella originale
        Vector2 rightDirection = new Vector2(+1f, 0);
        if (Physics2D.OverlapBox(pos + rightDirection, boxSize, maskObstacle))
        {
            return Vector2.left;
        }

        Vector2 boxSizeH = new Vector2(1, 1f);
        Vector2 upDirection = new Vector2(0f, 1f);
        if (Physics2D.OverlapBox(pos + upDirection, boxSizeH, maskObstacle))
        {
            return Vector2.down;
        }

        // Direzione verso il basso (asse Y negativo)
        Vector2 downDirection = new Vector2(0f, -1f);
        if (Physics2D.OverlapBox(pos + downDirection, boxSizeH, maskObstacle))
        {
            return Vector2.up;
        }

        // Se non trova alternative, ritorna la direzione originale
        return originalDirection;
    }


}

