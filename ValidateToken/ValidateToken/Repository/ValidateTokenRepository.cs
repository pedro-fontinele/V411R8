using Dapper;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ValidateToken.Model;

namespace ValidateToken.Repository
{
    public class ValidateTokenRepository : IValidateTokenRepository
    {
        private DataContext _dataContext;

        public ValidateTokenRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<IntegrationvariablesStatus>> GetAllAsync()
        {
            using (var connection = _dataContext.GetConnection())
            {
                try
                {
                    await connection.OpenAsync();

                    var result = await connection.QueryAsync<IntegrationvariablesStatus>(
                        sql: @"select iv.Name, 
                                      iv.Value, 
                                      c.SocialReason
                               from integrationvariables iv 
                               inner join Integrations i on iv.IntegrationId = i.id 
                               inner join Clients c on i.ClientId = c.id
                               where i.Name = 'Bling V3' 
                               and iv.Name = 'ACCESS TOKEN'
                               order by 3 desc"
                    );

                    await connection.CloseAsync();

                    return result.ToList();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
