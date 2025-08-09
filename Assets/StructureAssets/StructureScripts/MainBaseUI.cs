using StructureAssets.StructureScripts;
using UnityEngine;
using UnityEngine.UI;

public class MainBaseUI : MonoBehaviour, IStructureSelectionObserver, IStructureUIFacade
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Button produceMinerButton;
    [SerializeField] private Button closeButton;

    private MainBase _currentBase;

    private void Start()
    {
        Debug.Log("Ejecutando Start() de MainBaseUI");
        produceMinerButton.onClick.AddListener(OnProduceMinerClicked);
        closeButton.onClick.AddListener(HidePanel);
        
        panel.SetActive(false);
        
        FindFirstObjectByType<StructureSelectionNotifier>()?.RegisterObserver(this);
        Debug.Log("Usando StructureSelectionNotifier - Observer");
    }

    public void OnStructureSelected(IStructure structure)
    {
        Debug.Log("MainBaseUI recibió notificación de selección");
        if (structure is MainBase mainBase)
        {
            _currentBase = mainBase;
            panel.SetActive(true);
        }
        else
        {
            panel.SetActive(false);
        }
    }

    private void OnProduceMinerClicked()
    {
        _currentBase?.ProduceMiner();
    }

    public void ShowPanel(IStructure structure)
    {
        Debug.Log("ShowPanel() llamado por StructureUIManager en MainBaseUI - Facade");
        if (structure is MainBase mainBase)
        {
            _currentBase = mainBase;
            panel.SetActive(true);
        }
    }

    public void HidePanel()
    {
        panel.SetActive(false);
        _currentBase = null;
    }
}
