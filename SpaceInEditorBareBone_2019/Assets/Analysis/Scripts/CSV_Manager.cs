using UnityEngine;
using System.IO;

public static class CSV_Manager
{
    public enum typeDataCSV
    {
        SESSIONS,
        CRASHES,
        POSITIONS,
        LAPS
    }
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

    public static void AppendToCSV(string[] strings, typeDataCSV type_data)
    {
        //Check if folder exists
        string directory_path = Application.dataPath + "/" + csvDirectoryName;
        if (!Directory.Exists(directory_path))
        {
            Directory.CreateDirectory(directory_path);
        }
        //Check if files exist
        AllCSVExists();
        switch (type_data)
        {
            case typeDataCSV.SESSIONS:
                using (StreamWriter sw = File.AppendText(Application.dataPath + "/" + csvDirectoryName + "/" + csvFileSessions))
                {
                    string endString = "";
                    for (int i = 0; i < strings.Length; i++)
                    {
                        if (endString != "")
                        {
                            endString += csvSeparator;
                        }
                        endString += strings[i];
                    }
                    sw.WriteLine(endString);
                }
                break;
            case typeDataCSV.LAPS:
                using (StreamWriter sw = File.AppendText(Application.dataPath + "/" + csvDirectoryName + "/" + csvFileLaps))
                {
                    string endString = "";
                    for (int i = 0; i < strings.Length; i++)
                    {
                        if (endString != "")
                        {
                            endString += csvSeparator;
                        }
                        endString += strings[i];
                    }
                    sw.WriteLine(endString);
                }
                break;

            case typeDataCSV.CRASHES:
                using (StreamWriter sw = File.AppendText(Application.dataPath + "/" + csvDirectoryName + "/" + csvFileCrashes))
                {
                    string endString = "";
                    for (int i = 0; i < strings.Length; i++)
                    {
                        if (endString != "")
                        {
                            endString += csvSeparator;
                        }
                        endString += strings[i];
                    }
                    sw.WriteLine(endString);
                }
                break;

            case typeDataCSV.POSITIONS:
                using (StreamWriter sw = File.AppendText(Application.dataPath + "/" + csvDirectoryName + "/" + csvFilePositions))
                {
                    string endString = "";
                    for (int i = 0; i < strings.Length; i++)
                    {
                        if (endString != "")
                        {
                            endString += csvSeparator;
                        }
                        endString += strings[i];
                    }
                    sw.WriteLine(endString);
                }
                break;
        }
    }
    
   
    public static void AllCSVExists()
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
            sw.WriteLine(endString);
        }
    }
    public static uint NumberSessionsUser(string name)
    {
        uint counter = 0;

        StreamReader strReader = new StreamReader(Application.dataPath + "/" + csvDirectoryName + "/" + csvFileSessions);
        bool finish = false;

        while (!finish)
        {
            string data = strReader.ReadLine();
            if (data == null)
            {
                finish = true;
                break;
            }
            var data_values = data.Split(';');

            if (data_values[1].ToString() == name)
            {
                counter++;
            }
        }
        return counter;
    }

}
