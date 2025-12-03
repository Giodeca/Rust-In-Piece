using UnityEngine;

public class EnemyShooting : Enemy
{
    public float roationSpeed;
    [SerializeField] private Transform bulletSpawner;
    [SerializeField] private float bulletSpeed;

    public ES_Idle idle;
    public ES_Shooting shooting;


    public float fireRatio;
    public float elapseTime;

    protected override void Awake()
    {
        base.Awake();
        InizializeStates();
    }

    public override void InizializeStates()
    {
        base.InizializeStates();
        idle = new ES_Idle(this, stateMachine, this);
        shooting = new ES_Shooting(this, stateMachine, this);
        stateMachine.Inizialiaze(idle);
    }


    public void Shoot()
    {
        elapseTime += Time.deltaTime;
        if (fireRatio <= elapseTime)
        {
            ProjectileEnemy projectileShoot = ObjectPoolManager.Instance.EnemyProjectilePool.UseObject();
            projectileShoot.SetSpeed(bulletSpeed);
            projectileShoot.transform.position = bulletSpawner.position;
            projectileShoot.transform.rotation = Quaternion.identity;
            projectileShoot.gameObject.SetActive(true);
            //GameObject projectileShoot = Instantiate(projectile, bulletSpawner.position, Quaternion.identity);
            AudioManager.instance.PlaySFX(25, transform);

            projectileShoot.GetComponent<ProjectileEnemy>().directionShoot = GetForeward();
            projectileShoot.GetComponent<ProjectileEnemy>().DamagesModule = DamageModule();
            elapseTime = 0;
        }
    }

    private Vector3 GetForeward() => transform.right;


    public void Rotation()
    {

        transform.Rotate(0, 0, roationSpeed * Time.deltaTime);
    }

    private void OnDisable()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
