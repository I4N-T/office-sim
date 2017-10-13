using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EventLogScript : MonoBehaviour {

    public static EventLogScript instance;

    public List<string> EventLog = new List<string>();
    public Text eventText;
    int maxLines;
    string formattedText;


    void Awake()
    {
        if (instance != null)
        {
            GameObject.Destroy(instance);
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this);

        maxLines = 11;
    }

    public void AddEvent(string eventMessage)
    {
        EventLog.Add(eventMessage);

        if (EventLog.Count >= maxLines)
        {
            EventLog.RemoveAt(0);
        }

        formattedText = "";
   
        //print the log
        foreach (string line in EventLog)
        {
            formattedText += line + "\n";
        }

    }

    void Update()
    {
        eventText.text = formattedText;  
        
    }
   

}
