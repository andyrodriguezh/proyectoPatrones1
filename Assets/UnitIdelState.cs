using UnityEngine;

public class UnitIdelState : StateMachineBehaviour
{
    private AttackController attackController;
    
    
    // PATRÓN: STATE
    // Este bloque implementa el estado concreto "Idle" del patrón State.
    // Se ejecuta al entrar al estado y configura el material de la unidad.
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackController = animator.transform.GetComponent<AttackController>();
        
    }

    // PATRÓN: STATE
    // Este bloque mantiene el comportamiento del estado Idle,
    // revisando constantemente si hay un objetivo para cambiar a Follow.
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (attackController.targetToAttack != null)
        {
            animator.SetBool("isFollowing",true);
        }
    }
    
}