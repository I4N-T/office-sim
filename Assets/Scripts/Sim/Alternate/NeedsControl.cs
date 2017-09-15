using UnityEngine;
using System.Collections;

public class NeedsControl : MonoBehaviour {

    SimStats simStatsScript;

    void Start()
    {
        simStatsScript = gameObject.GetComponent<SimStats>();
        //NEED DEPLETIONS
        StartCoroutine(EnergyDeplete(5f));
        StartCoroutine(HungerDeplete(3f));
        StartCoroutine(BladderDeplete(2.25f));
    }

    //NEED METERS DEPLETE PERIODICALLY
    IEnumerator EnergyDeplete(float waitTime)
    {
        for (;;)
        {
            simStatsScript.energy -= 1;
            yield return new WaitForSeconds(waitTime);
        }
    }

    IEnumerator HungerDeplete(float waitTime)
    {
        for (;;)
        {
            simStatsScript.hunger -= 1;
            simStatsScript.hunger = Mathf.Clamp(simStatsScript.hunger, 0, 100);

            yield return new WaitForSeconds(waitTime);
        }
    }

    IEnumerator BladderDeplete(float waitTime)
    {
        for (;;)
        {
            simStatsScript.bladder -= 1;
            simStatsScript.bladder = Mathf.Clamp(simStatsScript.bladder, 0, 100);

            yield return new WaitForSeconds(waitTime);
        }
    }
}
