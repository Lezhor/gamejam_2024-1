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
                Instantiate(goalArrow, tile.Center, Quaternion.identity);
            }
        }
    }
}
