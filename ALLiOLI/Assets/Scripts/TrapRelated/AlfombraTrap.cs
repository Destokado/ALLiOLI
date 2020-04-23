using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SimpleAnimationsManager))]
public class AlfombraTrap : Trap
{
    
    private SimpleAnimationsManager animManager;

    private void Awake()
    {
        animManager = gameObject.GetComponent<SimpleAnimationsManager>();
    }

    // Start is called before the first frame update
    

    public override void Activate()
    {
       base.Activate();
       animManager.Play(0);
    }
}
