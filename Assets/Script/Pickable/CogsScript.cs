using UnityEngine;

public class CogsScript : MonoBehaviour, IPickable
{
    public void OnPickUp(ref float coin)
    {
        coin++;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AudioManager.instance.PlaySFX(0, transform);
            OnPickUp(ref PlayerManager.instance.playerCogCounter);
            EventManager.UpdateUI?.Invoke();
            GameManager.Instance.AddCogToMilestone();
            Destroy(gameObject);
        }
    }
}
