using UnityEngine;

public class EnemyCollisionCheck : MonoBehaviour
{
    private Enemy enemy;

    private void Start()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().TakeDamage(enemy.DamageModule(), enemy.knockbackPower, enemy.transform);
            Debug.Log(gameObject.name);
        }
    }
}

