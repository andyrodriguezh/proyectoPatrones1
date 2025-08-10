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
        Debug.Log($"Minero {miner.gameObject.name}: Entrando en estado Idle");
    }

    public void Update(Miner miner)
    {
        if (miner.targetMine != null)
        {
            float distanceToMine = Vector3.Distance(miner.transform.position, miner.targetMine.transform.position);
            if (distanceToMine <= miner.miningRadius)
            {
                miner.ChangeState(new MinerMiningState());
            }
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
        miningTimer = 0f;
        Debug.Log($"Minero {miner.gameObject.name}: Entrando en estado Mining");
    }

    public void Update(Miner miner)
    {
        if (miner.targetMine == null)
        {
            miner.ChangeState(new MinerIdleState());
            return;
        }

        float distanceToMine = Vector3.Distance(miner.transform.position, miner.targetMine.transform.position);
        if (distanceToMine > miner.miningRadius)
        {
            miner.ChangeState(new MinerIdleState());
            return;
        }

        miningTimer += Time.deltaTime;
        if (miningTimer >= miner.miningRate)
        {
            miningTimer = 0f;
            miner.ExtractGold();
        }
    }

    public void Exit(Miner miner)
    {
        Debug.Log($"Minero {miner.gameObject.name}: Saliendo de estado Mining");
    }
}

// Implementación de la clase Miner que utilizará los estados
public class Miner : MonoBehaviour
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
                if (isPlayer)
                {
                    ResourceManager.Instance.AddPlayerGold(extractedGold);
                }
                else
                {
                    ResourceManager.Instance.AddEnemyGold(extractedGold);
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, miningRadius);
    }
}