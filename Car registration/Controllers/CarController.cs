using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using CarRecordApp.Models;
using System.Data;

namespace CarRecordApp.Controllers
{
    public class CarController : Controller
    {
        private readonly string _connectionString;

        // appsettings.json se connection string read karne ke liye constructor
        public CarController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // 1. READ (Sab cars ki list dikhane ke liye)
        public IActionResult Index()
        {
            List<Car> carList = new List<Car>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Cars";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            carList.Add(new Car
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                CarName = reader["CarName"].ToString(),
                                OwnerName = reader["OwnerName"].ToString(),
                                ModelYear = Convert.ToInt32(reader["ModelYear"]),
                                ManufacturedDate = Convert.ToDateTime(reader["ManufacturedDate"]),
                                ChassisNumber = reader["ChassisNumber"].ToString()
                            });
                        }
                    }
                }
            }
            return View(carList);
        }

        // 2. CREATE (Form kholne ke liye)
        public IActionResult Create()
        {
            return View();
        }

        // 2. CREATE (Form submit hone par data save karne ke liye)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Car car)
        {
            // Custom validation for manufactured date (must be before current date)
            if (car.ManufacturedDate >= DateTime.Now.Date)
            {
                ModelState.AddModelError("ManufacturedDate", "Date aaj se pehle ki honi chahiye.");
            }

            if (ModelState.IsValid)
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string query = "INSERT INTO Cars (CarName, OwnerName, ModelYear, ManufacturedDate, ChassisNumber) VALUES (@CarName, @OwnerName, @ModelYear, @MfgDate, @Chassis)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CarName", car.CarName);
                        cmd.Parameters.AddWithValue("@OwnerName", car.OwnerName);
                        cmd.Parameters.AddWithValue("@ModelYear", car.ModelYear);
                        cmd.Parameters.AddWithValue("@MfgDate", car.ManufacturedDate);
                        cmd.Parameters.AddWithValue("@Chassis", car.ChassisNumber);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(car);
        }

        // 3. UPDATE (Purana data nikal kar form me bharne ke liye)
        public IActionResult Edit(int id)
        {
            Car car = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Cars WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            car = new Car
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                CarName = reader["CarName"].ToString(),
                                OwnerName = reader["OwnerName"].ToString(),
                                ModelYear = Convert.ToInt32(reader["ModelYear"]),
                                ManufacturedDate = Convert.ToDateTime(reader["ManufacturedDate"]),
                                ChassisNumber = reader["ChassisNumber"].ToString()
                            };
                        }
                    }
                }
            }
            if (car == null) return NotFound();
            return View(car);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Car car)
        {
            if (id != car.Id) return NotFound();
           
            if (car.ManufacturedDate >= DateTime.Now.Date)
            {
                ModelState.AddModelError("ManufacturedDate", "Manufactured Date must be before the current date.");
            }

            if (ModelState.IsValid)
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string query = "UPDATE Cars SET CarName=@CarName, OwnerName=@OwnerName, ModelYear=@ModelYear, ManufacturedDate=@MfgDate, ChassisNumber=@Chassis WHERE Id=@Id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", car.Id);
                        cmd.Parameters.AddWithValue("@CarName", car.CarName);
                        cmd.Parameters.AddWithValue("@OwnerName", car.OwnerName);
                        cmd.Parameters.AddWithValue("@ModelYear", car.ModelYear);
                        cmd.Parameters.AddWithValue("@MfgDate", car.ManufacturedDate);
                        cmd.Parameters.AddWithValue("@Chassis", car.ChassisNumber);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(car);
        }

      
        public IActionResult Delete(int id)
        {
            Car car = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Cars WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            car = new Car
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                CarName = reader["CarName"].ToString(),
                                OwnerName = reader["OwnerName"].ToString()
                            };
                        }
                    }
                }
            }
            if (car == null) return NotFound();
            return View(car);
        }

       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Cars WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}