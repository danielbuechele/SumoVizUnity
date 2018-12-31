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
    
    // UI elements
    public Button loadButton;
    public Button playButton;
    public Dropdown floorChooser;

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

        loadButton.onClick.AddListener(delegate () { this.importCrowditFiles(); });
        playButton.onClick.AddListener(delegate () { this.startMovePeds(); });
    }

    private void startMovePeds() {
        pm.changePlaying();
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

        Transform camera = GameObject.Find("Flycam").transform;
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
 
            gl.setTheme(new MarketplaceThemingMode());
            ScenarioLoader sl = new ScenarioLoader();
            simData = sl.getScenario(crowditFilePath, resFolderPath, gl);


            // set camera correctly
            float offset = 20;
            camera.position = new Vector3(sl.getSimData().minX - offset,
                sl.getSimData().maxElev,
                sl.getSimData().minY - offset);
            camera.parent = world.transform;
            camera.rotation = Quaternion.Euler(0, offset, 0);

            gameObject.GetComponent<PedestrianInitializer>().initializePeds(resFolderPath, simData);

            // init GUI elements: dropdown and play button
            
            // set dropdown options to display each floor
            floorChooser.GetComponent<ChooseOptions>().setOptions(simData);
            
            // init pedestrian mover
            pm.init(simData);
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

        floorChooser.GetComponent<ChooseOptions>().Reset();
        gameObject.GetComponent<PedestrianInitializer>().Reset();
        pm.Reset();

    }

    // Update is called once per frame
    void Update () {
		
	}
}
