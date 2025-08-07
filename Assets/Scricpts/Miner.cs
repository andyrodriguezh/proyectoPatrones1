using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Miner : MonoBehaviour
{
    [Header("Configuración de Minería")]
    public float miningRadius = 5f; // Incrementado para mayor tolerancia
    public float miningRate = 2f;

    [Header("Equipo")]
    public bool isPlayer = true;

    [Header("Referencias")]
    public GoldMine targetMine;

    // Estados del minero
    public enum MinerState { Idle, Mining }
    public MinerState currentState = MinerState.Idle;

    // Componentes
    private AttackController attackController;

    // Variables de control
    private float miningTimer = 0f;

    void Start()
    {
        attackController = GetComponent<AttackController>();
        attackController.SetIdlematerial();
    }

    void Update()
    {
        // Si no hay mina asignada, no hacemos nada
        if (targetMine == null)
        {
            if (currentState != MinerState.Idle)
            {
                SetState(MinerState.Idle);
            }
            return;
        }

        // Calcular distancia a la mina
        float distanceToMine = Vector3.Distance(transform.position, targetMine.transform.position);
        
        // Actualizar estado según la distancia a la mina
        if (distanceToMine <= miningRadius)
        {
            // Estamos lo suficientemente cerca para minar
            if (currentState != MinerState.Mining)
            {
                SetState(MinerState.Mining);
            }
            
            // Si estamos en estado de minería, extraer oro cada cierto tiempo
            if (currentState == MinerState.Mining)
            {
                miningTimer += Time.deltaTime;
                if (miningTimer >= miningRate)
                {
                    miningTimer = 0f;
                    ExtractGold();
                }
            }
        }
        else
        {
            // Estamos demasiado lejos para minar
            if (currentState != MinerState.Idle)
            {
                SetState(MinerState.Idle);
            }
        }
    }

    private void SetState(MinerState newState)
    {
        if (currentState == newState)
            return;
            
        Debug.Log($"Minero {gameObject.name}: Cambiando estado de {currentState} a {newState}");
        
        currentState = newState;
        
        switch (newState)
        {
            case MinerState.Idle:
                attackController.SetIdlematerial();
                miningTimer = 0f;
                break;
                
            case MinerState.Mining:
                attackController.SetAttackmaterial();
                miningTimer = 0f;
                break;
        }
    }

    private void ExtractGold()
    {
        if (targetMine == null)
            return;

        Debug.Log($"Minero: Intentando extraer oro de {targetMine.name}");

        if (targetMine.CanExtractGold())
        {
            int extractedGold = targetMine.ExtractGold();
            Debug.Log($"Minero: ¡Extracción exitosa! Obtuvo {extractedGold} unidades de oro");

            if (ResourceManager.Instance != null)
            {
                if (isPlayer)
                {
                    ResourceManager.Instance.AddPlayerGold(extractedGold);
                    Debug.Log($"Oro del jugador: {ResourceManager.Instance.playerGold}");
                }
                else
                {
                    ResourceManager.Instance.AddEnemyGold(extractedGold);
                }
            }
        }
        else
        {
            Debug.Log("Minero: La mina no tiene suficiente oro para extraer");
        }
    }
    

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, miningRadius);
    }
}