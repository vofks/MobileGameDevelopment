using System.Linq;
using UnityEngine;

public class DynamicBackground : MonoBehaviour
{
    private Transform playerTransform;

    void Update()
    {
        transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z);
    }

    public void SetPlayerTransform(Transform player)
    {
        playerTransform = player;
    }
}
