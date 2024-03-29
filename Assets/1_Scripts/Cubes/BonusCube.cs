using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using RDG;

public class BonusCube : MonoBehaviour
{
    [SerializeField] private float bonusTime;

    private GameManager gm;
    private CinemachineImpulseSource impSource;
    private float timer;
    private bool isBonus = true;

    private void Awake() 
    {
        gm = FindObjectOfType<GameManager>();
        impSource = FindObjectOfType<CinemachineImpulseSource>();
    }

    private void Update()
    {
        if (transform.localScale.x >= 10 && transform.localScale.z >= 10)
        {
            timer = bonusTime;
        }

        if (isBonus)
        {
            timer += Time.deltaTime;
            if (timer >= bonusTime)
            {
                gm.combo = 0;
                isBonus = false;
                gm.Clicker.gameObject.SetActive(false);
                gm.BonusBar.gameObject.SetActive(false);
                gm.BonusBar.fillAmount = 1;
                MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
                meshRenderer.material = gm.baseMat;
                meshRenderer.material.color = gm.cubeColor;
                SpawnNewCube(gm.xPrefab);
                enabled = false;
            }
            else if (timer < bonusTime)
            {
                if (!gm.BonusBar.gameObject.activeInHierarchy)
                {
                    gm.Clicker.gameObject.SetActive(true);
                    gm.BonusBar.gameObject.SetActive(true);
                }
                
                gm.BonusBar.fillAmount -= 1.0f / bonusTime * Time.deltaTime;
                gm.BonusBar.color = Color.Lerp(Color.green, Color.red, 1 - gm.BonusBar.fillAmount);
            }
        }

        if (isBonus && Input.GetMouseButtonDown(0) && (transform.localScale.x < 10 || transform.localScale.z < 10 ))
        {
            Vibration.Vibrate(100);
            impSource.GenerateImpulse();
            AudioManager.instance.PlaySFX(gm.CubeSfx, 8);

            if (transform.localScale.x < 10)
            {
                transform.localScale = new (transform.localScale.x + 0.05f, 1f, transform.localScale.z);
            }

            if (transform.localScale.z < 10)
            {
                transform.localScale = new (transform.localScale.x, 1f, transform.localScale.z + 0.05f);
            }
        }
    }

    private void SpawnNewCube(GameObject prefabCube)
    {
        GameObject cube = Instantiate(prefabCube, transform.parent);

        if (gm.isXTurn)
            cube.transform.localScale = transform.localScale;
        else
            cube.transform.localScale = new Vector3(transform.localScale.z, transform.localScale.y, transform.localScale.x);

        cube.transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);

        gm.UpdateColor();
        cube.GetComponent<MeshRenderer>().material.color = gm.cubeColor;
        cube.GetComponent<CubeBehaviour>().prevCube = transform;
    }
}
