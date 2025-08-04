using UnityEngine;

namespace StructureAssets.StructureScripts
{
    public class MilitaryBuilding : MonoBehaviour, IStructure
    {
        [SerializeField] private GameObject baseUnitPrefab;
        [SerializeField] private GameObject longDistanceUnitPrefab;
        [SerializeField] private Transform spawnPoint;

        public void Activate()
        {
            // sin acciones automáticas
        }

        public void TrainBasicUnit()
        {
            Instantiate(baseUnitPrefab, spawnPoint.position, Quaternion.identity);
        }

        public void TrainLongDistanceUnit()
        {
            Instantiate(longDistanceUnitPrefab, spawnPoint.position, Quaternion.identity);
        }
    }
}
