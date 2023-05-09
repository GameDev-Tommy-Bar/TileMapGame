using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;

/**
 * This class demonstrates the CaveGenerator on a Tilemap.
 *
 * By: Erel Segal-Halevi
 * Since: 2020-12
 */

public class TilemapCaveGenerator : MonoBehaviour
{
    [SerializeField]
    Tilemap tilemap = null;

    [Tooltip("The tile that represents a wall (an impassable block)")]
    [SerializeField]
    TileBase wallTile = null;

    [Tooltip("The tile that represents a floor (a passable block)")]
    [SerializeField]
    TileBase floorTile = null;

    [Tooltip("The percent of walls in the initial random map")]
    [Range(0, 1)]
    [SerializeField]
    float randomFillPercent = 0.5f;

    [Tooltip("Length and height of the grid")]
    [SerializeField]
    int gridSize = 100;

    [Tooltip("How many steps do we want to simulate?")]
    [SerializeField]
    int simulationSteps = 20;

    [Tooltip(
        "For how long will we pause between each simulation step so we can look at the result?"
    )]
    [SerializeField]
    float pauseTime = 1f;
    public int numSteps = 0;
    [SerializeField] GameObject player;
    private TilemapGraph graph = null;
    public AllowedTiles allowedTiles = null;
    int maxIterations = 50;
    int iterations = 0;
    bool valid = false;
    Vector3 PlayerPos;

    private CaveGenerator caveGenerator;

    void Start()
    {
        graph = new TilemapGraph(tilemap, allowedTiles.Get());

        //To get the ame random numbers each time we run the script
        Random.InitState(100);

        caveGenerator = new CaveGenerator(randomFillPercent, gridSize);
        caveGenerator.RandomizeMap();

        //For testing that init is working
        GenerateAndDisplayTexture(caveGenerator.GetMap());
        StartCoroutine(SimulateCavePattern());



        //Start the simulation

        }


    
    void update(){

       

        
    }
    bool possValid(Vector3 pos, Tilemap map)
    {
        int counter = 0;
        int attempts = 0;
        int maxAttempts = 2000;
        //use BFS to find a path from the player to the target
        Vector3Int startNode = map.WorldToCell(pos);
        while (attempts < maxAttempts)
        {
            //Debug.Log("attempts: " + attempts);
            int x = Random.Range(0, gridSize);
            int y = Random.Range(0, gridSize);
            Vector3 target = new Vector3(x, y, 0);
            Vector3Int endNode =map.WorldToCell(target);
            List<Vector3Int> shortestPath = BFS.GetPath(graph, startNode, endNode, 1000);
            Debug.Log("startNode: " + startNode+"end node = "+endNode);

            HashSet<Vector3Int> uniqeTiles = new HashSet<Vector3Int>(shortestPath);
            Debug.Log("unique count: "+uniqeTiles.Count);
            if (shortestPath.Count >= 100 && uniqeTiles.Count >= 100)
            {
                return true;
            }
            else if(shortestPath.Count > 0);
            {
                counter++;
                Debug.Log("shortest path size from "+startNode+ "to "+endNode+ "is  "+shortestPath.Count);

            }
            
            if(shortestPath.Count == 0){
                Debug.Log("NoPath from "+startNode+ "to "+endNode+ ",shortestpath count =  "+shortestPath.Count);
            }
            if(counter == 100){
                return true;
            }
            attempts++;
            Debug.Log("counter = "+counter+"  attempts = "+attempts+" uniqe HashSet size "+uniqeTiles.Count);
        }

        return false;
    }

    //Do the simulation in a coroutine so we can pause and see what's going on
    private IEnumerator SimulateCavePattern()
    {
        for (int i = 0; i < simulationSteps; i++)
        {
            numSteps++;
            Debug.Log("numSteps: " + numSteps);
            yield return new WaitForSeconds(pauseTime);

            //Calculate the new values
            caveGenerator.SmoothMap();

            //Generate texture and display it on the planes
            GenerateAndDisplayTexture(caveGenerator.GetMap());
        }
        Debug.Log("----------map generation completed!------------");
        Debug.Log("-----------check for position------------");
        Vector3 PlayerPos = player.transform.position;
        while (!valid && iterations < maxIterations )
        {
            if(iterations > 0){
                Vector3 newpos = new Vector3(Random.Range(0, gridSize), Random.Range(0, gridSize), 0);
                player.transform.position = newpos;
            }
            iterations++;
            valid = possValid(PlayerPos, tilemap);
            Debug.Log("iteration " + iterations + "valid: " + valid);
            
        }
        Debug.Log("check done, its valid? = "+valid);

    }

    //Generate a black or white texture depending on if the pixel is cave or wall
    //Display the texture on a plane
    private void GenerateAndDisplayTexture(int[,] data)
    {
        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                var position = new Vector3Int(x, y, 0);
                var tile = data[x, y] == 1 ? wallTile : floorTile;
                tilemap.SetTile(position, tile);
            }
        }
    }
}

