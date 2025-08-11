using UnityEngine;
using UnityEngine.AI;

public class UnitAttackState : StateMachineBehaviour
{
    NavMeshAgent agent;
    AttackController attackController;

    public float attackRate = 2f;
    private float attackTimer;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        attackController = animator.GetComponent<AttackController>();
        
        attackTimer = 0f;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Si ya no hay objetivo o fue destruido
        if (!attackController.IsHealer() && 
            (attackController.targetToAttack == null || 
             !attackController.targetToAttack.gameObject.activeInHierarchy))
        {
            animator.SetBool("isAttacking", false);
            attackController.targetToAttack = null;
            return;
        }


        if (!animator.transform.GetComponent<UnitMovement>().isCommandedToMove)
        {
            float attackRange = attackController.GetAttackRange();
            float distanceForTarget = Vector3.Distance(
                attackController.targetToAttack.position,
                animator.transform.position);

            // Evitar errores de movimiento si el agente fue destruido
            if (agent != null && agent.isOnNavMesh)
            {
                LookAtTarget();
                agent.SetDestination(animator.transform.position);
            }

            if (attackTimer <= 0)
            {
                attackController.PerformAttack();
                attackTimer = 1f / attackRate;
            }
            else
            {
                attackTimer -= Time.deltaTime;
            }

            if (distanceForTarget > attackRange)
            {
                animator.SetBool("isAttacking", false);
                attackController.targetToAttack = null;
            }
        }
    }

    private void LookAtTarget()
    {
        if (attackController.targetToAttack == null) return;

        Vector3 direction = attackController.targetToAttack.position - agent.transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        agent.transform.rotation = Quaternion.Euler(0f, lookRotation.eulerAngles.y, 0f);
    }
}
