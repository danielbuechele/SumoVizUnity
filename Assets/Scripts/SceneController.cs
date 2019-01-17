using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SFB;
using System.IO;
using System;
using System.Xml;
using System.IO.Compression;

public class SceneController : MonoBehaviour {
    private static SceneController sceneController;

    // UI elements, set via inspector
    [SerializeField] Button loadButton;
    [SerializeField] GameObject floorPanel;

    private Button recordButton;
    private Button playButton;
    private Transform camera;
    private GameObject controlsForLoadedScenario;

    // storage elements
    private string crowditFilePath;
    private string resFolderPath;

    // rendering elements
    private GeometryLoader gl;

    // control for movement
    private PedestrianMover pm;
    private SimData simData;



    public void Start() {
        gl = gameObject.GetComponent<GeometryLoader>();
        pm = gameObject.GetComponent<PedestrianMover>();
        controlsForLoadedScenario = GameObject.Find("ControlsScenarioLoaded");
        camera = GameObject.Find("Flycam").transform;
        recordButton = GameObject.Find("Record").GetComponent<Button>() as Button;
        playButton = GameObject.Find("Play").GetComponent<Button>() as Button;
        
        // do not display the panel for the floors if no scenario is loaded
        foreach (Renderer r in floorPanel.GetComponentsInChildren<Renderer>()) {
            r.enabled = false;
        }

        loadButton.onClick.AddListener(delegate () { this.importCrowditFiles(); });

        // do not display the play button as long as there are no pedestrians
        controlsForLoadedScenario.SetActive(false);
    }

 
    public static SceneController Instance() {
        if (!sceneController) {
            sceneController = FindObjectOfType(typeof(SceneController)) as SceneController;
            if (!sceneController)
                Debug.LogError("There needs to be one active SceneController script on a GameObject in your scene.");
        }
        return sceneController;
    }

    public void importCrowditFiles() {

        camera.parent = GameObject.Find("MainCameraParent").transform;

        resetSceneAndPlayer();

        // load simulation file and data
        bool continueOk = true;
        string scenariosPath = Application.dataPath + "/StreamingAssets/Scenarios";

        if (continueOk) {
            var crowditFilePaths = StandaloneFileBrowser.OpenFilePanel("", scenariosPath, "crowdit;*.crowdit", false); //Path.GetFileName(path))
            if (crowditFilePaths == null) // = cancel was clicked in open file dialog
                return;

            crowditFilePath = crowditFilePaths[0];
            if (crowditFilePath == "") // = cancel was clicked in open file dialog
                return;

            GameObject world = new GameObject("World");

            resFolderPath = Path.Combine(Path.GetDirectoryName(crowditFilePath), Path.GetFileNameWithoutExtension(crowditFilePath)) + "_res";
 
            gl.setTheme(new CrowditThemingMode());
            ScenarioLoader sl = new ScenarioLoader();
            simData = sl.getScenario(crowditFilePath, resFolderPath, gl);


            // set camera correctly
            float offset = 20;
            camera.position = new Vector3(sl.getSimData().minX - offset,
                sl.getSimData().maxElev,
                sl.getSimData().minY - offset);
            camera.parent = world.transform;
            camera.rotation = Quaternion.Euler(0, offset, 0);
            GameObject.Find("CameraPivot").transform.position = camera.position;
            GameObject.Find("LightSource").transform.position = camera.position;


            // init GUI elements: floors list and play button
            if (gameObject.GetComponent<PedestrianInitializer>().initializePeds(resFolderPath, simData)) {
                // do display the play button if there are pedestrians
                controlsForLoadedScenario.SetActive(true);

                // init pedestrian mover
                pm.init(simData);
            }

            GetComponent<DisplayFloorToggler>().setToggles(simData);

            // display the panel for the floors if no scenario is loaded
            floorPanel.SetActive(true);
        }
    }

    private void resetSceneAndPlayer() {
        FindObjectOfType<PedestrianMover>().Reset();

        if (GameObject.Find("World") != null) {
            foreach (Transform child in GameObject.Find("World").transform) {
                // Important: otherwise, garbage collector will destroy it at any time and that leads to awkward behavior
                DestroyImmediate(child.gameObject);
            }
            DestroyImmediate(GameObject.Find("World"));
        }
        if (GameObject.Find("Pedestrians") != null) {
            foreach (Transform child in GameObject.Find("Pedestrians").transform) {
                DestroyImmediate(child.gameObject);
            }
            DestroyImmediate(GameObject.Find("Pedestrians"));
        }
        Canvas canvas = GetComponentInParent<Canvas>();

             foreach (Toggle child in GetComponent<DisplayFloorToggler>().getCanvas().GetComponentsInChildren<Toggle>()) {
                DestroyImmediate(child.gameObject);
            }
 
        GetComponent<DisplayFloorToggler>().Reset();
        gameObject.GetComponent<PedestrianInitializer>().Reset();
        pm.Reset();


        // do not display controls
        controlsForLoadedScenario.SetActive(false);
    }
}
