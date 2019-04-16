namespace Assets.Scripts.TwinCAT
{
    /// <summary>
    /// TwinCAT variable model.
    /// </summary>
    public class TwincatVariable
    {
        public string Name { get; set; }
        public object Data { get; set; }
        public TwincatVariableType Type { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="variableName">Name of the TwinCAT variable.</param>
        /// <param name="programOrganizationUnit">POU the variable belongs to.</param>
        /// <param name="twincatVariableType">Add "q" when output or "i" when input to name.</param>
        public TwincatVariable(
            string variableName, 
            string programOrganizationUnit, 
            TwincatVariableType twincatVariableType)
        {
            string definition;

            switch (twincatVariableType)
            {
                case TwincatVariableType.Input:
                    definition = "i";
                    break;
                case TwincatVariableType.Output:
                    definition = "q";
                    break;
                default:
                    definition = "q";
                    break;
            }

            this.Name = programOrganizationUnit + "." + definition + variableName;
            this.Data = false;
            this.Type = twincatVariableType;
        }

        /// <summary>
        /// Returns Data as bool.
        /// </summary>
        /// <returns></returns>
        public bool DataAsBool()
        {
            return bool.Parse(Data.ToString());
        }
    }

    public enum TwincatVariableType
    {
        Input = 1,
        Output = 2
    }
}
