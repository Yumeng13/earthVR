using UnityEngine;

public class EarthRotate : MonoBehaviour
{
    public float rotationSpeed = 10f;
    public float tiltAngle = -30f; // 倾斜 30 度

    //void Start()
    //{
    //    // 如果这个脚本挂在子物体 Earth 上，我们让它的父物体（轴心）去负责倾斜
    //    if (transform.parent != null)
    //    {
    //        transform.parent.rotation = Quaternion.Euler(tiltAngle, 0, 0);
    //    }
    //}

    void Update()
    {
        // 地球本体只需要无脑绕着自己的本地 Y 轴自转即可
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.Self);
    }
}