using UnityEngine;

public class ITM_NoSquee : BaseItem
{
    public override bool OnUse()
    {
        Vector3 playerPosition = GameControllerScript.Instance.player.transform.position;
        Vector3 snappedPosition = SnapToGrid(playerPosition);

        Instantiate(WDNSModel, snappedPosition, GameControllerScript.Instance.cameraTransform.rotation);
        GameControllerScript.Instance.audioDevice.PlayOneShot(aud_Spray);

        return true;
    }

    private Vector3 SnapToGrid(Vector3 position)
    {
        float gridSize = 10f;

        float centerX = Mathf.Floor(position.x / gridSize) * gridSize;
        float centerZ = Mathf.Floor(position.z / gridSize) * gridSize;

        float snappedX = centerX + (gridSize / 2);
        float snappedZ = centerZ + (gridSize / 2);

        return new Vector3(snappedX, 5, snappedZ);
    }

    [SerializeField] private GameObject WDNSModel;
    [SerializeField] protected AudioClip aud_Spray;
}