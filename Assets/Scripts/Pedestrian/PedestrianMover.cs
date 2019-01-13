using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class PedestrianMover : MonoBehaviour {

    private bool initialized = false;
    private GameObject peds;
    private bool playing = false;
    private int roundCounter = 0;
    private PointerEventData eventData;

    private float currentTime;
    private float maxTime = 0;
    // for recording purpose: only first round is recorded
    private bool firstRound;


    // set in inspector
    [SerializeField] Sprite PauseSprite;
    [SerializeField] Sprite PlaySprite;
    [SerializeField] Button playButton;
    [SerializeField] Slider slider;
    [SerializeField] Text startTime;
    [SerializeField] Text endTime;

    public PedestrianMover() { }

    private void Start() {
        currentTime = 0;
        playButton.onClick.AddListener(delegate () { this.changePlaying(); });
    }

    internal bool isFirstRound() {
        return firstRound;
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
        firstRound = true;

        endTime.text = maxTime.ToString();
        startTime.text = currentTime.ToString();
        slider.value = 0;
    }

    public void changePlaying() {
        if (playing) {
            playing = false;
            playButton.image.sprite = PlaySprite;
        } else {
            playing = true;
            playButton.image.sprite = PauseSprite;
        }
    }

    public void Reset() {
        playing = false;
        initialized = false;
        playButton.image.sprite = PlaySprite;
    }

    public void Update() {
        if (playing && initialized) {
            currentTime = currentTime + Time.deltaTime;

            if (currentTime >= maxTime) { // new round
                currentTime = 0;
                firstRound = false;

                foreach (Transform ped in peds.transform) {
                    ped.GetComponent<Pedestrian>().reset();
                }
                roundCounter++;
            }
            updateSlider();

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

    private void updateSlider() {
        float lerpValue = currentTime / maxTime;
        slider.value = Mathf.Lerp(0f, 1f, (float)lerpValue);
        startTime.text = currentTime.ToString("0.##");
    }

    public void dragSlider(BaseEventData ev) {
        currentTime = slider.value * maxTime;
        startTime.text = currentTime.ToString("0.##");
    }
}
