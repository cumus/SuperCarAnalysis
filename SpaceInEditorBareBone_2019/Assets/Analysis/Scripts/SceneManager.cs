using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SceneManager : MonoBehaviour
{
    // keep the static reference to singleton private
    [HideInInspector] public static SceneManager SM;
    
    public GameObject cam2;
    public GameObject canvas;
    public InputField inputField;
    public GameObject car;
    public Goal goal;

    private string username;
    private string session_start;
    private string session_end;

    private uint current_lap;

    public uint total_registered_sessions;
    public uint total_registered_laps;
    public uint total_registered_crashes;

    void Awake()
    {
        SM = this;

        uint[] count = CSV_Manager.AllCSVExists();
        total_registered_sessions = count[0];
        total_registered_laps = count[1];
        total_registered_crashes = count[2];
    }
    
    void Start()
    {
        car.SetActive(false);
    }

    void OnApplicationQuit()
    {
        session_end = DateTime.Now.ToString();

        CSV_Manager.AppendToCSV(SessionsData(), CSV_Manager.typeDataCSV.SESSIONS);
    }

    string[] SessionsData()
    {
        string[] data = new string[4];

        data[0] = total_registered_sessions.ToString();
        data[1] = username;
        data[2] = session_start;
        data[3] = session_end;

        return data;
    }

    public void PlayPressed()
    {
        if (inputField.text.Length > 0)
        {
            cam2.SetActive(false);
            canvas.SetActive(false);
            car.SetActive(true);

            username = inputField.text;
            session_start = DateTime.Now.ToString();
            session_end = DateTime.Now.ToString();

            gameObject.GetComponent<CSV_Position>().BeginRace(total_registered_sessions.ToString(), username);
            goal.BeginRace(total_registered_sessions.ToString(), username);
        }
    }

    public uint GetSessionID() { return total_registered_sessions; }

    public string GetUsername() { return username; }

    public uint GetCurrentLapCount() { return current_lap; }
    
    public uint AddCrashGetCount() { return ++total_registered_crashes; }

    public uint AddLapGetCount()
    {
        current_lap++;
        return ++total_registered_laps;
    }
}
