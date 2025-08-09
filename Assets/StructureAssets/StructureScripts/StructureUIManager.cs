using System.Collections.Generic;
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
}