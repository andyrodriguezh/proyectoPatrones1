using UnityEngine;

namespace StructureAssets.StructureScripts
{
    public static class StructureFactory
    {
        public static GameObject CreateStructure(ObjectData data, Vector3 position)
        {
            GameObject objeto = Object.Instantiate(data.Prefab, position, Quaternion.identity);

            IStructure structure = objeto.GetComponent<IStructure>();
            if (structure != null)
            {
                structure.Activate();
            }

            Constructable constructable = objeto.GetComponent<Constructable>();
            if (constructable != null)
            {
                constructable.ConstructableWasPlaced();
            }

            return objeto;
        }
    }
}