using UnityEngine;
using UnityEngine.Events;

public class HealthTracker : MonoBehaviour
{
    [Tooltip("Maximum health for this object.")]
    [SerializeField]
    private int maxHealth = 100;

    [Tooltip("Starting/current health. If left at 0 it will initialize to Max Health on Awake.")]
    [SerializeField]
    private int currentHealth = 0;

    [Tooltip("Invoked when health reaches zero.")]
    [SerializeField]
    private UnityEvent onDeath;

    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;
    public bool IsDead => currentHealth <= 0;

    void Awake()
    {
        // If currentHealth wasn't explicitly set in the inspector, initialize it to maxHealth.
        if (currentHealth <= 0)
        {
            currentHealth = maxHealth;
        }
    }

    /// <summary>
    /// Decrease health by <paramref name="amount"/>. Clamped at zero. Triggers OnDeath when health reaches zero.
    /// </summary>
    public void TakeDamage(int amount)
    {
        if (amount <= 0 || IsDead)
        {
            return;
        }

        currentHealth = Mathf.Max(0, currentHealth - amount);

        if (currentHealth == 0)
        {
            onDeath?.Invoke();
            OnDeath();
        }
    }

    /// <summary>
    /// Increase health by <paramref name="amount"/>. Clamped at MaxHealth.
    /// </summary>
    public void Heal(int amount)
    {
        if (amount <= 0 || IsDead)
        {
            return;
        }

        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
    }

    /// <summary>
    /// Reset current health to MaxHealth (useful from the Inspector context menu).
    /// </summary>
    [ContextMenu("Reset Health")]
    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Override this in subclasses to respond to death. The inspector UnityEvent <see cref="onDeath"/> is also invoked.
    /// </summary>
    protected virtual void OnDeath()
    {
        // Default: do nothing. Override or subscribe to the inspector event.
    }
}
