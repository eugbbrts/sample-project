using BusinessEntities;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;
using System.Linq;

namespace Data.Indexes
{
    public class UsersListIndex : AbstractIndexCreationTask<User>
    {
        public UsersListIndex()
        {
            Map = users => from user in users
                           select new
                           {
                               user.Name,
                               user.Email,
                               user.Type,
                               user.Tags,
                           };

            Index(x => x.Type, FieldIndexing.NotAnalyzed);
        }
    }
}