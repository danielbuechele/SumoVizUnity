using UnityEngine;
using System.Collections.Generic;
using System;

public class StairExtrudeGeometry : Geometry {

    private static float treadDepth;
    private static float treadHeight;

    // for creating stair tread height if no height is given
    // this is according to the "Steigungsvehältnis", DIN 18065: 2s+a = 59 bis 65 cm; a refers to "antritt" ie. treadwidth, s refers
    // to steigung, ie. tread height. Thus we use the formula in order to estimate the tread height. We take the middle;-)
    private static float SLOPE_RATIO = 0.62f;

    public static GameObject createStair(string name, List<Vector2> verticesList, float height, 
        float elevation, Vector3 dirVect, int noOfTreads, GameObject treadPrefab) {
        int wallpoints = verticesList.Count;
        GameObject stair = new GameObject(name); // = parent object to top and side planes
        List<Vector2> sortedVertices = sortVertices(verticesList, dirVect, false);

        // set height if height is given, otherwise height is set when creating the first tread
        treadHeight = height / (noOfTreads);

        // TODO: this is only a work around, since stairs with no connected floor do not have a z direction vector:-/
        if (height == 0) {
            dirVect.y = 1;
        }
        GameObject tread = createFirstTread(sortedVertices, dirVect, elevation, noOfTreads, height, treadPrefab);
        tread.transform.SetParent(stair.transform);

        Vector3 firstTreadPos = tread.transform.position;
        for (int i = 1; i <= noOfTreads - 1; i++) {
            GameObject nextTread = Instantiate(tread,
                new Vector3(firstTreadPos.x + dirVect.x * i * treadDepth,
                firstTreadPos.y + dirVect.y * i * treadHeight,
                firstTreadPos.z + dirVect.z * i * treadDepth), tread.transform.rotation);
            nextTread.name = "tread " + i;
            nextTread.transform.SetParent(stair.transform);
        }
        return stair;
    }


    public static GameObject createEscalator(string name, List<Vector2> verticesList, float height, float elevation, 
        Vector3 dirVect, int noOfTreads, int noOfHorizontalTreads, bool againstDir, GameObject treadPrefab, Material escalatorTreadMaterial) {

        GameObject escalator = new GameObject(name); 

        if (againstDir) {
            dirVect = new Vector3(dirVect.x * -1, dirVect.y * -1, dirVect.z * -1);
        }

        List<Vector2> sortedVertices = sortVertices(verticesList, dirVect, againstDir);

        treadHeight = height / (noOfTreads - noOfHorizontalTreads) * dirVect.y;

        GameObject tread = createFirstTread(sortedVertices, dirVect, elevation, noOfTreads, height, treadPrefab);
        tread.transform.SetParent(escalator.transform);
        renderTread(tread, escalatorTreadMaterial);

        // for the horizontal treads, let them stick inside the floor
        if (dirVect.y > 0)
            tread.transform.position = new Vector3(tread.transform.position.x, tread.transform.position.y - treadHeight, tread.transform.position.z);
        Vector3 firstTreadPos = tread.transform.position;

        GameObject nextTread;
        for (int i = 1; i <= noOfHorizontalTreads - 1; i++) {
            nextTread = Instantiate(tread,
                new Vector3(firstTreadPos.x + dirVect.x * i * treadDepth,
                firstTreadPos.y,
                firstTreadPos.z + dirVect.z * i * treadDepth), tread.transform.rotation);
            nextTread.name = "horizontal-tread " + i;
            nextTread.transform.SetParent(escalator.transform);

            renderTread(nextTread, escalatorTreadMaterial);
        }

        for (int i = noOfHorizontalTreads; i <= noOfTreads - 1; i++) {
            nextTread = Instantiate(tread,
                new Vector3(firstTreadPos.x + dirVect.x * i * treadDepth,
                firstTreadPos.y + (i - noOfHorizontalTreads + 1) * treadHeight,
                firstTreadPos.z + dirVect.z * i * treadDepth),  tread.transform.rotation);
            nextTread.name = "tread " + i;
            nextTread.transform.SetParent(escalator.transform);
            renderTread(nextTread, escalatorTreadMaterial);
        }

        return escalator;
    }

