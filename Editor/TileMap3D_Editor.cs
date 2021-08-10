using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TileMap3D))]
public class TileMap3D_Editor : Editor
{
    Dictionary<Vector2Int, int> yValues = new Dictionary<Vector2Int, int>();
    Dictionary<Vector3Int, TileType> tileTypesAtPositions = new Dictionary<Vector3Int, TileType>();
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
                    if (j < yValues[new Vector2Int(i, k)])
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
                Vector2Int xz = new Vector2Int(i, j);
                for (int k=0; k < yValues[xz]; k++ )
                {
                    Vector3Int gridCord = new Vector3Int(i, k, j);
                    if (tileTypesAtPositions.ContainsKey(gridCord))
                    {
                        TileType t = tileTypesAtPositions[gridCord];
                        Vector3 position = new Vector3(i * tilemap.tileX, k * tilemap.tileHeight, j * tilemap.tileZ);
                        GameObject obToInstatiate = null;
                        Vector3 eulers = new Vector3(0, 0, 0);

                        switch (t)
                        {
                            case TileType.Fill:
                                obToInstatiate = tilemap.tileFill;
                                break;
                            case TileType.CornerUpLeft:
                                obToInstatiate = tilemap.tileCornerUpperLeft;
                                break;
                            case TileType.CornerUpRight:
                                obToInstatiate = tilemap.tileCornerUpperLeft;
                                eulers.Set(0, 90, 0);
                                break;
                            case TileType.CornerDownRight:
                                obToInstatiate = tilemap.tileCornerUpperLeft;
                                eulers.Set(0, 270, 0);
                                break;
                            case TileType.CornerDownLeft:
                                obToInstatiate = tilemap.tileCornerUpperLeft;
                                eulers.Set(0,180, 0);
                                break;
                            case TileType.SideUp:
                                obToInstatiate = tilemap.tileSideUp;
                                break;
                            case TileType.SideDown:
                                obToInstatiate = tilemap.tileSideUp;
                                eulers.Set(0, 180, 0); ;
                                break;
                            case TileType.SideLeft:
                                obToInstatiate = tilemap.tileSideUp;
                                eulers.Set(0, 270, 0);
                                break;
                            case TileType.SideRight:
                                obToInstatiate = tilemap.tileSideUp;
                                eulers.Set(0, 90, 0);
                                break;
                        }
                        GameObject obj = GameObject.Instantiate(obToInstatiate, position, obToInstatiate.transform.rotation);
                        obj.transform.rotation = Quaternion.Euler(eulers) * obj.transform.rotation;
                        obj.transform.parent = parent.transform;
                    }
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
                Vector2Int xz = new Vector2Int(i, j);
                for (int k=0; k < yValues[xz]; k++)
                {
                    Vector3Int cord = new Vector3Int(i, k ,j);
                    switch (t)
                    {
                        case TileType.Fill:
                            tileTypesAtPositions[cord] = TileType.Fill;
                            break;
                        case TileType.CornerUpLeft:
                            if (CheckRuleForCell(cord, new int[] { 0, -1, 0, -1, 1, 0, 1, 0 }, tilemap))
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
                            if (CheckRuleForCell(cord, new int[] { 0, 1, 0, 1, -1, 0, -1, 0 }, tilemap))
                                tileTypesAtPositions[cord] = TileType.CornerDownLeft;
                            break;
                        case TileType.SideUp:
                            if (CheckRuleForCell(cord, new int[] { 0, -1, 0, 1, 1, 0, 0, 0 }, tilemap))
                                tileTypesAtPositions[cord] = TileType.SideUp;
                            break;
                        case TileType.SideDown:
                            if (CheckRuleForCell(cord, new int[] { 0, 0, 0, 1, 1, 0, -1, 0 }, tilemap))
                                tileTypesAtPositions[cord] = TileType.SideDown;
                            break;
                        case TileType.SideLeft:
                            if(CheckRuleForCell(cord, new int[] { 0, 1, 0, -1, 0, 0, 1, 0 }, tilemap))
                                tileTypesAtPositions[cord] = TileType.SideLeft;
                            break;
                        case TileType.SideRight:
                            if (CheckRuleForCell(cord, new int[] { 0, 1, 0, 0, -1, 0, 1, 0 }, tilemap))
                                tileTypesAtPositions[cord] = TileType.SideRight;
                            break;
                    }
                }
                
               
            }
        }
    }

    bool CheckRuleForCell(Vector3Int cell, int[] rule, TileMap3D tilemap)
    {
        //0|1|2
        //3|X|4
        //6|7|8
        Vector2Int auxCell = new Vector2Int(cell.x, cell.z);
        Dictionary<int, Vector2Int> adyacentCells = new Dictionary<int, Vector2Int>();
        adyacentCells.Add(0, auxCell + new Vector2Int(-1, 1));
        adyacentCells.Add(1, auxCell + new Vector2Int(0, 1));
        adyacentCells.Add(2, auxCell + new Vector2Int(1, 1));
        adyacentCells.Add(3, auxCell + new Vector2Int(-1, 0));
        adyacentCells.Add(4, auxCell + new Vector2Int(1, 0));
        adyacentCells.Add(5, auxCell + new Vector2Int(-1, -1));
        adyacentCells.Add(6, auxCell + new Vector2Int(0, -1));
        adyacentCells.Add(7, auxCell + new Vector2Int(1, -1));
        for(int i=0; i< 8; i++)
        {
            if(rule[i] != 0 && CordsInRange(adyacentCells[i], tilemap) )
            {
                if (rule[i] == 1 && !isFilled[adyacentCells[i].x, cell.y , adyacentCells[i].y])
                    return false;
                if (rule[i] == -1 && isFilled[adyacentCells[i].x, cell.y , adyacentCells[i].y])
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
