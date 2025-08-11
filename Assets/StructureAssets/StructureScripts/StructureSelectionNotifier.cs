using System.Collections.Generic;
using StructureAssets.StructureScripts;
using UnityEngine;
namespace StructureAssets.StructureScripts
{
    public class StructureSelectionNotifier :MonoBehaviour
    {
        private readonly List<IStructureSelectionObserver> observers = new();

        public void RegisterObserver(IStructureSelectionObserver observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);
        }

        public void UnregisterObserver(IStructureSelectionObserver observer)
        {
            observers.Remove(observer);
        }

        public void NotifyStructureSelected(IStructure structure)
        {
            foreach (var observer in observers)
            {
                observer.OnStructureSelected(structure);
            }
        }
        
        public void NotifyClearedSelection()
        {
            NotifyStructureSelected(null);
        }
    }
}