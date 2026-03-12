using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;


namespace GAVETA_DA_SORTE_WPF
{
    public class Database
    {
        string connectionString =
            @"Server=GABRIEL;Database=gaveta_da_sorte;Trusted_Connection=True;TrustServerCertificate=True;";

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
