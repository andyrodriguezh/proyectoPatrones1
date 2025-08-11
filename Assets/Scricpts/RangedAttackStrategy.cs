using UnityEngine;

public class RangedAttackStrategy : IAttackStrategy
{
    private readonly float attackRange;

    public RangedAttackStrategy(float customRange = 5f)
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
                Debug.Log($"{attacker.name} realizó un ataque RANGO a {attacker.targetToAttack.name}");
            }
        }
    }

    public float GetAttackRange() => attackRange;
}