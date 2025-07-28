using System;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public Transform targetToAttack;
    
    public Material idleStateMaterial;
    public Material followStateMaterial;
    public Material attackStateMaterial;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && targetToAttack == null)
        {
            targetToAttack= other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") && targetToAttack != null)
        {
            targetToAttack= null;
        }
    }

    public void SetIdlematerial()
    {
        GetComponent<Renderer>().material = idleStateMaterial;
    }
    public void Setfollowmaterial()
    {
        GetComponent<Renderer>().material = followStateMaterial;
    }
    public void SetAttackmaterial()
    {
        GetComponent<Renderer>().material = attackStateMaterial; 
    }


    private void OnDrawGizmos()
    {
        //seguir
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,10f);
        //atacar
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,2.5f);
        //dejar de atacar
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position,3f);
        
    }
}
