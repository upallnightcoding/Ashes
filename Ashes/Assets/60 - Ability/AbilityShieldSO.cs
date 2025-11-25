using UnityEngine;

[CreateAssetMenu(fileName = "AbilityShieldSO", menuName = "Ashes/Ability Shield")]
public class AbilityShieldSO : ScriptableObject
{
    public string shieldName;

    public GameObject shieldPrefab;

    [Header("Shield Attributes ...")]
    public float duration;
    public int maxUsageCount;
}
