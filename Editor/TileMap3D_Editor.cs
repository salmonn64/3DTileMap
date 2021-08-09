using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TileMap3D))]
public class TileMap3D_Editor : Editor
{
    Dictionary<Vector2Int, int> yValues = new Dictionary<Vector2Int, int>();
    Dictionary<Vector2Int, TileType> tileTypesAtPositions = new Dictionary<Vector2Int, TileType>();
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Create"))
        {
            CreateMap((TileMap3D)target);
        }
    }

    void CreateMap(TileMap3D tilemap)
    {
        CalculateYValues(tilemap);

        tileTypesAtPositions.Clear();

        CheckRule(TileType.Fill, tilemap);

        InstatiateTileObjects(tilemap);

    }

    void InstatiateTileObjects(TileMap3D tilemap)
    {
        for (int i = 0; i < tilemap.heightMap.width; i++)
        {
            for (int j = 0; j < tilemap.heightMap.height; j++)
            {
                Vector2Int gridCord = new Vector2Int(i, j);
                TileType t = tileTypesAtPositions[gridCord];
                Vector3 position = new Vector3(i * tilemap.tileX, yValues[gridCord] * tilemap.tileHeight, j * tilemap.tileZ);
                switch (t)
                {
                    case TileType.Fill:
                        GameObject ob = GameObject.Instantiate(tilemap.tileFill, position, Quaternion.identity);
                        ob.transform.parent = tilemap.transform;
                        break;
                }
               
            }
        }
    }

    void CheckRule( TileType t, TileMap3D tilemap)
    {
        switch (t)
        {
            case TileType.Fill:
                for (int i = 0; i < tilemap.heightMap.width; i++)
                {
                    for(int j=0; j< tilemap.heightMap.height; j++)
                    {
                        tileTypesAtPositions[new Vector2Int(i, j)] = TileType.Fill;
                    }
                }
                break;
        }

    }

    void CalculateYValues(TileMap3D tilemap)
    {
        yValues.Clear();
        int width = tilemap.heightMap.width;
        int height = tilemap.heightMap.height;
        for (int i = 0; i < tilemap.heightMap.width; i++)
        {
            for (int j = 0; j < tilemap.heightMap.height; j++)
            {
                float c = tilemap.heightMap.GetPixel(i, j).r;
                yValues.Add(new Vector2Int(i, j), GetFloor(c, tilemap));
            }
        }
    }

    public int GetFloor(float color,TileMap3D tileMap)
    {
        float x = Mathf.Lerp(0, tileMap.numberOfFloors, color);
        return Mathf.RoundToInt(x);
    }

    enum TileType
    {
        CornerUpLeft = 1,
        SideUp = 2,
        CornerUpRight = 3,
        SideLeft = 4,
        Fill = 5,
        SideRight = 6,
        CornerDownLeft = 7,
        SideDown = 8,
        CornerDownRight = 9
    }
}
