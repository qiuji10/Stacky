using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBehaviour : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private bool isX;

    public Transform prevCube;
    public Vector3 pointA, pointB;

    private GameManager gm;
    private Rigidbody rb;
    private bool stop, isLeft;

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            stop = true;
            Vector3 pos = gm.camObj.transform.position;
            gm.camObj.transform.position = new Vector3(pos.x, pos.y + 1, pos.z);
        }

        if (!stop)
        {
            Moving();
        }
        else
        {
            speed = 0;
            if (isX)
            {
                float distance = CalDistance(transform.position.x, prevCube.position.x);

                if (distance > transform.localScale.x)
                {
                    rb.useGravity = true;
                    gm.EndGame();
                    enabled = false;
                    return;
                }

                float newXSize =  transform.localScale.x - distance;

                transform.localScale = new Vector3(newXSize, 1, transform.localScale.z);
                //here should add combo, and if distance  < 0.5 also counted as 0
                if (distance == 0)
                    transform.position = new Vector3(prevCube.position.x, transform.position.y, prevCube.position.z);
                else
                    transform.position = new Vector3((transform.position.x + prevCube.position.x) / 2, transform.position.y, transform.position.z);

                SpawnNewCube(gm.yPrefab);
            }
            else
            {
                float distance = CalDistance(transform.position.z, prevCube.position.z);

                if (distance > transform.localScale.x)
                {
                    rb.useGravity = true;
                    gm.EndGame();
                    enabled = false;
                    return;
                }

                float newXSize = transform.localScale.x - distance;

                transform.localScale = new Vector3(newXSize, 1, transform.localScale.z);

                if (distance == 0)
                    transform.position = new Vector3(prevCube.position.x, transform.position.y, prevCube.position.z);
                else
                    transform.position = new Vector3(transform.position.x, transform.position.y, (transform.position.z + prevCube.position.z) / 2);

                SpawnNewCube(gm.xPrefab);
            }
            gm.LevelUp();
            enabled = false;
        }
    }

    private void Moving()
    {
        if (isX)
        {
            if (isLeft)
            {
                transform.Translate(Vector3.left * speed * Time.deltaTime);
                if (transform.position.x < pointA.x)
                {
                    isLeft = false;
                }
            }
            else
            {
                transform.Translate(Vector3.right * speed * Time.deltaTime);
                if (transform.position.x > pointB.x)
                {
                    isLeft = true;
                }
            }
        }
        else
        {
            if (isLeft)
            {
                transform.Translate(Vector3.left * speed * Time.deltaTime);
                if (transform.position.z > pointA.z)
                {
                    isLeft = false;
                }
            }
            else
            {
                transform.Translate(Vector3.right * speed * Time.deltaTime);
                if (transform.position.z < pointB.z)
                {
                    isLeft = true;
                }
            }
        }
    }

    /// <summary>
    /// Calculate real distance between a and b, 
    /// a is current cube x/z position, b is previous cube x/z position.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    private float CalDistance(float a, float b)
    {
        if (a >= 0 && b >= 0)
        {
            if (a > b)
            {
                return a - b;
            }
            else
            {
                return b - a;
            }
        }
        else if (a < 0 && b > 0)
        {
            return Mathf.Abs(a) + b;
        }
        else if (a > 0 && b < 0)
        {
            return Mathf.Abs(b) + a;
        }
        else if (a < 0 && b <= 0)
        {
            if (a < b)
            {
                return Mathf.Abs(a) + b;
            }
            else
            {
                return Mathf.Abs(b) + a;
            }
        }
        else
        {
            return 0;
        }
    }

    private void SpawnNewCube(GameObject prefabCube)
    {
        GameObject cube = Instantiate(prefabCube, transform.parent);
        cube.transform.localScale = new Vector3(transform.localScale.z, transform.localScale.y, transform.localScale.x);

        if (isX)
            cube.transform.position = new Vector3(transform.position.x, transform.position.y + 1, cube.transform.position.z);
        else
            cube.transform.position = new Vector3(cube.transform.position.x, transform.position.y + 1, transform.position.z);

        gm.UpdateColor();
        cube.GetComponent<MeshRenderer>().material.color = gm.cubeColor;
        cube.GetComponent<CubeBehaviour>().prevCube = transform;
    }

    private void Resetter()
    {
        rb.useGravity = false;
        enabled = true;
    }
}
