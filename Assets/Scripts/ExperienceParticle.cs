using UnityEngine;

public class ExperienceParticle : MonoBehaviour
{
    public float exp = 0;
    public float forceMultiply = 0;
    public float damping = 10;
    public float lifetime = 0;

    private void Start()
    {
        Invoke("DestroyYourself", lifetime);
    }

    private void OnTriggerStay(Collider other)
    {
        PlayerBehavior script = other.GetComponent<PlayerBehavior>();
        if (script == null) return;
        Vector3 force = ((script.gameObject.transform.position + new Vector3(0,1,0)) - transform.position).normalized * forceMultiply;
        GetComponent<Rigidbody>().AddForce(force);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerBehavior script = other.GetComponent<PlayerBehavior>();
        if (script == null) return;
        GetComponent<Rigidbody>().drag = 0;
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerBehavior script = other.GetComponent<PlayerBehavior>();
        if (script == null) return;
        GetComponent<Rigidbody>().drag = damping;
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayerBehavior script = collision.gameObject.GetComponent<PlayerBehavior>();
        if (script == null) return;
        script.AddExperience(exp);
        Destroy(gameObject);
    }

    void DestroyYourself()
    {
        Destroy(gameObject);
    }
}
