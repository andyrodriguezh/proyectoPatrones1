using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoCamara : MonoBehaviour
{
    public Transform camaraPrincipal;
    public float velocidadMovimiento;

    public int borde = 5;

    public float zoomSpeed = 3f;
    public float maxZoom = 5f;
    public float minZoom = 1f;

    Vector3 movimiento;
    Vector2 bordePantalla;
    Vector2 centroMedio;
    Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        centroMedio = new Vector2(Screen.width / 2, Screen.height / 2);
        bordePantalla = new Vector2(Screen.width - borde, Screen.height - borde);

    }

    void Update()
    {
        if (Input.mousePosition.x < borde || Input.mousePosition.x > bordePantalla.x || Input.mousePosition.y < borde ||
            Input.mousePosition.y > bordePantalla.y)
        {
            movimiento=(Vector2)Input.mousePosition-centroMedio;
            movimiento.z = movimiento.y;
            movimiento.Normalize();
            camaraPrincipal.Translate(new Vector3(movimiento.x,0f,movimiento.y)*velocidadMovimiento*Time.deltaTime,0);
        }
        var zoom=Input.GetAxis("Mouse ScrollWheel");
        cam.orthographicSize-=zoom*zoomSpeed;
        cam.orthographicSize=Mathf.Clamp(cam.orthographicSize,minZoom,maxZoom);
    }

}
