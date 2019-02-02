﻿using System.Collections.Generic;

namespace Vidly.Models
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Movie> Movies { get; set; } 
    }
}