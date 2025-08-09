using System;
using System.Collections;
using System.Collections.Generic;
using StructureAssets.StructureScripts;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Grid grid;
    [SerializeField] private ObjectsDatabseSO database;
    [SerializeField] private GridData floorData, furnitureData;
    [SerializeField] private PreviewSystem previewSystem;
    [SerializeField] private ObjectPlacer objectPlacer;

    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    private ObjectData selectedObjectData;
    private IBuildingState buildingState;

    private void Start()
    {
        floorData = new();
        furnitureData = new();
    }

    public void StartPlacement(int ID)
    {
        Debug.Log("Should Start Placement");
        Debug.Log("Placement ID: " + ID);

        StopPlacement();
        
        selectedObjectData = database.GetObjectByID(ID);

        if (selectedObjectData == null)
        {
            Debug.LogError($"ObjectData con ID {ID} no encontrado.");
            return;
        }
        
        buildingState = new PlacementState(selectedObjectData, grid, previewSystem, floorData, furnitureData, objectPlacer);

        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    public void StartRemoving()
    {
        StopPlacement();

        buildingState = new RemovingState(grid, previewSystem, floorData, furnitureData, objectPlacer);

        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI())
        {
            Debug.Log("Pointer was over UI - Returned");
            return;
        }

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        buildingState.OnAction(gridPosition);

        // --- Quitar recursos requeridos (si tu sistema lo tiene) ---
        // ResourceManager.Instance.RemoveResourcesBasedOnRequirements(selectedObjectData, database);

        // --- Aplicar beneficios si los hay ---
        foreach (BuildBenefits bf in selectedObjectData.benefits)
        {
            CalculateAndAddBenefit(bf);
        }

        StopPlacement(); // Evita colocar múltiples sin querer
    }

    private void CalculateAndAddBenefit(BuildBenefits bf)
    {
        switch (bf.benefitType)
        {
            case BuildBenefits.BenefitType.Housing:
                // StatusManager.Instance.IncreaseHousing(bf.benefitAmount);
                break;
        }
    }

    private void StopPlacement()
    {
        if (buildingState == null)
            return;

        buildingState.EndState();

        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;

        lastDetectedPosition = Vector3Int.zero;
        buildingState = null;
        selectedObjectData = null;
    }

    private void Update()
    {
        if (buildingState == null)
            return;

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        if (lastDetectedPosition != gridPosition)
        {
            buildingState.UpdateState(gridPosition);
            lastDetectedPosition = gridPosition;
        }
    }
}
