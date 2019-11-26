using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSV_Position : MonoBehaviour
{
    public CSV_Data data_csv;
    public GameObject car;
    public Rigidbody car_rb;
    public float writeToCSVEverySecond = 5.0f;
    public struct positionCSVData
    {
        public uint session_id;
        public string player_name;
        public Time time;
        public float x, y, z;
        public float vel_x, vel_y, vel_z;
        public float rot_x, rot_y, rot_z;
    }

    private positionCSVData current_pos_data;
    void Start()
    {
        InvokeRepeating("CSVWrite", 2.0f, writeToCSVEverySecond);
    }

    // Update is called once per frame
    void Update()
    {
    }
    void CSVWrite()
    {
        CSV_Manager.AppendToCSV(PosData(), CSV_Manager.typeDataCSV.POSITIONS);
        Debug.Log("Writing for pos");
    }
    string[] PosData()
    {
        string[] data = new string[6];

        data[0] = data_csv.GetSessionID().ToString();
        data[1] = data_csv.GetPlayerName();
        data[2] = DateTime.Now.ToString();
        //pos
        string car_pos_x = car.transform.position.x.ToString();
        car_pos_x = car_pos_x.Replace(",", ".");
        string car_pos_y = car.transform.position.y.ToString();
        car_pos_y = car_pos_x.Replace(",", ".");
        string car_pos_z = car.transform.position.z.ToString();
        car_pos_z = car_pos_x.Replace(",", ".");

        data[3] = "(" + car_pos_x + "," + car_pos_y + "," + car_pos_z + ")";
        //Vel
        string car_vel_x = car_rb.velocity.x.ToString();
        car_vel_x = car_vel_x.Replace(",", ".");
        string car_vel_y = car_rb.velocity.y.ToString();
        car_vel_y = car_vel_y.Replace(",", ".");
        string car_vel_z = car_rb.velocity.z.ToString();
        car_vel_z = car_vel_z.Replace(",", ".");

        data[4] = "(" + car_vel_x + "," + car_vel_y + "," + car_vel_z + ")";
        //Rot
        string car_rot_x = car.transform.rotation.x.ToString();
        car_rot_x = car_rot_x.Replace(",", ".");
        string car_rot_y = car.transform.rotation.y.ToString();
        car_rot_y = car_rot_y.Replace(",", ".");
        string car_rot_z = car.transform.rotation.z.ToString();
        car_rot_z = car_rot_z.Replace(",", ".");
        string car_rot_w = car.transform.rotation.w.ToString();
        car_rot_w = car_rot_w.Replace(",", ".");

        data[5] = "(" + car_rot_x + "," + car_rot_y + "," + car_rot_z + "," + car_rot_w + ")";

        return data;
    }
}
