using UnityEngine;

public class ProjectileEnemy : MonoBehaviour
{
    [SerializeField] private float speedMovement;
    [SerializeField] private Rigidbody2D rb;
    public Vector3 directionShoot;
    [SerializeField] private Animator animator;
    [SerializeField] private CircleCollider2D circleCollider;

    [SerializeField] private Vector2 knockbackPower;
    [SerializeField] private float lifetime;
    public bool DamagesModule { get; set; }

    [SerializeField]private float lifetimeTimer;
    
    private int modifier;

    // Start is called before the first frame update
    void Start()
    {
        lifetimeTimer = lifetime;
        modifier = 1;
        circleCollider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        lifetimeTimer -= Time.deltaTime;

        Move(directionShoot);

        if (lifetimeTimer < 0)
            ObjectPoolManager.Instance.RecycleEnemyProjectiles(this);
    }

    private void Move(Vector2 direction)
    {
        rb.velocity = direction * speedMovement * modifier;
    }
    //Creare una funzione publica fare il destroy e chamarla KeyFrame quando finisce
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.tag;

        switch (tag)
        {
            case "Player":
                collision.GetComponent<Player>().TakeDamage(DamagesModule, knockbackPower, transform);
                Debug.Log(gameObject.name);
                WhenColliding();
                break;
            case "ground":
                WhenColliding();
                break;
            case "playerBullets":
                collision.GetComponent<Projectile>().WhenColliding();
                //Destroy(collision.gameObject);
                break;
        }
    }

    public void WhenColliding()
    {
        modifier = 0;
        circleCollider.enabled = false;
        bool randomProb = Random.Range(0f, 1f) > 0.5f;
        Move(new Vector2(0, 0));

        if (randomProb)
            animator.SetTrigger("Vfx1");
        else
            animator.SetTrigger("Vfx2");
    }

    public void SetSpeed(float speed)
    {
        speedMovement = speed;
    }

    private void OnEnable()
    {
        modifier = 1;
        lifetimeTimer = lifetime;
        circleCollider.enabled = true;
    }

    public void DestroyPrj()
    {
        ObjectPoolManager.Instance.RecycleEnemyProjectiles(this);
    }
}
