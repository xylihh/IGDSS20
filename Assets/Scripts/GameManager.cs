using UnityEngine;

public class GameManager : MonoBehaviour
{
    public MouseManager mouseMan;   // Initialize mouseManager to initialize settings according to generated Map
    public Texture2D heightmap;         // heightmap is the height map, which we get from Textures/Assets
    public Transform Map;               // Map is parent of all generated tiles

    // Initialize tile prefabs
    public Object waterPrefab;
    public Object sandPrefab;
    public Object grassPrefab;
    public Object forestPrefab;
    public Object stonePrefab;
    public Object mountainPrefab;

    // To rotate tile so it is more similar to the image shown in the exercise
    private Quaternion fixRot = Quaternion.Euler(0, 90, 0);

    // Generates map
    void Start()
    {
        mouseMan.panLimitX = new Vector2(-50, 180);
        mouseMan.panLimitZ = new Vector2(-250, 0);
        generateMap();
    }

    void generateMap()
    {
        float offset;
        for (int i = 0; i < heightmap.width; i++)
        {
            for (int j = 0; j < heightmap.height; j++)
            {

                Color col = heightmap.GetPixel(i, j);
                Object chosenPrefab = choosePrefab(col.maxColorComponent);
                
                if (j % 2 == 0)
                {
                    offset = 0f;
                }

                else
                {
                    offset = 5f;
                }
                


                Instantiate(chosenPrefab, new Vector3(i * 10f + offset, 0, -j * 8f), fixRot, Map);
            }
        }
    }

    Object choosePrefab(float colVal)
    {
        if (colVal == 0f)
        {
            return waterPrefab;
        }

        else if(colVal > 0f && colVal <= 0.2f)
        {
            return sandPrefab;
        }

        else if(colVal > 0.2f && colVal <= 0.4f)
        {
            return grassPrefab;
        }

        else if(colVal > 0.4f && colVal <= 0.6f)
        {
            return forestPrefab;
        }

        else if(colVal > 0.6f && colVal <= 0.8f)
        {
            return stonePrefab;
        }
        
        else if(colVal > 0.8f && colVal <= 1f)
        {
            return grassPrefab;
        }

        else
        {
            Debug.Log("Wrong colVal");
            return null;
        } 
    }
}
