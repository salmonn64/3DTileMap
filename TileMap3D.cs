using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap3D : MonoBehaviour
{
    [SerializeField]
    public Texture2D heightMap;

    [SerializeField]
    public GameObject tileCornerUpperLeft;

    [SerializeField]
    public GameObject tileSideUp;

    [SerializeField]
    public GameObject tileFill;

    [SerializeField]
    public float tileX;

    [SerializeField]
    public float tileZ;

    [SerializeField]
    public float tileHeight;

    [SerializeField]
    public int numberOfFloors;
    

    /*void Start()
    {
        int width = heightMap.width;
        int height = heightMap.height;
        for(int i=0; i< heightMap.width; i++)
        {
            for(int j=0; j<heightMap.height; j++)
            {
                float c = heightMap.GetPixel(i, j).r;
                Debug.Log(c);
                GameObject ob = GameObject.Instantiate(tileFill, new Vector3(i * tileX, tileHeight * GetFloor(c), j * tileY), Quaternion.identity);
            }
        }
    }*/






}