    private static void renderTread(GameObject nextTread, Material escalatorTreadMaterial) {
        foreach (Renderer renderer in nextTread.GetComponentsInChildren<MeshRenderer>()) {
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            renderer.material = escalatorTreadMaterial;
        }
    }

    private static List<Vector2> sortVertices(List<Vector2> verticesList, Vector3 dirVect, bool againstDir) {
        List<Vector2> sortedVertices = verticesList.ConvertAll(vertex => new Vector2(vertex.x, vertex.y));

        Vector2 firstTread = verticesList[1] - verticesList[0];
        // create dot product to check for orthogonality -> if 0, vectors are orthogonal
        float dotProduct = firstTread.x * dirVect.x +
                            firstTread.y * dirVect.z;

        // not orthogonal, means the vertices have to be changed in order
        if (Mathf.Abs(dotProduct - 0) > 0.1) {
            sortedVertices[0] = verticesList[1];
            sortedVertices[1] = verticesList[2];
            sortedVertices[2] = verticesList[3];
            sortedVertices[3] = verticesList[0];
            verticesList = sortedVertices.ConvertAll(vertex => new Vector2(vertex.x, vertex.y));
        }
 
        // check if direction is same as direction vector, otherwise switch.
        float dirX = verticesList[2].x - verticesList[1].x;
        if (Mathf.Abs(dirX - 0) < 0.1)
            dirX = 0;
        float dirY = verticesList[2].y - verticesList[1].y;
        if (Mathf.Abs(dirY - 0) < 0.1)
            dirY = 0;

        if (!Math.Sign(Math.Round(dirX, 1)).Equals(Math.Sign(Math.Round(dirVect.x, 1))) ||
            !Math.Sign(Math.Round(dirY, 1)).Equals(Math.Sign(Math.Round(dirVect.z, 1)))) {
            sortedVertices[0] = verticesList[2];
            sortedVertices[1] = verticesList[3];
            sortedVertices[2] = verticesList[0];
            sortedVertices[3] = verticesList[1];
            verticesList = sortedVertices.ConvertAll(vertex => new Vector2(vertex.x, vertex.y));
        }

        // make sure that we always extend the treads in positive direction
        firstTread = verticesList[1] - verticesList[0];
        if (firstTread.x < 0|| firstTread.y < 0) {
            sortedVertices[0] = verticesList[1];
            sortedVertices[1] = verticesList[0];
            sortedVertices[2] = verticesList[3];
            sortedVertices[3] = verticesList[2];
            verticesList = sortedVertices.ConvertAll(vertex => new Vector2(vertex.x, vertex.y));
        }
 
        Vector2 lengthTread = verticesList[2] - verticesList[1];
        return sortedVertices;
    }

    private static GameObject createFirstTread(List<Vector2> verticesList, Vector3 dirVect, float elevation, int noOfTreads, float height, GameObject treadPrefab) {

        Vector2 firstTread = verticesList[1] - verticesList[0];
        float treadwidth = firstTread.magnitude;

        Vector2 stairLength = verticesList[2] - verticesList[1];
        treadDepth = (stairLength.magnitude / noOfTreads);

        int sign = Math.Min(Math.Sign(Math.Round(dirVect.z, 1)), Math.Sign(Math.Round(dirVect.x, 1)));
        if (sign == 0) {
            sign = 1;
        }

        // set height
        if (height == 0) {
            treadHeight = (SLOPE_RATIO - treadDepth) / 2;
        }

        GameObject tread = Instantiate(treadPrefab, treadPrefab.transform.position, Quaternion.identity);

        tread.transform.position = new Vector3(verticesList[0].x, elevation, verticesList[0].y);

        float angle = Vector2.SignedAngle(Vector2.right, firstTread);
        
        // only rotate if not already aligned
        if (angle != 180 && angle != 0) {
            angle = angle * -1;
            tread.transform.Rotate(Vector3.up, angle);
            sign = sign * -1;
        }

        tread.name = "first tread";
        tread.transform.localScale = new Vector3(treadwidth, treadHeight*dirVect.y, treadDepth * sign);

        return tread;
    }


}
