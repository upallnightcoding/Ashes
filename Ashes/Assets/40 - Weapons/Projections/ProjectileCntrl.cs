using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCntrl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("On Projectile Enemy ...");

        if (other.gameObject.TryGetComponent<DamageCntrl>(out DamageCntrl damageCntrl))
        {
            damageCntrl.TakeDamage(10.0f);
        }
    }
}
