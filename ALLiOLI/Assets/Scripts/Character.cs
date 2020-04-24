using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Character : MonoBehaviour
{
    [SerializeField] private float speed = 10;
    [SerializeField] public Transform cameraTarget;
    [SerializeField] public Transform flagPosition;
    public Flag flag;

    public Player owner;

    public Vector3 movement
    {
        get { return _movement; }
        set { _movement = value.normalized; }
    }

    private Vector3 _movement;
    public bool isDead { get; private set; }
    private Rigidbody rb { get; set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.velocity = movement * speed;
    }

    public void Die()
    {
        isDead = true;
        if (flag != null) flag.Detach();
    }
}