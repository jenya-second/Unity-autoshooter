using UnityEngine;

public class EnemyShot : MonoBehaviour
{
    public GameObject projectileType;
    public float speed;
    public float baseSpeed;
    public float damage = 10;
    public float time = 2;
    public Animator anim;

    void Start()
    {
        Invoke("PreShot", time-0.6f);
        Invoke("Shot", time);
        baseSpeed = GetComponent<EnemyBehavior>().speed;
    }

    void PreShot()
    {
        anim.SetBool("Attack2", true);
        GetComponent<EnemyBehavior>().speed = 0.1f;
    }

    void Shot()
    {
        anim.SetBool("Attack2", false);
        if (GetComponent<EnemyBehavior>().dead) return;
        GetComponent<EnemyBehavior>().speed = baseSpeed;
        GameObject projectile = Instantiate(projectileType, transform.position, transform.rotation);
        projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * speed;
        projectile.GetComponent<ProjectileBehavior>().damage = damage/2;
        Invoke("PreShot", time - 0.6f);
        Invoke("Shot", time);
    }
}
