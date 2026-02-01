using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace DefaultNamespace
{
    [ExecuteAlways]
    public class EditorUIHider : MonoBehaviour
    {
        public List<GameObject> objectsToToggle;

        void Update()
        {
            if (!Application.isPlaying )
            {
                // Editor mode → disabled
                SetActiveAll(false);
            }
        }

        void SetActiveAll(bool state)
        {
            if (objectsToToggle == null) return;

            foreach (var obj in objectsToToggle)
            {
                if (obj != null)
                {
                    if (obj.TryGetComponent<UIDocument>(out var uiDocument))
                    {
                        uiDocument.rootVisualElement.visible = state;
                    }
                }
            }
        }
    }
}