using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap3D : MonoBehaviour
{
    [SerializeField]
    public Texture2D heightMap;

    [SerializeField]
    public GameObject tileTopFill;

    [SerializeField]
    public GameObject tileTopCorner;

    [SerializeField]
    public GameObject tileTopSide;

    [SerializeField]
    public GameObject tileTopInner;

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
