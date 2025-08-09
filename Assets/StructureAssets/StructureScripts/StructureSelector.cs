using UnityEngine;

namespace StructureAssets.StructureScripts
{
    public class StructureSelector : MonoBehaviour
    {
        [SerializeField] private LayerMask structureLayerMask;
        [SerializeField] private StructureSelectionNotifier notifier;
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.S))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 100f, structureLayerMask))
                {
                    if (hit.collider.TryGetComponent<IStructure>(out var structure))
                    {
                        GameObject go = ((MonoBehaviour)structure).gameObject;
                        Debug.Log("Estructura seleccionada: " + go.name);
                        notifier.NotifyStructureSelected(structure);
                    }
                }
            }
        }
    }
}
