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
    public Transform earth;

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
        if (earth == null)
        {
            subPointText.text = "0°0′0″N  0°0′0″E";
            return;
        }

        // 1. 将卫星的世界坐标转换到地球的局部坐标系中
        // InverseTransformPoint 已经完美考虑了地球的 30° 倾斜和实时的自转
        Vector3 localPos = earth.InverseTransformPoint(satellite.position).normalized;

        // 2. 计算纬度 (Latitude):
        // 既然地球绕 local up 旋转，localPos.y 就是相对于倾斜赤道面的高度
        float latitude = Mathf.Asin(localPos.y) * Mathf.Rad2Deg;

        // 3. 计算经度 (Longitude):
        // 设定地表 0° 经线指向地球的 Local +X 方向
        // 在标准数学极坐标中，以 Local +X 为 0°，逆时针旋转到 Local +Z
        // Atan2(z, x) 会在 localPos 位于 +X 轴时返回 0°，位于 +Z 轴时返回 90°
        float longitude = Mathf.Atan2(localPos.z, localPos.x) * Mathf.Rad2Deg;

        // 4. 转换为度分秒格式并拼接显示
        string latDMS = ToDMS(latitude, true);
        string lonDMS = ToDMS(longitude, false);

        subPointText.text = latDMS + "  " + lonDMS;
    }

    // Helper: convert decimal degrees to DMS string with hemisphere
    string ToDMS(float deg, bool isLatitude)
    {
        string hemi;
        float absDeg = Mathf.Abs(deg);
        if (isLatitude)
            hemi = deg >= 0 ? "N" : "S";
        else
            hemi = deg >= 0 ? "E" : "W";

        int d = Mathf.FloorToInt(absDeg);
        float rem = (absDeg - d) * 60f;
        int m = Mathf.FloorToInt(rem);
        float s = (rem - m) * 60f;

        return string.Format("{0}°{1}′{2:0.##}″{3}", d, m, s, hemi);
    }
}