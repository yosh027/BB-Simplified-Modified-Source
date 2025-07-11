using UnityEngine;

public class OldCameraScript : MonoBehaviour
{
    private void Start() => offset = transform.position - player.transform.position;

    private void Update()
    {
        if (!ps.gameOver & !this.ps.jumpRope & !FindObjectOfType<GameControllerScript>().KF.gamePaused & !FindObjectOfType<LearningGameManager>().learningActive)
        {
	        base.transform.position = this.player.transform.position + this.offset; //Teleport to the player, then move based on the offset vector
	        base.transform.rotation = this.player.transform.rotation * Quaternion.Euler(FreecamLookX, (float)this.lookBehind, 0f); //Rotate based on player direction + lookbehind
        }
        else if (ps.gameOver)
        {
	        base.transform.position = this.baldi.transform.position + this.baldi.transform.forward * 2f + new Vector3(0f, 5f, 0f); //Puts the camera in front of Baldi
	        base.transform.LookAt(new Vector3(this.baldi.position.x, this.baldi.position.y + 5f, this.baldi.position.z)); //Makes the player look at baldi with an offset so the camera doesn't look at the feet
        }
        if (ps.jumpRope)
        {
            base.transform.position = this.player.transform.position + this.offset + this.jumpHeightV3; //Apply the jump rope vector onto the normal offset
	        base.transform.rotation = this.player.transform.rotation * Quaternion.Euler(FreecamLookX, (float)this.lookBehind, 0f); //Rotate based on player direction + lookbehind
            velocity -= gravity * Time.deltaTime;
            jumpHeight += 2.7f * velocity * Time.deltaTime;
            if (jumpHeight <= 0f)
            {
                jumpHeight = 0f;
                if (Singleton<InputManager>.Instance.GetActionKey(InputAction.Jump))
                {
                    velocity = initVelocity;
                }
            }
            jumpHeightV3 = new Vector3(0f, jumpHeight, 0f);
        }
        if (!ps.gc.KF.gamePaused)
        {
            lookBehind = Singleton<InputManager>.Instance.GetActionKey(InputAction.LookBehind) ? 180 : 0;
        }
    }

    private void LateUpdate()
    {
        float num = Input.GetAxis("Mouse Y") * this.ps.mouseSensitivity;
        FreecamLookX -= num;
        FreecamLookX = Mathf.Clamp(FreecamLookX, -90f, 90f);
    }

    [Header("Player References")]
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerScript ps;

    [Header("Camera Settings")]
    [SerializeField] private float GameOverOffset;
    public Vector3 offset;
    public float FreecamLookX;

    [Header("Jump Rope Physics")]
    [SerializeField] private float initVelocity, velocity, gravity;
    public float jumpHeight;

    [Header("Baldi References")]
    [SerializeField] private Transform baldi;
    private int lookBehind;
    private Vector3 jumpHeightV3;
}