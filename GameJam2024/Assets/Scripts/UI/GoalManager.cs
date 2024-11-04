using UnityEngine;

namespace UI
{
    public class GoalManager : MonoBehaviour
    {
    
        public bool instantiatingEnabled = true;
        public GameObject goalCircle;
        public GameObject goalArrow;

        public void InstantiateForTile(WorldTile tile)
        {
            if (instantiatingEnabled)
            {
                Instantiate(goalCircle, tile.Center, Quaternion.identity);
                if (Camera.main != null)
                {
                    Instantiate(goalArrow, tile.Center, Quaternion.identity, Camera.main.transform);
                }
                else
                {
                    Instantiate(goalArrow, tile.Center, Quaternion.identity);
                }
            }
        }
    }
}
