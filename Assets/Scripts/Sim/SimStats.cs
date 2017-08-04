using UnityEngine;
using System.Collections;

public class SimStats : MonoBehaviour {

    //Personal stats
    public string simName;
    public bool isMale;
    public int age; /*{ get; set; }*/

    //Governing stats
    public int engineering; /*{ get; set; }*/
    public int sales; /*{ get; set; }*/
    public int labor; /*{ get; set; }*/
    public int management; /*{ get; set; }*/

    //Needs
    [Range(0,100)] public int hunger;
    public int energy;

   
}
