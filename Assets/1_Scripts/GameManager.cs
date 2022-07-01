using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public GameObject xPrefab, yPrefab;
	public GameObject camObj, baseObj, firstCube;

	[Header("Color Settings")]
	public Color cubeColor;
	public Color nextColor;

	[Range(0, 1)] public float r;
	[Range(0, 1)] public float g;
	[Range(0, 1)] public float b;

	public int level;

	[SerializeField] private float deviation;
	[SerializeField] private float adjustor;

	private bool rReach, gReach, bReach, endGame;

	private void Awake()
    {
		camObj = Camera.main.gameObject;
	}

    private void Start()
    {
		r = Random.Range(0.0f, 1.0f);
		g = Random.Range(0.0f, 1.0f);
		b = Random.Range(0.0f, 1.0f);

		nextColor = new Color(r, g, b, 1.0f);

		r = Random.Range(0.0f, 1.0f);
		g = Random.Range(0.0f, 1.0f);
		b = Random.Range(0.0f, 1.0f);

		cubeColor = new Color(r, g, b, 1.0f);

		baseObj.GetComponent<Renderer>().material.color = cubeColor;
		firstCube.GetComponent<Renderer>().material.color = cubeColor;
	}

    private void OnValidate()
    {
		cubeColor = new Color(r, g, b, 1.0f);
	}

	public void UpdateColor()
    {
        #region Red Region
        if (cubeColor.r < (nextColor.r + deviation) && cubeColor.r > (nextColor.r - deviation))
        {
			rReach = true;
			cubeColor.r = nextColor.r;
        }
        else if (nextColor.r > cubeColor.r)
            cubeColor.r = cubeColor.r + adjustor;
        else if (nextColor.r < cubeColor.r)
            cubeColor.r = cubeColor.r - adjustor;
		#endregion

		#region Green Region
		if (cubeColor.g < (nextColor.g + deviation) && cubeColor.g > (nextColor.g - deviation))
        {
			gReach = true;
            cubeColor.g = nextColor.g;
        }
        else if (nextColor.g > cubeColor.g)
            cubeColor.g = cubeColor.g + adjustor;
        else if (nextColor.g < cubeColor.g)
            cubeColor.g = cubeColor.g - adjustor;
		#endregion

		#region Blue Region
		if (cubeColor.b < (nextColor.b + deviation) && cubeColor.b > (nextColor.b - deviation))
		{
			bReach = true;
			cubeColor.b = nextColor.b;
		}
		else if (nextColor.b > cubeColor.b)
			cubeColor.b = cubeColor.b + adjustor;
        else if (nextColor.b < cubeColor.b)
			cubeColor.b = cubeColor.b - adjustor;
		#endregion

		if (rReach && gReach && bReach)
        {
			NextColor();
		}
	}

	private void NextColor()
    {
		r = Random.Range(0.0f, 1.0f);
		g = Random.Range(0.0f, 1.0f);
		b = Random.Range(0.0f, 1.0f);

		rReach = bReach = gReach = false;

		nextColor = new Color(r, g, b, 1.0f);
	}

	public void LevelUp()
	{
		level++;
		Debug.Log(level);
	}

	public void EndGame()
    {
		endGame = true;
		Debug.Log(endGame);
	}
}
