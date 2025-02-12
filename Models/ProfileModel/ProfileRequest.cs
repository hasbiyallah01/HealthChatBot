﻿using HealthChatBox.Core.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace HealthChatBox.Models.ProfileModel
{
    public class ProfileRequest
    {
        public int UserId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Height must be greater than 0.")]
        public int Height { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Weight must be greater than 0.")]

        public int Weight { get; set; }
        [Required]
        public string Nationality { get; set; } = default!;
        public string? BodyFat { get; set; } = default!;
        public string? DietType { get; set; } = default!;
        public string? SnackPreference { get; set; } = default!;
        public string? NoOfMealPerDay { get; set; } = default!;
        public ICollection<string> Allergies { get; set; } = new HashSet<string>();
        public ICollection<string> UserGoals { get; set; } = new HashSet<string>();
    }
}
