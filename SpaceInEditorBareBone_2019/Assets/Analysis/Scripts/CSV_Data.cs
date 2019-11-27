using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CSV_Data : MonoBehaviour
{
    private SessionsCSVData current_sessions_data;

    [System.Serializable]
    public struct SessionsCSVData
    {
        public uint session_id;
        public string player_name;
        public string session_start;
        public string session_end;
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        GetSessionID();
    }
    void OnApplicationQuit()
    {
       CSV_Manager.AppendToCSV(SessionsData(), CSV_Manager.typeDataCSV.SESSIONS);
    }
    public uint GetSessionID()
    {
        return current_sessions_data.session_id;
    }
    public string GetPlayerName()
    {
        return current_sessions_data.player_name;
    }

    string[] SessionsData() {
        current_sessions_data.session_end = DateTime.Now.ToString();
        string[] data = new string[4];
        data[0] = current_sessions_data.session_id.ToString();
        data[1] = current_sessions_data.player_name;
        data[2] = current_sessions_data.session_start;
        data[3] = current_sessions_data.session_end;

        return data;
    }

    public void BeginRace(string user_name)
    {
        current_sessions_data.player_name = user_name;
        current_sessions_data.session_id = CSV_Manager.NumberSessionsUser(user_name);
        current_sessions_data.session_start = DateTime.Now.ToString();
    }
}

