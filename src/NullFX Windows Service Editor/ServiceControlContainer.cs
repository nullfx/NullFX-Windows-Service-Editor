using System.ServiceProcess;

namespace NullFX.ServiceEditor {
    public class ServiceControlContainer {
        public ServiceController Controller { get; set; }

        public override string ToString ( ) {
            return Controller?.DisplayName ?? "Select a service";
        }

        public ServiceControlContainer ( ServiceController ctrl ) {
            Controller = ctrl;
        }
    }
}
