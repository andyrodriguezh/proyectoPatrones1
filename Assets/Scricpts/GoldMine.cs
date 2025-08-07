using UnityEngine;
using System.Collections;

public class GoldMine : MonoBehaviour
{
    [Header("Configuración de Recursos")]
    public int currentGold = 1000;           // Cantidad actual de oro
    public int maxGold = 1000;               // Capacidad máxima
    public int rechargeRate = 10;            // Cantidad de oro recargado por intervalo
    public float rechargeInterval = 5f;      // Intervalo de recarga en segundos

    [Header("Configuración de Extracción")]
    public int goldPerExtraction = 10;       // Oro extraído por operación
    public float extractionTime = 2f;        // Tiempo necesario para extraer

    private bool isRecharging = false;

    private void Start()
    {
        // Iniciar la recarga automática
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
            }
        }
    }

    private void OnDrawGizmos()
    {
        // Zona de interacción para los mineros
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 3f);
    }
    
    [Header("Depuración")]
    public bool showDebugInfo = true;
    public float debugInterval = 2f;
    private float debugTimer;

    private void Update()
    {
        if (showDebugInfo)
        {
            debugTimer += Time.deltaTime;
            if (debugTimer >= debugInterval)
            {
                Debug.Log($"Mina: {gameObject.name} - Oro actual: {currentGold}/{maxGold}");
                debugTimer = 0f;
            }
        }
    }
}