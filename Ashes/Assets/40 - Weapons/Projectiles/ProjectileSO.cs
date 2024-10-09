using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Projectile", menuName = "Ashes/Weapons/Projectile")]
public class ProjectileSO : ScriptableObject
{
    public string title;

    public GameObject prefab;

    public float force;

    public float destroyTiming;

    public bool manual;

    public int roundsPerSec;

    public int maxRounds;

    public float reloadSec;

    public GameObject explosion;

    /*public void Fire(Vector3 muzzlePoint, Vector3 direction)
    {
        GameObject projectile = Instantiate(prefab, muzzlePoint, Quaternion.identity);
        projectile.GetComponentInChildren<Rigidbody>().AddForce(direction * force, ForceMode.Impulse);

        Destroy(projectile, destroyTiming);
    }*/
}
