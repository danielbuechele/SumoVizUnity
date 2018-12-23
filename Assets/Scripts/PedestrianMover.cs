using UnityEngine;
using UnityEngine.UI;

public class PedestrianMover : MonoBehaviour {

    bool button_pressed = false;
    private GameObject peds;
    private PlaybackControl pc;

    private void Start()
    {
        peds = GameObject.Find("Pedestrians");
        pc = GameObject.Find("PlaybackControl").GetComponent<PlaybackControl>();
    }

    public void movePedestrians()
    {
        peds = GameObject.Find("Pedestrians");
        foreach (Transform ped in peds.transform)
        {
            ped.GetComponent<Pedestrian>().init();
        }

        pc.init();

        button_pressed = true;
    }

    public void Update()
    {
        if (button_pressed) {
             if (peds != null) {
                foreach (Transform ped in peds.transform)
                {
                    ped.GetComponent<Pedestrian>().move();
                }
            }
        }
    }


    public void pausePedestrians()
    {
       pc.playing = false;
       button_pressed = false;
     }



}
