using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointWalker : MonoBehaviour
{
    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;
    Transform cube;

    Vector3 direction;

    [SerializeField] float speed;

    private void Awake()
    {
        cube = GetComponent<Transform>();
    }

    private void Start()
    {
        cube.position = pointA.position;
    }

    private void Update()
    {
        if (cube.position.x >= pointA.position.x)
        {
            Debug.Log("Go to left");
            direction = new Vector3(pointB.position.x, cube.position.y, pointB.position.z);
        }
        else if (cube.position.x <= pointB.position.x)
        {
            Debug.Log("Go to right");
            direction = new Vector3(pointA.position.x, cube.position.y, pointA.position.z);
        }

        cube.Translate(direction * speed * Time.deltaTime);
    }
}
