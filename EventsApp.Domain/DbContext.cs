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
            //Company Orphan Entries
            var orphanedEventCompany =
                ChangeTracker.Entries()
                .Where(x => x.Entity.GetType().BaseType == typeof(EventCompany))
                .Select(x => ((EventCompany)x.Entity))
                .Where(x => x.Company == null)
                .ToList();
            Set<EventCompany>().RemoveRange(orphanedEventCompany);
             
            //Event Orphan Entries Begin
            var orphanedEntities =
                ChangeTracker.Entries()
                .Where(x => x.Entity.GetType().BaseType == typeof(EventCompany))
                .Select(x => ((EventCompany)x.Entity))
                .Where(x => x.Event == null)
                .ToList();
            Set<EventCompany>().RemoveRange(orphanedEntities);

            var orphanedEventPerson =
                ChangeTracker.Entries()
                .Where(x => x.Entity.GetType().BaseType == typeof(EventPerson))
                .Select(x => ((EventPerson)x.Entity))
                .Where(x => x.Event == null)
                .ToList();
            Set<EventPerson>().RemoveRange(orphanedEventPerson);
             
            var orphanedEventUser =
                ChangeTracker.Entries()
                .Where(x => x.Entity.GetType().BaseType == typeof(EventUser))
                .Select(x => ((EventUser)x.Entity))
                .Where(x => x.Event == null)
                .ToList();
            Set<EventUser>().RemoveRange(orphanedEventUser);
             
            var orphanedPhoto =
                ChangeTracker.Entries()
                .Where(x => x.Entity.GetType().BaseType == typeof(Photo))
                .Select(x => ((Photo)x.Entity))
                .Where(x => x.Event == null)
                .ToList();
            Set<Photo>().RemoveRange(orphanedPhoto);
             
            var orphanedAgendum =
                ChangeTracker.Entries()
                .Where(x => x.Entity.GetType().BaseType == typeof(Agendum))
                .Select(x => ((Agendum)x.Entity))
                .Where(x => x.Event == null)
                .ToList();
            Set<Agendum>().RemoveRange(orphanedAgendum);

            var orphanedIntrestedAgendum =
                ChangeTracker.Entries()
                .Where(x => x.Entity.GetType().BaseType == typeof(InterestedAgenda))
                .Select(x => ((InterestedAgenda)x.Entity))
                .Where(x => x.Event == null)
                .ToList();
            Set<InterestedAgenda>().RemoveRange(orphanedIntrestedAgendum);

            var orphanedFavouriteAgendum =
                ChangeTracker.Entries()
                .Where(x => x.Entity.GetType().BaseType == typeof(InterestedAgenda))
                .Select(x => ((InterestedAgenda)x.Entity))
                .Where(x => x.Agendum == null)
                .ToList();
            Set<InterestedAgenda>().RemoveRange(orphanedFavouriteAgendum);


            var orphanedEventAddress =
                ChangeTracker.Entries()
                .Where(x => x.Entity.GetType().BaseType == typeof(EventAddress))
                .Select(x => ((EventAddress)x.Entity))
                .Where(x => x.Event == null)
                .ToList();
            Set<EventAddress>().RemoveRange(orphanedEventAddress);

            var orphanedUserLanguages =
                ChangeTracker.Entries()
                .Where(x => x.Entity.GetType().BaseType == typeof(PreferredLanguage))
                .Select(x => ((PreferredLanguage)x.Entity))
                .Where(x => x.User == null)
                .ToList();
            Set<PreferredLanguage>().RemoveRange(orphanedUserLanguages);
            //Event Orphan Entries End
        }
    }
}
