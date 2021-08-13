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
    public GameObject tileMiddleFill;

    [SerializeField]
    public GameObject tileMiddleCorner;

    [SerializeField]
    public float tileX;

    [SerializeField]
    public float tileZ;

    [SerializeField]
    public float tileHeight;

    [SerializeField]
    public int numberOfFloors;
    
}
