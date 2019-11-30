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
    
    private static string[] csvSessionsHeaders = new string[4] {
        "session_id",
        "username",
        "session_start",
        "session_end"
    };
    private static string[] csvCrashesHeaders = new string[9] {
        "username",
        "crash_id",
        "position_x",
        "position_y",
        "position_z",
        "current_lap",
        "time",
        "session_id",
        "collision_obj_id"
    };
    private static string[] csvPositionsHeaders = new string[14] {
        "session_id",
        "username",
        "time",
        "position_x",
        "position_y",
        "position_z",
        "velocity_x",
        "velocity_y",
        "velocity_z",
        "rotation_x",
        "rotation_y",
        "rotation_z",
        "rotation_w",
        "current_lap"
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
        string path = Application.dataPath + "/" + csvDirectoryName;
        if (Directory.Exists(path))
        {
            switch (type_data)
            {
                case typeDataCSV.SESSIONS:
                    path += "/" + csvFileSessions;
                    AppendData(strings, path);
                    break;
                case typeDataCSV.LAPS:
                    path += "/" + csvFileLaps;
                    AppendData(strings, path);
                    break;
                case typeDataCSV.CRASHES:
                    path += "/" + csvFileCrashes;
                    AppendData(strings, path);
                    break;
                case typeDataCSV.POSITIONS:
                    path += "/" + csvFilePositions;
                    AppendData(strings, path);
                    break;
            }
        }
        else
        {
            Debug.Log("directory missing: " + path);
        }
    }

    // returns [sessions_count, laps_count, crashes_count,]
    public static uint[] AllCSVExists()
    {
        uint[] count = new uint[3];

        // Main directory
        string directory_path = Application.dataPath + "/" + csvDirectoryName;
        if (!Directory.Exists(directory_path))
        {
            Directory.CreateDirectory(directory_path);
        }

        // Sessions
        count[0] = 0;
        string csv_sessions = directory_path + "/" + csvFileSessions;
        if (!File.Exists(csv_sessions))
        {
            CreateCSVFile(csv_sessions, csvSessionsHeaders);
        }
        else
        {
            StreamReader strReader = new StreamReader(csv_sessions);
            string data = strReader.ReadLine();
            while (data != null)
            {
                data = strReader.ReadLine();
                count[0]++;
            }

            count[0]--; // remove header
        }

        // Laps
        count[1] = 0;
        string csv_laps = directory_path + "/" + csvFileLaps;
        if (!File.Exists(csv_laps))
        {
            CreateCSVFile(csv_laps, csvLapsHeaders);
        }
        else
        {
            StreamReader strReader = new StreamReader(csv_laps);
            string data = strReader.ReadLine();
            while (data != null)
            {
                data = strReader.ReadLine();
                count[1]++;
            }

            count[1]--; // remove header
        }

        // Crashes
        count[2] = 0;
        string csv_crashes = directory_path + "/" + csvFileCrashes;
        if (!File.Exists(csv_crashes))
        {
            CreateCSVFile(csv_crashes, csvCrashesHeaders);
        }
        else
        {
            StreamReader strReader = new StreamReader(csv_crashes);
            string data = strReader.ReadLine();
            while (data != null)
            {
                data = strReader.ReadLine();
                count[2]++;
            }

            count[2]--; // remove header
        }

        // Positions
        string csv_positions = directory_path + "/" + csvFilePositions;
        if (!File.Exists(csv_positions))
        {
            CreateCSVFile(csv_positions, csvPositionsHeaders);
        }

        return count;
    }

    static void CreateCSVFile(string path, string[] data)
    {
        using(StreamWriter sw = File.CreateText(path))
        {
            string endString = data[0];
            for(int i = 1; i < data.Length; i++)
            {
                endString += ";";
                endString += data[i]; 
            }
            sw.WriteLine(endString);
        }
    }

    static void AppendData(string[] strings, string path)
    {
        if (File.Exists(path))
        {
            using (StreamWriter sw = File.AppendText(path))
            {
                string endString = strings[0];

                for (int i = 1; i < strings.Length; i++)
                {
                    endString += ";";
                    endString += strings[i];
                }

                sw.WriteLine(endString);
            }
        }
        else
        {
            Debug.Log("file missing: " + path);
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
