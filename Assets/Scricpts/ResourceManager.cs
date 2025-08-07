using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    [Header("Recursos del Jugador")]
    public int playerGold = 0;

    [Header("Recursos del Enemigo")]
    public int enemyGold = 0;

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
        }
        else
        {
            Instance = this;
        }
        
        // Buscar las referencias por nombre
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

    public void AddPlayerGold(int amount)
    {
        playerGold += amount;
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
            Debug.Log($"Oro del jugador gastado: -{amount} → Restante: {playerGold}");
            UpdateUI();
            return true;
        }
        Debug.Log($"Oro insuficiente: Necesitas {amount}, tienes {playerGold}");
        return false;
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

    // Para depuración 
    void OnGUI()
    {
        // Solo mostrar si no tenemos UI configurada
        if (playerGoldText == null || enemyGoldText == null)
        {
            GUI.Box(new Rect(10, 10, 150, 70), "Recursos");
            GUI.Label(new Rect(20, 30, 140, 20), $"Oro Jugador: {playerGold}");
            GUI.Label(new Rect(20, 50, 140, 20), $"Oro Enemigo: {enemyGold}");
        }
    }
}