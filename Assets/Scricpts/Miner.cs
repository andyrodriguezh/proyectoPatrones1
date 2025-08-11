using UnityEngine;
using System.Collections;

// Interfaz base para los estados del minero
public interface IMinerState
{
    void Enter(Miner miner);
    void Update(Miner miner);
    void Exit(Miner miner);
}

// Estado base del minero
public class MinerIdleState : IMinerState
{
    public void Enter(Miner miner)
    {
        miner.GetComponent<AttackController>().SetIdlematerial();
        Debug.Log($"Minero {miner.gameObject.name}: Entrando en estado Idle");
    }

    public void Update(Miner miner)
    {
        if (miner.targetMine != null)
        {
            float distanceToMine = Vector3.Distance(miner.transform.position, miner.targetMine.transform.position);
            Debug.Log($"Distancia a la mina: {distanceToMine}, Radio de minería: {miner.miningRadius}");
            
            if (distanceToMine <= miner.miningRadius)
            {
                Debug.Log($"Minero {miner.gameObject.name} dentro del radio de minería. Cambiando a estado Mining.");
                miner.ChangeState(new MinerMiningState());
            }
            else 
            {
                // Si tiene una mina asignada pero está lejos, podría moverse hacia ella
                // (implementación futura)
            }
        }
        else
        {
            // Buscar mina cercana si no tiene una asignada
            miner.FindNearestMine();
        }
    }

    public void Exit(Miner miner)
    {
        Debug.Log($"Minero {miner.gameObject.name}: Saliendo de estado Idle");
    }
}

// Estado Mining del minero
public class MinerMiningState : IMinerState
{
    private float miningTimer = 0f;

    public void Enter(Miner miner)
    {
        miner.GetComponent<AttackController>().SetAttackmaterial();
        miningTimer = 0f;
        Debug.Log($"Minero {miner.gameObject.name}: Entrando en estado Mining");
    }

    public void Update(Miner miner)
    {
        if (miner.targetMine == null)
        {
            Debug.Log("Mina objetivo nula. Volviendo a Idle.");
            miner.ChangeState(new MinerIdleState());
            return;
        }

        float distanceToMine = Vector3.Distance(miner.transform.position, miner.targetMine.transform.position);
        if (distanceToMine > miner.miningRadius)
        {
            Debug.Log($"Minero {miner.gameObject.name} se alejó de la mina. Distancia: {distanceToMine}");
            miner.ChangeState(new MinerIdleState());
            return;
        }

        miningTimer += Time.deltaTime;
        if (miningTimer >= miner.miningRate)
        {
            miningTimer = 0f;
            miner.ExtractGold();
            Debug.Log($"Extracción de oro completada. Próxima extracción en {miner.miningRate} segundos.");
        }
    }

    public void Exit(Miner miner)
    {
        Debug.Log($"Minero {miner.gameObject.name}: Saliendo de estado Mining");
    }
}

// Implementación de la clase Miner que utilizará los estados
public class Miner : MonoBehaviour, IGoldMineObserver
{
    [Header("Configuración de Minería")]
    public float miningRadius = 5f;
    public float miningRate = 2f;

    [Header("Equipo")]
    public bool isPlayer = true;

    [Header("Referencias")]
    public GoldMine targetMine;

    private IMinerState currentState;

    void Start()
    {
        // Inicio en estado Idle
        ChangeState(new MinerIdleState());
        
        // Buscar mina si no está asignada
        if (targetMine == null)
        {
            FindNearestMine();
        }
        else
        {
            // Registrar como observador de la mina
            targetMine.AddObserver(this);
        }
    }

    void Update()
    {
        if (currentState != null)
        {
            currentState.Update(this);
        }
    }

    public void ChangeState(IMinerState newState)
    {
        if (currentState != null)
        {
            currentState.Exit(this);
        }

        currentState = newState;

        if (currentState != null)
        {
            currentState.Enter(this);
        }
    }

    public void ExtractGold()
    {
        if (targetMine == null)
            return;

        if (targetMine.CanExtractGold())
        {
            int extractedGold = targetMine.ExtractGold();

            if (ResourceManager.Instance != null)
            {
                ResourceManager.Instance.RegisterGoldExtraction(extractedGold, isPlayer, gameObject.name);
            }
        }
        else
        {
            Debug.Log($"Minero {gameObject.name}: La mina no tiene suficiente oro para extraer.");
        }
    }

    // Método para buscar la mina más cercana
    public void FindNearestMine()
    {
        GoldMine[] mines = FindObjectsOfType<GoldMine>();
        float closestDistance = float.MaxValue;
        GoldMine closestMine = null;

        foreach (var mine in mines)
        {
            float distance = Vector3.Distance(transform.position, mine.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestMine = mine;
            }
        }

        if (closestMine != null)
        {
            // Si teníamos una mina anterior, nos desregistramos como observador
            if (targetMine != null)
            {
                targetMine.RemoveObserver(this);
            }
            
            targetMine = closestMine;
            targetMine.AddObserver(this);
            Debug.Log($"Minero {gameObject.name}: Asignada mina más cercana a {closestDistance} unidades.");
        }
        else
        {
            Debug.LogWarning($"Minero {gameObject.name}: No se encontraron minas en la escena.");
        }
    }

    // Implementación de IGoldMineObserver
    public void OnGoldChanged(GoldMine mine, int currentGold, int maxGold)
    {
        // Si la mina se queda sin oro, podríamos buscar otra
        if (currentGold <= 0)
        {
            Debug.Log($"Minero {gameObject.name}: La mina se ha agotado, buscando otra.");
            targetMine = null;
            FindNearestMine();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, miningRadius);
    }
}