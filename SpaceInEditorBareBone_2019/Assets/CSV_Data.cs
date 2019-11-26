﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CSV_Data : MonoBehaviour
{
    public static CSV_Data instance;
    public string user_name;
    //Sessions

    private SessionsCSVData current_sessions_data;

    [System.Serializable]
    public struct SessionsCSVData
    {
        public int session_id;
        public string player_name;
        public string session_start;
        public string session_end;
    }
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        current_sessions_data.session_id = 0;
        current_sessions_data.player_name = user_name;
        current_sessions_data.session_start = DateTime.Now.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnApplicationQuit()
    {
       CSV_Manager.AppendToCSV(SessionsData(), CSV_Manager.typeDataCSV.SESSIONS);
    }

    string[] SessionsData() {
        current_sessions_data.session_end = DateTime.Now.ToString();
        string[] data = new string[4];
        data[0] = current_sessions_data.session_id.ToString();
        data[1] = current_sessions_data.player_name.ToString();
        data[2] = current_sessions_data.session_start.ToString();
        data[3] = current_sessions_data.session_end.ToString();

        return data;
    }
}

