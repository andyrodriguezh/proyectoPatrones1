using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using StructureAssets.StructureScripts;

public class BuildingSelectionUI : MonoBehaviour
{
    [SerializeField] private GameObject buildMenuPanel;

    [SerializeField] private Button toggleBuildMenuButton;

    [SerializeField] private Button buildMilitaryBuildingButton;

    [SerializeField] private PlacementSystem placementSystem;
    [SerializeField] private ObjectsDatabseSO database;

    private void Start()
    {
        buildMenuPanel.SetActive(false);

        toggleBuildMenuButton.onClick.AddListener(ToggleBuildMenu);

        
        buildMilitaryBuildingButton.onClick.AddListener(() => TryBuildStructure(2));
    }

    private void ToggleBuildMenu()
    {
        buildMenuPanel.SetActive(!buildMenuPanel.activeSelf);
        UpdateButtonInteractivity();
    }

    private void UpdateButtonInteractivity()
    {
      
        buildMilitaryBuildingButton.interactable = CheckRequirements(2);
    }

    private bool CheckRequirements(int id)
    {
        ObjectData data = database.GetObjectByID(id);
        return ResourceManager.Instance.HasEnoughResources(data.requirements);
    }

    private void TryBuildStructure(int id)
    {
        ObjectData data = database.GetObjectByID(id);
        
        Debug.Log($"Intentando construir: {data.Name}");
        
        foreach (BuildRequirement req in data.requirements)
        {
            Debug.Log($"Requiere: {req.resource} - {req.amount}");
        }

        if (ResourceManager.Instance.HasEnoughResources(data.requirements))
        {
            ResourceManager.Instance.AddPlayerGold(-65);
            placementSystem.StartPlacement(id);
            buildMenuPanel.SetActive(false);
        }
        else
        {
            Debug.Log("No hay suficientes recursos para construir: " + data.Name);
        }
    }
}

