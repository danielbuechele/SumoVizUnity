using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class StairExtrudeGeometry : Geometry {

    private static float treadWidth;
    private static float treadHeight;
    // for creating stair tread height if no height is given
    // this is according to the "Steigungsvehältnis", DIN 18065: 2s+a = 59 bis 65 cm; a refers to "antritt" ie. treadwidth, s refers
    // to steigung, ie. tread height. Thus we use the formula in order to estimate the tread height. We take the middle;-)
    private static float SLOPE_RATIO = 0.62f;

    public static GameObject createStair(string name, List<Vector2> verticesList, float height, float elevation, Vector3 dirVect, int noOfTreads) {
        int wallpoints = verticesList.Count;
        GameObject stair = new GameObject(name); // = parent object to top and side planes
        List<Vector2> sortedVertices = sortVertices(verticesList, dirVect, false);

        // TODO: this is only a work around, since stairs with no connected floor do not have a z direction vector:-/
        if (height == 0) {
            dirVect.y = 1;
        }

        GameObject tread = createFirstTread(sortedVertices, dirVect, elevation, noOfTreads, height);
        tread.transform.SetParent(stair.transform);

        Vector3 firstTreadPos = tread.transform.position;
        for (int i = 1; i <= noOfTreads - 1; i++) {
            GameObject nextTread = Instantiate(tread,
                new Vector3(firstTreadPos.x + dirVect.x * i * treadWidth,
                firstTreadPos.y + dirVect.y * i * treadHeight,
                firstTreadPos.z + dirVect.z * i * treadWidth), Quaternion.identity);
            nextTread.name = "tread " + i;
            nextTread.transform.SetParent(stair.transform);
        }
        return stair;
    }


    public static GameObject createEscalator(string name, List<Vector2> verticesList, float height, float elevation, Vector3 dirVect, int noOfTreads, int noOfHorizontalTreads, bool againstDir) {
        int wallpoints = verticesList.Count;
        GameObject escalator = new GameObject(name); // = parent object to top and side planes

        if (againstDir) {
            dirVect = new Vector3(dirVect.x * -1, dirVect.y * -1, dirVect.z * -1);
        }

        List<Vector2> sortedVertices = sortVertices(verticesList, dirVect, againstDir);

        GameObject tread = createFirstTread(sortedVertices, dirVect, elevation, noOfTreads - noOfHorizontalTreads, height);
        tread.transform.SetParent(escalator.transform);

        // for the horizontal treads, let them stick inside the floor
        if (dirVect.y > 0)
            tread.transform.position = new Vector3(tread.transform.position.x, tread.transform.position.y - treadHeight, tread.transform.position.z);
        Vector3 firstTreadPos = tread.transform.position;

        GameObject nextTread;
        for (int i = 1; i <= noOfHorizontalTreads - 1; i++) {
            nextTread = Instantiate(tread,
                new Vector3(firstTreadPos.x + dirVect.x * i * treadWidth,
                firstTreadPos.y,
                firstTreadPos.z + dirVect.z * i * treadWidth), Quaternion.identity);
            nextTread.name = "horizontal-tread " + i;
            nextTread.transform.SetParent(escalator.transform);
        }

        for (int i = noOfHorizontalTreads; i <= noOfTreads - 1; i++) {
            nextTread = Instantiate(tread,
                new Vector3(firstTreadPos.x + dirVect.x * i * treadWidth,
                firstTreadPos.y + (i - noOfHorizontalTreads + 1) * treadHeight,
                firstTreadPos.z + dirVect.z * i * treadWidth), Quaternion.identity);
            nextTread.name = "tread " + i;
            nextTread.transform.SetParent(escalator.transform);
        }

        return escalator;
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
        }
        verticesList = sortedVertices.ConvertAll(vertex => new Vector2(vertex.x, vertex.y));

        // check if direction is same as direction vector, otherwise switch.
        float dirX = verticesList[2].x - verticesList[1].x;
        if (Mathf.Abs(dirX - 0) < 0.01)
            dirX = 0;
        float dirY = verticesList[2].y - verticesList[1].y;
        if (Mathf.Abs(dirY - 0) < 0.01)
            dirY = 0;

        if (!Math.Sign(Math.Round(dirX, 2)).Equals(Math.Sign(Math.Round(dirVect.x, 2))) ||
            !Math.Sign(Math.Round(dirY, 2)).Equals(Math.Sign(Math.Round(dirVect.z, 2)))) {
            sortedVertices[0] = verticesList[2];
            sortedVertices[1] = verticesList[3];
            sortedVertices[2] = verticesList[0];
            sortedVertices[3] = verticesList[1];
        }
        return sortedVertices;
    }

    private static GameObject createFirstTread(List<Vector2> verticesList, Vector3 dirVect, float elevation, int noOfTreads, float height) {
        // get both possible "first treads"
        Vector2 firstTread = verticesList[1] - verticesList[0];
        float sign = 1f;
        if (firstTread.x < 0 || firstTread.y < 0) {
            sign = -1f;
        }
        Vector2 stairLength = verticesList[2] - verticesList[1];
        float signForLength = 1f;
        if (stairLength.x < 0 || stairLength.y < 0) {
            signForLength = -1f;
        }

        Vector2 orthogonal = verticesList[2] - verticesList[1];
        treadWidth = (orthogonal.magnitude / noOfTreads);

        GameObject tread = GameObject.CreatePrimitive(PrimitiveType.Cube);
        float xOffset = 0, yOffset = 0;
        if (Mathf.Abs(dirVect.x - 0) < 0.1) {
            xOffset = firstTread.magnitude * sign;
            yOffset = stairLength.magnitude / noOfTreads * signForLength;

        } else {
            xOffset = stairLength.magnitude / noOfTreads * signForLength;
            yOffset = firstTread.magnitude * sign;
        }

        if (height == 0) {
            treadHeight = (SLOPE_RATIO - treadWidth) / 2;
        } else {
            treadHeight = height / noOfTreads * dirVect.y;
        }
        tread.transform.position = new Vector3(verticesList[0].x + xOffset / 2, elevation + treadHeight / 2, verticesList[0].y + yOffset / 2);
        tread.transform.localScale = new Vector3(xOffset, treadHeight, yOffset);
        tread.name = "first tread";

        return tread;
    }


}
