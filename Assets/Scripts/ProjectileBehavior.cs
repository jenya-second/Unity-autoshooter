using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public float damage = 1;
    public float lifetime = 3;
    public bool enemy = false;

    private void Start()
    {
        Invoke("DestroyYourself", lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (enemy)
        {
            PlayerBehavior script = other.GetComponent<PlayerBehavior>();
            if (script == null) return;
            script.ApplyDamage(damage); 
        }
        else
        {
            EnemyBehavior script = other.GetComponent<EnemyBehavior>();
            if (script == null) return;
            script.ApplyDamage(damage);
        }
        GetComponent<AudioSource>().Play();
        GetComponent<SphereCollider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    void DestroyYourself()
    {
        Destroy(gameObject);
    }
}
