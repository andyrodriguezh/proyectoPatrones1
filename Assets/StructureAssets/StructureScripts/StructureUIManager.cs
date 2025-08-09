using System;
using System.Collections.Generic;
using UnityEngine;

namespace StructureAssets.StructureScripts
{
    public class StructureUIManager : MonoBehaviour, IStructureSelectionObserver
    {
        [SerializeField] private StructureSelectionNotifier notifier;
        [SerializeField] private MilitaryBuildingUI militaryUI;

        private readonly Dictionary<Type, IStructureUIFacade> map = new();
        private IStructureUIFacade active;

        private void Awake()
        {
            if (!notifier) notifier = FindFirstObjectByType<StructureSelectionNotifier>();
			
            map[typeof(MilitaryBuilding)] = militaryUI;

            HideAll();
        }

        private void OnEnable()
        {
            notifier?.RegisterObserver(this);
        }

        private void OnDisable()
        {
            notifier?.UnregisterObserver(this);
        }

        public void OnStructureSelected(IStructure structure)
        {
			 HideAll();
    		if (structure == null) return;

    		if (map.TryGetValue(structure.GetType(), out var facade))
    		{
        		active = facade;
        		active.ShowPanel(structure);
    		}

        }

        public void OnSelectionCleared() => HideAll();

        private void HideAll()
        {
            foreach (var f in map.Values) f.HidePanel();
            active = null;
        }
    }
}

/*using System.Collections.Generic;
using UnityEngine;

namespace StructureAssets.StructureScripts
{
    public class StructureUIManager : MonoBehaviour, IStructureSelectionObserver
    {
        [SerializeField] private List<MonoBehaviour> uiPanels = new();

        private List<IStructureUIFacade> facades = new();
        private IStructureUIFacade activePanel;

        private void Awake()
        {
            foreach (var panel in uiPanels)
            {
                if (panel is IStructureUIFacade facade)
                {
                    facades.Add(facade);
                }
                else
                {
                    Debug.LogWarning($"El objeto {panel.name} no implementa IStructureUIFacade.");
                }
            }
        }

        private void Start()
        {
            FindFirstObjectByType<StructureSelectionNotifier>()?.RegisterObserver(this);
        }

        public void OnStructureSelected(IStructure structure)
        {
            activePanel?.HidePanel();
            activePanel = null;

            if (structure == null) return;
            
            foreach (var facade in facades)
            {
                facade.ShowPanel(structure);
                if (IsPanelVisible(facade))
                {
                    activePanel = facade;
                    break;
                }
            }
        }

        private bool IsPanelVisible(IStructureUIFacade panel)
        {
            MonoBehaviour mb = panel as MonoBehaviour;
            return mb != null && mb.gameObject.activeInHierarchy;
        }
    }
}*/