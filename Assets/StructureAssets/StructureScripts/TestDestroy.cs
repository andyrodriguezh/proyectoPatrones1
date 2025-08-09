using UnityEngine;

namespace StructureAssets.StructureScripts
{
    public class TestDestroy : MonoBehaviour
    {
        [SerializeField] private GameObject playerMainBase;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                Destroy(playerMainBase);
                Debug.Log("MainBase destruida manualmente con la tecla K.");
            }
        }
    }
}