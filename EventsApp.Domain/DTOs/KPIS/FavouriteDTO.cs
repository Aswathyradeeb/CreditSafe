using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EventsApp.Domain.DTOs
{
    public class FavouriteDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int KPIId { get; set; }

    }
}

