using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointWalker : MonoBehaviour
{
    public enum CubeType { xCube, zCube }

    [SerializeField] float speed;
    public bool stop;

    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;
    Transform cube;

    public Transform PA { get { return pointA; } set { pointA = value; } }
    public Transform PB { get { return pointB; } set { pointB = value; } }

    public CubeType cubeType;
    private Vector3 direction;

    private void Awake()
    {
        cube = GetComponent<Transform>();
    }

    private void Start()
    {
        if (cubeType == CubeType.xCube)
            cube.position = pointB.position;
        else if (cubeType == CubeType.zCube)
            cube.position = pointA.position;
    }

    private void Update()
    {
        if (cubeType == CubeType.xCube)
        {
            if (cube.position.x >= pointA.position.x)
            {
                direction = new Vector3(pointB.position.x, 0, 0);
            }
            else if (cube.position.x <= pointB.position.x)
            {
                direction = new Vector3(pointA.position.x, 0, 0);
            }
        }
        else if (cubeType == CubeType.zCube)
        {
            if (cube.position.z >= pointA.position.z)
            {
                direction = new Vector3(pointA.position.z, 0, 0);
            }
            else if (cube.position.z <= pointB.position.z)
            {
                direction = new Vector3(pointB.position.z, 0, 0);
            }
        }

        if (!stop)
            cube.Translate(direction * speed * Time.deltaTime);
    }
}
