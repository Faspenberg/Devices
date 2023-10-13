using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureFunctions.Models
{
    public class SpeakerDataMessage
    {
        public string id { get; set; } = Guid.NewGuid().ToString();
        public bool IsActive { get; set; }
        public string Volume { get; set; }
        public string Location { get; set; } 
        public DateTime CurrentTime { get; set; }
    }
}
