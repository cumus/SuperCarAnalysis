using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CSV_Position : MonoBehaviour
{
    // HeatMap
    private GameObject HeatMapParent;
    public GameObject CubeHeatMap;

    public float trigger_frecuency = 1.0f;

    private GameObject car;
    private Rigidbody car_rb;

    private string session_id;
    private string username;

    void Start()
    {
        car = SceneManager.SM.car;
        car_rb = car.GetComponent<Rigidbody>();
        HeatMapParent = new GameObject("HeatMapParent");
    }

    void CSVWrite()
    {
        //GameObject heatMapCube = Instantiate(CubeHeatMap, new Vector3(car.transform.position.x, CubeHeatMap.transform.localScale.y / 2, car.transform.position.z), Quaternion.identity, HeatMapParent.transform);

        CSV_Manager.AppendToCSV(PosData(), CSV_Manager.typeDataCSV.POSITIONS);
    }

    string[] PosData()
    {
        string[] data = new string[14];

        data[0] = session_id;

        data[1] = username;

        data[2] = DateTime.Now.ToString();

        data[3] = car.transform.position.x.ToString();
        data[4] = car.transform.position.y.ToString();
        data[5] = car.transform.position.z.ToString();

        data[6] = car_rb.velocity.x.ToString();
        data[7] = car_rb.velocity.y.ToString();
        data[8] = car_rb.velocity.z.ToString();

        data[9] = car.transform.rotation.x.ToString();
        data[10] = car.transform.rotation.y.ToString();
        data[11] = car.transform.rotation.z.ToString();
        data[12] = car.transform.rotation.w.ToString();

        data[13] = SceneManager.SM.GetCurrentLapCount().ToString();

        return data;
    }

    public void BeginRace(string _username, string _session_id)
    {
        session_id = _session_id;
        username = _username;
        InvokeRepeating("CSVWrite", 0.0f, trigger_frecuency);
    }

    void OnApplicationQuit()
    {
        print("Creating HeatMap Prefab");
        PrefabUtility.SaveAsPrefabAsset(HeatMapParent, "Assets/HeatMapLastGame.prefab");
    }
}
