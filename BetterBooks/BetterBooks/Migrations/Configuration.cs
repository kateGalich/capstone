namespace BetterBooks.Migrations
{
    using BetterBooks.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.


            // create sample users with password q1q1q1
            var anna = new ApplicationUser
            {
                Id = "90c5caa7-663e-4478-9e08-0d2212c0552f",
                UserName = "anna@test.com",
                Email = "anna@test.com",
                PasswordHash = "AEpauxHluHAD5HA8FVm0fDl+A6Mv4qsI3MjYOXpnsSEkcT/5dZTKfLJ3gJLIPTIOXg==",
                SecurityStamp = "c82866dd-9742-4040-bd44-87b8062fd042",
            };
            var bob = new ApplicationUser
            {
                Id = "545e88a0-1eb0-4cc9-953e-60ba57047fe6",
                UserName = "bob@test.com",
                Email = "bob@test.com",
                PasswordHash = "AEPmIctYscIirG7bQBfKGmg9fhijYcQAKIVrDrpG3q8PsriE9A813NtMRIA16hk2wg==",
                SecurityStamp = "470d6042-7fa7-4916-a5ab-bf2068f94a77",
            };
            var cloe = new ApplicationUser
            {
                Id = "caaba992-491a-4c39-9e62-19c8a9a20bd9",
                UserName = "cloe@test.com",
                Email = "cloe@test.com",
                PasswordHash = "AEy0pb2Oi3RYrCVnteWV3yvjKcx5s5WjeGfDxP4Zmyo5NRn0FCqKBMoRcySZkhfsEg==",
                SecurityStamp = "eeb9d92b-96ca-4546-b212-a170735db8b8",
            };

            // create sample books
            Book blackCat = new Book { Author = "Edgar Allan Poe", Title = "The Black Cat" };
            Book goldBug = new Book { Author = "Edgar Allan Poe", Title = "The Gold-Bug" };
            Book hopFrog = new Book { Author = "Edgar Allan Poe", Title = "Hop-Frog" };
            Book hobbit = new Book { Author = "J.R.R. Tolkien", Title = "The Hobbit" };
            Book lordOfTheRings = new Book { Author = "J.R.R. Tolkien", Title = "The Lord of the Rings" };

            // add books to users
            anna.Books.Add(blackCat);
            anna.Books.Add(goldBug);
            bob.Books.Add(hopFrog);
            bob.Books.Add(hobbit);
            cloe.Books.Add(lordOfTheRings);

            // make test book requests
            anna.RequestedBooks.Add(hobbit);
            cloe.RequestedBooks.Add(hobbit);
            cloe.RequestedBooks.Add(blackCat);

            context.Users.AddOrUpdate(
                u => u.UserName,
                anna,
                bob,
                cloe
            );
            context.SaveChanges();
        }
    }
}
