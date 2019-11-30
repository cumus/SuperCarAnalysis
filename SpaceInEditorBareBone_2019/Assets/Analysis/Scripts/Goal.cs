using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Goal : MonoBehaviour
{
    private DateTime lap_started;
    private bool recently_finished = false;

    private string session_id;
    private string username;

    void OnTriggerEnter(Collider col)
    {
        if (!recently_finished && col.tag == "Player")
        {
            CSV_Manager.AppendToCSV(lapData(), CSV_Manager.typeDataCSV.LAPS);

            lap_started = DateTime.Now;
            recently_finished = true;
            StartCoroutine(ResetTag());
        }
    }

    private IEnumerator ResetTag()
    {
        yield return new WaitForSeconds(5);
        recently_finished = false;
    }

    string[] lapData()
    {
        string[] data = new string[4];

        data[0] = SceneManager.SM.AddLapGetCount().ToString();

        data[1] = session_id;

        data[2] = username;

        data[3] = (DateTime.Now - lap_started).ToString();

        return data;
    }

    public void BeginRace(string _session_id, string _username)
    {
        session_id = _session_id;
        username = _username;
        lap_started = DateTime.Now;
    }
}
