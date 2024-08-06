using UnityEngine;

public class RuneRotationHolder : MonoBehaviour
{
    public Transform origin;

    Transform myTransform;

    private void Awake()
    {
        myTransform = transform;
    }

    void LateUpdate()
    {
        if(origin != null)
        {
            myTransform.rotation = origin.rotation;
        }
    }
}
