using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Goal : MonoBehaviour {
    public CSV_Data data_csv;

    bool _finished;

    private lapsCSVData current_laps_data;

    DateTime race_started;

    [System.Serializable]
    public struct lapsCSVData
    {
        public uint lap_id;
        public string player_name;
        public string session_start;
        public string session_end;
    }

    // Use this for initialization
    void Start () {
        current_laps_data.lap_id = 0;
        race_started = DateTime.Now;
    }
	
	// Update is called once per frame
	void Update () {
	}

    void OnTriggerEnter(Collider col)
    {
        if ((col.tag == "Player" || col.transform.root.tag == "Player" ) && !_finished)
        {
            current_laps_data.lap_id++;
            
            CSV_Manager.AppendToCSV(lapData(), CSV_Manager.typeDataCSV.LAPS);

            _finished = true;            
            StartCoroutine(ResetTag());
            race_started = DateTime.Now;
        }
    }

    private IEnumerator ResetTag()
    {
        yield return new WaitForSeconds(3);
        _finished = false;
    }

    public uint GetLaps()
    {
        return current_laps_data.lap_id;
    }
    string[] lapData()
    {
        string[] data = new string[4];
        data[0] = current_laps_data.lap_id.ToString();
        data[1] = data_csv.GetSessionID().ToString();
        data[2] = data_csv.GetPlayerName();
        data[3] = (DateTime.Now - race_started).ToString();

        return data;
    }
}
