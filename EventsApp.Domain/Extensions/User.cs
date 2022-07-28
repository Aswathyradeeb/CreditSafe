 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsApp.Domain.Entities
{
    public partial class User
    {
        public virtual bool Update(User user)
        {
            //TODO:add the validation
            var result = true;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.PhoneNumber = user.PhoneNumber; 
            return result;
        }
    }
}
