using UnityEngine;
using UnityEngine.AI; 

public class UnitAttackState : StateMachineBehaviour
{
    NavMeshAgent agent; 
    AttackController attackController;



    public float stopAttackingDistance = 2f;

    public float attackRate=2f;
    private float attckTimer;
    

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        attackController= animator.GetComponent<AttackController>();
        attackController.SetAttackmaterial();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (attackController.targetToAttack!=null&&animator.transform.GetComponent<UnitMovement>().isCommandedToMove == false)
        {
            lockAtTarget();
            agent.SetDestination(attackController.targetToAttack.position);

            if (attckTimer <= 0)
            {
                Attack();
                attckTimer = 1f / attackRate;
            }
            else
            {
                attckTimer -= Time.deltaTime;
            }
            
            float distanceForTarget=Vector3.Distance(attackController.targetToAttack.position,animator.transform.position);
            if (distanceForTarget > stopAttackingDistance|| attackController.targetToAttack==null)
            {
                
                animator.SetBool("isAttacking",false);
            } 
                
            
        }
    }
	private void Attack()
    {
        var damageToInflict = attackController.unitDamage;

        attackController.targetToAttack.GetComponent<Unit>().TakeDamage(damageToInflict);

    }
    private void lockAtTarget()
    {
        Vector3 direction=attackController.targetToAttack.position-agent.transform.position;
        agent.transform.rotation=Quaternion.LookRotation(direction);
        
        var yRotation=agent.transform.rotation.eulerAngles.y;
        agent.transform.rotation=Quaternion.Euler(0,yRotation,0);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}

