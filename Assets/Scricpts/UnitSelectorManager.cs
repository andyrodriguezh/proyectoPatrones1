using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

public class UnitSelectorManager : MonoBehaviour
{
	// PATRÓN: SINGLETON
    // Se usa una propiedad estática Instance para mantener una sola instancia.
    public static UnitSelectorManager Instance { get; set; }
            
    public List<GameObject> allUnitsList=new List<GameObject>();
    public List<GameObject> unitsSelected=new List<GameObject>();
    
    public LayerMask clickable;
    public LayerMask ground;
    public LayerMask attackable;
    public GameObject groundMarker;
    public bool attacCursorVisible;

    private Camera cam;
       // PATRÓN: SINGLETON
    // Garantiza que solo exista una instancia de UnitSelectorManager.
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        cam = Camera.main;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickable))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    multiSelect(hit.collider.gameObject);
                }
                else
                {
                    selectByClicking(hit.collider.gameObject);  
                }
                
            }
            else
            {
                if(!Input.GetKey(KeyCode.LeftShift))
                DeselectAll();
            }
        }
        
        
        if (Input.GetMouseButtonDown(1)&& unitsSelected.Count > 0)
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
            {
              groundMarker.transform.position = hit.point;
              
              groundMarker.SetActive(false);
              groundMarker.SetActive(true);
                
            }
        } 
        
        if (unitsSelected.Count > 0&& AtleastOneOffensiveUnite(unitsSelected))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, attackable))
            {

                attacCursorVisible = true;
                if (Input.GetMouseButtonDown(1))
                {
                    Transform target = hit.transform;

                    foreach (GameObject unit in unitsSelected)
                    {
                        if (unit.GetComponent<AttackController>())
                        {
                            unit.GetComponent<AttackController>().targetToAttack = target;
                        }
                    }
                    
                }
                
            }
        }
        else
        {
            attacCursorVisible = false;  
        }
        
    }

    private bool AtleastOneOffensiveUnite(List<GameObject> unitsSelected)
    {
        foreach (GameObject unit in unitsSelected)
        {
            if (unit.GetComponent<AttackController>())
            {
                return true;
            }
            
        }
        return false;
    }

    private void multiSelect(GameObject unit)
    {
        if (unitsSelected.Contains(unit) == false)
        {
            unitsSelected.Add(unit);
            selectUnit(unit, true);
        }
        else
        {
            selectUnit(unit, false);
            unitsSelected.Remove(unit);
        }
    }

    public void DeselectAll()
    {
        foreach (var unit in unitsSelected)
        {
            selectUnit( unit, false);
        }
        groundMarker.SetActive(false);
        unitsSelected.Clear();
    }

    private void selectByClicking(GameObject unit)
    {
        DeselectAll();
        
        unitsSelected.Add(unit);
        selectUnit(unit, true);
        
    }

    private void EnableUnitMovement(GameObject unit, bool shouldMove)
    {
        unit.GetComponent<UnitMovement>().enabled = shouldMove;
    }

    private void TrigerSlectionIndicartor(GameObject unit, bool isVisible)
    {
        unit.transform.GetChild(0).gameObject.SetActive(isVisible);
    }

    public void DragSelect(GameObject unit)
    {
        if (unitsSelected.Contains(unit) == false)
        {
            unitsSelected.Add(unit);
            selectUnit( unit, true);
        }
    }

    private void selectUnit(GameObject unit, bool isSelected)
    {
        TrigerSlectionIndicartor(unit,isSelected);
        EnableUnitMovement(unit,isSelected);
    }
}
