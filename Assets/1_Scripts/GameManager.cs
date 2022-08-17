using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using Cinemachine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject xPrefab, yPrefab, bonusPrefab;
	public GameObject camObj, baseParent, baseObj, firstCube;

	[Header("GodMode")]
	public bool isGod;

	[Header("Menu Settings")]
	[SerializeField] private CanvasGroup menuCanvas;
	[SerializeField] private CanvasGroup gameplayCanvas;
	[SerializeField] private float menuFadeSpeed;
	public static bool isFirstTime;

	[Header("Game Base Settings")]
	public int level;
	public int combo;
	[SerializeField] private int rotationLevel;
	[SerializeField] private bool isSpeeded;
	[SerializeField] private float sceneTransitionTime;
	public bool IsSpeeded { get { return isSpeeded; } }

	[Header("Rotation Phase Settings")]
	[SerializeField] private bool rotatePhase;
	[SerializeField] private float rotationSpeed;
	private bool changeRotationDir;
	public bool RotatePhase { get { return rotatePhase; } }
	public bool ChangeRotationDir { get { return changeRotationDir; } set { changeRotationDir = value; } }

	[Header("Camera Settings")]
	public float camHeightPerBlock;
	[SerializeField] private float camBlendSpeed;
	public CinemachineVirtualCamera vcam;
	public CinemachineVirtualCamera vcam2;
	private CinemachineTargetGroup targetGrp;

	[Header("Color Settings")]
	public Color cubeColor;
	public Color nextColor;

	[Range(0, 1)] public float r;
	[Range(0, 1)] public float g;
	[Range(0, 1)] public float b;

	[SerializeField] private float deviation;
	[SerializeField] private float adjustor;

	private bool rReach, gReach, bReach, topReach, endGame;

	[Header("BG Color")]
	[SerializeField] private Renderer bgRender;
	[SerializeField] private Color topColor;
	[SerializeField] private Color nextTopColor;
	[SerializeField] private Color bottomColor;
	[SerializeField] private BgColorData colorData;
	private float timePassed;
	private float transTime = 5f;

	[Header("UI Settings")]
	[SerializeField] private TMP_Text scoreText;
	[SerializeField] private GameObject pauseUI;
	[SerializeField] private GameObject tapText;

	[Header("Buttons Settings")]
	[SerializeField] private Button startGameButton;
	[SerializeField] private Button nextGameButton;

	[Header("Materials")]
	public Material baseMat;

    private void Awake()
    {
        targetGrp = FindObjectOfType<CinemachineTargetGroup>();
	}

    private void Start()
    {
		StartCoroutine(IntroFade());
		StartCoroutine(SetTransCamPriority());

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

		baseObj.GetComponent<Renderer>().material.color = cubeColor;
		firstCube.GetComponent<Renderer>().material.color = cubeColor;

        int num = PlayerPrefs.GetInt("played");

        if (num == 0)
        {
            bgRender.material.SetColor("_Color1", bottomColor);
            bgRender.material.SetColor("_Color2", topColor);
            Debug.Log("first");
            PlayerPrefs.SetInt("played", 1);
        }
        else
        {
            bgRender.material.SetColor("_Color1", colorData.bottomColor);
            bgRender.material.SetColor("_Color2", colorData.topColor);
            StartCoroutine(SceneTransBGColorLerp());
        }
    }

    private void OnValidate()
    {
		cubeColor = new Color(r, g, b, 1.0f);
	}

    private void Update()
    {
		if (rotatePhase)
        {
			if (changeRotationDir)
            {
				baseParent.transform.eulerAngles += new Vector3(0, -rotationSpeed, 0);
			}
			else
            {
				baseParent.transform.eulerAngles += new Vector3(0, rotationSpeed, 0);
			}
		}

	}

	public void StartGame()
    {
		firstCube.SetActive(true);
		StartCoroutine(MenuFade(true));
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
		
		StartCoroutine(BgColorLerp());

		bgRender.material.SetColor("_Color1", bottomColor);
		bgRender.material.SetColor("_Color2", topColor);

		if (rReach && gReach && bReach)
        {
			NextColor();
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

    IEnumerator SceneTransBGColorLerp()
    {
		while (timePassed < transTime)
        {
			timePassed += Time.deltaTime;
			colorData.topColor = Color.Lerp(colorData.topColor, topColor, timePassed / transTime);
			colorData.bottomColor = Color.Lerp(colorData.bottomColor, bottomColor, timePassed / transTime);
			bgRender.material.SetColor("_Color1", colorData.bottomColor);
			bgRender.material.SetColor("_Color2", colorData.topColor);
			yield return null;
		}
		timePassed = 0f;
		yield return null;
	}

    IEnumerator BgColorLerp() 
	{
		topColor = Color.Lerp(topColor, nextTopColor, 5 * Time.deltaTime);
		if (topColor.g < (nextTopColor.g + deviation + 0.05) && topColor.g > (nextTopColor.g - deviation - 0.05))
		{
			topReach = true;
		}

		bottomColor = Color.Lerp(bottomColor, topColor, 3 * Time.deltaTime);

		yield return null;
	}

	public void LevelUp()
	{
		level++;
		scoreText.text = level.ToString();

		Vector3 pos = camObj.transform.position;
		camObj.transform.position = new Vector3(pos.x, pos.y + camHeightPerBlock, pos.z);
		vcam.transform.position = new Vector3(18.5f, vcam.transform.position.y + camHeightPerBlock, -18.5f);

		// Trigger Base Rotation
        if (RotatePhase)
        {
            ChangeRotationDir = !ChangeRotationDir;
        }
		else
        {
			if (level == rotationLevel)
            {
				rotatePhase = true;
			}
		}

		// Speed Up Rotation After lvl 40
		if (level == 40)
		{
			rotationSpeed *= 5;
		}

		// Every 25 levels change cube moving speed
		if (level % 25 == 0)
        {
			isSpeeded = !isSpeeded;
		}

		
	}

	public void GodMode()
    {
		isGod = !isGod;
    }

	public void EndGame()
    {
		endGame = true;
		nextGameButton.interactable = true;
		targetGrp.AddMember(baseObj.transform, 1, 0);
		colorData.bottomColor = bottomColor;
		colorData.topColor = topColor;
		StartCoroutine(EnableNextGameButton());
		Debug.Log(endGame);
	}

	public void NextGame()
    {
		vcam2.Priority = 11;
		StartCoroutine(ReloadScene(true));
	}

	public IEnumerator MenuFade(bool fadeToGame)
    {
		if (fadeToGame)
        {
			gameplayCanvas.gameObject.SetActive(true);

			while (menuCanvas.alpha > 0)
			{
				menuCanvas.alpha -= menuFadeSpeed;
				gameplayCanvas.alpha += menuFadeSpeed;
				yield return null;
			}

			menuCanvas.gameObject.SetActive(false);
		}
		else
        {
			menuCanvas.gameObject.SetActive(true);

			while (gameplayCanvas.alpha > 0)
			{
				gameplayCanvas.alpha -= menuFadeSpeed;
				menuCanvas.alpha += menuFadeSpeed;
				yield return null;
			}

			gameplayCanvas.gameObject.SetActive(false);
		}
		
    }

	public IEnumerator IntroFade()
    {
		menuCanvas.gameObject.SetActive(true);

		while (menuCanvas.alpha < 1)
		{
			menuCanvas.alpha += menuFadeSpeed;
			yield return null;
		}
	}

	public void PauseGame()
    {
		pauseUI.SetActive(true);
		gameplayCanvas.gameObject.SetActive(false);
		Time.timeScale = 0f;
	}

	public void ResumeGame()
    {
		Time.timeScale = 1f;
		gameplayCanvas.gameObject.SetActive(true);
		pauseUI.SetActive(false);
	}

	public void BackMenu()
    {
		Time.timeScale = 1f;
		pauseUI.SetActive(false);
		menuCanvas.gameObject.SetActive(false);
		vcam2.Priority = 11;
		colorData.bottomColor = bottomColor;
		colorData.topColor = topColor;
		//StartCoroutine(MenuFade(false));
		StartCoroutine(ReloadScene(false));

	}

	private IEnumerator ReloadScene(bool needFade)
    {
		if (needFade)
			StartCoroutine(MenuFade(false));

		yield return new WaitForSeconds(sceneTransitionTime);
		SceneManager.LoadScene(0);
	}

	private IEnumerator SetTransCamPriority()
    {
		yield return new WaitForSeconds(1);
		vcam2.Priority = 9;
		yield return new WaitForSeconds(2);
		startGameButton.interactable = true;
	}

	private IEnumerator EnableNextGameButton()
    {
		yield return new WaitForSeconds(1.8f);
		nextGameButton.gameObject.SetActive(true);
	}
}
