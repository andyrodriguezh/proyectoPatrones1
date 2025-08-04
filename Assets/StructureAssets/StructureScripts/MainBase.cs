using StructureAssets.StructureScripts;
using UnityEngine;

public class MainBase : MonoBehaviour, IStructure
{
    [SerializeField] private GameObject minerPrefab;
    [SerializeField] private Transform spawnPoint;

    public void Activate()
    {
        //modificar?***
    }

    public void ProduceMiner()
    {
        Instantiate(minerPrefab, spawnPoint.position, Quaternion.identity);
    }
}
