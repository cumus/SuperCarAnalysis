using UnityEngine;
using System.IO;

public static class CSV_Manager
{
    private static string csvDirectoryName = "Analysis/CSV_Files";

    private static string csvFileSessions = "sessions.csv";
    private static string csvFileCrashes = "crashes.csv";
    private static string csvFilePositions = "positions.csv";
    private static string csvFileLaps = "laps.csv";

    private static string csvSeparator = ";";

    private static string[] csvSessionsHeaders = new string[4] {
        "session_id",
        "username",
        "session_start",
        "session_end"
    };
    private static string[] csvCrashesHeaders = new string[7] {
        "username",
        "crash_id",
        "positon",
        "current_lap",
        "time",
        "session_id",
        "collision_obj_id"
    };
    private static string[] csvPositionsHeaders = new string[6] {
        "session_id",
        "username",
        "time",
        "position",
        "velocity",
        "rotation"

    };
    private static string[] csvLapsHeaders = new string[4] {
        "lap_id",
        "session_id",
        "username",
        "time"
    };

    public static void AppendToCSV(string[] strings)
    {
        //Check if folder exists
        string directory_path = Application.dataPath + "/" + csvDirectoryName;
        if (!Directory.Exists(directory_path)){
            Directory.CreateDirectory(directory_path);
        }
        //Check if files exist
        AllCSVExists();
    }
    static void AllCSVExists()
    {
        //Path
        string directory_path = Application.dataPath + "/" + csvDirectoryName;
        if (!Directory.Exists(directory_path))
        {
            Directory.CreateDirectory(directory_path);
        }
        //
        string csv_sessions = directory_path + "/" + csvFileSessions;
        if (!File.Exists(csv_sessions))
        {
            CreateCSVFile(csv_sessions, csvSessionsHeaders);
        }
        //
        string csv_crashes = directory_path + "/" + csvFileCrashes;
        if (!File.Exists(csv_crashes))
        {
            CreateCSVFile(csv_crashes, csvCrashesHeaders);
        }
        //
        string csv_positions = directory_path + "/" + csvFilePositions;
        if (!File.Exists(csv_positions))
        {
            CreateCSVFile(csv_positions, csvPositionsHeaders);
        }
        //
        string csv_laps = directory_path + "/" + csvFileLaps;
        if (!File.Exists(csv_laps))
        {
            CreateCSVFile(csv_laps, csvLapsHeaders);
        }
    }

    static void CreateCSVFile(string path, string[] data)
    {
        using(StreamWriter sw = File.CreateText(path))
        {
            string endString = "";
            for(int i = 0; i < data.Length; i++)
            {
                if(endString != "")
                {
                    endString += csvSeparator;
                }
                endString += data[i]; 
            }
        }
    }
}
