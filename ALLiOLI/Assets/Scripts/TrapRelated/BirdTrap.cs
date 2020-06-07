using UnityEngine;

[RequireComponent(typeof(SimpleAnimationsManager))]
public class BirdTrap : Trap
{
    private SimpleAnimationsManager animManager;
    [SerializeField] private Rigidbody poo;
    [SerializeField] private float shitForce = 100;

    public void Awake()
    {
        animManager = gameObject.GetComponent<SimpleAnimationsManager>();
    }

    protected override void Reload()
    {
        base.Reload();
        ((TransformAnimation) animManager.GetAnimation(0)).originTransform.SetProperties(poo.transform);
        poo.isKinematic = true;
        animManager.Play(0);
    }

    public override void Activate()
    {
        base.Activate();

        animManager.Stop(0);
        poo.isKinematic = false;
        poo.AddForce(Vector3.down*shitForce, ForceMode.Impulse);
    }
}