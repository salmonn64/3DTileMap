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

        for(int i=1; i<=9; i++)
        {
            CheckRule((TileType)i, tilemap);
        }
        
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
                    case TileType.CornerUpRight:
                        ob = GameObject.Instantiate(tilemap.tileCorner, position, Quaternion.identity);
                        ob.transform.localScale = new Vector3(-ob.transform.localScale.x, ob.transform.localScale.y, ob.transform.localScale.z);
                        break;
                    case TileType.CornerDownRight:
                        ob = GameObject.Instantiate(tilemap.tileCorner, position, Quaternion.identity);
                        ob.transform.localScale = new Vector3(ob.transform.localScale.x, ob.transform.localScale.y, -ob.transform.localScale.z);
                        break;
                    case TileType.CornerDownLeft:
                        ob = GameObject.Instantiate(tilemap.tileCorner, position, Quaternion.identity);
                        ob.transform.localScale = new Vector3(-ob.transform.localScale.x, ob.transform.localScale.y, -ob.transform.localScale.z);
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
                        if (CheckRuleForCell(cord, new int[] { 0, -1, 0, -1, 1, 0, 1, 0 }, tilemap ))
                            tileTypesAtPositions[cord] = TileType.CornerUpLeft;
                        break;
                    case TileType.CornerUpRight:
                        if (CheckRuleForCell(cord, new int[] { 0, -1, 0, 1, -1, 0, 1, 0 }, tilemap))
                            tileTypesAtPositions[cord] = TileType.CornerUpRight;
                        break;
                    case TileType.CornerDownRight:
                        if (CheckRuleForCell(cord, new int[] { 0, 1, 0, -1, 1, 0, -1, 0 }, tilemap))
                            tileTypesAtPositions[cord] = TileType.CornerDownRight;
                        break;
                    case TileType.CornerDownLeft:
                        if( CheckRuleForCell(cord, new int[]{ 0, 1, 0, 1, -1, 0, -1, 0 }, tilemap))
                        tileTypesAtPositions[cord] = TileType.CornerDownLeft;
                        break;

                }
            }
        }
    }

    bool CheckRuleForCell(Vector2Int cell, int[] rule, TileMap3D tilemap)
    {
        //0|1|2
        //3|X|4
        //6|7|8
        Dictionary<int, Vector2Int> adyacentCells = new Dictionary<int, Vector2Int>();
        adyacentCells.Add(0, cell + new Vector2Int(-1, 1));
        adyacentCells.Add(1, cell + new Vector2Int(0, 1));
        adyacentCells.Add(2, cell + new Vector2Int(1, 1));
        adyacentCells.Add(3, cell + new Vector2Int(-1, 0));
        adyacentCells.Add(4, cell + new Vector2Int(1, 0));
        adyacentCells.Add(5, cell + new Vector2Int(-1, -1));
        adyacentCells.Add(6, cell + new Vector2Int(0, -1));
        adyacentCells.Add(7, cell + new Vector2Int(1, -1));
        for(int i=0; i< 8; i++)
        {
            if(rule[i] != 0 && CordsInRange(adyacentCells[i], tilemap) )
            {
                if (rule[i] == 1 && !isFilled[adyacentCells[i].x, yValues[cell], adyacentCells[i].y])
                    return false;
                if (rule[i] == -1 && isFilled[adyacentCells[i].x, yValues[cell], adyacentCells[i].y])
                    return false;
            }
        }
        return true;
    }

    bool CordsInRange(Vector2Int v, TileMap3D tilemap)
    {
        if (v.x >= 0 && v.x < tilemap.heightMap.width && v.y < tilemap.heightMap.height && v.y >= 0)
            return true;
        else return false;
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
        Fill = 1,
        SideUp = 2,
        CornerUpRight = 3,
        SideLeft = 4,
        CornerUpLeft = 5,
        SideRight = 6,
        CornerDownLeft = 7,
        SideDown = 8,
        CornerDownRight = 9
    }
}
