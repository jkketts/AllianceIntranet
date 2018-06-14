using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AllianceIntranet.Data
{
    public class Connection
    {
        public string GetConnection(string DatabaseURL)
        {
            //Splitting string based on format provided by Heroku : postgres://<username>:<password>@<host>/<dbname>
            var split1 = DatabaseURL.Split("/");
            var split2 = split1[2].Split(":");
            var split3 = split2[1].Split("@");

            var UserID = split2[0];

            var Password = split3[0];

            var Host = split3[1];

            var Database = split1[3];

            var connectionString = $"Database={Database}; host={Host}; Port=5432; User ID={UserID}; Password={Password}; sslmode=Require; Trust Server Certificate=true";

            return connectionString;
            }
    }
}
