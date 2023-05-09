using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

/**
 * This component just keeps a list of allowed tiles.
 * Such a list is used both for pathfinding and for movement.
 */
public class AllowedTiles : MonoBehaviour
{
    [SerializeField]
    TileBase[] allowedTiles = null;

    public bool Contain(TileBase tile)
    {
        //i want to add boat tile to allowed tiles

        return allowedTiles.Contains(tile);
    }

    public TileBase[] Get()
    {
        return allowedTiles;
    }
}
