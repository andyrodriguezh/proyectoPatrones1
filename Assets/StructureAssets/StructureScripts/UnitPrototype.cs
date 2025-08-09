using System.Collections.Generic;
using UnityEngine;

namespace StructureAssets.StructureScripts
{
    [CreateAssetMenu(menuName = "Units/Unit Prototype", fileName = "UnitPrototype")]
    public class UnitPrototype : ScriptableObject
    {
        public string unitType;
        public GameObject prefab;
        public List<BuildRequirement> requirements;

        public GameObject Clone(Transform spawnPoint)
        {
            if (prefab == null) { Debug.LogWarning($"[UnitPrototype] Prefab faltante para {unitType}"); return null; }
            var pos = spawnPoint ? spawnPoint.position : Vector3.zero;
            return Instantiate(prefab, pos, Quaternion.identity);
        }
    }
}