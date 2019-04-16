using TwinCAT.Ads;
using UnityEngine;

namespace Assets.Scripts.TwinCAT
{
    /// <summary>
    /// Handles all the writes and reads to TwinCAT ADS.
    /// </summary>
    public class TwincatAdsController : MonoBehaviour
    {
        [SerializeField]
        private int twincatAdsPort = 851;
        private TcAdsClient twincatAdsClient;

        void Awake()
        {
            twincatAdsClient = new TcAdsClient();
            twincatAdsClient.Connect(twincatAdsPort);
        }

        /// <summary>
        /// Write the data to the variable in ADS.
        /// </summary>
        /// <param name="name">Name of the variable.</param>
        /// <param name="data">Data to be written to the variable.</param>
        /// <returns>True if successful write, false otherwise.</returns>
        public bool WriteToTwincat(string name, object data)
        {
            try
            {
                Debug.Log("Writing: " + name + " to " + data.ToString());
                twincatAdsClient.WriteAny(
                    twincatAdsClient.CreateVariableHandle(name),
                    data);

                return true;
            }
            catch (AdsErrorException)
            {
                Debug.Log("TwinCAT is not running.");
                return false;
            }
            
        }

        /// <summary>
        /// Write the data to the variable in ADS.
        /// </summary>
        /// <param name="twinCATVariable">TwinCAT variable to write.</param>
        /// <returns></returns>
        public bool WriteToTwincat(TwincatVariable twinCATVariable)
        {
            return WriteToTwincat(twinCATVariable.Name, twinCATVariable.Data);
        }

        /// <summary>
        /// Read a bool type variable in the ADS.
        /// </summary>
        /// <param name="name">Name of the variable.</param>
        /// <returns>Variable's current bool value.</returns>
        public bool ReadFromTwincat(string name)
        {
            var data = twincatAdsClient.ReadAny(
                twincatAdsClient.CreateVariableHandle(name),
                typeof(bool));

            return (bool)data;
        }

    }
}
