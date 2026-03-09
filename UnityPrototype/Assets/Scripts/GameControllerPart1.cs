using System.Collections.Generic;
using UnityEngine;

namespace FinalProject.HexStrategy
{
    /// <summary>
    /// Coordinates part-1 interactions: overlord spawn, selection, movement, and monster summoning.
    /// </summary>
    public class GameControllerPart1 : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Camera mainCamera;
        [SerializeField] private HexBoard board;
        [SerializeField] private OverlordUnit[] overlordChoices;
        [SerializeField] private MonsterUnit monsterPrefab;

        [Header("Scene Objects")]
        [SerializeField] private HexTile playerOneCastle;
        [SerializeField] private HexTile playerTwoCastle;

        private readonly Dictionary<int, OverlordUnit> _activeOverlords = new Dictionary<int, OverlordUnit>();
        private readonly Dictionary<int, MonsterUnit> _spawnedMonsters = new Dictionary<int, MonsterUnit>();

        private OverlordUnit _selectedOverlord;
        private MonsterUnit _selectedMonster;

        /// <summary>
        /// Initializes camera reference and spawns the two starting overlords.
        /// </summary>
        private void Start()
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }

            SpawnOverlordForPlayer(1, playerOneCastle, 0);
            SpawnOverlordForPlayer(2, playerTwoCastle, 1);
        }

        /// <summary>
        /// Reads mouse/keyboard commands each frame for select, move, and summon actions.
        /// </summary>
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                HandleSelectionClick();
            }

            if (Input.GetMouseButtonDown(1))
            {
                HandleCommandClick();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                TrySummonMonster(_selectedOverlord);
            }
        }

        /// <summary>
        /// Spawns a overlord for a player on a legal castle start tile.
        /// </summary>
        private void SpawnOverlordForPlayer(int playerId, HexTile castleTile, int choiceIndex)
        {
            if (castleTile == null || castleTile.tileType != HexTileType.Castle)
            {
                Debug.LogError($"Player {playerId} needs a castle tile start.");
                return;
            }

            var adjacentStructureCount = board.CountAdjacentStructures(castleTile);
            if (adjacentStructureCount < 3)
            {
                Debug.LogError($"Player {playerId} castle must have at least 3 adjacent fortress/tower tiles.");
                return;
            }

            var overlordPrefab = overlordChoices[Mathf.Clamp(choiceIndex, 0, overlordChoices.Length - 1)];
            var overlord = Instantiate(overlordPrefab);
            overlord.PlaceOnTile(castleTile);
            _activeOverlords[playerId] = overlord;
        }

        /// <summary>
        /// Selects a overlord or monster by left-clicking the tile it occupies.
        /// </summary>
        private void HandleSelectionClick()
        {
            var tile = RaycastTile();
            if (tile == null)
            {
                return;
            }

            foreach (var overlord in _activeOverlords.Values)
            {
                if (overlord.CurrentTile == tile)
                {
                    _selectedOverlord = overlord;
                    _selectedMonster = null;
                    return;
                }
            }

            foreach (var monster in _spawnedMonsters.Values)
            {
                if (monster != null && monster.CurrentTile == tile)
                {
                    _selectedMonster = monster;
                    _selectedOverlord = null;
                    return;
                }
            }
        }

        /// <summary>
        /// Commands selected piece to move to the right-clicked destination tile if legal.
        /// </summary>
        private void HandleCommandClick()
        {
            var destination = RaycastTile();
            if (destination == null)
            {
                return;
            }

            if (_selectedOverlord != null && _selectedOverlord.CanMoveTo(destination))
            {
                _selectedOverlord.PlaceOnTile(destination);
                return;
            }

            if (_selectedMonster != null && _selectedMonster.CanMoveTo(board, destination))
            {
                _selectedMonster.PlaceOnTile(destination);
            }
        }

        /// <summary>
        /// Summons one monster for the overlord's player if part-1 conditions are met.
        /// </summary>
        private void TrySummonMonster(OverlordUnit overlord)
        {
            if (overlord == null)
            {
                return;
            }

            if (_spawnedMonsters.ContainsKey(overlord.PlayerId))
            {
                Debug.Log("Part 1 limit reached: only one monster per player.");
                return;
            }

            if (!overlord.CanSummonFromCurrentTile())
            {
                Debug.Log("Overlord must be on castle, fortress, or tower to summon.");
                return;
            }

            var monster = Instantiate(monsterPrefab);
            monster.PlaceOnTile(overlord.CurrentTile);
            _spawnedMonsters[overlord.PlayerId] = monster;
            _selectedMonster = monster;
            _selectedOverlord = null;
        }

        /// <summary>
        /// Raycasts from the mouse cursor and returns the hit tile, if any.
        /// </summary>
        private HexTile RaycastTile()
        {
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, 500f))
            {
                return hit.collider.GetComponentInParent<HexTile>();
            }

            return null;
        }
    }
}
