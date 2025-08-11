using UnityEngine;

namespace StructureAssets.StructureScripts
{
    public class MilitaryBuilding : MonoBehaviour, IStructure
    {
        private const string SpawnChildName = "SpawnPointMilitary";

        [Header("Prototypes (ScriptableObjects)")]
        [SerializeField] private UnitPrototype melee;
        [SerializeField] private UnitPrototype range;
        [SerializeField] private UnitPrototype healer;
        [SerializeField] private UnitPrototype tank;

        private UnitFactory unitFactory;
        private Transform spawnPoint;
        private StructureSelectionNotifier notifier;

        private void Awake()
        {
            unitFactory = GetComponentInChildren<UnitFactory>(true);
            if (unitFactory == null)
            {
                Debug.LogError("[MilitaryBuilding] No se encontró UnitFactory como hijo del prefab.");
                return;
            }
            
            spawnPoint = transform.Find(SpawnChildName);
            if (spawnPoint == null)
            {
                foreach (var t in GetComponentsInChildren<Transform>(true))
                {
                    if (t.name == SpawnChildName) { spawnPoint = t; break; }
                }
            }

            if (spawnPoint == null)
            {
                Debug.LogError($"[MilitaryBuilding] No se encontró un hijo llamado '{SpawnChildName}'.");
                return;
            }

            unitFactory.Initialize(melee, range, healer, tank, spawnPoint);

            notifier = FindFirstObjectByType<StructureSelectionNotifier>();
        }
        
        public void OnSelected()
        {
            notifier ??= FindFirstObjectByType<StructureSelectionNotifier>();
            notifier?.NotifyStructureSelected(this);
        }

        public void OnSelectionCleared()
        {
            // Limpieza interna si hiciera falta
        }

        public void Activate()
        {
            Debug.Log("MilitaryBuilding colocado en escena");
        }
    }
}


/*using UnityEngine;

namespace StructureAssets.StructureScripts
{
    public class MilitaryBuilding : MonoBehaviour, IStructure
    {
        private StructureSelectionNotifier _notifier;

        [SerializeField] private UnitFactory unitFactory;
        [SerializeField] private Transform spawnPoint;

        [Header("Prototypes")]
        public UnitPrototype melee;
        public UnitPrototype range;
        public UnitPrototype healer;
        public UnitPrototype tank;

        private void Awake()
        {
            if (spawnPoint == null)
                spawnPoint = transform.Find("SpawnPointMilitary");
            
            if (unitFactory != null)
                unitFactory.Initialize(melee, range, healer, tank, spawnPoint);
        }
        
        private void Start()
        {
            _notifier = FindFirstObjectByType<StructureSelectionNotifier>();
        }
        
        public void Activate()
        {
            Debug.Log("MilitaryBuilding colocado en escena");
        }
        
        public void OnSelected()
        {
            _notifier?.NotifyStructureSelected(this);
        }

        public void OnSelectionCleared()
        {
            // limpiar cosas internas si es necesario
        }
        
    }
}*/