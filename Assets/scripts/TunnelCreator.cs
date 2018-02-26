using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelCreator : MonoBehaviour {

    public string seed;
    public GameObject wall;
    public GameObject sine;
    public int length;
    public bool useRandomSeed;
    [Range(0, 100)] public int randomFill;

    private System.Random prng;

    public void CreateSineTunnel()
    {
        if (useRandomSeed || seed == "")
        {
            seed = Time.deltaTime.GetHashCode().ToString();
        }

        prng = new System.Random(seed.GetHashCode());

        // A little bit of functional (ish) programming ;) 
        InstantiateTiles(
            CarveTunnel(
                MakeTunnelArray(length), GetSinePattern()
            )
        );      

    }

    // Gets all points on a sine curve
    int[] GetSinePattern()
    {
        int[,] tunnel = new int[9, length];
        int[] sinePattern = new int[length];

        // Generate random sine wave
        // Generate random coefficients
        float coefX = (prng.Next(0, 100) < randomFill) ? 0 : prng.Next(-25, 25) * .01f;
        float coefY = (prng.Next(0, 100) < randomFill) ? 0 : prng.Next(-25, 25) * .01f;

        // Gets x coord from mathematical function, stores it in array with y coord as index
        for (int y = 0; y < tunnel.GetLength(1); y++)
        {
            sinePattern[y] = 4 + (int)Math.Floor(Mathf.Sin(y * coefX) + Mathf.Cos(y * coefY) * Mathf.Sin(y * coefX * coefY));
            Debug.Log(sinePattern[y]);
        }


        return sinePattern;
    }
    

    // Sets up the initial array of specified length
    int[,] MakeTunnelArray(int length)
    {
        int[,] tunnel = new int[9, length];
        for (int x = 0; x < tunnel.GetLength(0); x++)
        {
            for (int y = 0; y < tunnel.GetLength(1); y++)
            {
                tunnel[x, y] = 1;
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
    void InstantiateSine(int[] sineLine)
    {
        for(int y = 0; y < sineLine.Length; y++)
        {
            GameObject tile = Instantiate(sine, new Vector3(sineLine[y], y, 0), Quaternion.identity) as GameObject;
            tile.name = sineLine[y] + ", " + y;
        }
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
}
