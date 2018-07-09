namespace Assets.Scripts.Models
{
    public class TwinCATVariable
    {
        public string name { get; set; }
        public object state { get; set; }

        public TwinCATVariable(string name, string programOrganizationUnit)
        {
            this.name = programOrganizationUnit + "." + name;
            this.state = false;
        }
    }
}
