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
    private bool stop, isLeft;

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            gameObject.name = gm.index.ToString();
            stop = true;
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
                float newXSize =  transform.localScale.x - distance;

                transform.localScale = new Vector3(newXSize, 1, transform.localScale.z);

                //here should add combo, and if distance  < 0.5 also counted as 0
                if (distance == 0)
                    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                else
                    transform.position = new Vector3((transform.position.x + prevCube.position.x) / 2, transform.position.y, transform.position.z);

                SpawnNewCube(gm.yPrefab);
            }
            else
            {
                float distance = CalDistance(transform.position.z, prevCube.position.z);
                float newXSize = transform.localScale.x - distance;

                transform.localScale = new Vector3(newXSize, 1, transform.localScale.z);

                if (distance == 0)
                    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                else
                    transform.position = new Vector3(transform.position.x, transform.position.y, (transform.position.z + prevCube.position.z) / 2);

                SpawnNewCube(gm.xPrefab);
            }
            gm.index++;
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
                Debug.Log("1a");
                return a - b;
            }
            else
            {
                Debug.Log("1b");
                return b - a;
            }
        }
        else if (a < 0 && b > 0)
        {
            Debug.Log("2");
            return Mathf.Abs(a) + b;
        }
        else if (a > 0 && b < 0)
        {
            Debug.Log("3");
            return Mathf.Abs(b) + a;
        }
        else if (a < 0 && b <= 0)
        {
            if (a < b)
            {
                Debug.Log("4a");
                return Mathf.Abs(a) + b;
            }
            else
            {
                Debug.Log("4b");
                return Mathf.Abs(b) + a;
            }
        }
        else
        {
            Debug.Log("5");
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

        cube.GetComponent<CubeBehaviour>().prevCube = transform;
    }
}
