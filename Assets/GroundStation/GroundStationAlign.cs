using UnityEngine;

public class GroundStationAlign : MonoBehaviour
{
    public Transform earth;

    void Start()
    {
        Vector3 dir =
            (transform.position - earth.position).normalized;

        transform.up = dir;
    }
}