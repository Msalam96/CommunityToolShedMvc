using CommunityToolShedMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace CommunityToolShedMvc.Security
{
    public class CustomPrincipal : IPrincipal
    {
        private CustomIdentity identity;
        private Person person;

        public CustomPrincipal(CustomIdentity identity, Person person)
        {
            this.identity = identity;
            this.person = person;
        }

        public Person Person
        {
            get
            {
                return person;
            }
        }

        public IIdentity Identity
        {
            get
            {
                return identity;
            }
        }

        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }

        public bool IsInRole(int communityId, string role)
        {
            return person.Roles.Exists(r => r.Id == communityId && r.RoleName == role);
            //bool roleFound = false;

            //foreach (var communityRole in person.Roles)
            //{
            //    if(communityRole.Id == communityId && communityRole.RoleName == role)
            //    {
            //        roleFound = true;
            //        break;
            //    }
            //}

            //return roleFound;
        }
    }
}