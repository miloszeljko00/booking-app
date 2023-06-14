using AccomodationSuggestion.Application.Dtos;
using AccomodationSuggestion.Domain.Entities;
using AccomodationSuggestion.Domain.Interfaces;
using AccomodationSuggestion.Infrastructure.Persistence.Settings;
using Microsoft.Extensions.Options;
using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AccomodationSuggestion.Infrastructure.Repositories
{
    public class Neo4jAccommodationSuggestionRepository : IAccommodationSuggestionRepository
    {
        private readonly IDriver driver;
        public Neo4jAccommodationSuggestionRepository(IOptions<DatabaseSettings> dbSettings)
        {
            driver = GraphDatabase.Driver(dbSettings.Value.Uri, AuthTokens.Basic(dbSettings.Value.Username, dbSettings.Value.Password));
        }

        public async Task<List<UserNode>> getAllUserNodesAsync()
        {
            //await driver.VerifyConnectivityAsync();
            using (var session = driver.AsyncSession())
            {

                var result = await session.RunAsync("MATCH (u:User) RETURN u.email AS email");

                var resultList = await result.ToListAsync();
                var users = new List<UserNode>();
                var emails = resultList.Select(x => x["email"].As<string>());
                foreach (var email1 in emails)
                {
                    //var user = record["u"].As<UserNode>();
                    users.Add(new UserNode(email1));
                }


                return users;
            }
        } 

        public async Task<UserNode> createUserAsync(string email)
        {
            await using var session = driver.AsyncSession();

            var userEmail = await session.ExecuteWriteAsync(async tx =>
            {
                var query = @"
                    MERGE (u:User {
                        email: $email     
                    })
                    RETURN u.email as email";
                var cursor = await tx.RunAsync(query, new { email });

                var record = await cursor.SingleAsync();
                
                return record["email"].As<string>();
            });
            UserNode returnNode = new UserNode(userEmail);
            return returnNode;
            
        }

        public async Task<AccommodationNode> createAccommodationNode(AccommodationNode accommodationNode)
        {
            await using var session = driver.AsyncSession();
            string accommodationName = accommodationNode.AccommodationName;
            string hostEmail = accommodationNode.HostEmail;
            var accData = await session.ExecuteWriteAsync(async tx =>
            {
                var query = @"
                    MERGE (a:Accommodation {
                        accommodationName: $accommodationName,
                        hostEmail: $hostEmail
                    })
                    RETURN a.accommodationName as accName, a.hostEmail as email";
                
                var cursor = await tx.RunAsync(query, new { accommodationName, hostEmail });

                var record = await cursor.SingleAsync();
                int a = 5;
                
                string accName = record["accName"].As<string>();
                string host = record["email"].As<string>();
                return new AccommodationNode(host, accName);
                
            });

            
            return accData;
        }
        public async Task<bool> createGradeRelationship(int grade, string accommodationName, string email)
        {
            await using var session = driver.AsyncSession();
           
            var accData = await session.ExecuteWriteAsync(async tx =>
            {
                var query = @"
                    MATCH (a:Accommodation {accommodationName: $accommodationName })
                    MATCH (u: User {email :$email})
                    MERGE (u) -[g:Graded{grade: $grade}]-> (a)                       
                    RETURN g.grade as grade";

                var cursor = await tx.RunAsync(query, new { accommodationName, email, grade });

                var record = await cursor.SingleAsync();
                int a = record["grade"].As<int>(); ;
                
                return a;

            });

            return true;
        }


    }
}
