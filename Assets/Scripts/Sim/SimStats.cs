using UnityEngine;
using System.Collections;

public class SimStats : MonoBehaviour {


    //SIM POSITION
    public Vector2 simPos;

    //PERSONAL STATS
    public string simName;
    public bool isMale;
    public int age; /*{ get; set; }*/

    //GOVERNING STATS

    //engineering is used to research new technologies, design new widgets (drafting station, CAD station, FEA)
    public int engineering; /*{ get; set; }*/

    //sales people work at sales stations, customer support station, and also go out on "sales quests" which may result in a special deal (recurring sales or some other unique bonus)
    public int sales; /*{ get; set; }*/

    //labor is used for building/assembling widgets. Higher skill points leads to higher quality widget which sells for more
    public int labor; /*{ get; set; }*/

    //Having a department manager results in all functions of that department being buffed. Sims can train other sims in any skill in order to gain management skill. Managers can not do regular tasks.
    public int management; /*{ get; set; }*/

    //SIM CAPABILITIES
    public enum SimJobs
    {
        Engineer,
        Production,
        Sales
    }
    public SimJobs jobState;

    public string simJobString;

    //engineering tasks GET RID OF THESE BOOLEANS AND USE SIMJOBS SWITCH INSTEAD
    public bool canEngineer;
    

    //sales tasks (do sales and custmr support need to be separate?)
    public bool canSales;
    //public bool canCustomerSupport;

    //labor tasks (maybe have a learned skills sysstem so a sim required to learn welding before they can weld etc)
    public bool canLabor;

    //management
    public bool isManager;

    //MOOD - determined by algorithm based on needs fullfillment. Mood can boost or decrease productivity.
    [Range(0, 100)] public int mood;

    //NEEDS
    [Range(0, 100)] public int energy;
    [Range(0, 100)] public int bladder;
    [Range(0, 100)] public int hunger; 
    [Range(0, 100)] public int stress;


    //FOR MOODLETS
    public bool hasPrivateOffice;

    //INVENTORY
    public GameObject itemInPossession;

    //CURRENT STATUS
    public GameObject objectInUse;

}
