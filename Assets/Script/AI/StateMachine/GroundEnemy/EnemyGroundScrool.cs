using System.Collections.Generic;
using UnityEngine;

public class EnemyGroundScrool : Enemy
{
    [SerializeField] protected Transform[] patrolPoints;
    public List<Vector3> checkPointPatrol = new List<Vector3>();

    public float patrolSpeed;
    public int indexPatrolPoint;

    public EG_Idle idle { get; set; }
    public EG_Patrol patrol { get; set; }

    protected override void Awake()
    {
        base.Awake();
        InizializeStates();
        SetPatrolPoints();
    }
    public override void InizializeStates()
    {
        base.InizializeStates();
        idle = new EG_Idle(this, stateMachine, this);
        patrol = new EG_Patrol(this, stateMachine, this);

        stateMachine.Inizialiaze(idle);
    }

    private void SetPatrolPoints()
    {
        foreach (Transform t in patrolPoints)
        {
            checkPointPatrol.Add(t.position);
        }
    }



}
