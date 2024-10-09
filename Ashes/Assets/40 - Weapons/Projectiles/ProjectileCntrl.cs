using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCntrl : MonoBehaviour
{
    [SerializeField] private GameObject explosion;

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
        if (other.gameObject.TryGetComponent<DamageCntrl>(out DamageCntrl damageCntrl))
        {
            if(damageCntrl.TakeDamage(10.0f))
            {
                GameObject go = Instantiate(explosion, transform.position, Quaternion.identity);
                Destroy(go, 3.0f);
                Destroy(other.gameObject);
            }
        }
    }
}
