public interface IAttackStrategy
{
    void ExecuteAttack(AttackController attacker);
    float GetAttackRange();
}