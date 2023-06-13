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

namespace AccomodationSuggestion.Infrastructure.Repositories
{
    public class Neo4jAccommodationSuggestionRepository: IAccommodationSuggestionRepository
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

        
    }
}
