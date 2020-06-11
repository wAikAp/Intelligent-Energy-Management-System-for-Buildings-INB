using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ISSF2020.Models
{
    public class UserModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required]
        [RegularExpression(@"^(?=.{3,20}$)(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$")]
        // username is 3-20 characters long | no _ or . at the beginning | no __ or _. or ._ or .. inside 
        // allowed characters | no _ or . at the end
        public string Username { get; set; }

        public string Email { get; set; }

        [Required]
        [StringLength(120, MinimumLength = 6)]
        public string Password { get; set; }

        public string Role { get; set; } = "default";

    }
}
