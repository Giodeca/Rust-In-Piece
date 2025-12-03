using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Animator Anim { get; private set; }
    public Rigidbody2D Rb { get; private set; }
    public CapsuleCollider2D colliderEntity { get; private set; }



    [Header("Stats")]
    [SerializeField] private float defaultSpeed;
    [SerializeField] float walkAccel;
    [SerializeField] float walkDecel;
    public bool useLerpedMovement;
    public float lerpAmount;
    [SerializeField] protected float invincibilityDuration;

    public int facingDir { get; private set; } = 1;
    protected bool facingRight = true;
    //public int knockbackDir { get; private set; }

    public System.Action onFlipped;

    [Header("Knockback info")]
    [SerializeField] private float timeStopDuration = 0.5f;
    [SerializeField] private Vector2 globalKnockbackPower = new Vector2(8, 10);
    [SerializeField] protected Vector2 knockbackOffset = new Vector2(.5f, 2);
    [SerializeField] private float knockbackDuration = .07f;
    [SerializeField] private bool useGlobalKnockback;
    [SerializeField] private Color InvincibilityColor = Color.cyan;
    protected bool isKnocked;
    public float KnockbackDuration { get => knockbackDuration; }
    public float TimeStopDuration { get => timeStopDuration; }
    private float knockbackDir { get; set; }


    [Header("Collision info")]
    public Transform attackCheck;
    public float attackCheckRadius = 1.2f;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance = 1;
    [SerializeField] protected ContactFilter2D contactFilter;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance = .8f;
    [SerializeField] protected LayerMask whatIsGround;

    public float DefaultSpeed { get { return defaultSpeed; } set { defaultSpeed = value; } }

    public bool IsDead { get; private set; }
    public bool IsInvincible { get; private set; }
    protected Vector2 KnockbackPower { get; set; }

    protected virtual void Awake()
    {
        colliderEntity = GetComponent<CapsuleCollider2D>();
    }

    protected virtual void Update()
    {

    }

    protected virtual void Start()
    {
        Anim = GetComponentInChildren<Animator>();
        //anim = this.GetComponentsInChildren<Animator>().First(x => x.gameObject.transform.parent != transform.parent);
        Rb = GetComponent<Rigidbody2D>();

        IsDead = false;
    }

    protected virtual void ReturnDefaultSpeed()
    {
        Anim.speed = 1;
    }

    public virtual void TakeDamage(bool isModuleDamaged, Vector2 knockbackPower, Transform direction)
    {
        if (!IsDead)
        {
            AudioManager.instance.PlaySFX(2, PlayerManager.instance.player.transform);
            SetupKnockbackPower(knockbackPower);
            SetupKnockbackDir(direction);
            StartCoroutine(InvincibileCoroutine());
        }

    }

    public IEnumerator InvincibileCoroutine()
    {
        MakeInvincible(true);
        yield return DamagedAnim();
        MakeInvincible(false);
    }

    public IEnumerator DamagedAnim()
    {
        float timer = invincibilityDuration;
        SpriteRenderer sprite = GetComponentInChildren<SpriteRenderer>();

        while (timer > 0)
        {
            sprite.color = InvincibilityColor;
            yield return new WaitForSeconds(Time.deltaTime);
            timer -= Time.deltaTime;
            sprite.color = Color.white;
            yield return new WaitForSeconds(Time.deltaTime);
            timer -= Time.deltaTime;
        }
    }

    public virtual void SetupKnockbackDir(Transform _damageDirection)
    {

        Debug.Log($"damage transform: {_damageDirection}");

        if (_damageDirection.position.x > transform.position.x)
            knockbackDir = -1;

        else if (_damageDirection.position.x < transform.position.x)
            knockbackDir = 1;

        Debug.Log($"knockback direction: {knockbackDir}");


        if (knockbackDir == facingDir)
            Flip();
    }

    public virtual void HitKnockback()
    {
        isKnocked = true;

        float xOffset = Random.Range(KnockbackPower.x, KnockbackPower.y);

        Rb.velocity = new Vector2((KnockbackPower.x + xOffset) * knockbackDir, KnockbackPower.y);

        //Debug.Log($"RB: {Rb}, {Rb.gameObject}\tKnockback power: {KnockbackPower}\tKnockback offset: {knockbackOffset}\tKnockback dir {knockbackDir}");
    }


    public void SetupKnockbackPower(Vector2 _knockbackPower)
    {
        if (useGlobalKnockback)
            KnockbackPower = globalKnockbackPower;
        else
            KnockbackPower = _knockbackPower;
    }


    public virtual void SetupZeroKnockbackPower()
    {
        isKnocked = false;
        KnockbackPower = new Vector2(0, 0);
    }

    #region Collision
    //public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public virtual bool IsGroundDetected() => Rb.IsTouching(contactFilter);

    public virtual bool isWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * facingDir, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);

    }
    #endregion

    public void MakeInvincible(bool _invincible) => IsInvincible = _invincible;

    public virtual void Die()
    {
        IsDead = true;

        Debug.LogError("CHIAMATA MORTE");

        //SceneManager.LoadScene("Tutorial");
        //SaveSystem.newSceneLoad = true;
        GameManager.Instance.LoadAsync();
        //GameManager.Instance.LoadData();

    }

    #region Velocity
    public virtual void SetZeroVelocity()
    {
        /*if (isKnocked)
            return;*/

        Rb.velocity = new Vector2(0, 0);
    }

    public virtual void InstantVelocity(float _xVelocity, float _yVelocity)
    {

        /*if (isKnocked)
            return;*/

        Rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }

    public virtual void LerpedVelocity(float targetSpeed)
    {
        //Calculate the direction we want to move in and our desired velocity
        //We can reduce are control using Lerp() this smooths changes to are direction and speed
        targetSpeed = Mathf.Lerp(Rb.velocity.x, targetSpeed, lerpAmount);

        float accelRate;

        //Gets an acceleration value based on if we are accelerating (includes turning) 
        //or trying to decelerate (stop). As well as applying a multiplier if we're air borne.
        accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? walkAccel : walkDecel;

        //Calculate difference between current velocity and desired velocity
        float speedDif = targetSpeed - Rb.velocity.x;

        //Calculate force along x-axis to apply to thr player
        float movement = speedDif * accelRate;

        //Convert this to a vector and apply to rigidbody
        Rb.AddForce(new Vector2(movement, Rb.velocity.y), ForceMode2D.Force);
        //Rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
        FlipController(targetSpeed);

    }

    public virtual void Move(float _xVelocity, float _yVelocity)
    {
        if (useLerpedMovement)
            LerpedVelocity(_xVelocity);
        else
            InstantVelocity(_xVelocity, _yVelocity);
    }

    #endregion

    #region Flip
    public virtual void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);

        if (onFlipped != null)
            onFlipped();
    }

    public virtual void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
            Flip();
        else if (_x < 0 && facingRight)
            Flip();
    }

    public virtual void SetupDefaultFacingDir(int _direction)
    {
        facingDir = _direction;

        if (facingDir == -1)
            facingRight = false;
    }
    #endregion
}