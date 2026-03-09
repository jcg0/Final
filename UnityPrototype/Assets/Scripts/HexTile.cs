using UnityEngine;

namespace FinalProject.HexStrategy
{
    /// <summary>
    /// Lists every terrain/building type used by the part-1 prototype.
    /// </summary>
    public enum HexTileType
    {
        Plain,
        Forest,
        Mountain,
        Volcano,
        Desert,
        RockyTracts,
        Swamp,
        Tundra,
        Castle,
        Fortress,
        Tower,
        River,
        Fjord,
        Ocean
    }

    /// <summary>
    /// Represents one hex tile on the board, including axial coordinates and tile category.
    /// </summary>
    public class HexTile : MonoBehaviour
    {
        [Header("Coordinates (Axial)")]
        public int q;
        public int r;

        [Header("Tile Data")]
        public HexTileType tileType;

        /// <summary>
        /// Returns true when this tile is a valid structure tile for overlords (castle/fortress/tower).
        /// </summary>
        public bool IsOverlordStructure => tileType == HexTileType.Castle || tileType == HexTileType.Fortress || tileType == HexTileType.Tower;

        /// <summary>
        /// Returns true when this tile is one of the water categories.
        /// </summary>
        public bool IsWater => tileType == HexTileType.River || tileType == HexTileType.Fjord || tileType == HexTileType.Ocean;
    }
}
