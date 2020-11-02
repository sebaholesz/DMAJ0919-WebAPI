using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class CoronaOperations
    {
        public List<Datum> GetTheRecords(string sqlQuery)
        {
            string connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            List<Datum> theReply = new List<Datum>();
            using (SqlConnection connDB = new SqlConnection(connString))
            {
                try
                {
                    connDB.Open();
                    var sqlCmd = new SqlCommand(sqlQuery, connDB);
                    var reader = sqlCmd.ExecuteReader();
                    while (reader.Read())
                    {
                        int index = 0;
                        Datum newData = new Datum();
                        newData.countrycode = reader.GetString(index++);
                        newData.date = reader.GetDateTime(index++).ToString();
                        newData.cases = reader.GetInt32(index++).ToString();
                        newData.deaths = reader.GetInt32(index++).ToString();
                        newData.recovered = reader.GetInt32(index++).ToString();
                        theReply.Add(newData);
                    }
                    reader.Close();
                    connDB.Close();
                }
                catch (SqlException ex)
                {
                    return (theReply);
                }

            }
            return (theReply);
        }

        public string InsertDatumEntry(Datum datum)
        {
            IDbConnection _db;
            _db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

            try
            {
                int rowsAffected = _db.Execute(@"INSERT INTO [dbo].[theStats] values (@countrycode, @date, @cases, @deaths, @recovered)",
                new { 
                    countrycode = datum.countrycode,
                    date = datum.date, 
                    cases = datum.cases,
                    deaths = datum.deaths,
                    recovered = datum.recovered
                });

                if (rowsAffected > 0)
                {
                    return "wow";
                }

                return "oh no";
            }
            catch(SqlException ex)
            {
                throw ex;
            }
        }
    }
}