using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [Header("Prefabs (Inspector)")]
    public GameObject enemyTypeA;
    public GameObject enemyTypeB;

    [Header("Puntos de aparición (Inspector)")]
    public Transform spawnPoint1;
    public Transform spawnPoint2;

    [Header("Puntos de destino (Inspector)")]
    public Transform targetPoint1;
    public Transform targetPoint2;

    [Header("Reglas")]
    public int requiredEnemyGold = 50;  // oro mínimo para generar
    public bool consumeGold = true;     // restar oro al generar

    [Header("Ejecución")]
    public bool spawnOnStart = true;

    void Update()
    {
        if (spawnOnStart) TrySpawn();
    }

    [ContextMenu("Try Spawn Now")]
    public void TrySpawn()
    {
        // Validaciones
        if (ResourceManager.Instance == null)
        {
            Debug.LogError("EnemySpawner: No hay ResourceManager en la escena.");
            return;
        }
        

        int enemyGold = ResourceManager.Instance.enemyGold;
        if (enemyGold < requiredEnemyGold)
        {
            Debug.Log($"EnemySpawner: Oro enemigo insuficiente ({enemyGold}/{requiredEnemyGold}).");
            return;
        }

        // Instanciar 2 unidades
        var enemy1 = Instantiate(enemyTypeA, spawnPoint1.position, spawnPoint1.rotation);
        var enemy2 = Instantiate(enemyTypeB, spawnPoint2.position, spawnPoint2.rotation);

        // Asignar destinos con NavMeshAgent
        var agent1 = enemy1.GetComponent<NavMeshAgent>();
        var agent2 = enemy2.GetComponent<NavMeshAgent>();

        if (agent1) agent1.SetDestination(targetPoint1.position);
        else Debug.LogError($"{enemy1.name} no tiene NavMeshAgent.");

        if (agent2) agent2.SetDestination(targetPoint2.position);
        else Debug.LogError($"{enemy2.name} no tiene NavMeshAgent.");

        if (consumeGold)
            ResourceManager.Instance.AddEnemyGold(-requiredEnemyGold);

        Debug.Log($"EnemySpawner: Generadas 2 unidades. enemyGold previo={enemyGold}. Destinos asignados.");
    }
}
