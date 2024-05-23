using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voting.Core.Model.Dto
{
    public class VoterUpdateModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public bool HasVoted { get; set; }
    }
}
