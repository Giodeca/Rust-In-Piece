using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, IEnemyMoveable, ITriggerCeckable
{
    [field: SerializeField] public float MaxHealth { get; set; } = 100f;
    public float currentHealth { get; set; }
    public Rigidbody2D rb { get; set; }
    public bool isFacingRight { get; set; }
    public bool isAggroed { get; set; }
    public bool isInSight { get; set; }
    public bool isInArea { get; set; }
    public Vector2 knockbackPower;

    [Range(0, 100)]
    [SerializeField] private int destroyModuleChance;

    public float RandomMovementRange = 5f;
    public float RandomMovementSpeed = 1;
    public EnemyStateMachine stateMachine { get; set; }
    protected virtual void Awake()
    {
        currentHealth = MaxHealth;
        rb = GetComponent<Rigidbody2D>();
        stateMachine = new EnemyStateMachine();
    }
    public virtual void InizializeStates()
    {

    }
    protected virtual void Update()
    {
        stateMachine.currentEnemyState.FrameUpdate();
    }

    protected virtual void FixedUpdate()
    {
        stateMachine.currentEnemyState.PhysicsUpdate();
    }
    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, MaxHealth);
        /*if (!IsDead)
        {
            GameObject vfx = Instantiate(bloodVfx, transform.position, Quaternion.identity);
            Destroy(vfx, .5f);
        }*/
        //StartCoroutine(HitKnockback());

        AudioManager.instance.PlaySFX(26, transform);

        if (currentHealth == 0)
            Die();
    }

    public void Die()
    {
        GameManager.Instance.AddEnemyToMilestone();
        gameObject.SetActive(false);
    }

    protected virtual void Move() { }
    protected virtual void CheckLeftRight() { }


    public void MoveEnemy(Vector2 velocity)
    {
        rb.velocity = velocity;
        CheckForLeftOrRightFacing(velocity);
    }

    public bool DamageModule()
    {
        int random = Random.Range(0, 101);

        return random <= destroyModuleChance;
    }

    public void CheckForLeftOrRightFacing(Vector2 velocity)
    {
        if (isFacingRight && velocity.x < 0)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = !isFacingRight;
        }
        else if (!isFacingRight && velocity.x > 0)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = !isFacingRight;
        }
    }

    public void SetAggroStatus(bool isAggroed)
    {
        this.isAggroed = isAggroed;
    }

    public void SetStrikingBool(bool isInSight)
    {
        this.isInSight = isInSight;
    }
    public void SetPatrolActive(bool isInArea)
    {
        this.isInArea = isInArea;
    }

}
