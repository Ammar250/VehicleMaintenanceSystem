using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;
using VehicleMaintenanceSystem.Models;
using System.Data.SqlClient;
using PLIC_Web_Poratal.Models;
using VehicleMaintenanceSystem.Models;

namespace VehicleMaintenanceSystem.Controllers
{
    public class HomeController : Controller
    {
        db _db = new db();
        private readonly IWebHostEnvironment _iwebHostEnvironment;
        private string _connectionString = "DefaultConnection";
        private readonly IConfiguration _configuration;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                
                    using (SqlConnection conn = new SqlConnection(_db.GetConfiguration().GetConnectionString("DefaultConnection")))
                    {
                        SqlCommand cmd = new SqlCommand("GetMaintenanceDetail", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        conn.Close();
                        conn.Open();                      
                        DataSet ds = new DataSet();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(ds);
                        }

                        conn.Close();

                    VehicleMaintenanceViewModel model = new VehicleMaintenanceViewModel
                    {
                            MaintenanceDetails = ds,
                         

                        };

                        ViewBag.Details = ds;
                        return View("Index", model);


                    }              

            }
            catch (Exception ex)
            {
               
                return Json(new { success = false, message = "Error showing details.", error = ex.Message });
            }
        }

        public IActionResult AddMaintenance()
        {
            try
            {              
                    DataSet dataSet1 = new DataSet();
                    SqlConnection conn1 = new SqlConnection(_db.GetConfiguration().GetConnectionString("DefaultConnection"));
                    {
                        conn1.OpenAsync();
                        SqlCommand command = new SqlCommand("GetVehicleDetail", conn1);
                        command.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter dataAdapter1 = new SqlDataAdapter(command);
                        dataAdapter1.Fill(dataSet1);
                        conn1.Close();
                        VehicleMaintenanceViewModel model = new VehicleMaintenanceViewModel
                        {
                            Vehicles = dataSet1,
                           
                        };

                    return View("AddMaintenace",model);
                }
               
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }

        [HttpPost]
        public async Task<ActionResult> CreateMaintenace(Maintenance CreateMaintenanceModel)
        {
            SqlTransaction _Transaction = null;
            try
            {             
                    using (SqlConnection conn = new SqlConnection(_db.GetConfiguration().GetConnectionString("DefaultConnection")))
                    {
                        if (conn.State != ConnectionState.Open)
                            conn.Open();
                        SqlCommand cmd = new SqlCommand("InsertmaintenanceRecord", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlTransaction transaction = conn.BeginTransaction();
                        cmd.Transaction = transaction;
                        cmd.Parameters.AddWithValue("@mainType", CreateMaintenanceModel.mainType);
                        cmd.Parameters.AddWithValue("@mainDate", CreateMaintenanceModel.mainDate);
                        cmd.Parameters.AddWithValue("@mainDescription", CreateMaintenanceModel.mainDescription);
                        cmd.Parameters.AddWithValue("@mainNotes", CreateMaintenanceModel.mainNotes);
                        cmd.Parameters.AddWithValue("@vehicleId", CreateMaintenanceModel.@vehicleId);                      
                        await cmd.ExecuteNonQueryAsync();
                        transaction.Commit();
                        return Json(new { success = true, message = "Maintenace Record Added Successfully." });
                    }
               
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = "Error adding Maintenance record.", error = ex.Message });
            }

        }


        [HttpPost]
        public async Task<ActionResult> DeleteMaintenace(int maintenanceId)
        {
            SqlTransaction _Transaction = null;
            try
            {
              
                    using (SqlConnection conn = new SqlConnection(_db.GetConfiguration().GetConnectionString("DefaultConnection")))
                    {
                        if (conn.State != ConnectionState.Open)
                            conn.Open();
                        SqlCommand cmd = new SqlCommand("DeleteRecord", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlTransaction transaction = conn.BeginTransaction();
                        cmd.Transaction = transaction;
                        cmd.Parameters.AddWithValue("@maintenanceId", maintenanceId);
                        await cmd.ExecuteNonQueryAsync();
                        transaction.Commit();
                        return Json(new { success = true, message = "Record Deleted Successfully." });
                    }             

            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = "Error Deleting Record.", error = ex.Message });
            }

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
