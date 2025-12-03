using CustomUnityLibrary;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;

    [SerializeField] private ProjectileEnemy[] enemyProjectiles;
    [SerializeField] private Projectile[] playerProjectiles;
    [SerializeField] private Projectile[] playerChargedShot;

    [HideInInspector] public ObjectPool<ProjectileEnemy> EnemyProjectilePool { get; private set; }
    [HideInInspector] public ObjectPool<Projectile> PlayerProjectilePool { get; private set; }
    [HideInInspector] public ObjectPool<Projectile> PlayerChargedShotPool { get; private set; }

    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance.gameObject);
        else
            Instance = this;

        EnemyProjectilePool = new ObjectPool<ProjectileEnemy>(enemyProjectiles);
        PlayerProjectilePool = new ObjectPool<Projectile>(playerProjectiles);
        PlayerChargedShotPool = new ObjectPool<Projectile>(playerChargedShot);

       /*foreach (var projectile in enemyProjectiles)
            projectile.SetObjectPool(EnemyProjectilePool);*/

        foreach (var projectile in playerProjectiles)
            projectile.SetObjectPool(PlayerProjectilePool);

        foreach (var projectile in playerChargedShot)
            projectile.SetObjectPool(PlayerChargedShotPool);
    }

    public void RecycleProjectiles(ObjectPool<Projectile> pool, Projectile projectile)
    {
        pool.RecycleObject(projectile);
        projectile.gameObject.SetActive(false);
    }

    public void RecycleEnemyProjectiles(ProjectileEnemy projectile)
    {
        EnemyProjectilePool.RecycleObject(projectile);
        projectile.gameObject.SetActive(false);
    }
}
