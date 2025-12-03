using CustomUnityLibrary;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lifetime;
    [SerializeField] protected float damage;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private CircleCollider2D circleCollider;
    [SerializeField] private Animator animator;
    private float lifetimeTimer;
    [HideInInspector] public bool moduleIsDamaged;
    [SerializeField] private GameObject textDamage;

    private ObjectPool<Projectile> poolReference;

    private int modifier;
    // Start is called before the first frame update
    void Start()
    {
        modifier = 1;
        lifetimeTimer = lifetime;
        //circleCollider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        lifetimeTimer -= Time.deltaTime;

        rb.velocity = transform.right * speed * modifier;

        if (lifetimeTimer < 0)
            ObjectPoolManager.Instance.RecycleProjectiles(poolReference, this);
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.tag;

        switch (tag)
        {
            case "enemy":
                collision.GetComponent<Enemy>().TakeDamage(damage);
                GameObject textDamages = Instantiate(textDamage, collision.transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
                textDamages.GetComponent<HitPointUi>().AssignText(damage);
                WhenColliding();
                break;
            case "ground":
                WhenColliding();
                break;
            case "enemyBullets":
                if (!moduleIsDamaged)
                    collision.GetComponent<ProjectileEnemy>().WhenColliding();

                WhenColliding();
                break;
        }
    }

    public void WhenColliding()
    {
        modifier = 0;
        circleCollider.enabled = false;
        bool randomProb = Random.Range(0f, 1f) > 0.5f;
        rb.velocity = new Vector2(0, 0);

        if (randomProb)
            animator.SetTrigger("Vfx1");
        else
            animator.SetTrigger("Vfx2");

    }

    public void SetObjectPool(ObjectPool<Projectile> poolRef)
    {
        poolReference = poolRef;
    }

    private void OnEnable()
    {
        modifier = 1;
        lifetimeTimer = lifetime;
        circleCollider.enabled = true;
    }

    public void DestroyPrj()
    {
        ObjectPoolManager.Instance.RecycleProjectiles(poolReference, this);
    }
}
