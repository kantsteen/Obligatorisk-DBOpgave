using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obligatorisk_DBOpgave
{
    public class Facility
    {
        public int FacilityId { get; set; }
        public string Name { get; set; }
        public int HotelNo { get; set; }

        public override string ToString()
        {
            return $"ID: {FacilityId}, Name; {Name}";
        }
    }
}
