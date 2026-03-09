using UnityEngine;

namespace FinalProject.HexStrategy
{
    /// <summary>
    /// Runtime data/behavior for a player overlord piece.
    /// </summary>
    public class OverlordUnit : MonoBehaviour
    {
        [SerializeField] private int playerId;

        /// <summary>
        /// Owning player identifier used by the game controller.
        /// </summary>
        public int PlayerId => playerId;

        /// <summary>
        /// Tile the overlord is currently occupying.
        /// </summary>
        public HexTile CurrentTile { get; private set; }

        /// <summary>
        /// Places this overlord on a tile and snaps world position to the tile.
        /// </summary>
        public void PlaceOnTile(HexTile tile)
        {
            CurrentTile = tile;
            transform.position = tile.transform.position;
        }

        /// <summary>
        /// Returns true if the destination is a legal overlord structure tile.
        /// </summary>
        public bool CanMoveTo(HexTile targetTile)
        {
            return targetTile != null && targetTile.IsOverlordStructure;
        }

        /// <summary>
        /// Returns true if the overlord is standing on a valid summon structure tile.
        /// </summary>
        public bool CanSummonFromCurrentTile()
        {
            return CurrentTile != null && CurrentTile.IsOverlordStructure;
        }
    }
}
