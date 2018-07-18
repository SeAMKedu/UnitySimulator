﻿using UnityEngine;
using Assets.Scripts.Models;

namespace Assets.Scripts.TwinCAT
{
    public class Box : MonoBehaviour
    {

        public string boxShakerName = "BoxShaker";
        public string programOrganizationUnit = "MAIN";

        private TwinCATVariable boxShaker;
        private TwinCAT_ADS twincatADS;
        Animator animator;

        [HideInInspector]
        public bool boxIsShaking = false;

        void Awake()
        {
            boxShaker = new TwinCATVariable(boxShakerName, programOrganizationUnit);
        }

        void Start()
        {
            twincatADS = GetComponentInParent<TwinCAT_ADS>();
            animator = GetComponent<Animator>();
        }


        void Update()
        {
            if (!boxIsShaking)
                ReadAndCheck();
        }

        private void ReadAndCheck()
        {
            if (twincatADS.ReadFromTwincat(boxShaker.name))
            {
                boxIsShaking = true;
                animator.SetTrigger("Shake");
            }
        }

        public void AnimationFinished()
        {
            animator.SetTrigger("Still");
            boxIsShaking = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.name + " triggered me!");
        }

        private void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject.tag == "Product")
            {
                if (boxIsShaking)
                {
                    if(collision.rigidbody.velocity == new Vector3(0, 0, 0))
                    {
                        Debug.Log("Shake thyself");
                        Product product = collision.gameObject.GetComponent<Product>();
                        product.ShakeMe();
                    }
                    
                }
            }
        }


    }
}
