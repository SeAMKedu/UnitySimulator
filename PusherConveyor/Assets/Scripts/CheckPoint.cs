using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.TwinCAT;

public class CheckPoint : MonoBehaviour
{
    public bool objectCanMove = false;
    private Product product = null;

    private bool keepChecking = false;

    void Start ()
    {
		
	}
	
	void Update ()
    {
        if (keepChecking)
        {
            if (product.IamMoving == false && objectCanMove == false)
            {
                product.ReachedCheckPoint();
                keepChecking = false;
            }
                
        }
	}

    void OnTriggerEnter(Collider collidedObject)
    {
        if (collidedObject.gameObject.CompareTag("Product"))
        {
            product = collidedObject.gameObject.GetComponentInChildren<Product>();
            if (product.IamMoving && objectCanMove)
                product.ReachedCheckPoint();

            else if (product.IamMoving == false && objectCanMove == false)
                product.ReachedCheckPoint();

            else if (product.IamMoving == true && objectCanMove == false)
            {
                keepChecking = true;
            }

        }
    }

}
