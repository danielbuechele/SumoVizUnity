using UnityEngine;
using UnityEngine.UI;


public class PedestrianMover : MonoBehaviour {

    private bool initialized = false;
    private GameObject peds;
    private bool playing = false;
    private decimal currentTime;
    private int roundCounter = 0;
    private decimal maxTime = 0;

    // set in inspector
    public Sprite PauseSprite;
    public Sprite PlaySprite;
    public Button but;

    public PedestrianMover() { }

    private void Start() {
        currentTime = 0;
    }

    internal void init(SimData simData) {
        peds = simData.getPedestrianGameObject();
        foreach (Transform ped in peds.transform) {
            ped.GetComponent<Pedestrian>().init();
        }
        initialized = true;
        roundCounter = 0;
        currentTime = 0;
        maxTime = simData.getMaxTime();
    }

    public void changePlaying() {
        if (playing) {
            playing = false;
            but.image.sprite = PauseSprite;
        } else {
            playing = true;
            but.image.sprite = PlaySprite;
        }
    }

    public void Reset() {
        playing = false;
        initialized = false;
        but.image.sprite = PauseSprite;
    }

    public void Update() {
        if (playing && initialized) {
            currentTime = (currentTime + (decimal)Time.deltaTime);

            if (currentTime >= maxTime) { // new round
                currentTime = 0;

                foreach (Transform ped in peds.transform) {
                    ped.GetComponent<Pedestrian>().reset();
                }
                roundCounter++;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            playing = !playing;
        }

        if (initialized) {
            foreach (Transform ped in peds.transform) {
                ped.GetComponent<Pedestrian>().move(currentTime);
            }
        }
    }
}
