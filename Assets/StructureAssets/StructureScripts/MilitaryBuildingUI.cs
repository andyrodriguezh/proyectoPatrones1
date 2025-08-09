using UnityEngine;
using UnityEngine.UI;

namespace StructureAssets.StructureScripts
{
    public class MilitaryBuildingUI : MonoBehaviour, IStructureUIFacade
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private Button meleeButton, rangeButton, healerButton, tankButton;
        [SerializeField] private Button closeButton;

        private UnitFactory unitFactory;
        private IStructure _current;

        private void Awake()
        {
            meleeButton.onClick.AddListener(() => TrySpawn("Melee"));
            rangeButton.onClick.AddListener(() => TrySpawn("Range"));
            healerButton.onClick.AddListener(() => TrySpawn("Healer"));
            tankButton.onClick.AddListener(() => TrySpawn("Tank"));
            if (closeButton) closeButton.onClick.AddListener(HidePanel);
            HidePanel();
        }

        public void ShowPanel(IStructure structure)
        {
            _current = structure;
            
            var go = ((MonoBehaviour)structure).gameObject;
            unitFactory = go.GetComponentInChildren<UnitFactory>(true);

            if (!unitFactory)
                Debug.LogWarning("[MBUI] No encontré UnitFactory como hijo del MilitaryBuilding seleccionado.");

            panel.SetActive(true);
        }

        public void HidePanel()
        {
            panel.SetActive(false);
            _current = null;
            unitFactory = null;
        }

        private void TrySpawn(string unitType)
        {
            if (!unitFactory) { Debug.LogWarning("[MBUI] UnitFactory es null."); return; }

            var reqs = unitFactory.GetRequirements(unitType);
            if (reqs == null || reqs.Count == 0) { Debug.LogWarning($"[MBUI] No hay requisitos para {unitType}.");
                return; }

            var rm = ResourceManager.Instance;
            if (rm == null) { Debug.LogWarning("[MBUI] ResourceManager.Instance es null."); return; }

            if (rm.HasEnoughResources(reqs))
            {
                rm.ConsumeResources(reqs);
                unitFactory.CreateUnit(unitType);
            }
            else
            {
                Debug.Log("No hay recursos suficientes para " + unitType);
            }
        }
    }
}


/*using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StructureAssets.StructureScripts
{
    public class MilitaryBuildingUI : MonoBehaviour, IStructureUIFacade
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private Button meleeButton, rangeButton, healerButton, tankButton;
		[SerializeField] private Button closeButton;
        [SerializeField] private UnitFactory unitFactory;
		private IStructure _current;

        private void Awake()
        {
            if (!unitFactory) unitFactory = FindFirstObjectByType<UnitFactory>();
            meleeButton.onClick.AddListener(() => TrySpawn("Melee"));
            rangeButton.onClick.AddListener(() => TrySpawn("Range"));
            healerButton.onClick.AddListener(() => TrySpawn("Healer"));
            tankButton.onClick.AddListener(() => TrySpawn("Tank"));
			
			if (closeButton != null)
                closeButton.onClick.AddListener(HidePanel);            

            HidePanel();
        }

        public void ShowPanel(IStructure structure)
        {
            _current = structure;
        	panel.SetActive(true);
        }

        public void HidePanel() => panel.SetActive(false);

        private void TrySpawn(string unitType)
        {
            if (!unitFactory){ Debug.LogWarning("[MBUI] Falta UnitFactory"); return; }
            var reqs = unitFactory.GetRequirements(unitType);
            if (reqs == null){ Debug.LogWarning($"[MBUI] No hay requisitos para {unitType}"); return; }

            var rm = ResourceManager.Instance;
            if (rm == null){ Debug.LogWarning("[MBUI] ResourceManager.Instance es null"); return; }

            if (rm.HasEnoughResources(reqs))
            {
                rm.ConsumeResources(reqs);
                unitFactory.CreateUnit(unitType);
            }
            else
            {
                Debug.Log($"No hay recursos suficientes para {unitType}");
            }
        }
    }
}*/