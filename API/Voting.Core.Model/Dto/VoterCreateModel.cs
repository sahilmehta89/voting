﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voting.Core.Model.Dto
{
    public class VoterCreateModel
    {
        [Required]
        public string Name { get; set; }
    }
}
