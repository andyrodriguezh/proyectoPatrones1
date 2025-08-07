using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Interfaz 
public interface IGoldMineObserver
{
    void OnGoldChanged(GoldMine mine, int currentGold, int maxGold);
}

// Interfaz para el publicador (la mina)
public interface IGoldMineSubject
{
    void AddObserver(IGoldMineObserver observer);
    void RemoveObserver(IGoldMineObserver observer);
    void NotifyObservers();
}

public class GoldMine : MonoBehaviour, IGoldMineSubject
{
    [Header("Configuración de Recursos")]
    public int currentGold = 1000;
    public int maxGold = 1000;
    public int rechargeRate = 10;
    public float rechargeInterval = 5f;

    [Header("Configuración de Extracción")]
    public int goldPerExtraction = 10;
    public float extractionTime = 2f;

    private bool isRecharging = false;
    private List<IGoldMineObserver> observers = new List<IGoldMineObserver>();

    private void Start()
    {
        StartCoroutine(RechargeOverTime());
    }

    public bool CanExtractGold()
    {
        return currentGold >= goldPerExtraction;
    }

    public int ExtractGold()
    {
        if (!CanExtractGold())
            return 0;

        currentGold -= goldPerExtraction;
        
        NotifyObservers();

        return goldPerExtraction;
    }

    private IEnumerator RechargeOverTime()
    {
        isRecharging = true;

        while (isRecharging)
        {
            yield return new WaitForSeconds(rechargeInterval);

            if (currentGold < maxGold)
            {
                currentGold = Mathf.Min(currentGold + rechargeRate, maxGold);
                NotifyObservers();
            }
        }
    }

    // Implementación 
    public void AddObserver(IGoldMineObserver observer)
    {
        if (!observers.Contains(observer))
        {
            observers.Add(observer);
        }
    }

    public void RemoveObserver(IGoldMineObserver observer)
    {
        observers.Remove(observer);
    }

    public void NotifyObservers()
    {
        foreach (var observer in observers)
        {
            observer.OnGoldChanged(this, currentGold, maxGold);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 3f);
    }
}