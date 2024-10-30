using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    
    [SerializeField] private int width;
    [SerializeField] private int depth;
    [SerializeField] private int maxCubesToGenerate;
    [SerializeField] private float maxHeigth;
    [SerializeField] private float wildness;

    
    [SerializeField] private float waterHeight;
    [SerializeField] private float stoneHeight;

    
    [SerializeField] private Transform grassGameobject;
    [SerializeField] private Transform stoneGameobject;
    [SerializeField] private Transform waterGameobject;
    [SerializeField] private Transform playerPrefab;

    //[Tab("Seed")]
    //[SerializeField] private int seed;


    Transform[,] map;


    private void Start()
    {
        map = new Transform[width, depth];
        GenerateMapPices();
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        int randomW = Random.Range(0, width);
        int randomD = Random.Range(0, depth);

        Tile spawnPointTile = map[randomW, randomD].GetComponent<Tile>();
        Vector3 spawnPoint = spawnPointTile.GetPosition();

        var player = Instantiate(playerPrefab, spawnPoint, Quaternion.identity, transform);
    }

    private void GenerateMapPices()
    {
        //Random.InitState(seed);

        float startX = Random.Range(-1000, 1000);
        float startY = Random.Range(-1000, 1000);

        int cubesToGenerate = width * depth;
        if (cubesToGenerate > maxCubesToGenerate)
        {
            Debug.Log($"Generating {cubesToGenerate - maxCubesToGenerate} over limit, returning");
            return;
        }

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < depth; j++)
            {

                var height = Mathf.PerlinNoise(
                    ((float)i / (width / wildness)) + startX,
                    ((float)j / (depth / wildness)) + startY);

                Transform mapPicePrefab = GetTile(height);
                var mappice = Instantiate(mapPicePrefab, new Vector3(i, 0, j), Quaternion.identity, transform);
                mapPicePrefab.localScale = new Vector3(1, height * maxHeigth, 1);
                map[i, j] = mapPicePrefab;
            }
        }

    }

    private Transform GetTile(float scaleY)
    {
        scaleY = scaleY * 10;
        if (scaleY < waterHeight)
        {
            return waterGameobject;
        }
        else if (scaleY > waterHeight && scaleY < stoneHeight)
        {
            return grassGameobject;
        }
        else
        {
            return stoneGameobject;
        }
    }
}
