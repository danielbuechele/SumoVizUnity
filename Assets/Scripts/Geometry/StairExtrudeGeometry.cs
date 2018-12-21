using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StairExtrudeGeometry : Geometry
{

    private static float treadWidth;

    public static void create(string name, List<Vector2> verticesList, float height, float elevation,  Vector3 dirVect, int noOfTreads)
    {
        int wallpoints = verticesList.Count;
        GameObject stair = new GameObject(name); // = parent object to top and side planes

        GameObject tread = createTreads(verticesList, dirVect, elevation, noOfTreads, height);
        tread.transform.SetParent(stair.transform);
        GameObject.Find("GeometryLoader").GetComponent<GeometryLoader>().setWorldAsParent(stair);

        Vector3 firstTreadPos = tread.transform.position;
        for (int i = 1; i <= noOfTreads - 1; i++)
        {
            GameObject nextTread = Instantiate(tread,
                new Vector3(firstTreadPos.x + dirVect.x * i * treadWidth,
                firstTreadPos.y + dirVect.z * i* (height / noOfTreads),
                firstTreadPos.z + dirVect.y * i * treadWidth), Quaternion.identity);
            nextTread.name = "tread " + i;
            nextTread.transform.SetParent(stair.transform);
        }
    }

    private static GameObject createTreads(List<Vector2> verticesList, Vector3 dirVect, float elevation, int noOfTreads, float height)
    {
        // get both possible "first treads"
        Vector2 firstTreadUp = verticesList[1] - verticesList[0];
        Vector2 firstTreadDown = verticesList[2] - verticesList[1];
        Vector2 orthogonal = verticesList[2] - verticesList[1];

        GameObject tread = GameObject.CreatePrimitive(PrimitiveType.Cube);
        float treadwidth = verticesList[1].x - verticesList[0].x;
        float treaddepth = (verticesList[2].y - verticesList[1].y) / noOfTreads;
        float treadheight = height / noOfTreads;

        // create dot product to check for orthogonality -> if 0, vectors are orthogonal
        float dotProduct = firstTreadUp.x * dirVect.x +
                            firstTreadUp.y * dirVect.y;

        // change both vectors if orthogonal: cannot be more precise - since coordinates are only with two digits
        if (Mathf.Abs(dotProduct - 0) > 0.1)
        {
            if (dotProduct < 0)
            {
                treadwidth = verticesList[2].x - verticesList[1].x;
                treaddepth = (verticesList[1].y - verticesList[0].y) / noOfTreads;

                tread.transform.position = new Vector3(verticesList[1].x + treadwidth, elevation, verticesList[1].y + treaddepth);
            }
            else
            {
                treadwidth = verticesList[2].x - verticesList[1].x;
                treaddepth = (verticesList[3].y - verticesList[2].y) / noOfTreads;
                treadheight = height / noOfTreads;

                tread.transform.position = new Vector3(verticesList[2].x + treadwidth, elevation, verticesList[2].y + treaddepth);
            }
            orthogonal = verticesList[1] - verticesList[0];
        } else
        {
            tread.transform.position = new Vector3(verticesList[0].x + treadwidth / 2, elevation + treadheight / 2, verticesList[0].y + treaddepth);

        }

        tread.transform.localScale = new Vector3(treadwidth, treadheight, treaddepth);
        tread.name = "first tread";
        treadWidth = (orthogonal.magnitude / noOfTreads);

        return tread;
    }


}
