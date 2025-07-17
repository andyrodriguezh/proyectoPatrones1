using UnityEngine;

public interface ICommand {
    void Ejecutar();
}

public class GenerarEsferaCommand : ICommand {
    private Vector3 posicion;
    private EsferaFactory factory;

    public GenerarEsferaCommand(Vector3 posicion, EsferaFactory factory) {
        this.posicion = posicion;
        this.factory = factory;
    }

    public void Ejecutar() {
        factory.CrearEsfera(posicion);
    }
}
