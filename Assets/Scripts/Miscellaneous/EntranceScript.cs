using UnityEngine;
using System.Collections.Generic;

public class EntranceScript : MonoBehaviour
{
    public void Disable() => gameObjects.ForEach(obj => obj.SetActive(false));
    
    public void Enable()
    {
        gameObjects.ForEach(obj => obj.SetActive(true));
        if (gc.finaleMode)
        {
            wall.material = map;
        }
    }

    [Header("References")]
    [SerializeField] private GameControllerScript gc;

    [Header("Entrance Materials and Wall")]
    [SerializeField] private Material map;
    [SerializeField] private MeshRenderer wall;

    [Header("Game Objects List")]
    [SerializeField] private List<GameObject> gameObjects = new List<GameObject>();
}