using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Project.Models;

namespace Project.Structure
{
    public class Person :Entity
    {
        public readonly int ENTITYTYPEID = (int)projectEnums.entityType.Person;
        public person PERSON { get; set; }

        public Person() { }

        public void Load(int personEntityId) 
        {
            using (var ctx = new accountingEntities())
            {
                var person = ctx.people
                    .Where(x => x.ID.Equals(personEntityId))
                    .SingleOrDefault();

                PERSON = person;

                if (person == null)
                    throw new Exception("no such a Person Exists");
            }    
        }

        public void New(string firstName,string lastName) 
        {
            using (var ctx = new accountingEntities())
            {   
                var checkDuplication = ctx.people.Where(x => x.firstname == firstName && x.lastname == lastName).FirstOrDefault();
                if (checkDuplication != null)
                    throw new Exception("Person Duplicated");

                var newPerson = new person() 
                {
                    firstname=firstName,
                    lastname=lastName,
                    ID = base.ENTITY.ID
                };
                ctx.people.AddObject(newPerson);
                ctx.SaveChanges();

                this.PERSON = newPerson;
            }
            
        }
        public List<account> createAccounts(int currencyID) 
        {
            List<account> accounts = new List<account>();

            var acc_W = new WAccount();
            var acc_AR = new ARAccount();
            var acc_AP = new APAccount();
            var acc_EXP = new EXPAccount();
            var acc_INC = new INCAccount();
            var acc_CCCASH = new CCCASHAccount();
            var acc_DBCASH = new DBCASHAccount();

            return accounts;
        }
    }
    

}
