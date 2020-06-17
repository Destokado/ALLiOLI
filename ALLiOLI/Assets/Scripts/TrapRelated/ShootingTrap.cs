using System.Collections;
using FMODUnity;
using UnityEngine;

[RequireComponent(typeof(SimpleAnimationsManager))]
public class ShootingTrap : Trap
{
    private SimpleAnimationsManager animManager;
    [SerializeField] private Rigidbody killingObject;
    [SerializeField] private float throwSpeed;
    private Vector3 throwDirection => radarTrigger.transform.forward;

    protected override void Awake()
    {
        base.Awake();
        animManager = gameObject.GetComponent<SimpleAnimationsManager>();
    }

    protected override void Reload()
    {
        base.Reload();
        ((TransformAnimation) animManager.GetAnimation(0)).originTransform.SetProperties(killingObject.transform);
        killingObject.isKinematic = true;
        animManager.Play(0);
    }

    public override void Activate()
    {
        base.Activate();

        animManager.Stop(0);
        killingObject.isKinematic = false;
        StartCoroutine(Impulse());
    }

    private IEnumerator Impulse()
    {
        yield return new WaitForFixedUpdate();
        killingObject.velocity = throwDirection.normalized * throwSpeed;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(killingObject.transform.position, throwDirection);
    }
}