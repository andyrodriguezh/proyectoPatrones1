using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class BuildRequirement
{
    public string resource;
    public int amount;
}

// Interfaz base para los comandos de recursos
public interface IResourceCommand
{
    bool Ejecutar();
    bool Deshacer();
}

// Comando para añadir oro al jugador
public class AgregarOroJugador : IResourceCommand
{
    private readonly int _cantidad;
    private readonly ResourceManager _manager;

    public AgregarOroJugador(ResourceManager manager, int cantidad)
    {
        _manager = manager;
        _cantidad = cantidad;
    }

    public bool Ejecutar()
    {
        _manager.AgregarOroJugadorDirecto(_cantidad);
        return true;
    }

    public bool Deshacer()
    {
        return _manager.GastarOroJugadorDirecto(_cantidad);
    }
}

// Comando para gastar oro del jugador
public class GastarOroJugador : IResourceCommand
{
    private readonly int _cantidad;
    private readonly ResourceManager _manager;

    public GastarOroJugador(ResourceManager manager, int cantidad)
    {
        _manager = manager;
        _cantidad = cantidad;
    }

    public bool Ejecutar()
    {
        return _manager.GastarOroJugadorDirecto(_cantidad);
    }

    public bool Deshacer()
    {
        _manager.AgregarOroJugadorDirecto(_cantidad);
        return true;
    }
}

// Comando para añadir oro al enemigo
public class AgregarOroEnemigo : IResourceCommand
{
    private readonly int _cantidad;
    private readonly ResourceManager _manager;

    public AgregarOroEnemigo(ResourceManager manager, int cantidad)
    {
        _manager = manager;
        _cantidad = cantidad;
    }

    public bool Ejecutar()
    {
        _manager.AgregarOroEnemigoDirecto(_cantidad);
        return true;
    }

    public bool Deshacer()
    {
        return _manager.GastarOroEnemigoDirecto(_cantidad);
    }
}

// Historial de comandos 
public class HistorialRecursos
{
    private Stack<IResourceCommand> _comandos = new Stack<IResourceCommand>();

    public bool Ejecutar(IResourceCommand comando)
    {
        bool resultado = comando.Ejecutar();
        if (resultado)
        {
            _comandos.Push(comando);
        }
        return resultado;
    }

    public bool Deshacer()
    {
        if (_comandos.Count > 0)
        {
            var ultimoComando = _comandos.Pop();
            return ultimoComando.Deshacer();
        }
        return false;
    }
}

public class ResourceManager : MonoBehaviour
{
    #region Singleton

    private static ResourceManager _instance;

    public static ResourceManager Instance
    {
        get { return _instance; }
        private set { _instance = value; }
    }

    #endregion

    #region Campos y propiedades


    [Header("Recursos del Jugador")]
    [SerializeField] private int _playerGold = 0;
    public int playerGold { get { return _playerGold; } private set { _playerGold = value; } }

    [Header("Recursos del Enemigo")]
    [SerializeField] private int _enemyGold = 0;
    public int enemyGold { get { return _enemyGold; } private set { _enemyGold = value; } }

    // Diccionario para gestionar múltiples tipos de recursos
    private Dictionary<string, int> _resources = new Dictionary<string, int>();

    [Header("UI")]
    [SerializeField] private string playerGoldTextName = "PlayerGoldText";
    [SerializeField] private string enemyGoldTextName = "EnemyGoldText";

    private Text _playerGoldText;
    private Text _enemyGoldText;

    // Historial de comandos
    private HistorialRecursos _historial = new HistorialRecursos();

    #endregion

    #region Métodos Unity Lifecycle

    private void Awake()
    {
        InitializeSingleton();
        InitializeResources();
        FindUIReferences();
    }

    private void Start()
    {
        UpdateUI();
    }

    #endregion

    #region Métodos de inicialización

    private void InitializeSingleton()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void InitializeResources()
    {
        _resources["Gold"] = _playerGold;
    }

