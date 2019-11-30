using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Obstacle : MonoBehaviour
{
    public uint collision_obj_id;

    private GameObject car;
    private bool finished = false;

    void Start()
    {
        car = SceneManager.SM.car;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Car" && !finished)
        {
            CSV_Manager.AppendToCSV(CrashesData(), CSV_Manager.typeDataCSV.CRASHES);

            finished = true;
            StartCoroutine(ResetTag());
        }
    }

    private IEnumerator ResetTag()
    {
        yield return new WaitForSeconds(3);
        finished = false;
    }

    string[] CrashesData()
    {
        string[] data = new string[9];

        data[0] = SceneManager.SM.GetUsername();

        data[1] = SceneManager.SM.AddCrashGetCount().ToString();

        data[2] = car.transform.position.x.ToString();
        data[3] = car.transform.position.y.ToString();
        data[4] = car.transform.position.z.ToString();

        data[5] = SceneManager.SM.GetCurrentLapCount().ToString();

        data[6] = DateTime.Now.ToString();

        data[7] = SceneManager.SM.GetSessionID().ToString();

        data[8] = collision_obj_id.ToString();

        return data;
    }
}
