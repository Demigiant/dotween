using UnityEngine;
using System.Collections;

public class FollowCamTopDown : MonoBehaviour
{
    public Transform player;
    public Transform cam;
    // Speed of the player
    public float speedXSecond = 4;
    // Camera offset when moving
    public float camOffset = 2;
    // Speed when animating camera offset
    public float camOffsetSpeed = 2;
    // Speed when returning camera to original position (when player is not moving).
    // You will want to keep this lower than the camOffsetSpeed to allow for better game feel ;)
    public float camResetSpeed = 1;

    Vector3 _moveDirection;

    void Update()
    {
        _moveDirection = Vector3.zero;
        if (Input.GetKey(KeyCode.UpArrow)) _moveDirection.z = 1;
        else if (Input.GetKey(KeyCode.DownArrow)) _moveDirection.z = -1;
        if (Input.GetKey(KeyCode.RightArrow)) _moveDirection.x = 1;
        else if (Input.GetKey(KeyCode.LeftArrow)) _moveDirection.x = -1;

        if (_moveDirection.sqrMagnitude > 0) {
            // The player is moving
            player.position += _moveDirection * speedXSecond * Time.deltaTime;
            // Determine camera's ideal position and lerp there
            Vector3 optimalCamPos = _moveDirection * camOffset;
            optimalCamPos.y = cam.localPosition.y;
            cam.localPosition = Vector3.Slerp(cam.localPosition, optimalCamPos, Time.deltaTime * camOffsetSpeed);
        } else {
            // Not moving. Just control camera
            Vector3 optimalCamPos = Vector3.zero;
            optimalCamPos.y = cam.localPosition.y;
            cam.localPosition = Vector3.Slerp(cam.localPosition, optimalCamPos, Time.deltaTime * camResetSpeed);
        }
    }
}