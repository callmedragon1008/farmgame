using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float offsetZ = 5f;    // Offset theo trục Z (khoảng cách phía sau người chơi)
    public float offsetY = 2f;    // Offset theo trục Y (độ cao so với người chơi)
    public float smoothing = 2f;  // Độ mượt mà khi di chuyển
    Transform playerPos;

    void Start()
    {
        playerPos = FindObjectOfType<PlayerController>().transform;
    }

    void Update()
    {
        if (playerPos != null)
        {
            FollowPlayer();
        }
    }

    void FollowPlayer()
    {
        // Tính vị trí mục tiêu: lấy Y từ người chơi và thêm offsetY
        Vector3 targetPosition = new Vector3(
            playerPos.position.x,
            playerPos.position.y + offsetY,
            playerPos.position.z - offsetZ
        );

        // Di chuyển camera mượt mà tới vị trí mục tiêu
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime);
    }
}