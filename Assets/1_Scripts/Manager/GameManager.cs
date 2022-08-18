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
	public bool isXTurn;
	private bool breakHighScore;
	[SerializeField] private int rotationLevel;
	[SerializeField] private bool isSpeeded;
	[SerializeField] private int enableSpeedLvl;
	[SerializeField] private int disableSpeedLvl;
	[SerializeField] private float sceneTransitionTime;
	public bool IsSpeeded { get { return isSpeeded; } }
	private Database database;
	private Pooler pooler;

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

	private bool rReach, gReach, bReach, topReach;

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
	[SerializeField] private TMP_Text endText;
	[SerializeField] private GameObject pauseUI;
	[SerializeField] private GameObject clickIndicator;
	[SerializeField] private Image bonusBar;
	public GameObject Clicker { get { return clickIndicator; } set { clickIndicator = value; } }
	public Image BonusBar { get { return bonusBar; } set { bonusBar = value; } }

	[Header("Buttons Settings")]
	[SerializeField] private Button startGameButton;
	[SerializeField] private Button nextGameButton;

	[Header("Audio Settings")]
	[SerializeField] private AudioData cubeSfx;
	[SerializeField] private AudioData failStackSfx;
	public AudioData CubeSfx { get { return cubeSfx; } }

	[Header("Materials")]
	public Material baseMat;

    private void Awake()
    {
        targetGrp = FindObjectOfType<CinemachineTargetGroup>();
		database = GetComponent<Database>();
		pooler = GetComponent<Pooler>();
	}

    private void Start()
    {
		AudioManager.instance.BGM_Source.pitch = Random.Range(0.8f, 1.65f);

		Application.targetFrameRate = 300;
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
		if (rotatePhase && Time.timeScale == 1)
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
		ComboSFX(combo);

		if (level > database.playerData.highscore)
        {
			breakHighScore = true;
			database.playerData.highscore = level;
			database.SaveGame();
        }

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
		if (level == 80)
		{
			rotationSpeed *= 5;
		}

		// Every 10 or 5 levels change cube moving speed
		if (level % enableSpeedLvl == 0)
        {
			isSpeeded = !isSpeeded;
		}

		if (isSpeeded && level % disableSpeedLvl == 0)
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
		if (breakHighScore)
        {
			endText.text = $"New Highscore!\n\nTap to Menu";
		}

		nextGameButton.interactable = true;
		targetGrp.AddMember(baseObj.transform, 1, 0);
		colorData.bottomColor = bottomColor;
		colorData.topColor = topColor;
		StartCoroutine(EnableNextGameButton());
	}

	public void NextGame()
    {
		vcam2.Priority = 11;
		StartCoroutine(GameplayFadeOff());
		StartCoroutine(ReloadScene(false));
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

	public IEnumerator GameplayFadeOff()
    {
		while (gameplayCanvas.alpha > 0)
		{
			gameplayCanvas.alpha -= menuFadeSpeed;
			menuCanvas.alpha += menuFadeSpeed;
			yield return null;
		}

		gameplayCanvas.gameObject.SetActive(false);
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
		StartCoroutine(ReloadScene(false));
	}

	private IEnumerator ReloadScene(bool needFade)
    {
		if (needFade)
			StartCoroutine(MenuFade(false));

		float timer = 0;
		float vol = AudioManager.instance.BGM_Source.volume;

		while (timer <= 1.5f)
        {
			timer += Time.deltaTime;
			AudioManager.instance.BGM_Source.volume = Mathf.Lerp(vol, 0, timer/ 1.5f);
			yield return null;
		}

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

	private void ComboSFX(int num)
    {
		if (num == 0)
        {
			int rand = Random.Range(0, 7);
			AudioManager.instance.PlaySFX(failStackSfx, rand);
		}
		else
        {
			AudioManager.instance.PlaySFX(cubeSfx, num);
        }
	}

	public void EnableComboParticle(Transform cube)
    {
		Transform particle = pooler.GetFromPool(CubeType.comboParticle);
		particle.position = new Vector3(cube.position.x, cube.position.y - 0.5f, cube.position.z);
		particle.rotation = cube.rotation;
		particle.localScale = new Vector3(cube.localScale.x / 10f, cube.localScale.y / 10f, cube.localScale.z / 10f);
		particle.gameObject.SetActive(true);
		StartCoroutine(PlayParticle(particle.gameObject));
	}

	IEnumerator PlayParticle(GameObject particle)
    {
		yield return new WaitForSeconds(1.5f);
		particle.gameObject.SetActive(false);
	}
}
