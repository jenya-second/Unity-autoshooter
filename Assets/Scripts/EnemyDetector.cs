using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    public GameObject enemy;
    public float radius = 10;

    private void Update()
    {
        if (enemy == null) return;
        if (enemy.GetComponent<EnemyBehavior>().dead)
        {
            enemy = null;
        }
    }

    private void Start()
    {
        GetComponent<SphereCollider>().radius = radius;
    }

    private void OnTriggerStay(Collider other)
    {
        EnemyBehavior script = other.GetComponent<EnemyBehavior>();
        if (script == null) return;
        if (script.dead) return;
        if (enemy == null)
        {
            enemy = script.gameObject;
            return;
        }
        float range1 = (script.gameObject.transform.position - gameObject.transform.position).magnitude;
        float range2 = (enemy.transform.position - gameObject.transform.position).magnitude;
        if (range1 < range2)
        {
            enemy = script.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        EnemyBehavior script = other.GetComponent<EnemyBehavior>();
        if (script == null) return;
        if (enemy == script.gameObject)
        {
            enemy = null;
            return;
        }
    }
}
