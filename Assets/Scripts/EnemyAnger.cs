using UnityEngine;

public class EnemyAnger : MonoBehaviour
{
    public AnimationCurve posByTimeCurve;
    Vector3 actualPos;
    Quaternion actualRot;
    float startTime = 0;
    public Animator anim;

    void Start()
    {
        Invoke("Run", 5);
    }

    void Update()
    {
        bool anger = GetComponent<EnemyBehavior>().anger;
        bool dead = GetComponent<EnemyBehavior>().dead;
        if (!anger || dead) return;
        float time = LevelProgress.instance.time - startTime;
        Vector3 pos = actualPos + actualRot * Vector3.forward * posByTimeCurve.Evaluate(time);
        gameObject.transform.position = pos;
    }

    void Run()
    {
        anim.SetBool("Attack1", true);
        startTime = LevelProgress.instance.time;
        actualPos = gameObject.transform.position;
        actualRot = gameObject.transform.rotation;
        GetComponent<EnemyBehavior>().anger = true;
        Invoke("Stop", 2);
    }

    void Stop()
    {
        anim.SetBool("Attack1", false);
        GetComponent<EnemyBehavior>().anger = false;
        Invoke("Run", 5);
    }
}
