using UnityEngine;

public class Unit : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       UnitSelectorManager.Instance.allUnitsList.Add(gameObject); 
    }

    private void OnDestroy()
    {
        UnitSelectorManager.Instance.allUnitsList.Remove(gameObject);  
    }

    
    
}
