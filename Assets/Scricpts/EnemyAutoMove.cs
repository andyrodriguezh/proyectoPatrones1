using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAutoMove : MonoBehaviour
{
    public Transform puntoDestino; // Asignar en el inspector

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (puntoDestino != null)
        {
            agent.SetDestination(puntoDestino.position);
        }
        else
        {
            Debug.LogWarning($"{name} no tiene punto de destino asignado.");
        }
    }
}