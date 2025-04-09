using UnityEngine;

public class TrashMechanics : MonoBehaviour
{
    private Rigidbody _rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        _rb.linearDamping = GameManager.Instance.GetLinearDamping();
        if (transform.position.y < 0.23f)
        {
            Destroy(gameObject);
            GameManager.Instance.GarbageDropped();
        }
    }
}
