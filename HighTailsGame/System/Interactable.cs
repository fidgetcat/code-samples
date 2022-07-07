using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {
    public LevelScript Levelscript;
    public char flag;
	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                    GameObject.FindGameObjectWithTag("sfxx").GetComponent<SFXDude>().PlaySelect();
                    Levelscript.SendFlag(flag);
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
