using UnityEngine;

public class TrapCollision : MonoBehaviour
{
    [SerializeField] private int destroyModuleChance;
    [SerializeField] private Vector2 knockbackPower;
    // Start is called before the first frame update
    public bool DamageModule()
    {
        int random = Random.Range(0, 101);

        return random <= destroyModuleChance;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Colpito");
            collision.gameObject.GetComponent<Player>().TakeDamage(DamageModule(), knockbackPower, transform);
            Debug.Log(gameObject.name);
        }
    }
}
