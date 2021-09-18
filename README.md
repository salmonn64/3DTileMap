# 3DTileMap
 Tool for creating tile maps with height maps in unity.
 
 ![image](https://user-images.githubusercontent.com/50729585/133871984-f93c09e3-5d73-4fa6-97df-25de5900893c.png)

 
## Heightmap setup.
 Heightmap needs to have the following import options:
 * Read/Write Enabled = true.
 * Non power of 2 as "None"
 * Filter mode: Point (no filter).
 
![image](https://user-images.githubusercontent.com/50729585/133870144-e75403bf-ba09-4c3d-982f-1749bf25a7bf.png)

## Using the script.
Create an empty object and add the Tile Map 3D component.

![image](https://user-images.githubusercontent.com/50729585/133870347-2d5e8d36-9acf-444a-aca5-a38484714ca7.png)

You will need your heightmap and a prefabs of your tiles. Set the corresponding prefab in the inspector.

![Tiles](https://user-images.githubusercontent.com/50729585/133871287-5144395e-7bd8-4d1d-b670-eb816baf0414.png)

Now, you have to specify the x,y and z dimensions of your tiles ( height is the y dimension ), the example tiles dimensions are 2x2x2.

![image](https://user-images.githubusercontent.com/50729585/133871493-8ee807ff-98bd-4e81-a5f0-24c73cf41fcc.png)

Now you have to set the number of floors. If you set this value to *x* then the color of every pixel in your heightmap will be rounded to the closest *m := (1/x)k* and the tilemap will have a height of *k* floors for that element of the grid.

Number of floors = 5

![image](https://user-images.githubusercontent.com/50729585/133871807-d697663c-2464-4e4d-8f5c-e9df069b0c7c.png)

Number of floors = 15

![image](https://user-images.githubusercontent.com/50729585/133871848-65fa01b2-9a3c-4573-98de-5d19516a1ff6.png)

When everything is set up, click the button *Create*.

![image](https://user-images.githubusercontent.com/50729585/133870295-89ddcae8-2685-4bcb-bf86-89c0d0c6a018.png)




