using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCntrl : MonoBehaviour
{
    private float maxDamage = 30.0f;

    public void TakeDamage(float damage)
    {
        maxDamage -= damage;

        if (maxDamage <= 0.0f)
        {
            Destroy(gameObject);
        }
    }
}
