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

## Part B
[Play Now](https://tommy-bar.itch.io/tilemap-game-objects-power)
### map generating position algorithm
#### added algorithm use the BFS from class to check if the player has at least 100 unique steps to do
#### if not, new position is generated
#### play with the keyboard or mouse clicks
#### we made changes in TilemapCaveGenerator.cs
#### that method checks if the player position is valid with 100 setps or more.
    bool possValid(Vector3 pos, Tilemap map)
#### and added code to the below method
    private IEnumerator SimulateCavePattern()
    -------- some code ---------------------
            Debug.Log("-----------check for position------------");
        Vector3 PlayerPos = player.transform.position;
        bool is_on_grass = false;
        while(!is_on_grass){
            TileBase tileOnNewPosition = TileOnPosition(PlayerPos);
        if (allowedTiles.Contain(tileOnNewPosition)) {
            is_on_grass = true;
        }
        else{
            PlayerPos.x = Random.Range(0, gridSize);
            PlayerPos.y = Random.Range(0, gridSize);
        }
        player.transform.position = PlayerPos; 
        }
        while (!valid && iterations < maxIterations )
        {
            if(iterations > 0){
                Vector3 newpos = new Vector3(Random.Range(0, gridSize), Random.Range(0, gridSize), 0);
                player.transform.position = newpos;
            }
            iterations++;
            valid = possValid(PlayerPos, tilemap);
            
        }
        if(valid){
            Debug.Log("check done in "+iterations+" iterations");
        }

    }


