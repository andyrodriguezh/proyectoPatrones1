using UnityEngine;

public class Unit : MonoBehaviour
{
    
    private float unitsHealth;
    public float unitMaxHealth=100;
    
    public HealthTracker healthTracker;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       UnitSelectorManager.Instance.allUnitsList.Add(gameObject); 
       unitsHealth=unitMaxHealth;
       UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        healthTracker.UpdateSliderValue(unitsHealth,unitMaxHealth);
        if (unitsHealth <= 0)
        {
            Destroy(gameObject);
        }
        
    }
    
    public void ReceiveHealing(int amount)
    {
        unitsHealth = Mathf.Min(unitsHealth + amount, unitMaxHealth);
        UpdateHealthUI();
    }

    public float GetHealthPercentage()
    {
        return unitsHealth / unitMaxHealth;
    }




    private void OnDestroy()
    {
        // Proteger si el selector no aplica (enemigos no lo usan)
        if (UnitSelectorManager.Instance != null)
        {
            UnitSelectorManager.Instance.allUnitsList.Remove(gameObject);
        }
    }


    



    public void TakeDamage(int damageToInflict)
    {
        unitsHealth -= damageToInflict;
        UpdateHealthUI();
    }
}
