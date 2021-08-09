using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TileMap3D))]
public class TileMap3D_Editor : Editor
{
    Dictionary<Vector2Int, int> yValues = new Dictionary<Vector2Int, int>();
    Dictionary<Vector2Int, TileType> tileTypesAtPositions = new Dictionary<Vector2Int, TileType>();
    bool[,,] isFilled;
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

        isFilled = new bool[tilemap.heightMap.width, tilemap.numberOfFloors, tilemap.heightMap.height];
        for (int i = 0; i < tilemap.heightMap.width; i++)
        {
            for (int j = 0; j < tilemap.numberOfFloors; j++)
            {
                for (int k = 0; k < tilemap.heightMap.height; k++)
                {
                    if (j <= yValues[new Vector2Int(i, k)])
                        isFilled[i, j, k] = true;
                    else
                        isFilled[i, j, k] = false;
                }
            }
        }

        tileTypesAtPositions.Clear();

        CheckRule(TileType.Fill, tilemap);

        CheckRule(TileType.CornerUpLeft, tilemap);

        InstatiateTileObjects(tilemap);

    }

    void InstatiateTileObjects(TileMap3D tilemap)
    {
        GameObject parent = new GameObject("map");
        parent.transform.parent = tilemap.transform;
        for (int i = 0; i < tilemap.heightMap.width; i++)
        {
            for (int j = 0; j < tilemap.heightMap.height; j++)
            {
                Vector2Int gridCord = new Vector2Int(i, j);
                TileType t = tileTypesAtPositions[gridCord];
                Vector3 position = new Vector3(i * tilemap.tileX, yValues[gridCord] * tilemap.tileHeight, j * tilemap.tileZ);
                GameObject ob = null;
                switch (t)
                {
                    case TileType.Fill:
                        ob = GameObject.Instantiate(tilemap.tileFill, position, Quaternion.identity);
                        break;
                    case TileType.CornerUpLeft:
                        ob = GameObject.Instantiate(tilemap.tileCorner, position, Quaternion.identity);
                        break;

                }
                ob.transform.parent = parent.transform;
                for (int k=0; k<yValues[gridCord]; k++)
                {
                    ob = GameObject.Instantiate(tilemap.tileFill, new Vector3(position.x, k*tilemap.tileHeight, position.z), Quaternion.identity);
                    ob.transform.parent = parent.transform;
                }
               
            }
        }
    }

    void CheckRule( TileType t, TileMap3D tilemap)
    {
        for(int i=0; i< tilemap.heightMap.width; i++)
        {
            for (int j = 0; j < tilemap.heightMap.height; j++)
            {
                Vector2Int cord = new Vector2Int(i, j);
                switch (t)
                {
                    case TileType.Fill:
                        tileTypesAtPositions[cord] = TileType.Fill;
                        break;
                    case TileType.CornerUpLeft:
                        bool flag = true;
                        int yvalue = yValues[cord];
                        if (i - 1 >= 0 && isFilled[i - 1, yValues[cord],j])
                            flag = false;
                        if (j + 1 < tilemap.heightMap.height && isFilled[i , yValues[cord], j + 1])
                            flag = false;
                        if (i + 1 < tilemap.heightMap.width && !isFilled[i + 1 , yValues[cord], j])
                            flag = false;
                        if (j - 1 >= 0  && !isFilled[i , yValues[cord], j - 1])
                            flag = false;
                        if (flag)
                            tileTypesAtPositions[cord] = TileType.CornerUpLeft;
                        break;
                }
            }
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
