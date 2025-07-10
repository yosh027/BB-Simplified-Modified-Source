#if UNITY_EDITOR
using TMPro;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

#region Enums
public enum ReturnType
{
    Success,
    Warning,
    Error,
    None
}
#endregion

#region ReturnType Wrapper
public class CommandReturnType
{
    public CommandReturnType(ReturnType returnType, string msg)
    {
        ReturnType = returnType;
        Message = msg;
    }

    public ReturnType ReturnType;
    public string Message;
}
#endregion

[RequireComponent(typeof(Canvas))]
public class BaseDebugger : MonoBehaviour
{
    #region Methods
    private void Start() => GetCommands();

    private void Update()
    {
        HandleToggle();
        HandleCommandExecution();
    }
    #endregion

    #region Debug Command Logic
    private void HandleToggle()
    {
        if (!Input.GetKeyDown(DebuggerToggleKey) || !GameControllerScript.Instance.debugMode) return;

        Active = !Active;
        DebugHud.SetActive(Active);

        if (Active)
        {
            OriginalTScale = Time.timeScale;
            Time.timeScale = 0f;
            CommandInputBox.Select();
            GameControllerScript.Instance.KF.UnlockMouse();
        }
        else
        {
            Time.timeScale = OriginalTScale;
            GameControllerScript.Instance.KF.LockMouse();
        }
    }

    private void HandleCommandExecution()
    {
        if (!Input.GetKeyDown(KeyCode.Return) || string.IsNullOrEmpty(CommandInputBox.text)) return;

        Debug.Log("Finding a valid command");

        string[] arguments = CommandInputBox.text.Split(' ');
        string cmdName = arguments[0];
        object[] commandArgs = arguments.Skip(1).ToArray();

        foreach (BaseCommand command in Commands)
        {
            if (cmdName != command.CommandName) continue;

            CommandReturnType returnValue = command.Execute(commandArgs);

            OutputText.color = GetColorFromType(returnValue.ReturnType);
            OutputText.text = returnValue.Message;

            if (!GameControllerScript.Instance.debugMode)
            {
                Time.timeScale = OriginalTScale;
                GameControllerScript.Instance.KF.LockMouse();
            }

            return;
        }

        OutputText.color = GetColorFromType(ReturnType.Error);
        OutputText.text = "[ERROR] Invalid command";
    }

    private Color GetColorFromType(ReturnType type)
    {
        switch (type)
        {
            case ReturnType.Success: return Color.green;
            case ReturnType.Warning: return Color.yellow;
            case ReturnType.Error: return Color.red;
            default: return Color.white;
        }
    }

    private void GetCommands()
    {
        Commands = GetComponentsInChildren<BaseCommand>().ToList();
        foreach (BaseCommand command in Commands)
        {
            Debug.Log(command.name);
        }
    }
    #endregion

    #region Serialized Fields
    [Header("UI Elements")]
    [SerializeField] private TMP_InputField CommandInputBox;
    [SerializeField] private TextMeshProUGUI OutputText;
    [SerializeField] private GameObject DebugHud;

    [Header("Debugger Settings")]
    [SerializeField] private KeyCode DebuggerToggleKey = KeyCode.Tab;
    #endregion

    #region Private Fields
    private List<BaseCommand> Commands = new List<BaseCommand>();
    private bool Active;
    private float OriginalTScale;
    #endregion
}
#endif