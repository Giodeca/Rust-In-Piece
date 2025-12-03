using UnityEngine;

public class EnemyShootingDirectional : Enemy
{
    [SerializeField] int projectileNumber;
    [SerializeField] private float bulletSpeed;

    Vector2 startPoint;
    [SerializeField] private float radious;

    public ESD_Idle idle;
    public ESD_Shooting shooting;

    private float elapseTime;
    [SerializeField] float fireRate = 1;

    protected override void Awake()
    {
        base.Awake();
        startPoint = transform.position;
        InizializeStates();

    }

    public override void InizializeStates()
    {
        base.InizializeStates();
        idle = new ESD_Idle(this, stateMachine, this);
        shooting = new ESD_Shooting(this, stateMachine, this);
        stateMachine.Inizialiaze(idle);
    }


    public void Shoot()
    {
        elapseTime += Time.deltaTime;
        if (elapseTime >= fireRate)
        {
            spawnProjectile(projectileNumber);
            AudioManager.instance.PlaySFX(25, transform);

            elapseTime = 0;
        }
    }
    private void spawnProjectile(float numberOfProjectile)
    {
        float angleSteep = 360f / numberOfProjectile;
        float angle = 0;

        for (int i = 0; i < numberOfProjectile; i++)
        {
            float projectilePosX = startPoint.x + Mathf.Sin((angle * Mathf.PI) / 180) * radious;
            float projectilePosY = startPoint.y + Mathf.Cos((angle * Mathf.PI) / 180) * radious;

            Vector2 projectileVector = new Vector2(projectilePosX, projectilePosY);
            //Vector2 projectileMoveDirection = (projectileVector - startPoint).normalized * moveSpeed;
            Vector2 projectileMoveDirection = (projectileVector - startPoint).normalized;

            ProjectileEnemy proj = ObjectPoolManager.Instance.EnemyProjectilePool.UseObject();
            proj.SetSpeed(bulletSpeed);
            proj.transform.position = startPoint; 
            proj.transform.rotation = Quaternion.identity;
            proj.gameObject.SetActive(true);

            //var proj = Instantiate(projectile, startPoint, Quaternion.identity);

            proj.GetComponent<ProjectileEnemy>().DamagesModule = DamageModule();
            proj.GetComponent<ProjectileEnemy>().directionShoot = projectileMoveDirection;
            angle += angleSteep;
        }
    }
}
