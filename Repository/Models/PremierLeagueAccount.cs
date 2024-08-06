﻿using System;
using System.Collections.Generic;

namespace Repository.Models
{
    public partial class PremierLeagueAccount
    {
        public int AccId { get; set; }
        public string Password { get; set; } = null!;
        public string? EmailAddress { get; set; }
        public string Description { get; set; } = null!;
        public int? Role { get; set; }
    }
}