﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Texture2D heightmap;                             // heightmap is the height map, which we get from Textures/Assets
    public Transform Map;                                   // Map is parent of all generated tiles
    public Tile[] _tilePrefabs;                             //References to the tile prefabs
    private Quaternion fixRot = Quaternion.Euler(0, 90.0f, 0); // To rotate tile so it is more similar to the image shown in the exercise

    // Customizable options
    public float tileHeightVar = 40f;
    
    #region Map generation
    private Tile[,] _tileMap; //2D array of all spawned tiles
    #endregion

    #region Buildings
    public GameObject[] _buildingPrefabs; //References to the building prefabs
    public int _selectedBuildingPrefabIndex = 0; //The current index used for choosing a prefab to spawn from the _buildingPrefabs list
    #endregion


    #region Resources
    private Dictionary<ResourceTypes, float> _resourcesInWarehouse = new Dictionary<ResourceTypes, float>(); //Holds a number of stored resources for every ResourceType

    //A representation of _resourcesInWarehouse, broken into individual floats. Only for display in inspector, will be removed and replaced with UI later
    [SerializeField]
    private float _ResourcesInWarehouse_Fish;
    [SerializeField]
    private float _ResourcesInWarehouse_Wood;
    [SerializeField]
    private float _ResourcesInWarehouse_Planks;
    [SerializeField]
    private float _ResourcesInWarehouse_Wool;
    [SerializeField]
    private float _ResourcesInWarehouse_Clothes;
    [SerializeField]
    private float _ResourcesInWarehouse_Potato;
    [SerializeField]
    private float _ResourcesInWarehouse_Schnapps;
    #endregion

    #region Enumerations
    public enum ResourceTypes { None, Fish, Wood, Planks, Wool, Clothes, Potato, Schnapps }; //Enumeration of all available resource types. Can be addressed from other scripts by calling GameManager.ResourceTypes
    #endregion

    #region MonoBehaviour
    void Start()
    {
        initializeTypes();
        generateMap();
        foreach (Tile tile in _tileMap)
        {
            tile._neighborTiles = FindNeighborsOfTile(tile);
        }
        PopulateResourceDictionary();
    }
       void Update()
    {
        // HandleKeyboardInput();
        // UpdateInspectorNumbersForResources();
    }
    #endregion

    #region Methods

    void initializeTypes()
    {
        for (int i = 0; i < 6; i++)
        {
            _tilePrefabs[i]._type = (Tile.TileTypes)i + 1;
        }
    }

    // Generates map
    void generateMap()
    {
        _tileMap =  new Tile[heightmap.width, heightmap.height];
        float offset;

        for (int i = 0; i < heightmap.width; i++)
        {
            for (int j = 0; j < heightmap.height; j++)
            {
                Color col = heightmap.GetPixel(i, j);
                float tileHeight = col.maxColorComponent;
                Tile chosenPrefab = choosePrefab(tileHeight);
                
                if (j % 2 == 0)
                {
                    offset = 0f;
                }

                else
                {
                    offset = 5f;
                }
                
                Tile instTile = Instantiate(chosenPrefab, new Vector3(i * 10f + offset, tileHeight * tileHeightVar, -j * 8f), fixRot, Map);
                _tileMap[i, j] = instTile;
                instTile._coordinateWidth = i;
                instTile._coordinateHeight = j;
                instTile.name += "[" + i + "," + j + "]";
            }
        }
    }

    Tile choosePrefab(float colVal)
    {
        if (colVal == 0f)
        {
            return _tilePrefabs[0];
        }

        else if(colVal > 0f && colVal <= 0.2f)
        {
            return _tilePrefabs[1];
        }

        else if(colVal > 0.2f && colVal <= 0.4f)
        {
            return _tilePrefabs[2];
        }

        else if(colVal > 0.4f && colVal <= 0.6f)
        {
            return _tilePrefabs[3];
        }

        else if(colVal > 0.6f && colVal <= 0.8f)
        {
            return _tilePrefabs[4];
        }
        
        else if(colVal > 0.8f && colVal <= 1f)
        {
            return _tilePrefabs[5];
        }

        else
        {
            Debug.Log("Wrong colVal");
            return null;
        } 
    }

    //Makes the resource dictionary usable by populating the values and keys
    void PopulateResourceDictionary()
    {
        _resourcesInWarehouse.Add(ResourceTypes.None, 0);
        _resourcesInWarehouse.Add(ResourceTypes.Fish, 0);
        _resourcesInWarehouse.Add(ResourceTypes.Wood, 0);
        _resourcesInWarehouse.Add(ResourceTypes.Planks, 0);
        _resourcesInWarehouse.Add(ResourceTypes.Wool, 0);
        _resourcesInWarehouse.Add(ResourceTypes.Clothes, 0);
        _resourcesInWarehouse.Add(ResourceTypes.Potato, 0);
        _resourcesInWarehouse.Add(ResourceTypes.Schnapps, 0);
    }

    //Sets the index for the currently selected building prefab by checking key presses on the numbers 1 to 0
    void HandleKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _selectedBuildingPrefabIndex = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _selectedBuildingPrefabIndex = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _selectedBuildingPrefabIndex = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            _selectedBuildingPrefabIndex = 3;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            _selectedBuildingPrefabIndex = 4;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            _selectedBuildingPrefabIndex = 5;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            _selectedBuildingPrefabIndex = 6;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            _selectedBuildingPrefabIndex = 7;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            _selectedBuildingPrefabIndex = 8;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            _selectedBuildingPrefabIndex = 9;
        }
    }

    //Updates the visual representation of the resource dictionary in the inspector. Only for debugging
    void UpdateInspectorNumbersForResources()
    {
        _ResourcesInWarehouse_Fish = _resourcesInWarehouse[ResourceTypes.Fish];
        _ResourcesInWarehouse_Wood = _resourcesInWarehouse[ResourceTypes.Wood];
        _ResourcesInWarehouse_Planks = _resourcesInWarehouse[ResourceTypes.Planks];
        _ResourcesInWarehouse_Wool = _resourcesInWarehouse[ResourceTypes.Wool];
        _ResourcesInWarehouse_Clothes = _resourcesInWarehouse[ResourceTypes.Clothes];
        _ResourcesInWarehouse_Potato = _resourcesInWarehouse[ResourceTypes.Potato];
        _ResourcesInWarehouse_Schnapps = _resourcesInWarehouse[ResourceTypes.Schnapps];
    }

    //Checks if there is at least one material for the queried resource type in the warehouse
    public bool HasResourceInWarehoues(ResourceTypes resource)
    {
        return _resourcesInWarehouse[resource] >= 1;
    }

    //Is called by MouseManager when a tile was clicked
    //Forwards the tile to the method for spawning buildings
    public void TileClicked(int height, int width)
    {
        Tile t = _tileMap[height, width];

        PlaceBuildingOnTile(t);
    }

    //Checks if the currently selected building type can be placed on the given tile and then instantiates an instance of the prefab
    private void PlaceBuildingOnTile(Tile t)
    {
        //if there is building prefab for the number input
        if (_selectedBuildingPrefabIndex < _buildingPrefabs.Length)
        {
            //TODO: check if building can be placed and then istantiate it

        }
    }

    //Returns a list of all neighbors of a given tile
    private List<Tile> FindNeighborsOfTile(Tile t)
    {
        List<Tile> result = new List<Tile>();
        
        int height = t._coordinateHeight;
        int width = t._coordinateWidth;

        if (width == 0) // Left column
        {
            result.Add(_tileMap[width + 1, height]);
            if (height == 0) // Upper left corner
            {
                result.Add(_tileMap[width, height + 1]);
                result.Add(_tileMap[width + 1, height + 1]);
            }
            
            else if (height == heightmap.height - 1) // Lower left corner
            {
                result.Add(_tileMap[width, height - 1]);
                result.Add(_tileMap[width + 1, height - 1]);
            }
            else // Side tiles left column
            {
                result.Add(_tileMap[width, height + 1]);
                result.Add(_tileMap[width + 1, height + 1]);
                result.Add(_tileMap[width, height - 1]);
                result.Add(_tileMap[width + 1, height - 1]);
            }
        }
        else if (width == heightmap.width - 1) // Last column
        {
            result.Add(_tileMap[width - 1, height]);
            if (height == 0) // Upper right corner
            {
                result.Add(_tileMap[width, height + 1]);
                result.Add(_tileMap[width - 1, height + 1]);
            }
            else if (height == heightmap.height - 1) // Lower right corner
            {
                result.Add(_tileMap[width, height - 1]);
                result.Add(_tileMap[width - 1, height - 1]);
            }
            else // Side tiles right column
            {
                result.Add(_tileMap[width, height + 1]);
                result.Add(_tileMap[width - 1, height + 1]);
                result.Add(_tileMap[width, height - 1]);
                result.Add(_tileMap[width - 1, height - 1]);
            }
        }
        else
        {
            result.Add(_tileMap[width - 1, height]);
            result.Add(_tileMap[width + 1, height]);
            if (height == 0) // Side tiles top row
            {
                result.Add(_tileMap[width - 1, height + 1]);    
                result.Add(_tileMap[width, height + 1]);
                result.Add(_tileMap[width + 1, height + 1]);
            }
            else if (height == heightmap.height - 1) // Side tiles lower row
            {
                result.Add(_tileMap[width - 1, height - 1]);    
                result.Add(_tileMap[width, height - 1]);
                result.Add(_tileMap[width + 1, height - 1]);
            }
            else // Inner tiles
            {
                result.Add(_tileMap[width - 1, height + 1]);    
                result.Add(_tileMap[width, height + 1]);
                result.Add(_tileMap[width + 1, height + 1]);
                result.Add(_tileMap[width - 1, height - 1]);    
                result.Add(_tileMap[width, height - 1]);
                result.Add(_tileMap[width + 1, height - 1]);
            }
        }
        return result;
    }

    #endregion
}
