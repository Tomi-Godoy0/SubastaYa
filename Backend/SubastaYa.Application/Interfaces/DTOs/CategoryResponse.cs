using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaYa.Application.Interfaces.DTOs
{
    public class CategoryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IconUrl { get; set; }
    }
}
