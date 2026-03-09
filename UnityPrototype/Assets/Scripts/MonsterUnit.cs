using UnityEngine;

namespace FinalProject.HexStrategy
{
    /// <summary>
    /// Runtime data/behavior for a summoned monster unit.
    /// </summary>
    public class MonsterUnit : MonoBehaviour
    {
        [SerializeField] private int movementRange = 1;

        /// <summary>
        /// Maximum move distance in hexes (part 1 expects 1).
        /// </summary>
        public int MovementRange => movementRange;

        /// <summary>
        /// Tile the monster is currently occupying.
        /// </summary>
        public HexTile CurrentTile { get; private set; }

        /// <summary>
        /// Places this monster on a tile and offsets its render height slightly.
        /// </summary>
        public void PlaceOnTile(HexTile tile)
        {
            CurrentTile = tile;
            transform.position = tile.transform.position + new Vector3(0f, 0.2f, 0f);
        }

        /// <summary>
        /// Returns true if destination is reachable this turn under part-1 movement constraints.
        /// </summary>
        public bool CanMoveTo(HexBoard board, HexTile destination)
        {
            if (board == null || CurrentTile == null || destination == null)
            {
                return false;
            }

            if (movementRange != 1)
            {
                return false;
            }

            return board.AreNeighbors(CurrentTile, destination);
        }
    }
}
