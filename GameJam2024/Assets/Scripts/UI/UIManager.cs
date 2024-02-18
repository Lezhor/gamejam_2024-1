using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class UIManager : MonoBehaviour
    {

        public bool IsMouseOverUI()
        {
            // TODO - With raycasts
            return EventSystem.current.IsPointerOverGameObject();
        }

    }
}
