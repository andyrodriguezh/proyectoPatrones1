using UnityEngine;
using UnityEngine.AI;

public class UnitFollowState : StateMachineBehaviour
{
    private AttackController attackController;
    private NavMeshAgent agent;

    // Al entrar al estado, obtener referencias necesarias
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackController = animator.transform.GetComponent<AttackController>();
        agent = animator.transform.GetComponent<NavMeshAgent>();
       
    }

    // Lógica de seguimiento al objetivo
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        if (attackController.targetToAttack == null || 
            !attackController.targetToAttack.gameObject.activeInHierarchy)
        {
            animator.SetBool("isFollowing", false);
            attackController.targetToAttack = null;
            return;
        }

        if (!animator.transform.GetComponent<UnitMovement>().isCommandedToMove)
        {
            float attackRange = attackController.GetAttackRange();
            float distanceToTarget = Vector3.Distance(
                attackController.targetToAttack.position,
                animator.transform.position
            );

            // Si aún no está en rango, continuar siguiendo
            if (distanceToTarget > attackRange)
            {
                if (agent.isOnNavMesh)
                    agent.SetDestination(attackController.targetToAttack.position);
            }
            // Si está en rango, detenerse y atacar (o curar si es healer)
            else
            {
                if (agent.isOnNavMesh)
                    agent.SetDestination(animator.transform.position);

                animator.SetBool("isAttacking", true);
            }

            animator.transform.LookAt(attackController.targetToAttack);
        }
    }
}
