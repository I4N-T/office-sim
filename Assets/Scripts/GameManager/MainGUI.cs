using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainGUI : MonoBehaviour {

    public Text dollarsText;
    

	// Use this for initialization
	void Start () {
	

	}
	
	// Update is called once per frame
	void Update () {

        dollarsText.text = "Dollars: $" + GameStats.dollars;
        
	}
}
