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

            var accData = await session.ExecuteWriteAsync(async tx =>
            {
                var query = @"
                    MERGE (a:Accommodation {
                        accommodationId: $accId
                    })
                    RETURN a.accommodationId AS id";
                var cursor = await tx.RunAsync(query, new { accommodationNode.AccommodationId });
            //a {.accommodationId, .accommodationName, .hostEmail} as a
            //.accommodationId as id, a.accommodationName as accName, a.hostEmail as email
            //,accommodationName: $accName, hostEmail: $hostEmail , accommodationNode.AccommodationName, accommodationNode.HostEmail

                var record = await cursor.SingleAsync();

                /*string accId  =  record["id"].As<string>();
                string accName = record["accName"].As<string>();
                string hostEmail = record["email"].As<string>();
                return new AccommodationNode(hostEmail, accId, accName);*/
                return record["id"].As<string>();
            });

            //AccommodationNode accNode = new AccommodationNode(accData["accommodationId"], accData["accommodationName"], accData["hostEmail"]);
            return accommodationNode;
        }
    }
}
