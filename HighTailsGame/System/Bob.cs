using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bob : MonoBehaviour {
    public float midpointdistance = 0.002f;
    public float origin;
    public bool floatUp = false;
    public float mod = 0f;

    public float topbound;
    public float botbound;
	// Use this for initialization
	void Start () {
        origin = gameObject.transform.position.y;
        topbound = origin + midpointdistance;
        botbound = origin - midpointdistance;
	}
	
	// Update is called once per frame
	void Update () {
        if (floatUp)
        {
            mod += 0.02f * Time.deltaTime;
        }
        else
        {
            mod += -0.02f * Time.deltaTime;
        }
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + mod, gameObject.transform.position.z);
        if (gameObject.transform.position.y >=topbound)
        {
            mod = 0f;
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y-0.02f * Time.deltaTime, gameObject.transform.position.z);
            if (floatUp)
                floatUp = false;
            else
                floatUp = true;
        }
        else if (gameObject.transform.position.y <= botbound)
        {
            mod = 0f;
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.02f * Time.deltaTime, gameObject.transform.position.z);
            if (floatUp)
                floatUp = false;
            else
                floatUp = true;
        }
	}
}
