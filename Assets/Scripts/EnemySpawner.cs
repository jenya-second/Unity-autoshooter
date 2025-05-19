using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;
    public AnimationCurve enemyPerSecondByTimeCurve;
    public GameObject[] enemyType;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            transform.parent = null;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartSpawning()
    {
        int oo = Mathf.Min(enemyType.Length, (LevelProgress.instance.stage + 1)/2);
        float x = Random.Range(-20, 20);
        float y = Random.Range(-20, 20);
        int stage = LevelProgress.instance.stage;
        GameObject enemy = Instantiate(enemyType[Random.Range(0, oo)], new Vector3(x, 1, y), new Quaternion(0, 0, 0, 0));
        enemy.GetComponent<EnemyBehavior>().player = Progress.instance.player;
        float time = (LevelProgress.instance.time / LevelProgress.instance.timeOfStage) * 60;
        float freq = enemyPerSecondByTimeCurve.Evaluate(time) / 10 * (float)Mathf.Sqrt(stage);
        if (freq == 0 || !LevelProgress.instance.inProgress) return;
        Invoke("StartSpawning", 1 / freq);
    }

    public void KillAllYourFriends()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Enemy");
        CancelInvoke("StartSpawning");
        for (int i = 0; i < gos.Length; i++)
        {
            Destroy(gos[i]);
        }
    }

}
