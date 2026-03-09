using System.Collections.Generic;
using UnityEngine;

namespace FinalProject.HexStrategy
{
    /// <summary>
    /// Stores and queries hex tiles by axial coordinates for adjacency and lookup logic.
    /// </summary>
    public class HexBoard : MonoBehaviour
    {
        /// <summary>
        /// Axial neighbor offsets (pointy-top coordinate system).
        /// </summary>
        private static readonly Vector2Int[] NeighborDirections =
        {
            new Vector2Int(1, 0),
            new Vector2Int(1, -1),
            new Vector2Int(0, -1),
            new Vector2Int(-1, 0),
            new Vector2Int(-1, 1),
            new Vector2Int(0, 1)
        };

        private readonly Dictionary<Vector2Int, HexTile> _tiles = new Dictionary<Vector2Int, HexTile>();

        /// <summary>
        /// Builds the internal coordinate->tile index from child HexTile components.
        /// </summary>
        private void Awake()
        {
            _tiles.Clear();
            var tiles = GetComponentsInChildren<HexTile>();
            foreach (var tile in tiles)
            {
                _tiles[new Vector2Int(tile.q, tile.r)] = tile;
            }
        }

        /// <summary>
        /// Attempts to fetch a tile by axial coordinates.
        /// </summary>
        public bool TryGetTile(int q, int r, out HexTile tile) => _tiles.TryGetValue(new Vector2Int(q, r), out tile);

        /// <summary>
        /// Enumerates existing neighboring tiles around a source tile.
        /// </summary>
        public IEnumerable<HexTile> GetNeighbors(HexTile origin)
        {
            var originCoord = new Vector2Int(origin.q, origin.r);
            foreach (var dir in NeighborDirections)
            {
                if (_tiles.TryGetValue(originCoord + dir, out var neighbor))
                {
                    yield return neighbor;
                }
            }
        }

        /// <summary>
        /// Counts how many adjacent tiles are fortress or tower structures.
        /// </summary>
        public int CountAdjacentStructures(HexTile origin)
        {
            var count = 0;
            foreach (var tile in GetNeighbors(origin))
            {
                if (tile.tileType == HexTileType.Fortress || tile.tileType == HexTileType.Tower)
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// Checks whether two tiles are immediate hex neighbors.
        /// </summary>
        public bool AreNeighbors(HexTile a, HexTile b)
        {
            var delta = new Vector2Int(b.q - a.q, b.r - a.r);
            foreach (var dir in NeighborDirections)
            {
                if (dir == delta)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
