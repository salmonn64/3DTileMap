using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TileMap3D))]
public class TileMap3D_Editor : Editor
{
    Dictionary<Vector2Int, int> yValues = new Dictionary<Vector2Int, int>();
    bool[,,] isFilled;
    bool[,,] tileInstatiated;
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
        isFilled = new bool[tilemap.heightMap.width, tilemap.numberOfFloors +1, tilemap.heightMap.height];
        tileInstatiated = new bool[tilemap.heightMap.width, tilemap.numberOfFloors +1, tilemap.heightMap.height];
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

                    tileInstatiated[i, j, k] = false;
                }
            }
        }

        GameObject map = new GameObject("map");
        map.transform.parent = tilemap.transform;
        GameObject[] tiles = { tilemap.tileMiddleCorner, tilemap.tileMiddleFill, tilemap.tileCornerUpperLeft, tilemap.tileSideUp, tilemap.tileFill };
        for(int i=0; i< tiles.Length; i++)
        {
            if(tiles[i] !=null)
                FillWithObject((TileMap3DRule.RuleType)i, tilemap, tiles[i], map);
        }
        
    }

    void FillWithObject(TileMap3DRule.RuleType type, TileMap3D tilemap, GameObject obj, GameObject parent)
    {

        TileMap3DRule r = new TileMap3DRule(type);
        FillWithObject(r, tilemap, obj, parent);
    }

    void FillWithObject(TileMap3DRule rule, TileMap3D tilemap, GameObject obj, GameObject parent)
    {
        Quaternion rot = Quaternion.identity;
        Vector3 eulers = new Vector3(0, 0, 0);
        for (int i = 0; i < 4; i++)
        {
            CheckRuleAndInstatiate(rule, tilemap, obj, rot, parent);
            rule.Rotate90();
            eulers += new Vector3(0, 90, 0);
            rot = Quaternion.Euler(eulers);
        }
    }

    bool CheckRuleForCell( Vector3Int cell, TileMap3DRule rule, TileMap3D tilemap)
    {
        foreach(KeyValuePair<Vector3Int,int> p in rule.rule)
        {
            Vector3Int sum = cell + p.Key;
            if(CordsInRange( sum, tilemap)){
                if (p.Value == 1 && !isFilled[sum.x, sum.y, sum.z])
                    return false;
                if (p.Value == -1 && isFilled[sum.x, sum.y, sum.z])
                    return false;
            }
        }
        return true;
    }

    void CheckRuleAndInstatiate(TileMap3DRule rule, TileMap3D tilemap, GameObject obj, Quaternion rotation,GameObject parent)
    {
        for(int i =0; i< tilemap.heightMap.width; i++)
        {
            for(int j=0; j<tilemap.heightMap.height; j++)
            {
                for(int k=0; k< yValues[new Vector2Int(i,j)]; k++)
                {
                    if( !tileInstatiated[i,k,j] && CheckRuleForCell(new Vector3Int(i, k, j), rule, tilemap))
                    {
                        GameObject aux = GameObject.Instantiate(obj, new Vector3(i * tilemap.tileX, k * tilemap.tileHeight, j * tilemap.tileZ), obj.transform.rotation);
                        aux.transform.rotation = rotation * aux.transform.rotation;
                        aux.transform.parent = parent.transform; 
                        tileInstatiated[i, k, j] = true;
                    }
                }
            }
        }
    }
 
    bool CordsInRange(Vector3Int v, TileMap3D tilemap)
    {
        if (v.x >= 0 && v.x < tilemap.heightMap.width && v.z < tilemap.heightMap.height && v.z >= 0 && v.y >=0 && v.y <= tilemap.numberOfFloors)
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

}
