
using UnityEngine;

public class EnemyFlyMelee : Enemy
{
    //Per ogni nemico la probabilita di rompere un modulo on collision
    //Modifica il takeDamage del player
    public EnemyAttack attack { get; set; }
    public EnemyChase chase { get; set; }
    public EnemyIdle idle { get; set; }
    public EnemyPatrol patrol { get; set; }

    public float movementSpeed = 2f;

    public Vector2 moveDirection;

    protected override void Awake()
    {
        base.Awake();

        InizializeStates();
    }

    public override void InizializeStates()
    {
        base.InizializeStates();
        idle = new EnemyIdle(this, stateMachine, this);
        attack = new EnemyAttack(this, stateMachine, this);
        chase = new EnemyChase(this, stateMachine, this);
        patrol = new EnemyPatrol(this, stateMachine, this);

        stateMachine.Inizialiaze(idle);
    }
    private void OnDrawGizmos()
    {


        Vector2 pos = new Vector2(transform.position.x, transform.position.y);

        // Parametri delle box
        Vector2 boxSizeLeftRight = new Vector2(0.5f, 2);
        Vector2 boxSizeUpDown = new Vector2(1, 1);

        // Direzioni
        Vector2 leftDirection = new Vector2(-1f, 0);
        Vector2 rightDirection = new Vector2(1f, 0);
        Vector2 upDirection = new Vector2(0f, 1f);
        Vector2 downDirection = new Vector2(0f, -1);

        // Colori per le box
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(pos + leftDirection, boxSizeLeftRight); // Sinistra

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos + rightDirection, boxSizeLeftRight); // Destra

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(pos + upDirection, boxSizeUpDown); // Sopra

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(pos + downDirection, boxSizeUpDown); // Sotto


    }
}
