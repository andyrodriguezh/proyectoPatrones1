using System.Collections.Generic;
using StructureAssets.StructureScripts;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }
    
    private Dictionary<string, int> resources = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        //Recursos quemados para prueba
        resources["Gold"] = 500;
    }

    public bool HasEnoughResources(List<BuildRequirement> requirements)
    {
        foreach (BuildRequirement req in requirements)
        {
            if (!resources.ContainsKey(req.resource) || resources[req.resource] < req.amount)
            {
                return false;
            }
        }
        return true;
    }

    public void ConsumeResources(List<BuildRequirement> requirements)
    {
        foreach (BuildRequirement req in requirements)
        {
            if (resources.ContainsKey(req.resource))
            {
                resources[req.resource] -= req.amount;
            }
        }
    }
    
    public void PrintResources()
    {
        foreach (var res in resources)
        {
            Debug.Log($"{res.Key}: {res.Value}");
        }
    }
}