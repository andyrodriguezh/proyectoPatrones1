using System.Collections.Generic;
using StructureAssets.StructureScripts;
using UnityEngine;

public class MainBase : MonoBehaviour, IStructure //FactoryMethod
{
    [SerializeField] private GameObject minerPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private List<BuildRequirement> requirements;
    [SerializeField] private bool isPlayerBase = true;

    public void Activate()
    {
        Debug.Log("MainBase colocada y activada");
    }

    public void ProduceMiner()
    {
        if (!ResourceManager.Instance.HasEnoughResources(requirements))
        {
            Debug.Log("No hay suficiente oro para producir minero.");
            return;
        }

        ResourceManager.Instance.ConsumeResources(requirements);

        Instantiate(minerPrefab, spawnPoint.position, Quaternion.identity);
        Debug.Log("Minero producido desde MainBase.");
    }
    
    private void OnDestroy()
    {
        if (isPlayerBase)
        {
            Debug.Log("MainBase del jugador destruida. Fin del juego.");
            GameOverManager.Instance.TriggerGameOver();
        }
    }
}