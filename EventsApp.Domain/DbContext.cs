using EventsApp.Domain.Entities;
using System.Data.Entity;
using System.Linq;

namespace EventsApp.Domain.Entities
{
    public partial class eventsappEntities : DbContext
    {  
        public override int SaveChanges()
        {
            HandleOrphans();
            return base.SaveChanges();
        }

        private void HandleOrphans()
        {
            
             
            


            //Event Orphan Entries End
        }
    }
}
