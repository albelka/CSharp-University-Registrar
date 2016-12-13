using System.Data.SqlClient;

namespace Registrar
{
  public class DB
  {
    public static SqlConnection Connection()
    {
      return new SqlConnection(DBConfiguration.ConnectionString);
    }
  }
}
