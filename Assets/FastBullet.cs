using UnityEngine;

public class FastBullet : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private bool moveKey;

    public void Launch()
    {
        Destroy(gameObject, 5);
        moveKey = true;
    }

    void Update()
    {
        if (moveKey)
            transform.position += transform.forward * Time.deltaTime * speed;
    }
}
