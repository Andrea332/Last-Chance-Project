using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public bool isFlickering;
    public float timeDelay1;
    public float timeDelay2;
    public float timeDelay3;

    // Update is called once per frame
    void Update()
    {
        if (!isFlickering)
        {
            StartCoroutine(FlickeringLight());
        }
    }

    IEnumerator FlickeringLight()
    {
        isFlickering = true;
        gameObject.GetComponent<Light>().enabled = true;
        yield return new WaitForSeconds(timeDelay1);
        gameObject.GetComponent<Light>().enabled = false;
        yield return new WaitForSeconds(timeDelay2);
        timeDelay3 = Random.Range(0.01f, 0.5f);
        gameObject.GetComponent<Light>().enabled = true;
        yield return new WaitForSeconds(timeDelay3);
        timeDelay3 = Random.Range(0.01f, 0.5f);
        gameObject.GetComponent<Light>().enabled = false;
        yield return new WaitForSeconds(timeDelay3);
        isFlickering = false;
    }
}
