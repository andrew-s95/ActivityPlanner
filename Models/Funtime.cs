using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeltExam.Models
{
    public class NoPastDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if ((DateTime)value <= DateTime.Now)
                return new ValidationResult("Date must be a future date");
            return ValidationResult.Success;
        }
    }

    public class Funtime
    {
        [Key]
        public int FuntimeId { get; set; }
        
        [Required]
        public string Title { get; set; }

        public TimeSpan Time { get; set; }

        [NoPastDate(ErrorMessage = "Date must be a future date")]
        public DateTime Date { get; set; }

        public int Duration { get; set; }
        public string Length { get; set; }

        [Required]
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public int UserId { get; set; }
        public User Planner { get; set; }
        public List<Association> Associations { get; set; }
    }
}