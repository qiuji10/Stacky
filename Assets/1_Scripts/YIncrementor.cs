using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YIncrementor : MonoBehaviour
{
    public List<Transform> points;

    public void IncrementY()
    {
        for (int i = 0; i < points.Count; i++)
        {
            points[i].position = new Vector3(points[i].position.x, points[i].position.y + 1, points[i].position.z);
        }
    }
}
