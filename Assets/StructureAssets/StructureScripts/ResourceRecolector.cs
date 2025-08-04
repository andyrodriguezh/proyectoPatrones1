using UnityEngine;

namespace StructureAssets.StructureScripts
{
    public class ResourceRecolector : MonoBehaviour, IStructure
    {
        public void Activate()
        {
            // hay que notificar al sistema económico que hay un nuevo nodo activo.(??)
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }
    }
}
