using UnityEngine;

public class MeleWeapon : MonoBehaviour
{
    public delegate void Action();
    public event Action onCollide;

    void Update()
    {
        float rot = Time.deltaTime * Progress.Func(transform.parent.GetComponent<WeaponBehavior>().attackSpeed * (1 + transform.parent.transform.parent.GetComponent<PlayerBehavior>().attackSpeed/10f)) * (90 / Mathf.PI);
        transform.parent.transform.Rotate(0, rot, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform weapon = transform.parent;
        Transform player = weapon.transform.parent;
        float damage = weapon.GetComponent<WeaponBehavior>().damage + player.GetComponent<PlayerBehavior>().damageIncrease;
        EnemyBehavior script = other.GetComponent<EnemyBehavior>();
        if (script == null) return;
        if (onCollide != null)
        {
            onCollide();
        }
        GetComponent<AudioSource>().Play();
        script.ApplyDamage(Progress.Func(damage));
    }
}
