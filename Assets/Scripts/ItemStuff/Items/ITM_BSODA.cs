using UnityEngine;

public class ITM_BSODA : BaseItem
{
    public override bool OnUse()
    {
        GameControllerScript Contoller = GameControllerScript.Instance;
        Instantiate(bsodaSpray, Contoller.player.transform.position, Contoller.cameraTransform.rotation);

        Contoller.player.ResetGuilt("drink", 1f);
        GameControllerScript.Instance.audioDevice.PlayOneShot(aud_Soda);
        return true;
    }
    
    [SerializeField] private GameObject bsodaSpray;
    [SerializeField] private AudioClip aud_Soda;
}
