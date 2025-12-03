using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public int destroyModuleChance;
    [SerializeField] private Vector2 knockbackPower;

    public bool DamageModule()
    {
        int random = Random.Range(0, 101);

        return random <= destroyModuleChance;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.GetComponent<Player>().TakeDamage(DamageModule(), knockbackPower, transform);
            Debug.Log(gameObject.name);
        }
    }
}
