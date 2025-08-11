using UnityEngine;

public class AttackController : MonoBehaviour
{
    public Transform targetToAttack;
    public Material idleStateMaterial;
    public Material followStateMaterial;
    public Material attackStateMaterial;

    public bool isPlayer;
    public int unitDamage;
    [SerializeField] private bool isHealerUnit;
    [SerializeField] private int healAmount = 10;

    [SerializeField] private bool isRangedUnit;
    [SerializeField] private float customAttackRange = 2f;

    private IAttackStrategy attackStrategy;

    private void Start()
    {
        if (isHealerUnit)
            attackStrategy = new HealStrategy(customAttackRange, healAmount);
        else if (isRangedUnit)
            attackStrategy = new RangedAttackStrategy(customAttackRange);
        else
            attackStrategy = new MeleeAttackStrategy(customAttackRange);
    }

    public void PerformAttack()
    {
        if (targetToAttack == null || !targetToAttack.gameObject.activeInHierarchy)
            return;

        attackStrategy?.ExecuteAttack(this);
    }

    public float GetAttackRange()
    {
        return attackStrategy != null ? attackStrategy.GetAttackRange() : 2.0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (targetToAttack != null) return;

        if (isHealerUnit)
        {
            bool sameTeam = (isPlayer && other.CompareTag("Player")) || (!isPlayer && other.CompareTag("Enemy"));
            if (sameTeam)
            {
                Unit unit = other.GetComponent<Unit>();
                if (unit != null && unit.GetHealthPercentage() < 1.0f)
                {
                    targetToAttack = other.transform;
                    Debug.Log($"{name} detectó aliado herido: {other.name}");
                }
            }
        }
        else if (isPlayer && other.CompareTag("Enemy"))
        {
            targetToAttack = other.transform;
        }
        else if (!isPlayer && other.CompareTag("Player"))
        {
            targetToAttack = other.transform;
        }
    }

    public bool IsHealer()
    {
        return isHealerUnit;
    }

    private void OnTriggerExit(Collider other)
    {
        if (targetToAttack != null &&
            ((isPlayer && other.CompareTag("Enemy")) || (!isPlayer && other.CompareTag("Player"))))
        {
            targetToAttack = null;
        }
    }

    // Métodos para cambiar el material según el estado
    public void SetIdlematerial()
    {
        var renderer = GetComponent<Renderer>();
        if (renderer != null && idleStateMaterial != null)
            renderer.material = idleStateMaterial;
    }

    public void Setfollowmaterial()
    {
        var renderer = GetComponent<Renderer>();
        if (renderer != null && followStateMaterial != null)
            renderer.material = followStateMaterial;
    }

    public void SetAttackmaterial()
    {
        var renderer = GetComponent<Renderer>();
        if (renderer != null && attackStateMaterial != null)
            renderer.material = attackStateMaterial;
    }
}
