using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class AdditionalGameCustomizer : MonoBehaviour
{
    #region UnityCallbacks
    private void Awake() => Instance = this;

    private void Start()
    {
        InitializeCustomAdditions();
        SkyBoxHandling();
        ScrambleItems();
    }

    private void Update()
    {
        CameraShaking();
        FlashlightCode();
        StaminaStyleHandling();
        KeyFunctions();
        CurrencySystem();
    }
    #endregion

    #region Initialization
    private void InitializeCustomAdditions()
    {
        TMP.SetActive(OldDetentionTimer);
        Clock.SetActive(!OldDetentionTimer);
        GaugeManager.SetActive(Gauges);
    }
    #endregion

    #region VisualEffects
    private void CameraShaking()
    {
        GameObject cameraObject = GameObject.FindGameObjectWithTag("MainCamera");
        if (cameraObject != null)
        {
            Camera cameraComponent = cameraObject.GetComponent<Camera>();
            if (cameraComponent != null)
            {
                cameraComponent.fieldOfView = CameraShake ? Random.Range(58, 62) : 60;
            }
        }
    }

    private void FlashlightCode()
    {
        GameObject cameraObject = GameObject.FindGameObjectWithTag("MainCamera");
        if (cameraObject != null)
        {
            Light light = cameraObject.GetComponent<Light>();
            if (light != null)
            {
                light.enabled = isFlashlightOn;
            }
        }
    }
    #endregion

    #region StaminaManagement
    private void StaminaStyleHandling()
    {
        var staminaMap = new Dictionary<StaminaDisplay, GameObject>
        {
            { StaminaDisplay.Old, OldStamina},
            { StaminaDisplay.PreOld, PreOldStamina},
            { StaminaDisplay.Normal, NewStamina},
            { StaminaDisplay.Vertical, VerticalStamina },
            { StaminaDisplay.Circle, CircleStamina }
        };

        OldStamina.SetActive(false);
        PreOldStamina.SetActive(false);
        NewStamina.SetActive(false);
        VerticalStamina.SetActive(false);
        CircleStamina.SetActive(false);

        if (staminaMap.ContainsKey(StaminaStyle))
        {
            staminaMap[StaminaStyle].SetActive(true);
        }

        if (StaminaStyle == StaminaDisplay.Old)
        {
            bool YouNeedRest = GameControllerScript.Instance.player.stamina < 0f;
            if (warning.activeSelf != YouNeedRest)
            {
                warning.SetActive(YouNeedRest);
            }
        }
    }
    #endregion

    #region InputHandling
    private void KeyFunctions()
    {
        if (Time.timeScale == 0f) return;

        if (Input.GetKeyDown(KeyCode.R) && ItemDropping)
        {
            int selectedSlot = ItemManager.Instance.ItemSelection;
            if (ItemManager.Instance.Inventory[selectedSlot].ItemInstance != null)
            {
                ItemManager.Instance.DropItem(selectedSlot);
            }
        }

        if (FlashLight && Input.GetKeyDown(KeyCode.F))
        {
            isFlashlightOn = !isFlashlightOn;
        }
    }
    #endregion

    #region SkyboxManagement
    private void SkyBoxHandling()
    {
        switch (SetSkybox)
        {
            case SkyboxStyle.Default:
                RenderSettings.skybox = DefaultSky;
                currentSkybox = SkyboxStyle.Default;
                break;
            case SkyboxStyle.Day:
                RenderSettings.skybox = NormalSky;
                currentSkybox = SkyboxStyle.Day;
                break;
            case SkyboxStyle.Sunset:
                RenderSettings.skybox = TwilightSky;
                currentSkybox = SkyboxStyle.Sunset;
                break;
            case SkyboxStyle.Night:
                RenderSettings.skybox = NightSky;
                currentSkybox = SkyboxStyle.Night;
                break;
        }
    }
    #endregion

    #region Currency
    private void CurrencySystem()
    {
        if (ReworkedCurrency)
        {
            Counter.SetActive(true);
            AudioSource audioDevice = GameControllerScript.Instance.audioDevice;
            currencyCounter.text = "$" + Cash.ToString("F2");

            if (Cash >= 0.25)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (Sych.ScreenRaycastMatchesTag("VendingMachine", out RaycastHit hit, 10f))
                    {
                        var vendingMachine = hit.collider.GetComponent<VendingMachineScript>();
                        if (vendingMachine != null)
                        {
                            if (!ItemManager.Instance.IsInventoryFull())
                            {
                                Cash = Cash - 0.25;
                                audioDevice.PlayOneShot(aud_Drop);
                                vendingMachine.DispenseItem();
                            }
                        }
                    }
                    else if (Sych.ScreenRaycastMatchesTag("Phone", out hit, 10f))
                    {
                        var tapePlayer = hit.collider.GetComponent<TapePlayerScript>();
                        if (tapePlayer != null)
                        {
                            Cash = Cash - 0.25;
                            audioDevice.PlayOneShot(aud_Drop);
                            tapePlayer.Play();
                        }
                    }
                }
            }
        }
        else
        {
            Counter.SetActive(false);
        }
    }
    #endregion

    #region RandomizedItems
    private void ScrambleItems()
    {
        if (RandomizeItems)
        {
            List<Vector3> list = new List<Vector3>();

            foreach (PickupScript pickupScript in FindObjectsOfType<PickupScript>())
            {
                if (pickupScript.gameObject != quarter && !pickupScript.SpawnAtRandom)
                {
                    list.Add(pickupScript.transform.position);
                }
            }
            foreach (PickupScript pickupScript2 in FindObjectsOfType<PickupScript>())
            {
                if (pickupScript2.gameObject != quarter && !pickupScript2.SpawnAtRandom)
                {
                    int index = Random.Range(0, list.Count);
                    pickupScript2.transform.position = list[index];
                    list.RemoveAt(index);
                }
            }
        }
    }
    #endregion

    #region SerializedFields
    [Header("Gameplay Addons")]
    public bool RandomizeJumps;
    public bool NoYCTP, DetentionAfterScissorUse, AnOldRule, ItemDropping, SkipCraftersAttack, ReworkedCurrency, RandomizeItems;

    [Header("Visual Addons")]
    public StaminaDisplay StaminaStyle = StaminaDisplay.Normal;
    public bool RandomizeBookColor, Indicator, FinalModeTV, Gauges, OldDetentionTimer, ExitCounter, FlashLight, CameraShake;
    public SkyboxStyle SetSkybox = SkyboxStyle.Day;

    [Header("Serialized References")]
    public Sprite[] BookColors;
    public Material NormalSky, NormalRedSky, NightSky, RedNightSky, TwilightSky, RedTwilightSky, DefaultSky;
    [SerializeField] private GameObject warning, Clock, TMP, OldStamina, PreOldStamina, NewStamina, VerticalStamina, CircleStamina, GaugeManager, Counter;
    [SerializeField] private TMP_Text currencyCounter;
    [SerializeField] private AudioClip aud_Drop;
    [SerializeField] private GameObject quarter;
    #endregion

    #region RuntimeVariables
    private bool isFlashlightOn = false;
    public static AdditionalGameCustomizer Instance;
    [HideInInspector] public SkyboxStyle currentSkybox;
    [HideInInspector] public double Cash = 0.00;
    #endregion

    #region Enums
    public enum SkyboxStyle { Default, Day, Sunset, Night }
    public enum StaminaDisplay { Old, PreOld, Normal, Vertical, Circle }
    #endregion
}