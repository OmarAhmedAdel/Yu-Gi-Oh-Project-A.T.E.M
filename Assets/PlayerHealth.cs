using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float Health, MaxHealth;
    public string HealthBarName;
    [SerializeField] GameObject prefabsToSpawn ;

    private HealthBarUI healthBar;
    private float damage = 0; 
    void Start()
    {
        GameObject barObject = GameObject.Find(HealthBarName);
        if (barObject != null)
        {
            healthBar = barObject.GetComponent<HealthBarUI>();
            if (healthBar != null)
            {
                healthBar.SetMaxHealth(MaxHealth);
            }
            else
            {
                Debug.LogWarning("HealthBarUI component not found on 'HealthBar' GameObject.");
            }
        }
        else
        {
            Debug.LogWarning("GameObject named 'HealthBar' not found.");
        }
    }


    public void SetHealth()
    {
        int healthChange = -Random.Range(5, 21); // Random int between 5 and 20 (inclusive), then made negative
        damage = (-healthChange);
        Health += healthChange;
        Health = Mathf.Clamp(Health, 0, MaxHealth);
        healthBar.SetHealth(Health);
        if (Health == 0)
        {
            gameObject.SetActive(false);
            prefabsToSpawn.SetActive(true);
        }
    }
        public float GetHealth()
    {
        return Health;
    }
    public float GetDamage()
    {
        return damage;
    }
}