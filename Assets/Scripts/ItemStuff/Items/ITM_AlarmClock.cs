using UnityEngine;

public class ITM_AlarmClock : BaseItem
{
    [SerializeField] private GameObject alarmClock;

    public override bool OnUse()
    {
	    GameObject gameObject = Instantiate(alarmClock, GameControllerScript.Instance.player.transform.position, GameControllerScript.Instance.cameraTransform.rotation);
	    gameObject.GetComponent<AlarmClockScript>().baldi = GameControllerScript.Instance.baldiScrpt;

        return true;
    }
}
