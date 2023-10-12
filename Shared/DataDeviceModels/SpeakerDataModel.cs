using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataDeviceModels
{
    public class SpeakerDataModel
    {
        public string ContainerName { get; set; } = "speakerData";


        public bool IsActive { get; set; }

        public string? Volume { get; set; }

        public DateTime CurrentTime { get; set; }

        public string Location { get; set; }
    }
}
