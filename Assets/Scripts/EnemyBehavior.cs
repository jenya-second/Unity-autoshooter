using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public GameObject player = null;
    public float speed = 10;
    public float health = 100;
    public float damage = 10;
    public GameObject particle;
    public float experience = 10;
    public bool dead = false;
    public bool anger = false;
    public Animator anim;

    private void Start()
    {
        if (LevelProgress.instance == null) return;
        health = health * (1 + LevelProgress.instance.stage / 10f);
        damage = damage * (1 + LevelProgress.instance.stage / 10f);
    }

    void Update()
    {
        if (player == null || anger || dead) return;
        Vector3 rot = (player.transform.position - gameObject.transform.position);
        Vector3 pos = rot.normalized * speed * Time.deltaTime;
        gameObject.transform.position += pos;
        if (pos == Vector3.zero)
        {
            anim.SetBool("Idle", true);
            return;
        }
        gameObject.transform.rotation = Quaternion.LookRotation(pos);
        anim.SetBool("Idle", false);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerBehavior script = other.gameObject.GetComponent<PlayerBehavior>();
        if (script == null) return;
        script.ApplyDamage(damage);
        GetComponent<AudioSource>().Play();
    }

    public void ApplyDamage(float value)
    {
        health -= value;
        Statistic.instance.damage += value;
        transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().material.SetFloat("_gr", 0);
        Invoke("ChangeColor", 0.1f);
        if (health <= 0)
        {
            if (dead) return;
            dead = true;
            Death();
        }
    }

    void ChangeColor()
    {
        transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().material.SetFloat("_gr", 1);
    }

    void Death()
    {
        anim.SetBool("Dead", true);
        Invoke("Dead",0.9f);
    }

    void Dead()
    {
        Statistic.instance.kills++;
        GameObject exp = Instantiate(particle, transform.position, new Quaternion(0, 0, 0, 0));
        exp.GetComponent<ExperienceParticle>().exp = experience;
        Destroy(gameObject);
    }
}
