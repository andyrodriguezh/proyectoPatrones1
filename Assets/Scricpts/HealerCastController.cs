using UnityEngine;

public class HealerCastController : MonoBehaviour
{
    public float castRange = 5f;
    public int healAmount = 20;
    public KeyCode castKey = KeyCode.Q;
    public string allyTag = "Player"; // Puedes cambiar a Enemy si es un healer enemigo

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetKeyDown(castKey))
        {
            AttemptHeal();
        }
    }

    void AttemptHeal()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            if (hit.collider.CompareTag(allyTag))
            {
                Unit unit = hit.collider.GetComponent<Unit>();
                if (unit != null && unit.GetHealthPercentage() < 1.0f)
                {
                    float distance = Vector3.Distance(transform.position, unit.transform.position);
                    if (distance <= castRange)
                    {
                        unit.ReceiveHealing(healAmount);
                        Debug.Log($"{name} lanzó curación a {unit.name} por {healAmount} HP");
                    }
                    else
                    {
                        Debug.Log("Objetivo fuera de rango.");
                    }
                }
                else
                {
                    Debug.Log("Unidad no necesita curación.");
                }
            }
            else
            {
                Debug.Log("El objetivo no es aliado.");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, castRange);
    }
}