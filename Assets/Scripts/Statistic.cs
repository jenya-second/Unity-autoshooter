using UnityEngine;

public class Statistic : MonoBehaviour
{
    public static Statistic instance;

    public float kills = 0;
    public float experiense = 0;
    public float healing = 0;
    public float damage = 0;
    float time = 0;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            transform.parent = null;
            time = Time.realtimeSinceStartup;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public string GetTime()
    {
        int m = (int)(Time.realtimeSinceStartup - time) / 60;
        string mi = m.ToString();
        if (mi.Length == 0) mi = "00";
        if (mi.Length == 1) mi = 0 + mi;
        int s = (int)(Time.realtimeSinceStartup - time) - m * 60;
        string se = s.ToString();
        if (se.Length == 0) se = "00";
        if (se.Length == 1) se = 0 + se;
        string t = mi + ":" + se;
        return t;
    }
}
