using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsCntrl : MonoBehaviour
{
    [SerializeField] private ProjectileSO[] projectileList;

    [SerializeField] private Transform muzzlePoint;

    private bool[] isFiring;
    private bool[] endAutomatic;
    private int[] maxRounds;
    private int[] rounds;

    // Start is called before the first frame update
    void Start()
    {
        isFiring = new bool[projectileList.Length];
        endAutomatic = new bool[projectileList.Length];
        maxRounds = new int[projectileList.Length];
        rounds = new int[projectileList.Length];

        for (int i = 0; i < projectileList.Length; i++)
        {
            isFiring[i] = false;
            endAutomatic[i] = false;
            maxRounds[i] = 5;
            rounds[i] = 5;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fire(int weaponIndex)
    {
        if (!isFiring[weaponIndex])
        {
            StartCoroutine(StartFiring(weaponIndex));
        }
    }

    private IEnumerator StartFiring(int weaponIndex)
    {
        isFiring[weaponIndex] = true;
        endAutomatic[weaponIndex] = false;

        ProjectileSO projectile = projectileList[weaponIndex];

        GameObject prefab = projectile.prefab;
        float force = projectile.force;
        float destroyTiming = projectile.destroyTiming;
        bool manual = projectile.manual;
        float firingSequenceSec = 1.0f / projectile.roundsPerSec;
        float reloadSec = projectile.reloadSec;

        bool firstShot = true;

        while (((manual && firstShot) || (!manual && !endAutomatic[weaponIndex])) && (rounds[weaponIndex] > 0))
        {
            GameObject go = Instantiate(prefab, muzzlePoint.position, Quaternion.identity);
            go.GetComponentInChildren<Rigidbody>().AddForce(transform.forward * force, ForceMode.Impulse);
            Destroy(go, destroyTiming);
            yield return new WaitForSeconds(firingSequenceSec);
            firstShot = false;
            rounds[weaponIndex]--;
        }

        if (rounds[weaponIndex] == 0)
        {
            yield return new WaitForSeconds(reloadSec);
            rounds[weaponIndex] = projectile.maxRounds;
        }

        isFiring[weaponIndex] = false;
    }

    public void StopFiring(int weaponIndex)
    {
        endAutomatic[weaponIndex] = true;
    }
}
