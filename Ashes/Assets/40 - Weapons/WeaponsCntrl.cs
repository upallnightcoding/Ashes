using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsCntrl : MonoBehaviour
{
    [SerializeField] private ProjectileSO[] projectileList;

    [SerializeField] private Transform muzzlePoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fire(int weaponIndex)
    {
        ProjectileSO projectile = projectileList[weaponIndex];

        GameObject prefab = projectile.prefab;
        float force = projectile.force;
        float destroyTiming = projectile.destroyTiming;

        GameObject go = Instantiate(prefab, muzzlePoint.position, Quaternion.identity);
        go.GetComponentInChildren<Rigidbody>().AddForce(transform.forward * force, ForceMode.Impulse);

        Destroy(go, destroyTiming);
    }
}
