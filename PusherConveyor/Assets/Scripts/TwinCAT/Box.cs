using UnityEngine;

namespace Assets.Scripts.TwinCAT
{
    public class Box : MonoBehaviour
    {
        [SerializeField]
        private string boxShakerName = "BoxShaker";
        [SerializeField]
        private string programOrganizationUnit = "MAIN";

        private TwincatVariable boxShaker;
        private TwincatAdsController twincatADS;
        Animator animator;

        [HideInInspector]
        public bool boxIsShaking = false;

        void Awake()
        {
            boxShaker = new TwincatVariable(
                boxShakerName,
                programOrganizationUnit,
                TwincatVariableType.Output);
        }

        void Start()
        {
            twincatADS = GetComponentInParent<TwincatAdsController>();
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            if (!boxIsShaking)
                ReadAndCheck();
        }

        private void ReadAndCheck()
        {
            if (twincatADS.ReadFromTwincat(boxShaker.Name))
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
