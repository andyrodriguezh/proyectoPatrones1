using System.Collections.Generic;
using UnityEngine;
namespace StructureAssets.StructureScripts
{
    [CreateAssetMenu]
    public class ObjectsDatabseSO : ScriptableObject
    {
        public List<ObjectData> objectsData;


        public ObjectData GetObjectByID(int id)
        {
            foreach (ObjectData obj in objectsData)
            {
                if (obj.ID == id)
                {
                    return obj;
                }
            }

            return new();
        }

    }

    [System.Serializable]
    public class ObjectData
    {
        [field: SerializeField]
        public string Name { get; private set; }

        [field: SerializeField]
        public int ID { get; private set; }

        [field: SerializeField]
        [TextArea(3, 10)]
        public string description;

        [field: SerializeField]
        public Vector2Int Size { get; private set; } = Vector2Int.one;

        [field: SerializeField]
        public GameObject Prefab { get; private set; }

        [field: SerializeField]
        public List<BuildRequirement> requirements { get; private set; }

        [field: SerializeField]
        public List<BuildBenefits> benefits { get; private set; }
        
        [field: SerializeField]
        public StructureType Type { get; private set; }

  
    }
    
    public enum StructureType
    {
                MainBase,
                ResourceCollector,
                MilitaryBuilding
    }
    


    [System.Serializable]
    public class BuildBenefits
    {
        public enum BenefitType
        {
            Housing
        }


        public string benefit;
        public Sprite benefitIcon;
        public BenefitType benefitType;
        public int benefitAmount;
    }
}