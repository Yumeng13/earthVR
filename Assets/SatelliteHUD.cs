using UnityEngine;
using TMPro;

public class SatelliteHUD : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text baseStationText;
    public TMP_Text dataRateText;
    public TMP_Text subPointText;

    [Header("Objects")]
    public Transform satellite;
    public Transform antenna;

    private GameObject[] baseStations;

    void Start()
    {
        baseStations = GameObject.FindGameObjectsWithTag("BaseStation");
    }

    void Update()
    {
        UpdateHUD();
    }

    void UpdateHUD()
    {
        // =========================
        // 1. 找最近基站
        // =========================

        GameObject nearestStation = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject station in baseStations)
        {
            float d = Vector3.Distance(
                satellite.position,
                station.transform.position
            );

            if (d < minDistance)
            {
                minDistance = d;
                nearestStation = station;
            }
        }

        // =========================
        // 2. 当前基站显示
        // =========================

        if (nearestStation != null)
        {
            baseStationText.text = nearestStation.name;
        }

        // =========================
        // 3. 计算锅盖夹角
        // =========================

        Vector3 dirToStation =
            (nearestStation.transform.position - antenna.position).normalized;

        float angle =
            Vector3.Angle(antenna.forward, dirToStation);

        // =========================
        // 4. 数据速率计算
        // =========================

        // 距离影响
        float distanceFactor =
            Mathf.Clamp01(1f - minDistance / 10f);

        // 天线方向影响
        float angleFactor =
            Mathf.Clamp01(Mathf.Cos(angle * Mathf.Deg2Rad));

        // 综合质量
        float quality =
            distanceFactor * angleFactor;

        // 速率
        float dataRate =
            Mathf.Lerp(10f, 500f, quality);

        dataRateText.text = dataRate.ToString("F1") + " Mbps";

        // =========================
        // 5. 星下点计算
        // =========================

        Vector3 p = satellite.position.normalized;

        float latitude =
            Mathf.Asin(p.y) * Mathf.Rad2Deg;

        float longitude =
            Mathf.Atan2(p.x, -p.z) * Mathf.Rad2Deg;

        subPointText.text =
            latitude.ToString("F1")
            + "°, "
            + longitude.ToString("F1")
            + "°";
    }
}