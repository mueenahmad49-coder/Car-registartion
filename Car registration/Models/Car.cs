using System;
using System.ComponentModel.DataAnnotations;

namespace CarRecordApp.Models
{
    public class Car
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Car Name is required.")]
        [StringLength(20, ErrorMessage = "Car Name cannot exceed 20 characters.")]
        public string CarName { get; set; }

        [Required(ErrorMessage = "Owner Name is required.")]
        [StringLength(20, ErrorMessage = "Owner Name cannot exceed 20 characters.")]
        public string OwnerName { get; set; }

        [Required(ErrorMessage = "Model Year is required.")]
        [Range(2000, 2021, ErrorMessage = "Model Year must be between 2000 and 2021.")]
        public int ModelYear { get; set; }

        [Required(ErrorMessage = "Manufactured Date is required.")]
        [DataType(DataType.Date)]
        public DateTime ManufacturedDate { get; set; }

        [Required(ErrorMessage = "Chassis Number is required.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Chassis Number must be exactly 10 digits.")]
        public string ChassisNumber { get; set; }
    }
}