# Unity week 5: Two-dimensional scene-building and path-finding

## Part A
[Play Now](https://tommy-bar.itch.io/tilemap-game-objects-power)
### add power tiles to the game
#### horst tiles - now you can go to trip on the mountains
#### boat tiles - now you can go to trip on the water
#### dragon tiles - now you have the power to destroy mountains
#### play with the keyboard or mouse click

##### we made some changes to the scripts KeyboardMoverByTile.cs
###### first we added alowed tiles for each mode
```
    [SerializeField]
    AllowedTiles onWater = null;

    [SerializeField]
    AllowedTiles onMountin = null;

    [SerializeField]
    AllowedTiles thepower = null;
    
    private bool onGoat = false;
    private bool onBoat = false;
    private bool ondragon = false;
```
###### and added varilable for know wich mode is on

    AllowedTiles currentAllowedTiles = null;
    
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

  
  ###### every step will be checked 
    if (currentAllowedTiles.Contain(tileOnNewPosition))
        {
            transform.position = newPosition;
        }
        else
        {
            Debug.Log("You cannot walk on " + tileOnNewPosition + "!");
        }
