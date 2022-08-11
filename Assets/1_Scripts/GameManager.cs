using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public GameObject xPrefab, yPrefab;
	public GameObject camObj, baseObj, firstCube;

	[Header("GodMode")]
	public bool isGod;

	[Header("Color Settings")]
	public Color cubeColor;
	public Color nextColor;

	[Range(0, 1)] public float r;
	[Range(0, 1)] public float g;
	[Range(0, 1)] public float b;

	public int level;

	[SerializeField] private float deviation;
	[SerializeField] private float adjustor;

	private bool rReach, gReach, bReach, bottomReach, topReach, endGame;

	[Header("BG Color")]
	[SerializeField] private Renderer bgRender;
	[SerializeField] private Color topColor;
	[SerializeField] private Color nextTopColor;
	[SerializeField] private Color bottomColor;
	[SerializeField] private Color nextBottomColor;

	

	//private void Awake()
 //   {
	//	camObj = Camera.main.gameObject;
	//}

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

		r = Random.Range(0.0f, 1.0f);
		g = Random.Range(0.0f, 1.0f);
		b = Random.Range(0.0f, 1.0f);

		topColor = new Color(r, g, b, 1.0f);

		r = Random.Range(0.0f, 1.0f);
		g = Random.Range(0.0f, 1.0f);
		b = Random.Range(0.0f, 1.0f);

		bottomColor = new Color(r, g, b, 1.0f);

		r = Random.Range(0.0f, 1.0f);
		g = Random.Range(0.0f, 1.0f);
		b = Random.Range(0.0f, 1.0f);

		nextTopColor = new Color(r, g, b, 1.0f);

		r = Random.Range(0.0f, 1.0f);
		g = Random.Range(0.0f, 1.0f);
		b = Random.Range(0.0f, 1.0f);

		nextBottomColor = new Color(r, g, b, 1.0f);

		baseObj.GetComponent<Renderer>().material.color = cubeColor;
		firstCube.GetComponent<Renderer>().material.color = cubeColor;

		bgRender.material.SetColor("_Color1", bottomColor);
		bgRender.material.SetColor("_Color2", topColor);
	}

    private void OnValidate()
    {
		cubeColor = new Color(r, g, b, 1.0f);
	}

	public void UpdateColor()
    {
		#region Cube Color
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
		#endregion

		#region Bottom Region
		if (bottomColor.r < (nextBottomColor.r + deviation) && bottomColor.r > (nextBottomColor.r - deviation))
		{
			bottomReach = true;
			bottomColor.r = nextBottomColor.r;
		}
		else if (nextBottomColor.r > bottomColor.r)
			bottomColor.r = bottomColor.r + adjustor;
		else if (nextBottomColor.r < bottomColor.r)
			bottomColor.r = bottomColor.r - adjustor;

		if (bottomColor.g < (nextBottomColor.g + deviation) && bottomColor.g > (nextBottomColor.g - deviation))
		{
			bottomReach = true;
			bottomColor.g = nextBottomColor.g;
		}
		else if (nextBottomColor.g > bottomColor.g)
			bottomColor.g = bottomColor.g + adjustor;
		else if (nextBottomColor.g < bottomColor.g)
			bottomColor.g = bottomColor.g - adjustor;

		if (bottomColor.b < (nextBottomColor.b + deviation) && bottomColor.b > (nextBottomColor.b - deviation))
		{
			bottomReach = true;
			bottomColor.b = nextBottomColor.b;
		}
		else if (nextBottomColor.b > bottomColor.b)
			bottomColor.b = bottomColor.r + adjustor;
		else if (nextBottomColor.b < bottomColor.b)
			bottomColor.b = bottomColor.b - adjustor;
		#endregion

		#region Top Region
		if (topColor.r < (nextTopColor.r + deviation) && topColor.r > (nextTopColor.r - deviation))
		{
			topReach = true;
			topColor.r = nextTopColor.r;
		}
		else if (nextTopColor.r > topColor.r)
			topColor.r = topColor.r + adjustor;
		else if (nextTopColor.r < topColor.r)
			topColor.r = topColor.r - adjustor;

		if (topColor.g < (nextTopColor.g + deviation) && topColor.g > (nextTopColor.g - deviation))
		{
			topReach = true;
			topColor.g = nextTopColor.g;
		}
		else if (nextTopColor.g > topColor.g)
			topColor.g = topColor.g + adjustor;
		else if (nextTopColor.g < topColor.g)
			topColor.g = topColor.g - adjustor;

		if (topColor.b < (nextTopColor.b + deviation) && topColor.b > (nextTopColor.b - deviation))
		{
			topReach = true;
			topColor.b = nextTopColor.b;
		}
		else if (nextTopColor.b > topColor.b)
			topColor.b = topColor.r + adjustor;
		else if (nextTopColor.b < topColor.b)
			topColor.b = topColor.b - adjustor;
		#endregion
		
		bgRender.material.SetColor("_Color1", topColor);
		bgRender.material.SetColor("_Color2", bottomColor);

		if (rReach && gReach && bReach)
        {
			NextColor();
		}

		if (bottomReach)
		{
			r = Random.Range(0.0f, 1.0f);
			g = Random.Range(0.0f, 1.0f);
			b = Random.Range(0.0f, 1.0f);
			bottomReach = false;
			nextBottomColor = new Color(r, g, b, 1.0f);
		}

		if (topReach)
		{
			r = Random.Range(0.0f, 1.0f);
			g = Random.Range(0.0f, 1.0f);
			b = Random.Range(0.0f, 1.0f);
			topReach = false;
			nextTopColor = new Color(r, g, b, 1.0f);
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
