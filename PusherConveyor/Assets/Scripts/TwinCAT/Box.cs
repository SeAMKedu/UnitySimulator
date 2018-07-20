using UnityEngine;
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

    }
}
