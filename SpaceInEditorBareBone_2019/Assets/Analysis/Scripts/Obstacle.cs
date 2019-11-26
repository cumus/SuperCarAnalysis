using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Obstacle : MonoBehaviour {
    public Goal data_goal;
    public CSV_Data data_csv;
    public GameObject car;
    public struct crashCSVData
    {
        public string player_name;
        public uint crash_id;
        public float x, y, z;
        public uint current_lap;
        public Time time;
        public uint session_id;
        public uint collision_obj;
    }
    public uint collision_obj;
    private crashCSVData current_crash_data;
    bool finished = false;
    void Start()
    {
        current_crash_data.collision_obj = collision_obj;
    }
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Car" && !finished)
        {
           
            CSV_Manager.AppendToCSV(CrashesData(), CSV_Manager.typeDataCSV.CRASHES);
            current_crash_data.crash_id++;
            StartCoroutine(ResetTag());
            finished = true;
            Debug.Log("HOLA");

        }
    }
    private IEnumerator ResetTag()
    {
        yield return new WaitForSeconds(3);
        finished = false;
    }
    string[] CrashesData()
    {
        string[] data = new string[7];
        data[0] = data_csv.GetPlayerName();
        data[1] = current_crash_data.crash_id.ToString();

        //pos
        string car_pos_x = car.transform.position.x.ToString();
        car_pos_x = car_pos_x.Replace(",", ".");
        string car_pos_y = car.transform.position.y.ToString();
        car_pos_y = car_pos_x.Replace(",", ".");
        string car_pos_z = car.transform.position.z.ToString();
        car_pos_z = car_pos_x.Replace(",", ".");

        data[2] = "(" + car_pos_x + "," + car_pos_y + "," + car_pos_z + ")";
        data[3] = data_goal.GetLaps().ToString();
        data[4] = DateTime.Now.ToString();
        data[5] = data_csv.GetSessionID().ToString();
        data[6] = current_crash_data.collision_obj.ToString();

        return data;
    }
}
