namespace VehicleMaintenanceSystem.Models
{
    public class Maintenance
    {
       public string mainType { get; set; }
       public DateTime mainDate { get; set; }
      public  int vehicleId { get; set; }
      public  string mainDescription { get; set; }
      public string mainNotes { get; set; }
    }
}
