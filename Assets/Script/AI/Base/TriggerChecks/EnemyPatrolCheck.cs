using UnityEngine;

public class EnemyPatrolCheck : MonoBehaviour
{
    [SerializeField] private Enemy enemy;

    /*private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
    }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            enemy.SetPatrolActive(true);

    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            enemy.SetPatrolActive(false);

    }
}
