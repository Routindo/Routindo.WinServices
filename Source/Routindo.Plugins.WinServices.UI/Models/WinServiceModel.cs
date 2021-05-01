namespace Routindo.Plugins.WinServices.UI.Models
{
    public class WinServiceModel
    {
        private readonly string _displayName;

        public WinServiceModel(string serviceName, string displayName)
        {
            this.ServiceName = serviceName;
            this._displayName = displayName;
        }
        public string ServiceName { get; }

        public string DisplayName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_displayName))
                    return ServiceName;
                return _displayName;
            }
        } 

        public override string ToString()
        {
            return DisplayName;
        }
    }
}
