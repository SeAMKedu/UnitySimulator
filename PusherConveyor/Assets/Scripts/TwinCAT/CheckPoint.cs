using UnityEngine;

namespace Assets.Scripts.TwinCAT
{
    public class CheckPoint : MonoBehaviour
    {
        public bool objectCanMove = false;
        private Product product = null;

        private bool keepChecking = false;

        void Update()
        {
            if (keepChecking)
            {
                if (!product.Moving && !objectCanMove)
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
                if (product.Moving && objectCanMove)
                    product.ReachedCheckPoint();

                else if (product.Moving == false && !objectCanMove)
                    product.ReachedCheckPoint();

                else if (product.Moving == true && !objectCanMove)
                {
                    keepChecking = true;
                }
            }
        }

    }
}
