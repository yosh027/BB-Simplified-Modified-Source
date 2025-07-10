using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIController : MonoBehaviour
{
    #region Callbacks
    private void Start()
    {
        if (unlockOnStart & !joystickEnabled)
        {
            cc.UnlockCursor();
        }
    }

    private void OnEnable()
    {
        dummyButtonPC.Select();
        UpdateControllerType();
    }

    private void Update() => UpdateControllerType();
    #endregion

    #region Methods
    public void SwitchMenu()
    {
        SelectDummy();
        UpdateControllerType();
    }

    public void EnableControl() => uiControlEnabled = true;

    public void DisableControl() => uiControlEnabled = false;
    #endregion

    #region UI Handling
    private void UpdateControllerType()
    {
        if (!joystickEnabled & usingJoystick)
        {
            joystickEnabled = true;
            if (controlMouse)
            {
                cc.LockCursor();
            }
        }
        else if (joystickEnabled & !usingJoystick)
        {
            joystickEnabled = false;
            if (controlMouse)
            {
                cc.UnlockCursor();
            }
        }
        UIUpdate();
    }

    private void UIUpdate()
    {
        if (uiControlEnabled)
        {
            if (joystickEnabled)
            {
                if (EventSystem.current.currentSelectedGameObject.tag != buttonTag & firstButton != null)
                {
                    firstButton.Select();
                    firstButton.OnSelect(null);
                }
            }
            else
            {
                SelectDummy();
            }
        }
    }

    private void SelectDummy() => dummyButtonPC.Select();
    #endregion

    #region Properties
    [SerializeField]
    private bool usingJoystick
    {
        get
        {
            return false;
        }
    }
    #endregion

    #region Serialized Fields
    [Header("Cursor Control")]
    [SerializeField] private CursorControllerScript cc;

    [Header("Control Settings")]
    [SerializeField] private bool controlMouse;
    [SerializeField] private bool unlockOnStart, uiControlEnabled;

    [Header("UI Elements")]
    public Selectable firstButton;
    public Selectable dummyButtonPC;

    [Header("Joystick Settings")]
    [SerializeField] private string buttonTag;
    #endregion

    #region Private Variables
    private bool joystickEnabled;
    #endregion
}