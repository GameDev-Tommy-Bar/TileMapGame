using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

/**
 * This component allows the player to move by clicking the arrow keys,
 * but only if the new position is on an allowed tile.
 */
public class KeyboardMoverByTile : KeyboardMover
{
    [SerializeField]
    Tilemap tilemap = null;

    [SerializeField]
    AllowedTiles allowedTiles = null;

    [SerializeField]
    AllowedTiles onWater = null;

    [SerializeField]
    AllowedTiles onMountin = null;

    [SerializeField]
    AllowedTiles thepower = null;

    [SerializeField]
    TileBase grassTile = null;

    AllowedTiles currentAllowedTiles = null;
    private bool onGoat = false;
    private bool onBoat = false;
    private bool ondragon = false;

    private TileBase TileOnPosition(Vector3 worldPosition)
    {
        Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);
        return tilemap.GetTile(cellPosition);
    }

    void Update()
    {
        //restart the game
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        Vector3 newPosition = NewPosition();
        TileBase tileOnNewPosition = TileOnPosition(newPosition);
        if (tileOnNewPosition.name == "dragon")
        {
            Debug.Log("You now have the power of the dragon!");
            ondragon = !ondragon;
        }

        if (tileOnNewPosition.name == "GOAT")
        {
            Debug.Log("You are on a goat!");
            onGoat = !onGoat;
        }
        if (tileOnNewPosition.name == "boat")
        {
            Debug.Log("You are on a boat!");
            onBoat = !onBoat;
        }

        if (onBoat == true)
        {
            currentAllowedTiles = onWater;
            ondragon = false;
            onGoat = false;
        }
        else if (onGoat == true)
        {
            currentAllowedTiles = onMountin;
            ondragon = false;
            onBoat = false;
        }
        else if (ondragon == true)
        {
            onBoat = false;
            onGoat = false;
            currentAllowedTiles = thepower;
            if (tileOnNewPosition.name == "mountains")
            {
                transform.position = newPosition;
                tilemap.SetTile(tilemap.WorldToCell(newPosition), grassTile);
                Debug.Log("You have destroyed the mountains!");
            }
        }
        else
        {
            currentAllowedTiles = allowedTiles;
        }

        if (currentAllowedTiles.Contain(tileOnNewPosition))
        {
            transform.position = newPosition;
        }
        else
        {
            Debug.Log("You cannot walk on " + tileOnNewPosition + "!");
        }
    }
}
