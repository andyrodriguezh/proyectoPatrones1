using UnityEngine;

public class GoldMineTest : MonoBehaviour
{
    public GoldMine targetMine;
    public KeyCode extractKey = KeyCode.E;
    
    private void Update()
    {
        if (Input.GetKeyDown(extractKey) && targetMine != null)
        {
            if (targetMine.CanExtractGold())
            {
                int goldExtracted = targetMine.ExtractGold();
                Debug.Log($"Oro extraído: {goldExtracted}. Oro restante: {targetMine.currentGold}");
            }
            else
            {
                Debug.Log("No hay suficiente oro para extraer");
            }
        }
    }
}