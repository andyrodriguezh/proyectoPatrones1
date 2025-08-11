using System.Collections.Generic;
using UnityEngine;
using StructureAssets.StructureScripts;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> placedGameObjects = new();

    public int PlaceObject(ObjectData data, Vector3 position)
    {
        GameObject newObject = StructureFactory.CreateStructure(data, position);
        placedGameObjects.Add(newObject);
        return placedGameObjects.Count - 1;
    }

    public void RemoveObjectAt(int gameObjectIndex)
    {
        if (gameObjectIndex < 0 || gameObjectIndex >= placedGameObjects.Count)
            return;

        if (placedGameObjects[gameObjectIndex] != null)
        {
            Destroy(placedGameObjects[gameObjectIndex]);
            placedGameObjects[gameObjectIndex] = null;
        }
    }
}