using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transcore.TagInventory.Common.Exceptions;

namespace Transcore.TagInventory.DataAccess
{
    public class SqlDataAccess : IDataAccess
    {
        protected readonly string _connectionString;

        public SqlDataAccess(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public DataSet Execute(string proc, SqlParameter[] sqlParams)
        {
            DataSet ds = new DataSet();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(proc, connection))
                {
                    if (sqlParams != null && sqlParams.Length > 0)
                        command.Parameters.AddRange(sqlParams);

                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataAdapter da = new SqlDataAdapter(command))
                    {
                        da.Fill(ds);
                    }
                }
            }

            return ds;
        }

        public int ExecuteNonQuery(string proc, SqlParameter[] sqlParams)
        {
            int rowsEffected = 0;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(proc, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddRange(sqlParams);

                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();

                        try
                        {
                            rowsEffected = command.ExecuteNonQuery();
                            //command.Parameters["@opv_biID"].Value;
                        }
                        catch (Exception ex)
                        {
                            throw new BadRequestException(ex.Message);
                        }


                        connection.Close();
                    }
                }
            }

            return rowsEffected;
        }

        public object ExecuteScaler(string proc, SqlParameter[] sqlParams)
        {
            object id = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(proc, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddRange(sqlParams);

                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();

                        id = command.ExecuteScalar();

                        /*
                        try
                        {
                            id = command.ExecuteScalar();
                        }
                        catch (Exception ex)
                        {
                            throw new BadRequestException(ex.Message);
                        }*/
                        

                        connection.Close();
                    }
                }
            }

            return id;
        }
    }
}
