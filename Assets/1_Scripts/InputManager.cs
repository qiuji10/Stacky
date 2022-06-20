using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] PointWalker pointWalker;
    [SerializeField] YIncrementor yIncrementor;
    [SerializeField] GameObject xPrefab;
    [SerializeField] GameObject zPrefab;
    [SerializeField] Camera cam;

    private bool isX;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            pointWalker.stop  = true;
            yIncrementor.IncrementY();
            cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y + 1, cam.transform.position.z);

            if (isX)
            {
                isX = false;
                GameObject x = Instantiate(xPrefab);
                PointWalker pw = x.GetComponent<PointWalker>();
                pw.PA = yIncrementor.points[0];
                pw.PB = yIncrementor.points[1];

                pointWalker = x.GetComponent<PointWalker>();

            }
            else
            {
                isX = true;
                GameObject z = Instantiate(zPrefab);
                PointWalker pw = z.GetComponent<PointWalker>();
                pw.PA = yIncrementor.points[2];
                pw.PB = yIncrementor.points[3];

                pointWalker = z.GetComponent<PointWalker>();
            }
        }
    }
}
