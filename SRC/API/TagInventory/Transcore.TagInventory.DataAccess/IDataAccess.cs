using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transcore.TagInventory.DataAccess
{
    public interface IDataAccess
    {
        DataSet Execute(string proc, SqlParameter[] sqlParams);

        int ExecuteNonQuery(string proc, SqlParameter[] sqlParams);

        object ExecuteScaler(string proc, SqlParameter[] sqlParams);
    }
}