    private void FindUIReferences()
    {
        GameObject playerTextObj = GameObject.Find(playerGoldTextName);
        if (playerTextObj != null)
            _playerGoldText = playerTextObj.GetComponent<Text>();

        GameObject enemyTextObj = GameObject.Find(enemyGoldTextName);
        if (enemyTextObj != null)
            _enemyGoldText = enemyTextObj.GetComponent<Text>();

        if (_playerGoldText == null || _enemyGoldText == null)
            Debug.LogWarning("ResourceManager: No se pudieron encontrar elementos de la UI");
    }

    #endregion

    #region Métodos públicos de interfaz

 
    public void AddPlayerGold(int amount)
    {
        IResourceCommand comando = new AgregarOroJugador(this, amount);
        _historial.Ejecutar(comando);
    }

    public void AddEnemyGold(int amount)
    {
        IResourceCommand comando = new AgregarOroEnemigo(this, amount);
        _historial.Ejecutar(comando);
    }

    public bool SpendPlayerGold(int amount)
    {
        IResourceCommand comando = new GastarOroJugador(this, amount);
        return _historial.Ejecutar(comando);
    }

    // Método para deshacer el último comando 
    public bool UndoLastOperation()
    {
        return _historial.Deshacer();
    }

    #endregion

    #region Métodos directos (para uso interno por los comandos)

   
    internal void AgregarOroJugadorDirecto(int amount)
    {
        _playerGold += amount;
        _resources["Gold"] = _playerGold; 
        UpdateUI();
    }

    internal bool GastarOroJugadorDirecto(int amount)
    {
        if (_playerGold >= amount)
        {
            _playerGold -= amount;
            _resources["Gold"] = _playerGold; 
            Debug.Log($"Oro del jugador gastado: -{amount} → Restante: {_playerGold}");
            UpdateUI();
            return true;
        }
        Debug.Log($"Oro insuficiente: Necesitas {amount}, tienes {_playerGold}");
        return false;
    }

    internal void AgregarOroEnemigoDirecto(int amount)
    {
        _enemyGold += amount;
        Debug.Log($"Oro del enemigo actualizado: +{amount} → Total: {_enemyGold}");
        UpdateUI();
    }

    internal bool GastarOroEnemigoDirecto(int amount)
    {
        if (_enemyGold >= amount)
        {
            _enemyGold -= amount;
            UpdateUI();
            return true;
        }
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
                if (_playerGold < req.amount)
                    return false;
            }
            else if (!_resources.ContainsKey(req.resource) || _resources[req.resource] < req.amount)
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
            else if (_resources.ContainsKey(req.resource))
            {
                _resources[req.resource] -= req.amount;
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

        if (!_resources.ContainsKey(resource))
            _resources[resource] = 0;

        _resources[resource] += amount;
    }

    // Imprime los recursos actuales en consola
    public void PrintResources()
    {
        Debug.Log($"Gold (Player): {_playerGold}");
        Debug.Log($"Gold (Enemy): {_enemyGold}");

        foreach (var res in _resources)
        {
            if (res.Key != "Gold") // Ya mostramos el oro arriba
                Debug.Log($"{res.Key}: {res.Value}");
        }
    }

    #endregion

    #region Métodos específicos de juego

    // Método para registrar extracción por minero
    public void RegisterGoldExtraction(int amount, bool isPlayer, string minerName)
    {
        if (isPlayer)
            AddPlayerGold(amount);
        else
            AddEnemyGold(amount);

        Debug.Log($"Minero '{minerName}' extrajo {amount} de oro.");
    }

    #endregion

    #region Métodos UI

    private void UpdateUI()
    {
        if (_playerGoldText != null)
        {
            _playerGoldText.text = "Oro: " + _playerGold;
        }

        if (_enemyGoldText != null)
        {
            _enemyGoldText.text = "Oro Enemigo: " + _enemyGold;
        }
    }

    void OnGUI()
    {
        if (_playerGoldText == null || _enemyGoldText == null)
        {
            GUI.Box(new Rect(10, 10, 150, 70), "Recursos");
            GUI.Label(new Rect(20, 30, 140, 20), $"Oro Jugador: {_playerGold}");
            GUI.Label(new Rect(20, 50, 140, 20), $"Oro Enemigo: {_enemyGold}");
        }
    }

    #endregion
}