using UnityEngine;
using Assets.Scripts.Models;

namespace Assets.Scripts.TwinCAT
{
    public class Pusher : MonoBehaviour
    {

        public string pusherName = "Pusher";
        public string programOrganizationUnit = "MAIN";

        private TwinCATVariable pusherStart;
        private TwinCATVariable pusherReset;
        private TwinCAT_ADS twincatADS;
        Animator animator;

        private bool pusherMoving;

        void Awake()
        {
            pusherStart = new TwinCATVariable(pusherName + "Push", programOrganizationUnit);
            pusherReset = new TwinCATVariable(pusherName + "Reset", programOrganizationUnit);
            pusherMoving = false;
        }

        void Start()
        {
            twincatADS = GetComponentInParent<TwinCAT_ADS>();
            animator = GetComponent<Animator>();
        }


        void Update()
        {
            if (!pusherMoving)
                ReadAndCheck();
        }

        private void ReadAndCheck()
        {
            if (twincatADS.ReadFromTwincat(pusherStart.name))
            {
                pusherMoving = true;
                animator.SetTrigger("Pushing");
            }

            if (twincatADS.ReadFromTwincat(pusherReset.name))
            {
                pusherMoving = true;
                animator.SetTrigger("Returning");
            }
        }

        public void AnimationFinished()
        {
            pusherMoving = false;
        }

    }
}
