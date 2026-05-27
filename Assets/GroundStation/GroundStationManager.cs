using UnityEngine;

public class GroundStationManager : MonoBehaviour
{
    public Transform satellite;

    public Transform[] stations;

    public Transform currentTarget;

    void Update()
    {
        float minDistance = Mathf.Infinity;

        Transform nearest = null;

        foreach (Transform station in stations)
        {
            float dist =
                Vector3.Distance(
                    satellite.position,
                    station.position
                );

            if (dist < minDistance)
            {
                minDistance = dist;
                nearest = station;
            }
        }

        currentTarget = nearest;
    }
}