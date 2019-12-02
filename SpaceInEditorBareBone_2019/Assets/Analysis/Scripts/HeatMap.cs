using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
public class HeatMap : MonoBehaviour
{
    public enum HeatmapSHOW
    {
        DENSITY,
        AVERAGE_SPEED
    };
    private Material[,] mat;
    public GameObject HeatMapCube;
    private float map_size_x = 500.0f;
    private float map_size_z = 500.0f;
    [Space]
    [Header("Max grid and recommend size is 23")]
    public int grid_size_x = 23;
    public int grid_size_z = 23;
    private int counter_materials;
    public Material HeatMapMaterial;
    [HideInInspector] public HeatmapSHOW type;
    [HideInInspector] public float y_scale = 10.0f;
    [HideInInspector] public bool normalized = false;
    [HideInInspector] public bool hide_empty = false;

    [HideInInspector] public int entries_count;
    [HideInInspector] public float max_speed;
    [HideInInspector] public float max_entries_per_grid;
    [HideInInspector] public float max_average_speed;

    private GameObject[,] heatMapsArray = null;
    private float[,,] grid_data = null;

    private int current_grid_size_x;
    private int current_grid_size_z;
    private float size_x { get { return map_size_x / (float)current_grid_size_x; } }
    private float size_z { get { return map_size_z / (float)current_grid_size_z; } }

    public bool GridCreated()
    {
        return heatMapsArray != null;
    }

    public void ClearGrid()
    {
        Transform[] transforms = GetComponentsInChildren<Transform>(true);

        foreach (Transform t in transforms)
        {
            if (t.gameObject != gameObject)
            {
                DestroyImmediate(t.gameObject, true);
            }
        }
        mat = null;
        heatMapsArray = null;
        grid_data = null;
    }

    public void ReloadGrid()
    {
        ClearGrid();
        BuildGrid();
    }

    public void BuildGrid()
    {
        // Update user defined size
        if(grid_size_x > 24)
        {
            grid_size_x = 23;
        }
        if(grid_size_z > 24)
        {
            grid_size_z = 23;
        }
        current_grid_size_x = grid_size_x;
        current_grid_size_z = grid_size_z;

        // Instanciate childs and data
        heatMapsArray = new GameObject[current_grid_size_x, current_grid_size_z];
        grid_data = new float[current_grid_size_x, current_grid_size_z, 2];
        entries_count = 0;
        max_speed = 0.0f;
        mat = new Material[current_grid_size_x,current_grid_size_z];
        for (int i = 0; i < current_grid_size_x; i++)
        {
            for (int j = 0; j < current_grid_size_z; j++)
            {
                
                // Instanciate childs
                heatMapsArray[i, j] = Instantiate(
                    HeatMapCube,
                    new Vector3(((float)i * size_x) + (size_x / 2.0f), 0.01f, ((float)j * size_z) + (size_z / 2.0f)),
                    Quaternion.identity,
                    transform);
               
                mat[i,j] = new Material(heatMapsArray[i, j].GetComponent<Renderer>().sharedMaterial);
                heatMapsArray[i, j].GetComponent<Renderer>().sharedMaterial = (Material)Instantiate(mat[i, j] );
                heatMapsArray[i, j].transform.localScale = new Vector3(size_x, 0.005f, size_z);

                // Reset grid_data
                grid_data[i, j, 0] = 0;
                grid_data[i, j, 1] = 0;
            }
        }


        // fill grid_data
        ReadCSVPosition();

        // Setup cubes with grid_data
        UpdateMarkers();
    }

