using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class BuildRequirement
{
    public string resource;
    public int amount;
}

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    // Recursos directos para compatibilidad con código existente
    [Header("Recursos del Jugador")]
    public int playerGold = 0;

    [Header("Recursos del Enemigo")]
    public int enemyGold = 0;

    // Diccionario para gestionar múltiples tipos de recursos
    private Dictionary<string, int> resources = new Dictionary<string, int>();

    [Header("UI")]
    [SerializeField] private string playerGoldTextName = "PlayerGoldText";
    [SerializeField] private string enemyGoldTextName = "EnemyGoldText";
    
    private Text playerGoldText;
    private Text enemyGoldText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
        // Inicializar diccionario
        resources["Gold"] = playerGold;
        
        // Buscar elementos UI
        GameObject playerTextObj = GameObject.Find(playerGoldTextName);
        if (playerTextObj != null)
            playerGoldText = playerTextObj.GetComponent<Text>();
            
        GameObject enemyTextObj = GameObject.Find(enemyGoldTextName);
        if (enemyTextObj != null)
            enemyGoldText = enemyTextObj.GetComponent<Text>();
            
        if (playerGoldText == null || enemyGoldText == null)
            Debug.LogWarning("ResourceManager: No se pudieron encontrar elementos de la UI");
    }

    private void Start()
    {
        UpdateUI();
    }

    #region Métodos para compatibilidad con código legacy

    public void AddPlayerGold(int amount)
    {
        playerGold += amount;
        resources["Gold"] = playerGold; // Sincronizar con diccionario
        Debug.Log($"Oro del jugador actualizado: +{amount} → Total: {playerGold}");
        UpdateUI();
    }

    public void AddEnemyGold(int amount)
    {
        enemyGold += amount;
        Debug.Log($"Oro del enemigo actualizado: +{amount} → Total: {enemyGold}");
        UpdateUI();
    }

    public bool SpendPlayerGold(int amount)
    {
        if (playerGold >= amount)
        {
            playerGold -= amount;
            resources["Gold"] = playerGold; // Sincronizar con diccionario
            Debug.Log($"Oro del jugador gastado: -{amount} → Restante: {playerGold}");
            UpdateUI();
            return true;
        }
        Debug.Log($"Oro insuficiente: Necesitas {amount}, tienes {playerGold}");
        return false;
    }

    #endregion

    #region Métodos para nuevo sistema basado en diccionario

    // Verifica si hay suficientes recursos para cumplir los requisitos
    public bool HasEnoughResources(List<BuildRequirement> requirements)
    {
        if (requirements == null) return true;

        foreach (var req in requirements)
        {
            if (req.resource.Equals("Gold", System.StringComparison.OrdinalIgnoreCase))
            {
                if (playerGold < req.amount)
                    return false;
            }
            else if (!resources.ContainsKey(req.resource) || resources[req.resource] < req.amount)
            {
                return false;
            }
        }
        return true;
    }

    // Consume los recursos indicados en los requisitos
    public void ConsumeResources(List<BuildRequirement> requirements)
    {
        if (requirements == null) return;

        foreach (var req in requirements)
        {
            if (req.resource.Equals("Gold", System.StringComparison.OrdinalIgnoreCase))
            {
                SpendPlayerGold(req.amount);
            }
            else if (resources.ContainsKey(req.resource))
            {
                resources[req.resource] -= req.amount;
            }
        }
    }

    // Método para agregar recursos al diccionario
    public void AddResource(string resource, int amount)
    {
        if (resource.Equals("Gold", System.StringComparison.OrdinalIgnoreCase))
        {
            AddPlayerGold(amount);
            return;
        }

        if (!resources.ContainsKey(resource))
            resources[resource] = 0;
            
        resources[resource] += amount;
    }

    // Imprime los recursos actuales en consola
    public void PrintResources()
    {
        Debug.Log($"Gold (Player): {playerGold}");
        Debug.Log($"Gold (Enemy): {enemyGold}");
        
        foreach (var res in resources)
        {
            if (res.Key != "Gold") // Ya mostramos el oro arriba
                Debug.Log($"{res.Key}: {res.Value}");
        }
    }

    #endregion

    // Método para registrar extracción por minero
    public void RegisterGoldExtraction(int amount, bool isPlayer, string minerName)
    {
        if (isPlayer)
            AddPlayerGold(amount);
        else
            AddEnemyGold(amount);

        Debug.Log($"Minero '{minerName}' extrajo {amount} de oro.");
    }

    private void UpdateUI()
    {
        if (playerGoldText != null)
        {
            playerGoldText.text = "Oro: " + playerGold;
        }

        if (enemyGoldText != null)
        {
            enemyGoldText.text = "Oro Enemigo: " + enemyGold;
        }
    }

    void OnGUI()
    {
        if (playerGoldText == null || enemyGoldText == null)
        {
            GUI.Box(new Rect(10, 10, 150, 70), "Recursos");
            GUI.Label(new Rect(20, 30, 140, 20), $"Oro Jugador: {playerGold}");
            GUI.Label(new Rect(20, 50, 140, 20), $"Oro Enemigo: {enemyGold}");
        }
    }
}