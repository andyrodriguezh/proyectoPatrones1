using UnityEngine;

public class SimpleEsferaFactory : EsferaFactory {
    public GameObject esferaPrefab;

    public override GameObject CrearEsfera(Vector3 posicion) {
        return Instantiate(esferaPrefab, posicion, Quaternion.identity);
    }
}
