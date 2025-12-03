using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class EnemyGroundMelee : Enemy
{
    public EGM_Attack attack { get; set; }
    public EGM_Chase chase { get; set; }
    public EGM_Idle idle { get; set; }
    public EGM_Patrol patrol { get; set; }

    [SerializeField] protected Transform[] patrolPoints;
    public List<Vector3> checkPointPatrol = new List<Vector3>();
    LayerMask groundLayer;
    public float radiusCheck;
    public float patrolSpeed;
    public float chaseSpeed;
    public int indexPatrolPoint;
    Vector2 origin;

    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance = 1;
    [SerializeField] protected LayerMask whatIsGround;

    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

    protected override void Awake()
    {
        base.Awake();
        SetPatrolPoints();
        InizializeStates();
        groundLayer = LayerMask.GetMask("Ground");
    }

    public override void InizializeStates()
    {
        base.InizializeStates();
        idle = new EGM_Idle(this, stateMachine, this);
        attack = new EGM_Attack(this, stateMachine, this);
        chase = new EGM_Chase(this, stateMachine, this);
        patrol = new EGM_Patrol(this, stateMachine, this);

        stateMachine.Inizialiaze(idle);
    }

    private void SetPatrolPoints()
    {
        foreach (Transform t in patrolPoints)
        {
            checkPointPatrol.Add(t.position);
        }
    }

    public bool CheckForGround()
    {
        Vector2 origin = new Vector2(transform.position.x, transform.position.y - 0.8f);
        bool isGrounded = Physics2D.OverlapCircle(origin, radiusCheck, groundLayer);

        return isGrounded;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector2(transform.position.x, transform.position.y - 0.8f), radiusCheck);
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));

    }
}
