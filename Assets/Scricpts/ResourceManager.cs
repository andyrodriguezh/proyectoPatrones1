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
    public Text playerGoldText;
    
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
    }
    
    private void Start()
    {
        UpdateUI();
    }
    
    public void AddPlayerGold(int amount)
    {
        playerGold += amount;
        UpdateUI();
    }
    
    public void AddEnemyGold(int amount)
    {
        enemyGold += amount;
    }
    
    public bool SpendPlayerGold(int amount)
    {
        if (playerGold >= amount)
        {
            playerGold -= amount;
            UpdateUI();
            return true;
        }
        return false;
    }
    
    private void UpdateUI()
    {
        if (playerGoldText != null)
        {
            playerGoldText.text = "Oro: " + playerGold;
        }
    }
}