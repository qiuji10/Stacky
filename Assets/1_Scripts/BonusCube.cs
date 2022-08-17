using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusCube : MonoBehaviour
{
    [SerializeField] private float bonusTime;

    private GameManager gm;
    private float timer;
    private bool isBonus = true;

    private void Awake() 
    {
        gm = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if (isBonus)
        {
            timer += Time.deltaTime;
            if (timer >= bonusTime)
            {
                isBonus = false;
                MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
                meshRenderer.material = gm.baseMat;
                meshRenderer.material.color = gm.cubeColor;
                SpawnNewCube(gm.xPrefab);
                enabled = false;
            }
        }

        if (isBonus && Input.GetMouseButtonDown(0) && (transform.localScale.x < 10 || transform.localScale.z < 10 ))
        {
            Handheld.Vibrate();

            if (transform.localScale.x < 10)
            {
                transform.localScale = new (transform.localScale.x + 0.05f, 1f, transform.localScale.z);
            }

            if (transform.localScale.z < 10)
            {
                transform.localScale = new (transform.localScale.x, 1f, transform.localScale.z + 0.05f);
            }
        }

        if (transform.localScale.x >= 10 && transform.localScale.z >= 10 )
        {
            timer = bonusTime;
        }
    }

    private void SpawnNewCube(GameObject prefabCube)
    {
        GameObject cube = Instantiate(prefabCube, transform.parent);
        cube.transform.localScale = transform.localScale;
        cube.transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);

        gm.UpdateColor();
        cube.GetComponent<MeshRenderer>().material.color = gm.cubeColor;
        cube.GetComponent<CubeBehaviour>().prevCube = transform;
    }
}
