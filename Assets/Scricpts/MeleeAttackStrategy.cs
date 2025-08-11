using UnityEngine;

public class MeleeAttackStrategy : IAttackStrategy
{
    private readonly float attackRange;

    public MeleeAttackStrategy(float customRange = 2f)
    {
        attackRange = customRange;
    }

    public void ExecuteAttack(AttackController attacker)
    {
        if (attacker.targetToAttack != null)
        {
            var distance = Vector3.Distance(attacker.transform.position, attacker.targetToAttack.position);
            if (distance <= attackRange)
            {
                attacker.targetToAttack.GetComponent<Unit>().TakeDamage(attacker.unitDamage);
                Debug.Log($"{attacker.name} realizó un ataque MELEE a {attacker.targetToAttack.name}");
            }
        }
    }

    public float GetAttackRange() => attackRange;
}