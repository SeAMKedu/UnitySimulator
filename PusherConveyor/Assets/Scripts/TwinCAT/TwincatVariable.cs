namespace Assets.Scripts.TwinCAT
{
    /// <summary>
    /// TwinCAT variable model.
    /// </summary>
    public class TwincatVariable
    {
        public string Name { get; set; }
        public object Data { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="variableName">Name of the TwinCAT variable.</param>
        /// <param name="programOrganizationUnit">POU the variable belongs to.</param>
        public TwincatVariable(string variableName, string programOrganizationUnit)
        {
            this.Name = programOrganizationUnit + "." + variableName;
            this.Data = false;
        }
    }
}
