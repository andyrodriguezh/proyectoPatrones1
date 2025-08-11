using System.Collections.Generic;
using UnityEngine;

namespace StructureAssets.StructureScripts
{
    public class UnitFactory : MonoBehaviour
    {
        private Transform spawnPoint;
        private readonly Dictionary<string, UnitPrototype> map = new();
        
        public void Initialize(UnitPrototype meleeProto, UnitPrototype rangeProto, UnitPrototype healerProto, UnitPrototype tankProto, Transform spawn)
        {
            spawnPoint = spawn;
            map.Clear();
            Register(meleeProto);
            Register(rangeProto);
            Register(healerProto);
            Register(tankProto);
        }

        private void Register(UnitPrototype proto)
        {
            if (proto == null) return;
            if (string.IsNullOrWhiteSpace(proto.unitType))
            {
                Debug.LogWarning("[UnitFactory] unitType vacío en un prototype.");
                return;
            }
            map[proto.unitType] = proto;
        }

        public List<BuildRequirement> GetRequirements(string unitType)
        {
            return map.TryGetValue(unitType, out var proto) ? proto.requirements : null;
        }

        public GameObject CreateUnit(string unitType)
        {
            if (!map.TryGetValue(unitType, out var proto))
            {
                Debug.LogWarning($"[UnitFactory] No hay prototipo para '{unitType}'.");
                return null;
            }
            return proto.Clone(spawnPoint);
        }

        
        public GameObject CreateUnit(UnitPrototype proto)
        {
            if (proto == null) { Debug.LogWarning("[UnitFactory] Prototype nulo."); return null; }
            return proto.Clone(spawnPoint);
        }
    }
}