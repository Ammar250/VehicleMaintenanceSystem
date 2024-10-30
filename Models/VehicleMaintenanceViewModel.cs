using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace VehicleMaintenanceSystem.Models
{
    public class VehicleMaintenanceViewModel
    {
        public DataSet MaintenanceDetails { get; set; }
        
        public DataSet Vehicles { get; set; }


    }
}