    void ReadCSVPosition()
    {
        try // trigger error on index-out-of-range
        {
            // Read CSV
            StreamReader strReader = new StreamReader("Assets/Analysis/CSV_Files/positions.csv");
            string line = strReader.ReadLine();
            for (line = strReader.ReadLine(); line != null; line = strReader.ReadLine())
            {
                // Separate string values
                int firstPos = 0;
                int endPos = 0;
                string[] values = new string[8];
                for (int i = 0; i < 8; i++)
                {
                    firstPos = line.IndexOf(';', endPos);
                    endPos = line.IndexOf(';', firstPos + 1);
                    values[i] = line.Substring(firstPos + 1, endPos - firstPos - 1);
                }

                // Parse string values
                float posX = float.Parse(values[2]);
                float posZ = float.Parse(values[4]);
                float speedX = float.Parse(values[5]);
                float speedZ = float.Parse(values[7]);

                // Calculate corresponding cell
                int grid_pos_x = (int)(posX / size_x);
                int grid_pos_z = (int)(posZ / size_z);

                // Check if within limits
                if (grid_pos_x < size_x && grid_pos_z < size_z)
                {
                    // Save count
                    entries_count++;
                    grid_data[grid_pos_x, grid_pos_z, 0]++;

                    // Save speed
                    float speed = Mathf.Sqrt((speedX * speedX) + (speedZ * speedZ));
                    max_speed = Mathf.Max(speed, max_speed);
                    grid_data[grid_pos_x, grid_pos_z, 1] += speed;
                }
            }

            // Calculate max entries and max avg speed
            for (int i = 0; i < current_grid_size_x; i++)
            {
                for (int j = 0; j < current_grid_size_z; j++)
                {
                    max_entries_per_grid = Mathf.Max(grid_data[i, j, 0], max_entries_per_grid);
                    max_average_speed = Mathf.Max(grid_data[i, j, 1] / grid_data[i, j, 0], max_average_speed);

                }
            }
        }
        catch
        {
            Debug.Log("Error reading");
        }
    }

    void UpdateMarkers()
    {
        for (int i = 0; i < current_grid_size_x; i++)
        {
            for (int j = 0; j < current_grid_size_z; j++)
            {
                float height = 0.01f;

                if (grid_data[i, j, 0] > 0)
                {
                    switch (type)
                    {
                        case HeatmapSHOW.DENSITY:
                            {
                                if (normalized)
                                    height = grid_data[i, j, 0] / (float)entries_count;
                                else
                                    height = grid_data[i, j, 0] / max_entries_per_grid;

                                break;
                            }
                        case HeatmapSHOW.AVERAGE_SPEED:
                            {
                                if (normalized)
                                    height = (grid_data[i, j, 1] / grid_data[i, j, 0]) / max_speed;

                                else
                                    height = (grid_data[i, j, 1] / grid_data[i, j, 0]) / max_average_speed;

                                break;
                            }
                    }
                    height *= y_scale;
                }

                heatMapsArray[i, j].GetComponent<Renderer>().sharedMaterial.color = new Color(1.0f - height/5, height/5, 0, 0.7f);

                heatMapsArray[i, j].transform.localScale = new Vector3(
                    size_x,
                    height * y_scale,
                    size_z);

                heatMapsArray[i, j].transform.position = new Vector3(
                    ((float)i * size_x) + (size_x / 2.0f),
                    height * y_scale * 0.5f,
                    ((float)j * size_z) + (size_z / 2.0f));

                // Set cube visibility
                heatMapsArray[i, j].SetActive(!hide_empty || grid_data[i, j, 0] > 0);
            }
        }
    }

    public void SetType(HeatmapSHOW t)
    {

        if (type != t)
        {
            type = t;
            UpdateMarkers();
        }
    }

    public void UpdateYScale(float new_scale)
    {

        if (y_scale != new_scale)
        {
            y_scale = new_scale;
            UpdateMarkers();
        }
    }

    public void ToggleNormalized(bool n)
    {
        if (normalized != n)
        {
            normalized = n;
            UpdateMarkers();
        }
    }

    public void ToggleHideEmpty(bool h)
    {
        if (hide_empty != h)
        {
            hide_empty = h;
            UpdateMarkers();
        }
    }
}
