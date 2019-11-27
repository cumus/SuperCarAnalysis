using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    // keep the static reference to singleton private
    public static SceneManager SM;

    private CSV_Position csv_pos;
    private CSV_Data csv_data;

    public GameObject cam2;
    public GameObject canvas;
    public InputField inputField;
    public GameObject car;
    
    void Awake()
    {
        SM = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // create missing files
        CSV_Manager.AllCSVExists();

        csv_pos = gameObject.GetComponent<CSV_Position>();
        csv_data = gameObject.GetComponent<CSV_Data>();

        car.SetActive(false);
    }

    public void PlayPressed()
    {
        if (inputField.text.Length > 0)
        {
            cam2.SetActive(false);
            canvas.SetActive(false);
            car.SetActive(true);

            csv_pos.BeginRace();
            csv_data.BeginRace(inputField.text);
        }
    }
}
