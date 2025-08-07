using UnityEngine;

public class HealStrategy : IAttackStrategy
{
    private readonly float healRange;
    private readonly int healAmount;

    public HealStrategy(float range = 5f, int amount = 10)
    {
        healRange = range;
        healAmount = amount;
    }

    public void ExecuteAttack(AttackController attacker)
    {
        if (attacker.targetToAttack == null) return;

        float distance = Vector3.Distance(attacker.transform.position, attacker.targetToAttack.position);
        if (distance <= healRange)
        {
            Unit unit = attacker.targetToAttack.GetComponent<Unit>();
            if (unit != null && unit.GetHealthPercentage() < 1.0f)
            {
                unit.ReceiveHealing(healAmount);
                Debug.Log($"{attacker.name} curó a {unit.name} en {healAmount} puntos de vida");
            }
        }
    }

    public float GetAttackRange() => healRange;
}

