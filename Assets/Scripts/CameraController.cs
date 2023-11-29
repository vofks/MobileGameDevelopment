using System.Linq;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float cameraSpeed;

    [SerializeField]
    private float aheadDistance;

    private Transform playerTransform;
    private float lookahead;

    private void Update()
    {
        if (!playerTransform) return;

        transform.position = new Vector3(playerTransform.position.x + lookahead, playerTransform.position.y, transform.position.z);
        lookahead = Mathf.Lerp(lookahead, (aheadDistance * playerTransform.localScale.x), Time.deltaTime * cameraSpeed);
    }

    public void SetPlayer(PlayerController player)
    {
        print("SetPlayer()");
        playerTransform = player.GetComponent<Transform>();
    }
}
