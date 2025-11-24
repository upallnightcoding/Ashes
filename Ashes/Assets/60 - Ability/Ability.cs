using UnityEngine;

namespace Ashes.Ability
{
    public abstract class Ability : MonoBehaviour
    {
        public abstract void Execute(GameObject go);
    }
}

