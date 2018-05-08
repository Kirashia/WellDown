using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelCreator : MonoBehaviour
{

    public string seed;
    public GameObject wall;
    public GameObject exitTile;
    public GameObject sine;
    public GameObject platform;
    public int length;
    public bool useRandomSeed;
    [Range(0, 100)] public int randomFill;
    public GameObject[] enemies;

    private System.Random prng;
    private GameObject wallHolder;
    private GameObject enemyHolder;
    private GameObject platformHolder;

    // Main calling function
    public void CreateSineTunnel()
    {
        // Create holder gameobjects
        wallHolder = new GameObject("Walls");
        enemyHolder = new GameObject("Enemies");
        platformHolder = new GameObject("Platforms");

        if (useRandomSeed || seed == "")
        {
            seed = Time.deltaTime.ToString();
        }

        prng = new System.Random(seed.GetHashCode());

        // A little bit of functional (ish) programming ;) 
        InstantiateTiles(
            FixOpenAreas(
                AddSine(
                    MakeTunnelArray(length),
                    GetSinePattern(), 
                    GetSinePattern()
                )
            )
        );

        GenerateBorders();
        GenerateExit();

    }

    // Adds platforms and assigns areas to enemies
    int[,] FixOpenAreas(int[,] v)
    {
        int prevScore = 0;
        int currentScore = 0;
        int repeat = 0;
        int start = 0;

        for (int y = 0; y < v.GetLength(1) - 50; y++)
        {
            for (int x = 0; x < v.GetLength(0); x++)
            {
                if (v[x, y] == 0 || v[x, y] == -1)
                {
                    if (currentScore == 0)
                        start = x;

                    currentScore++;
                }
            }

            if ((prevScore == currentScore || prevScore == currentScore -1 || prevScore == currentScore +1) && prevScore > 0 && currentScore > 0)
            {
                repeat++;
                int randomStart = prng.Next(start, start + (9 - currentScore));
                if (repeat > 3 && OneIn(10))
                {
                    int randomLength = prng.Next(1, currentScore - 2);
                    for (int i = 0; i < randomLength; i++)
                    {
                        v[randomStart + i, y] = 2;
                    }
                    repeat = 0;
                    prevScore = 0;
                }
                if (OneIn(5) && repeat > 1)
                {
                    // Create enemy
                    GameObject enemy = GetRandomEnemy();
                    GameObject instance;
                    switch (enemy.name)
                    {
                        case "Blob":
                            //Debug.Log(randomStart + ", " + y);
                            randomStart = prng.Next(start, start + (currentScore));
                            instance = Instantiate(enemy, new Vector3(randomStart, y), Quaternion.identity, enemyHolder.transform) as GameObject;
                            Debug.Log(enemy.name);
                            instance.name = "Blob";
                            break;
                    }
                }
            }
            else
            {
                repeat = 0;
            }
            prevScore = currentScore;
            currentScore = 0;
        }

        return v;
    }

    // For instantiating enemies
    GameObject GetRandomEnemy()
    {
        return enemies[prng.Next(0, enemies.Length)];
    }

    // For making block grids
    int GetScore(int[,] tunnel, int xP, int yP)
    {
        int score = 0;
        for (int x = xP - 1; x <= xP + 1; x++)
        {
            for (int y = yP - 1; y <= yP + 1; y++)
            {
                if (x >=  9 || y >=  length || x < 0 || y < 0)
                {
                    continue;
                }
                else if (x == xP && y == yP)
                {
                    continue;
                }

                // Increments return value if there is a -1 there
                score += (tunnel[x, y] == -1 || tunnel[x, y] == 3) ? 1 : 0;
            }
        }

        return score;
    }

    // Gets all points on a sine curve
    int[] GetSinePattern()
    {
        int[] sinePattern = new int[length];

        // Generate random sine wave
        // Generate random coefficients
        float coefX = prng.Next(-25, 25) * .01f;
        float coefY = prng.Next(-25, 25) * .01f;

        // Gets x coord from mathematical function, stores it in array with y coord as index
        for (int y = 0; y < length; y++)
        {
            sinePattern[y] = 4 + (int)Math.Floor(Mathf.Sin(y * coefX) + Mathf.Cos(y * coefY) * Mathf.Sin(y * coefX * coefY));
        }

        return sinePattern;
    }

    // Sets up the initial array of specified length
    int[,] MakeTunnelArray(int length)
    {
        int[,] tunnel = new int[9, length + 50];

        // Starting platform
        tunnel[3, length + 5] = 2;
        tunnel[4, length + 5] = 2;
        tunnel[5, length + 5] = 2;

        for (int x = 0; x < 9; x++)
        {
            for (int y = 0; y < length; y++)
            {
                tunnel[x, y] = (prng.Next(0,100) > randomFill)? 0 : -1;
            }
        }

        return tunnel;
    }

    // Carves a tunnel around the sine curve
    int[,] CarveTunnel(int[,] tunnel, int[] sineLine)
    {
        for (int y = 0; y < sineLine.Length; y++)
        {
            tunnel = DrawCircleAround(new Vector2(sineLine[y], y), 3, tunnel);
        }

        return tunnel;
    }

    // Carves a circle around a single point
    int[,] DrawCircleAround(Vector2 point, int radius, int[,] tunnel)
    {
        List<Vector2> debugList = new List<Vector2>();
        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
            {
                // Lies in or on the circle
                if (x * x + y * y <= radius * radius)
                {
                    int drawX = (int)point.x + x;
                    int drawY = (int)point.y + y;

                    if (CoordInMapBounds(new Vector2(drawX, drawY)))
                    {
                        // Make the point a floor tile
                        tunnel[drawX, drawY] = 0;
                        debugList.Add(new Vector2(drawX, drawY));
                    }
                }
            }
        }

        return tunnel;
    }

    // Debugging
    void InstantiateSine(int[] sineLineLeft, int[] sineLineRight)
    {
        for (int y = 0; y < sineLineLeft.Length; y++)
        {
            GameObject tile = Instantiate(sine, new Vector3(sineLineLeft[y] - 4, y, 0), Quaternion.identity) as GameObject;
            tile.name = sineLineLeft[y] + ", " + y;
        }

        for (int y = 0; y < sineLineRight.Length; y++)
        {
            GameObject tile = Instantiate(sine, new Vector3(sineLineRight[y] + 4, y, 0), Quaternion.identity) as GameObject;
            tile.name = sineLineRight[y] + ", " + y;
        }
    }

    // Creates the walls of the map using two sine curves
    int[,] AddSine(int[,] tunnel, int[] sineLineLeft, int[] sineLineRight)
    {
        // Left
        for (int y = 0; y < sineLineLeft.Length; y++)
        {
            try
            {
                // Fill from the line to the left of the screen
                for (int x = sineLineLeft[y] - 4; x >= 0; x--)
                {
                    tunnel[x, y] = 1;
                }
            }
            catch
            {
                continue;
            }
        }
        
        // right
        for (int y = 0; y < sineLineRight.Length; y++)
        {
            try
            {
                // Fill from the line to the left of the screen
                for (int x = sineLineRight[y] + 4; x >= 0; x++)
                {
                    tunnel[x, y] = 1;
                }
            }
            catch
            {
                continue;
            }
        }

        return tunnel;
    }

    // Creates the final map
    void InstantiateTiles(int[,] tunnel)
    {
        for (int x = 0; x < tunnel.GetLength(0); x++)
        {
            for (int y = 0; y < tunnel.GetLength(1); y++)
            {
                if (tunnel[x, y] == 1)
                {
                    GameObject tile = Instantiate(wall, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                    tile.transform.parent = wallHolder.transform;
                    tile.name = x + ", " + y;
                }
                else if (tunnel[x, y] == 2)
                {
                    GameObject tile = Instantiate(platform, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                    tile.transform.parent = platformHolder.transform;
                    tile.name = x + ", " + y;
                }
                else if (tunnel[x, y] == -10)
                {
                    GameObject tile = Instantiate(sine, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                    tile.name = "Block";
                }
            }
        }
    }

    // Generate borders around map
    void GenerateBorders()
    {
        // Borders on sides
        for (int i = 0; i < length + 50; i++)
        {
            // Left
            GameObject tile = Instantiate(wall, new Vector3(-1, i, 0), Quaternion.identity) as GameObject;
            tile.name = "Wall";
            tile.transform.parent = wallHolder.transform;

            // Right
            tile = Instantiate(wall, new Vector3(9, i, 0), Quaternion.identity) as GameObject;
            tile.name = "Wall";
            tile.transform.parent = wallHolder.transform;
        }
    }

    // Places the exit tiles
    void GenerateExit()
    {
        int[,] exit = new int[9, 6];

        for (int x = 0; x < exit.GetLength(0); x++)
        {
            for (int y = 0; y < exit.GetLength(1); y++)
            {
                exit[x, y] = 0;
            }
        }

        exit[0, 1] = 1;
        exit[1, 2] = 1;
        exit[2, 3] = 1;
        exit[2, 4] = 1;
        exit[2, 5] = 1;
        exit[6, 5] = 1;
        exit[6, 4] = 1;
        exit[6, 3] = 1;
        exit[7, 2] = 1;
        exit[8, 1] = 1;

        Instantiate(exitTile, new Vector3(4f, -4f, 0f), Quaternion.identity);

        for (int x = 0; x < exit.GetLength(0); x++)
        {
            for (int y = 0; y < exit.GetLength(1); y++)
            {
                if (exit[x, y] == 1)
                {
                    GameObject tile = Instantiate(wall, new Vector3(x, -y, 0), Quaternion.identity) as GameObject;
                    tile.transform.parent = wallHolder.transform;
                    tile.name = x + ", " + y;
                }
            }
        }

    }

    // Adds a 50-tile high column for the player to fall through at the beginning of the map
    void GenerateEntrance()
    {
        int[,] entrance = new int[9, 6];

        for (int x = 0; x < entrance.GetLength(0); x++)
        {
            for (int y = 0; y < entrance.GetLength(1); y++)
            {
                entrance[x, y] = 0;
            }
        }

        entrance[0, 1] = 1;
        entrance[1, 2] = 1;
        entrance[2, 3] = 1;
        entrance[2, 4] = 1;
        entrance[2, 5] = 1;
        entrance[6, 5] = 1;
        entrance[6, 4] = 1;
        entrance[6, 3] = 1;
        entrance[7, 2] = 1;
        entrance[8, 1] = 1;

        for (int x = 0; x < entrance.GetLength(0); x++)
        {
            for (int y = 0; y < entrance.GetLength(1); y++)
            {
                if (entrance[x, y] == 1)
                {
                    GameObject tile = Instantiate(wall, new Vector3(x, -y, 0), Quaternion.identity) as GameObject;
                    tile.transform.parent = wallHolder.transform;
                    tile.name = x + ", " + y;
                }
            }
        }
    }

    // Returns false if the coordinate does not exist on the map
    bool CoordInMapBounds(Vector2 c)
    {
        if (c.x < 0 || c.y < 0)
            return false;

        if (c.x > 9 - 1 || c.y > length - 1)
            return false;

        return true;
    }

    // Random chance function
    bool OneIn(int chance)
    {
        return prng.Next(1, chance) == 1;
    }
}