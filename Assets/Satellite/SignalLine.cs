using UnityEngine;

public class SignalLine : MonoBehaviour
{
    public Transform antenna;

    public GroundStationManager manager;

    private LineRenderer lr;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (manager.currentTarget == null)
            return;

        lr.SetPosition(
            0,
            antenna.position
        );

        lr.SetPosition(
            1,
            manager.currentTarget.position
        );
    }
}