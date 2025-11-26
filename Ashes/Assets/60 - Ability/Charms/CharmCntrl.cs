using UnityEngine;
using DG.Tweening;

public class CharmCntrl : MonoBehaviour
{
    private float rotationDuration = 5.0f;
    private Ease rotationEase = Ease.Linear;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.DORotate(new Vector3(0.0f, 360.0f, 0.0f), rotationDuration, RotateMode.FastBeyond360)
            .SetEase(rotationEase)
            .SetLoops(-1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
