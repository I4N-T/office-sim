using UnityEngine;
using System.Collections;

public class SalesBenchScript : MonoBehaviour {

    public Vector2 salesBenchPos;
    public Vector2 salesBenchUsePos;

    //PRODUCTION PROGRESS STUFF
    [Range(0, 100)]
    public int progressCount;
    public bool inProgress;
    bool isProgressCoroutineStarted;
    Coroutine progressCoroutine = null;
    Coroutine skillExpCoroutine = null;

    //WIDGETS INTERACTION
    WidgetScript thisWidgetScript;
    int widgetSellPrice;


    //SIM INTERACTING WITH THIS BENCH
    GameObject sim;
    SimAI simAIScript;

    //QUALITY/PRICE
    float augmentFactor;

    //RENDERER (for delete coroutine)
    SpriteRenderer rend;

    //AUDIO
    AudioClip sellSoundClip;
    AudioClip deleteSoundClip;
    public AudioSource sfxSource;


    void Start () {

        //Source will stay with sellsoundclip unless deletion happens
        sfxSource.clip = AudioScript.instance.sellSoundClip;
        deleteSoundClip = AudioScript.instance.deleteSoundClip;

        GameStats.hasSalesBench = true;
        salesBenchPos = gameObject.transform.position;

        //ADD THIS ITEM TO THE WIDGETBENCH ARRAY IN GAMESTATS
        GameStats.countSalesBench++;
        GameStats.salesBenchList.Add(this.gameObject);

        progressCount = 0;

    }
	
	
	void Update () {
        //Update sales bench position 
        salesBenchPos = gameObject.transform.position;

        if (inProgress)
        {
            SalesInProgress();
        }
        else if (!inProgress)
        {
            simAIScript = null;
        }

        //this stuff stops bench from running when sim isn't working on it anymore
        if (simAIScript != null)
        {
            if (simAIScript.simStatsScript.objectInUse == null)
            {
                StopCoroutine(progressCoroutine);
                StopCoroutine(skillExpCoroutine);
                isProgressCoroutineStarted = false;
                inProgress = false;
            }
            else if (simAIScript.simStatsScript.objectInUse != null)
            {
                if (simAIScript.simStatsScript.objectInUse.tag != "SalesBench")
                {
                    StopCoroutine(progressCoroutine);
                    StopCoroutine(skillExpCoroutine);
                    isProgressCoroutineStarted = false;
                    inProgress = false;
                }
            }
        }
        

    }

    void SalesInProgress()
    {
        if (!isProgressCoroutineStarted)
        {
            progressCoroutine = StartCoroutine(ProgressIncrement(.5f));
            skillExpCoroutine = StartCoroutine(SkillExpIncrement(3f));
        }

        if (progressCount >= 100)
        {
            //stop coroutine and set inProgress to false
            StopCoroutine(progressCoroutine);
            StopCoroutine(skillExpCoroutine);
            inProgress = false;
            //simAIScript.isWidgetBenchInProgress = false;

            thisWidgetScript = GameStats.widgetList[0].GetComponent<WidgetScript>();

            //have chance to sell a widget

            //price based on quality 
            if (thisWidgetScript.widgetQuality == "Bad")
            {
                widgetSellPrice += 50;
            }
            else if (thisWidgetScript.widgetQuality == "Average")
            {
                widgetSellPrice += 75;
            }
            if (thisWidgetScript.widgetQuality == "Good")
            {
                widgetSellPrice += 100;
            }

            //base price augmented by widget level (determined by engineering research)
            augmentFactor = (float)GameStats.widgetDesignLevel / 100f;
            widgetSellPrice += (int)(widgetSellPrice * augmentFactor);

            //bonus or detriment based on sales skill
            if (simAIScript.simStatsScript.sales <= 3)
            {
                widgetSellPrice = Mathf.RoundToInt(widgetSellPrice * .75f);
            }
            else if (simAIScript.simStatsScript.sales > 3 && simAIScript.simStatsScript.sales < 7)
            {
            }
            else if (simAIScript.simStatsScript.sales >= 7)
            {
                widgetSellPrice = Mathf.RoundToInt(widgetSellPrice * 1.25f);
            }

            GameStats.dollars += widgetSellPrice;
            print("widget sold for $" + widgetSellPrice);
            sfxSource.Play();
            Destroy(GameStats.widgetList[0]);

            //set progressCount back to 0
            progressCount = 0;
            widgetSellPrice = 0;

            //if more widgets yet to sell, keep going; else if no widgets then go idle
            if (GameStats.countWidgetInStockpile >= 0)
            {
                isProgressCoroutineStarted = false;
            }
            
        }
    }

    IEnumerator ProgressIncrement(float waitTime)
    {
        isProgressCoroutineStarted = true;

        while (inProgress)
        {
            progressCount += 1;
            yield return new WaitForSeconds(waitTime);

        }
    }

    IEnumerator SkillExpIncrement(float waitTime)
    {
        isProgressCoroutineStarted = true;

        while (inProgress)
        {
            simAIScript.simStatsScript.salesExp += 1;
            yield return new WaitForSeconds(waitTime);

        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Sim")
        {
            simAIScript = col.gameObject.GetComponent<SimAI>();
            if (simAIScript.simFSMScript.mainState == SimFSM.MainFSM.Task && simAIScript.simFSMScript.taskState == SimFSM.TaskFSM.Sales)
            {
                inProgress = true;
            }
        }
    }

    //When sim goes to use bathroom, must set objectinuse to null in order to stop the coroutine
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Sim")
        {
            //this sets the sim's objectinuse to null once it leaves the bench
            //the reason we declare a new SimStats object here is in case another unrelated sim happens to cross through the trigger
            SimAI simAIHere = col.gameObject.GetComponent<SimAI>();
            simAIHere.simStatsScript.objectInUse = null;

        }
    }

    /*void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Sim")
        {
            inProgress = false;
        }
    }*/

    //DELETE OBJECT
    void OnMouseDown()
    {
        //print("click happened");
        if (DeleteScript.isDelete)
        {
            StartCoroutine(Delete());
        }
    }

    IEnumerator Delete()
    {
        //play sound
        sfxSource.clip = deleteSoundClip;
        sfxSource.Play();
        //disable sprite
        rend = gameObject.GetComponent<SpriteRenderer>();
        rend.enabled = false;

        DeleteScript.isDelete = false;

        yield return new WaitForSecondsRealtime(.75f);

        //destroy object
        Destroy(gameObject);
    }
}
