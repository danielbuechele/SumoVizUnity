using UnityEngine;
using System.IO;
using SFB;

public class ScenarioImporter_Button : MonoBehaviour {

    private RuntimeInitializer ri;
    private GeometryLoader gl;
    private ScenarioLoader sl;

    private void Start() {
        ri = GameObject.Find("RuntimeInitializer").GetComponent<RuntimeInitializer>();
        gl = GameObject.Find("GeometryLoader").GetComponent<GeometryLoader>();
        sl = GameObject.Find("ScenarioLoader").GetComponent<ScenarioLoader>();
    }

    public void importAccurateOutput() {
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

            var crowditFilePath = crowditFilePaths[0];
            if (crowditFilePath == "") // = cancel was clicked in open file dialog
                return;
 
            GameObject world = new GameObject("World");

            string resFolderPath = Path.Combine(Path.GetDirectoryName(crowditFilePath), Path.GetFileNameWithoutExtension(crowditFilePath)) + "_res";
            ri.setCrowditFileAndResFolder(crowditFilePath, resFolderPath);
            ri.geometryLoader = gl;
            ri.geometryLoader.setTheme(new MarketplaceThemingMode()); // EvaktischThemingMode ());

            sl.loadScenario(crowditFilePath, resFolderPath, gl);

            // set camera correctly
            float offset = 20;
            camera.position = new Vector3(sl.getSimData().minX - offset,
                sl.getSimData().maxElev,
                sl.getSimData().minY - offset);
            camera.parent = world.transform;
            camera.rotation = Quaternion.Euler(0, offset, 0);


            // TODO: old code - still needed??
            //ri.boundingPoints = sl.getBoundingPoints (); // if needed again, like in the camera tour for instance, re-create this list. currently only each floor knows their bounding points

            //string projectFolderName = path.Substring (scenariosPath.Length, path.Length - scenariosPath.Length - Path.GetFileName (path).Length); // TODO make this more solid
            //ri.relativeTrajFilePath = sl.getRelativeTrajFilePath () != "" ? "Assets/StreamingAssets/Scenarios" + projectFolderName + sl.getRelativeTrajFilePath () : "no_path_to_trajectory_file";
            //ri.absoluteTrajFilePath = Path.GetDirectoryName (path) + "/" + sl.getRelativeTrajFilePath ();
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

    }
}