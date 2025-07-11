using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W) | Input.GetKey(KeyCode.A) | Input.GetKey(KeyCode.S) | Input.GetKey(KeyCode.D))
        {
            if (Input.GetKey(KeyCode.LeftShift) && gc.player.stamina >= 0)
            {
                this.footstepsRun.SetActive(true);
                this.footstepsWalk.SetActive(false);
            }
            else
            {
                this.footstepsWalk.SetActive(true);
                this.footstepsRun.SetActive(false);
            }
        }
        else
        {
            this.footstepsWalk.SetActive(false);
            this.footstepsRun.SetActive(false);
        }
    }

    public GameControllerScript gc;

    public GameObject footstepsWalk;
    public GameObject footstepsRun;
}