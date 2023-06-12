using AccomodationSuggestion.Application.Dtos;
using AccomodationSuggestion.Domain.Interfaces;
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
        public Neo4jAccommodationSuggestionRepository()
        {
            driver = GraphDatabase.Driver("neo4j+s://ce07430c.databases.neo4j.io:7687", AuthTokens.Basic("neo4j", "v_1UcQcuqrmVtpPb1jxgJs6jqVZjNzeo1QTj23xBGuU"));
            //await driver.VerifyConnectivityAsync();
        }

        public async Task<List<UserNodeDTO>> getAllUserNodesAsync()
        {
            using (var session = driver.AsyncSession())
            {
                // Run a Cypher query to retrieve all user nodes
                var result = await session.RunAsync("MATCH (u:User) RETURN u");

                // Process the result and collect user nodes
                var resultList = result.ToListAsync();
                var users = new List<UserNodeDTO>();
                foreach (var record in resultList.Result)
                {
                    var user = record["u"].As<UserNodeDTO>();
                    users.Add(user);
                }

                return users;
            }
        }
    }
}
