using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataDeviceModels
{
    public  class FanDataModel
    {
        public string ContainerName { get; set; } = "fanData";

        public bool IsActive { get; set; }
    
        public DateTime CurrentTime { get; set; }

        public string Location { get; set; }
    
    }
}
