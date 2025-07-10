#if UNITY_EDITOR
using UnityEngine;

public class BaseCommand : MonoBehaviour
{
    public string CommandName;
    public virtual CommandReturnType Execute(object[] args)
    {
        return new CommandReturnType(ReturnType.None, "hey vsauce, micheal here");
    }
}
#endif