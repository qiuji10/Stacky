using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using RDG;

public class CubeBehaviour : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private bool isX;

    public Transform prevCube;
    public Vector3 pointA, pointB;

    private GameManager gm;
    private Pooler pooler;
    private Rigidbody rb;
    private bool stop, isLeft;

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        pooler = FindObjectOfType<Pooler>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        if (gm.IsSpeeded)
            speed = 25;
        else
            speed = 20;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !IsPointerOverUIObject())
        {
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
                float distance = CalDistance(transform.localPosition.x, prevCube.localPosition.x);

                if (gm.isGod)
                    distance = 0;

                if (distance < 0.35f || gm.isGod)
                {
                    distance = 0;
                    Vibration.Vibrate(10);
                    gm.combo += 1;
                }
                else
                {
                    gm.combo = 0;
                }

                if (distance > transform.localScale.x)
                {
                    rb.useGravity = true;
                    rb.isKinematic = false;
                    gameObject.layer = LayerMask.NameToLayer("DropBox");
                    gm.EndGame();
                    gameObject.isStatic = true;
                    enabled = false;
                    return;
                }

                float newXSize =  transform.localScale.x - distance;

                transform.localScale = new Vector3(newXSize, 1, transform.localScale.z);

                if (distance == 0)
                    transform.localPosition = new Vector3(prevCube.localPosition.x, transform.localPosition.y, prevCube.localPosition.z);
                else
                    transform.localPosition = new Vector3((transform.localPosition.x + prevCube.localPosition.x) / 2f, transform.localPosition.y, transform.localPosition.z);

                if (distance != 0)
                {
                    Transform dropBox = pooler.GetFromPool(CubeType.dropCube);

                    dropBox.eulerAngles = Vector3.zero;

                    if (transform.position.x > prevCube.position.x)
                    {
                        //Debug.Log($"{transform.position.x} \n {prevCube.position.x} \n {distance + (newXSize / 2)}");
                        dropBox.position = new Vector3(transform.position.x + (distance + (newXSize / 2f)), transform.position.y, transform.position.z);
                    }
                    else
                    {
                        //Debug.Log($"{transform.position.x} \n {prevCube.position.x} \n {distance + (newXSize / 2)}");
                        dropBox.position = new Vector3(transform.position.x - (distance + (newXSize / 2f)), transform.position.y, transform.position.z);
                    }

                    dropBox.localScale = new Vector3(distance, 1f, transform.localScale.z);

                    dropBox.GetComponent<MeshRenderer>().material.color = gm.cubeColor;

                    dropBox.gameObject.SetActive(true);
                }

                if (gm.combo == 7)
                {
                    gm.combo = 0;
                    SpawnBonusCube(gm.bonusPrefab);
                }
                else
                    SpawnNewCube(gm.yPrefab);
            }
            else
            {
                float distance = CalDistance(transform.localPosition.z, prevCube.localPosition.z);
                if (gm.isGod)
                    distance = 0;

                if (distance < 0.35f || gm.isGod)
                {
                    distance = 0;
                    Vibration.Vibrate(10);
                    gm.combo += 1;
                }
                else
                {
                    gm.combo = 0;
                }

                if (distance > transform.localScale.x)
                {
                    rb.useGravity = true;
                    rb.isKinematic = false;
                    gameObject.layer = LayerMask.NameToLayer("DropBox");
                    gm.EndGame();
                    gameObject.isStatic = true;
                    enabled = false;
                    return;
                }

                float newXSize = transform.localScale.x - distance;

                transform.localScale = new Vector3(newXSize, 1f, transform.localScale.z);

                if (distance == 0)
                    transform.localPosition = new Vector3(prevCube.localPosition.x, transform.localPosition.y, prevCube.localPosition.z);
                else
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, (transform.localPosition.z + prevCube.localPosition.z) / 2f);

                if (distance != 0)
                {
                    Transform dropBox = pooler.GetFromPool(CubeType.dropCube);

                    dropBox.eulerAngles = Vector3.zero;

                    if (transform.position.z > prevCube.position.z)
                    {
                        //Debug.Log($"{transform.position.z} \n {prevCube.position.z} \n {distance + (newXSize / 2)}");
                        dropBox.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + (distance + (newXSize / 2f)));
                    }
                    else
                    {
                        //Debug.Log($"{transform.position.z} \n {prevCube.position.z} \n {distance + (newXSize / 2)}");
                        dropBox.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - (distance + (newXSize / 2f)));
                    }

                    dropBox.localScale = new Vector3(transform.localScale.z, 1f, distance);

                    dropBox.GetComponent<MeshRenderer>().material.color = gm.cubeColor;

                    dropBox.gameObject.SetActive(true);
                }

                if (gm.combo == 7)
                {
                    gm.combo = 0;
                    SpawnBonusCube(gm.bonusPrefab);
                }
                else
                    SpawnNewCube(gm.xPrefab);
            }
            gm.LevelUp();
            gameObject.layer = LayerMask.NameToLayer("DropBox");
            rb.constraints = RigidbodyConstraints.FreezeAll;
            enabled = false;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Deactivator"))
        {
            gameObject.SetActive(false);
        }
    }

    private void Moving()
    {
        if (isX)
        {
            if (isLeft)
            {
                transform.Translate(Vector3.left * speed * Time.deltaTime);
                if (transform.localPosition.x < pointA.x)
                {
                    isLeft = false;
                }
            }
            else
            {
                transform.Translate(Vector3.right * speed * Time.deltaTime);
                if (transform.localPosition.x > pointB.x)
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
                if (transform.localPosition.z > pointA.z)
                {
                    isLeft = false;
                }
            }
            else
            {
                transform.Translate(Vector3.right * speed * Time.deltaTime);
                if (transform.localPosition.z < pointB.z)
                {
                    isLeft = true;
                }
            }
        }
    }

    /// <summary>
    /// Calculate positive distance between a and b, 
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
            cube.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 1f, cube.transform.localPosition.z);
        else
            cube.transform.localPosition = new Vector3(cube.transform.localPosition.x, transform.localPosition.y + 1f, transform.localPosition.z);

        gm.UpdateColor();
        cube.GetComponent<MeshRenderer>().material.color = gm.cubeColor;
        cube.GetComponent<CubeBehaviour>().prevCube = transform;
    }

    private void SpawnBonusCube(GameObject bonusCube)
    {
        GameObject cube = Instantiate(bonusCube, transform.parent);
        cube.transform.localScale = transform.localScale;
        cube.transform.rotation = transform.rotation;
        cube.transform.position = new Vector3(0f, transform.position.y + 1f, 0f);
        gm.UpdateColor();
        cube.GetComponent<MeshRenderer>().material.color = gm.cubeColor;
    }

    public void Drop()
    {
        gameObject.layer = LayerMask.NameToLayer("DropBox");
        rb.isKinematic = false;
        rb.useGravity = true;
        enabled = false;
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventCurPos = new PointerEventData(EventSystem.current);
        eventCurPos.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventCurPos, results);
        return results.Count > 0;
    }
}
