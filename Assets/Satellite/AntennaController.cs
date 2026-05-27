using UnityEngine;

public class AntennaController : MonoBehaviour
{
    public Transform pitchPart;

    public float yawSpeed = 60f;
    public float pitchSpeed = 60f;

    float pitchAngle = 0f;

    void Update()
    {
        // 左右输入
        float yaw = Input.GetAxis("Horizontal");

        // 上下输入
        float pitch = Input.GetAxis("Vertical");

        // 左右旋转（Yaw）
        transform.Rotate(
            Vector3.up,
            yaw * yawSpeed * Time.deltaTime,
            Space.Self
        );

        // Pitch角度累计
        pitchAngle -= pitch * pitchSpeed * Time.deltaTime;

        // 限制上下角度
        pitchAngle = Mathf.Clamp(pitchAngle, -80f, 80f);

        // 应用Pitch
        pitchPart.localRotation =
            Quaternion.Euler(pitchAngle, 0f, 0f);
    }
}