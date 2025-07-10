using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameControllerScript : MonoBehaviour
{
    #region SingletonSetup
    private void Awake() => Instance = this;
    public static GameControllerScript Instance;
    #endregion

    #region UnityCallbacks
    private void Start()
    {
        InitializeGameSettings();
        UpdateNotebookCount();
    }

    private void Update()
    {
        if (!KF.gamePaused)
        {
            FinaleModeAnnoyance();
        }

        GameOverFunction();
    }
    #endregion

    #region Initialization
    private void InitializeGameSettings()
    {
        Singleton<Options>.Instance.GetVolume();
        Singleton<Options>.Instance.GetVSync();

        Time.timeScale = 1f;
        AudioListener.pause = false;
        cullingMask = PlayerCamera.cullingMask;

        audioQueue = GetComponent<AudioQueueScript>();
        Math = GetComponent<LearningGameManager>();
        progress = GetComponent<EndingManager>();

        mode = PlayerPrefs.GetString("CurrentMode");

        if (mode == "endless")
        {
            baldiScrpt.endless = true;
            sockCript.endless = true;
        }

        gameOverDelay = 0.5f;
        schoolMusic.Play();
    }
    #endregion

    #region NotebookManagement
    public void UpdateNotebookCount()
    {
        notebookCount.text = mode == "story" ? $"{notebooks}/{maxNotebooks}" : $"{notebooks}";

        if (mode == "endless" && notebooks / maxNotebooks > lastRespawnCount)
        {
            lastRespawnCount = notebooks / maxNotebooks;
            EndlessModeRestart();
        }

        if (notebooks == maxNotebooks && mode == "story" && !finaleMode)
        {
            ActivateFinaleMode();
        }

        if (AdditionalGameCustomizer.Instance?.ExitCounter == true && notebooks == maxNotebooks)
        {
            notebookCount.text = $"{exitsReached}/4";
        }
    }

    public void CollectNotebook(float numberOfNotebooks)
    {
        notebooks += Mathf.FloorToInt(numberOfNotebooks);
        UpdateNotebookCount();
    }
    #endregion

    #region HelperFunctions
    private void EndlessModeRestart()
    {
        ItemsToRespawn.ForEach(item => item.SetActive(true));
        MachinesToRestock.ForEach(machine => machine?.RestockVendingMachine());
    }
    #endregion

    #region SpoopModeHandling
    public void GetAngry(float value)
    {
        if (!spoopMode)
        {
            ActivateSpoopMode();
        }
        baldiScrpt.GetAngry(value);
    }

    public void ActivateSpoopMode()
    {
        spoopMode = true;

        entrances.ForEach(e => e.Enable());
        ObjectsToDisable.ForEach(o => o.SetActive(false));
        ObjectsToEnable.ForEach(o => o.SetActive(true));

        schoolMusic.Stop();
        Math.learnMusic.Stop();

        if (AdditionalGameCustomizer.Instance != null && !AdditionalGameCustomizer.Instance.NoYCTP)
        {
            Math.learnMusic.PlayOneShot(aud_Hang);
        }
        else
        {
            StartCoroutine(audioQueue.FadeOut(schoolMusic, 0.25f));
        }
    }
    #endregion

    #region GameOverLogic
    private void GameOverFunction()
    {
        if (!player.gameOver) return;

        AudioListener.pause = true;
        gamaOvarDevice.ignoreListenerPause = true;
        Time.timeScale = 0f;

        PlayerCamera.farClipPlane = gameOverDelay * 400f;
        gameOverDelay -= Time.unscaledDeltaTime;

        if (!gamaOvarDevice.isPlaying)
        {
            audOverVal = (int)Random.Range(0f, LoseSounds.Length);
            gamaOvarDevice.PlayOneShot(LoseSounds[audOverVal]);
        }

        if (mode == "endless" && notebooks > PlayerPrefs.GetInt("HighBooks") && !highScoreText.activeSelf)
        {
            highScoreText.SetActive(true);
        }

        if (gameOverDelay <= 0f)
        {
            if (mode == "endless")
            {
                if (notebooks > PlayerPrefs.GetInt("HighBooks"))
                {
                    PlayerPrefs.SetInt("HighBooks", notebooks);
                }
                PlayerPrefs.SetInt("CurrentBooks", notebooks);
            }
            Time.timeScale = 1f;
            SceneManager.LoadScene(gameoverScene);
        }
    }
    #endregion

    #region FinaleModeManagement
    private void ActivateFinaleMode()
    {
        finaleMode = true;
        entrances.ForEach(exits => exits.Disable());
    }

    private void FinaleModeAnnoyance()
    {
        if (!finaleMode || audioDevice.isPlaying) return;

        if (exitsReached == 2)
        {
            PlayAudioClip(aud_ChaosStartLoop, true);
        }
        else if (exitsReached == 3 && !progress.GetSecret & !progress.GetResults)
        {
            PlayAudioClip(aud_ChaosFinal, true);
        }
    }

    private void PlayAudioClip(AudioClip clip, bool loop)
    {
        audioDevice.clip = clip;
        audioDevice.loop = loop;
        audioDevice.Play();
    }
    #endregion

    #region ExitCounterHandling
    public void ExitReached()
    {
        exitsReached++;

        if (AdditionalGameCustomizer.Instance != null && AdditionalGameCustomizer.Instance.ExitCounter)
        {
            UpdateNotebookCount();
            Icon.Rebind();
            Icon.Play("Icon2Spin", -1, 0f);
        }

        if (exitEasingCoroutine != null)
        {
            StopCoroutine(exitEasingCoroutine);
        }
        exitEasingCoroutine = StartCoroutine(exitEasing(exitsReached));

        if (exitsReached == 1)
        {
            audioDevice.PlayOneShot(aud_Switch, 0.8f);
            if (AdditionalGameCustomizer.Instance != null)
            {
                switch (AdditionalGameCustomizer.Instance.currentSkybox)
                {
                    case AdditionalGameCustomizer.SkyboxStyle.Day:
                        RenderSettings.skybox = AdditionalGameCustomizer.Instance.NormalRedSky;
                        break;
                    case AdditionalGameCustomizer.SkyboxStyle.Sunset:
                        RenderSettings.skybox = AdditionalGameCustomizer.Instance.RedTwilightSky;
                        break;
                    case AdditionalGameCustomizer.SkyboxStyle.Night:
                        RenderSettings.skybox = AdditionalGameCustomizer.Instance.RedNightSky;
                        break;
                }
            }
            escapeMusic.pitch = 0.5f;
        }

        if (exitsReached == 2)
        {
            audioDevice.PlayOneShot(aud_Switch, 0.8f);
            audioDevice.clip = aud_ChaosStart;
            audioDevice.Play();
        }

        if (exitsReached == 3)
        {
            audioDevice.PlayOneShot(aud_Switch, 0.8f);
            audioDevice.clip = aud_ChaosBuildUp;
            audioDevice.Play();
        }
    }

    private IEnumerator exitEasing(int exitCount)
    {
        float duration = 7f;
        Color start = RenderSettings.ambientLight;
        Color target = Color.Lerp(new Color(1, 0.7f, 0.7f), Color.red, Mathf.Clamp01(exitCount / 3f));

        for (float t = 0; t < 1; t += Time.deltaTime / duration)
        {
            RenderSettings.ambientLight = Color.Lerp(start, target, Mathf.SmoothStep(0, 1, t));
            yield return null;
        }

        RenderSettings.ambientLight = target;
    }
    #endregion

    #region PlayerTeleportation
    public void CraftersTeleport()
    {
        if (player.hugging)
        {
            player.hugging = false;
            player.sweepingFailsave = 0f;
        }
        else if (player.jumpRope)
        {
            player.jumpRope = false;
            player.DeactivateJumpRope();
            player.playtime.Disappoint();
        }

        var newPos = AILocationSelector.SetNewTargetForAgent(null, "default") + Vector3.up * player.height;
        player.transform.position = newPos;
        baldi.transform.position = newPos;
    }

    public IEnumerator TeleporterFunction()
    {
        player.movementLocked = true;
        playerCollider.enabled = false;

        int teleports = Random.Range(12, 16);
        float delay = 0.2f;
        const float increaseFactor = 1.1f;

        for (int i = 0; i < teleports; i++)
        {
            yield return new WaitForSeconds(delay);
            PlayerTeleport();
            delay *= increaseFactor;
        }

        player.movementLocked = false;
        playerCollider.enabled = true;
    }

    private void PlayerTeleport()
    {
        player.transform.position = AILocationSelector.SetNewTargetForAgent(null, "default") + Vector3.up * player.height;
        audioDevice.PlayOneShot(aud_Teleport);
    }
    #endregion

    #region SerializedFields
    [Header("Player & Camera References")]
    public PlayerScript player;
    public Transform cameraTransform;
    public Camera PlayerCamera;
    public Collider playerCollider;
    public CharacterController playerCharacter;

    [Header("Scripts")]
    public KeyFunctions KF;
    public BaldiScript baldiScrpt;
    public CraftersScript sockCript;
    public PlaytimeScript playtimeScript;
    public FirstPrizeScript firstPrizeScript;
    [SerializeField] private AILocationSelectorScript AILocationSelector;

    [Header("Game Mode & Settings")]
    public string mode;
    public int notebooks, maxNotebooks, UnlockAmount;
    public bool debugMode;
    [SerializeField] private string gameoverScene;

    [Header("Serialized References")]
    [SerializeField] private TMP_Text notebookCount;
    [SerializeField] private List<EntranceScript> entrances = new List<EntranceScript>();
    [SerializeField] private GameObject highScoreText, baldi;
    public List<GameObject> ObjectsToEnable = new List<GameObject>();
    [SerializeField] private List<GameObject> ObjectsToDisable, ItemsToRespawn = new List<GameObject>();
    [SerializeField] private List<VendingMachineScript> MachinesToRestock = new List<VendingMachineScript>();
    public Animator Icon;
    public Material SpriteRenderer;
    public Sprite Present;

    [Header("Audio References")]
    [SerializeField] private AudioClip[] LoseSounds;
    public AudioSource audioDevice, schoolMusic, escapeMusic, gamaOvarDevice;
    public AudioClip aud_Hang, aud_Rattling, aud_Unlocked, aud_ItemCollect, SchoolhouseEscape, aud_Collected;
    [SerializeField] private AudioClip aud_ChaosStart, aud_ChaosStartLoop, aud_ChaosBuildUp, aud_ChaosFinal, aud_Switch, aud_Teleport;
    #endregion

    #region PrivateFields
    private AudioQueueScript audioQueue;
    private int audOverVal;
    private float gameOverDelay;
    [HideInInspector] public int lastRespawnCount, failedNotebooks, exitsReached, cullingMask;
    [HideInInspector] public bool spoopMode, finaleMode;
    [HideInInspector] public Coroutine exitEasingCoroutine;
    [HideInInspector] public LearningGameManager Math;
    [HideInInspector] public EndingManager progress;
    #endregion
}