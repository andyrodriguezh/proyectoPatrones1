using System.Collections;
using System.Collections.Generic;
using StructureAssets.StructureScripts;
using UnityEngine;

public class PlacementState : IBuildingState
{
    private ObjectData selectedObjectData;
    private Grid grid;
    private PreviewSystem previewSystem;
    private GridData floorData;
    private GridData furnitureData;
    private ObjectPlacer objectPlacer;

    public PlacementState(ObjectData selectedObjectData,
        Grid grid,
        PreviewSystem previewSystem,
        GridData floorData,
        GridData furnitureData,
        ObjectPlacer objectPlacer)
    {
        this.selectedObjectData = selectedObjectData;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.floorData = floorData;
        this.furnitureData = furnitureData;
        this.objectPlacer = objectPlacer;

        if (selectedObjectData != null && selectedObjectData.Prefab != null)
        {
            previewSystem.StartShowingPlacementPreview(selectedObjectData.Prefab, selectedObjectData.Size);
        }
        else
        {
            throw new System.Exception($"Missing ObjectData or Prefab.");
        }
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        if (!CheckPlacementValidity(gridPosition))
            return;

        // Instanciar y activar el edificio usando ObjectData
        Vector3 worldPosition = grid.CellToWorld(gridPosition);
        int index = objectPlacer.PlaceObject(selectedObjectData, worldPosition);
        
        GridData selectedData = GetAllFloorIDs().Contains(selectedObjectData.ID) ? floorData : furnitureData;

        selectedData.AddObjectAt(gridPosition,
                                 selectedObjectData.Size,
                                 selectedObjectData.ID,
                                 index);

        previewSystem.UpdatePosition(worldPosition, false);
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        bool validity = CheckPlacementValidity(gridPosition);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), validity);
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition)
    {
        GridData selectedData = GetAllFloorIDs().Contains(selectedObjectData.ID) ? floorData : furnitureData;
        return selectedData.CanPlaceObjectAt(gridPosition, selectedObjectData.Size);
    }

    //objetos que son piso
    private List<int> GetAllFloorIDs()
    {
        return new List<int> { 11 }; //cantidad de piso
    }
}
