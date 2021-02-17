using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat_Application.Models
{
    public class DatabaseSettings: IDatabaseSettings
    {
        public string ConversationsCollectionName { get; set; }
        public string UsersCollectionName { get; set; }

        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IDatabaseSettings
    {
        string ConversationsCollectionName { get; set; }
        string UsersCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
