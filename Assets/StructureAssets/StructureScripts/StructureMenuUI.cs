using UnityEngine;
using UnityEngine.UI;

namespace StructureAssets.StructureScripts
{
    public class StructureMenuUI : MonoBehaviour
    {
        [SerializeField] private GameObject menuPanel;
        [SerializeField] private Button closeButton;

        private StructureSelectionNotifier _notifier;
        
        private void Awake()
        {
            if (menuPanel != null) menuPanel.SetActive(false);
        }
        
        private void Start()
        {
            _notifier = FindFirstObjectByType<StructureSelectionNotifier>();
            closeButton.onClick.AddListener(CloseAndClear);
        }
        
        private void CloseAndClear()
        {
            menuPanel.SetActive(false);
            _notifier?.NotifyClearedSelection();
        }

        public void ShowPanel()
        {
            menuPanel.SetActive(true);
        }

        public void HidePanel()
        {
            menuPanel.SetActive(false);
        }
    }
}