using System;

namespace FluentRest.Tests
{
    public class UserData
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public string AddressLine1 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string PostalCode { get; set; }


        public static UserData Create()
        {
            var user = new UserData
            {
                Id = DateTime.Now.Ticks,
                FirstName = "Test",
                LastName = "User",
                EmailAddress = "Test.User@email.com",
                AddressLine1 = "123 Main St",
                City = "New York",
                State = "NY",
                PostalCode = "10026"
            };

            return user;
        }
    }
}