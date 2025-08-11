using UnityEngine;

namespace StructureAssets.StructureScripts
{
    public class StructureSelector : MonoBehaviour
    {
        [SerializeField] private LayerMask structureLayerMask;
        [SerializeField] private StructureSelectionNotifier notifier;

        private void Awake()
        {
            if (!notifier) notifier = FindFirstObjectByType<StructureSelectionNotifier>();
        }

        private void Update()
        {
            // S + Click izquierdo
            if (Input.GetKey(KeyCode.S) && Input.GetMouseButtonDown(0))
            {
                var cam = Camera.main;
                if (!cam) return;

                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 1000f, structureLayerMask))
                {
                    if (hit.collider.TryGetComponent<IStructure>(out var structure))
                    {
                        notifier?.NotifyStructureSelected(structure);
                        Debug.Log("[StructureSelector] Estructura seleccionada: " + ((MonoBehaviour)structure).name);
                    }
                    else Debug.LogWarning("[StructureSelector] El collider impactado no tiene IStructure.");
                }
                else Debug.LogWarning("[StructureSelector] Raycast no impactó en la máscara 'Structure'.");
            }

            // Escape para limpiar selección
            if (Input.GetKeyDown(KeyCode.Escape))
                notifier?.NotifyClearedSelection();
        }
    }
}


/*using UnityEngine;

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
}*/
