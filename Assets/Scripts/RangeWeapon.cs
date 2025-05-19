using UnityEngine;

public class RangeWeapon : MonoBehaviour
{
    public GameObject projectileType;
    public float speed = 100;
    bool canShoot = true;
    Transform weapon;
    Transform player;
    public AudioClip sound;

    private void Start()
    {
        weapon = transform.parent;
        player = weapon.transform.parent;
    }

    void Update()
    {
        if (Time.timeScale == 0) return;
        GameObject enemy = weapon.GetComponent<EnemyDetector>().enemy;
        if (enemy == null) return;
        Vector3 enemyPos = enemy.transform.position;
        Vector3 weaponPos = transform.position;
        Quaternion rot = Quaternion.LookRotation(enemyPos - new Vector3(weaponPos.x, enemyPos.y, weaponPos.z));
        weapon.transform.rotation = rot;
        if (canShoot)
        {
            float attackSpeed = weapon.GetComponent<WeaponBehavior>().attackSpeed * ( 1 + player.GetComponent<PlayerBehavior>().attackSpeed/10f);
            SpawnProjectile();
            canShoot = false;
            Invoke("SetCanShoot", 1/ Progress.Func(attackSpeed/2));
        }
    }

    void SpawnProjectile()
    {
        float damage = Progress.Func(weapon.GetComponent<WeaponBehavior>().damage + player.GetComponent<PlayerBehavior>().damageIncrease);
        GameObject projectile = Instantiate(projectileType, transform.position, transform.rotation);
        projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * speed;
        projectile.GetComponent<ProjectileBehavior>().damage = damage;
        if(sound != null)
        {
            projectile.GetComponent<AudioSource>().clip = sound;
        }
    }

    void SetCanShoot()
    {
        canShoot = true;
    }
}
