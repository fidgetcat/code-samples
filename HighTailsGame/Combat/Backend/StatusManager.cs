using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusManager : MonoBehaviour {
    public enum Status { Stunned,Asleep};
    public List<Status> Statuses;
    public Actor MyActor;

    public Image StatusImage;

    public Sprite Stunned;
    public Sprite Asleep;

    public int index;
    public float timer;
	void Start () {
        Statuses = new List<Status>();
        index = 0;
        timer = 0f;
	}

    public void Init(Actor input)
    {
        MyActor = input;
        var psrv = GameObject.FindGameObjectWithTag("prefserv").GetComponent<PrefabServer>();
        StatusImage = Instantiate(psrv.StatusPrefab, this.gameObject.transform).GetComponentInChildren<Image>();
        Stunned = psrv.StunnedIcon;
        Asleep = psrv.SleepIcon;
    }

    public void UpdateStatuses()
    {
        Statuses = new List<Status>();
        if(MyActor.CurrentAffects.Exists(x=>x is Stun))
        {
            Statuses.Add(Status.Stunned);
        }
        if (MyActor.CurrentAffects.Exists(x => x is Sleep))
        {
            Statuses.Add(Status.Asleep);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Statuses.Count > 0)
        {
            timer += Time.deltaTime * 80f;
            if (timer >= 100f)
            {
                index++;
                timer = 0f;
                if (index > (Statuses.Count - 1))
                {
                    index = 0;
                    if (StatusImage.gameObject.activeSelf)
                    {
                        StatusImage.gameObject.SetActive(false);
                    }
                    else
                    {
                        StatusImage.gameObject.SetActive(true);
                        switch (Statuses[index])
                        {
                            case Status.Asleep:
                                StatusImage.sprite = Asleep;
                                break;
                            case Status.Stunned:
                                StatusImage.sprite = Stunned;
                                break;
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    if (!StatusImage.gameObject.activeSelf)
                        StatusImage.gameObject.SetActive(true);
                    switch (Statuses[index])
                    {
                        case Status.Asleep:
                            StatusImage.sprite = Asleep;
                            break;
                        case Status.Stunned:
                            StatusImage.sprite = Stunned;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        else
        {
            if (StatusImage.gameObject.activeSelf)
                StatusImage.gameObject.SetActive(false);
            if (timer > 0f)
                timer = 0f;
            if (index > 0)
                index = 0;
        }
	}
}
