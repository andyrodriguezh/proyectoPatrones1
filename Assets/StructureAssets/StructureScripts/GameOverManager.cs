using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance;

    [SerializeField] private GameObject gameOverPanel;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        
        gameOverPanel.SetActive(false);
    }

    public void TriggerGameOver()
    {
        // Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
    }
    

    public void RestartGame()
    {
        Debug.Log("Botón Restart clickeado");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}