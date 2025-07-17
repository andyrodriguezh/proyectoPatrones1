using UnityEngine;

public class CuboInteractivo : MonoBehaviour {
    public EsferaFactory factory;

    void OnMouseDown() {
        Vector3 posicionEsfera = transform.position + Vector3.up * 2; // posición encima del cubo
        ICommand comando = new GenerarEsferaCommand(posicionEsfera, factory);
        comando.Ejecutar();
    }
}