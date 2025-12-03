public interface IDamageable
{
    void TakeDamage(float damage);
    void Die();

    float MaxHealth { get; set; }
    float currentHealth { get; set; }
}
