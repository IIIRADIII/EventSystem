using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace LampaEventService
{
    class EasySQL
    {
        public EasySQL()
        {
        }
        public EasySQL(string query)
        {
            queryString = query;
        }
        public string queryString { get; set; }
        SqlConnection connection = new SqlConnection(@"Data Source=192.168.0.77\SQL2008;Initial Catalog=projectOne;" +
          "Persist Security Info=True;User ID=sa;Password=ilovesql");


        public Dictionary<string, string> RowToDictionary()
        {
            connection.Open();
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            SqlCommand command = new SqlCommand(queryString, connection);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                reader.Read();
                dictionary = Enumerable.Range(0, reader.FieldCount).ToDictionary(i => reader.GetName(i), i => reader.GetValue(i).ToString());
            }
            connection.Close();
            return dictionary;
        }

        public List<int> IntColToList()
        {
            connection.Open();
            List<int> list = new List<int>();
            SqlCommand command = new SqlCommand(queryString, connection);
            
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(Convert.ToInt32(reader.GetValue(0)));
                }
            }
            connection.Close();
            return list;
        }

        public List<string> StrColToList()
        {
            connection.Open();
            List<string> list = new List<string>();
            SqlCommand command = new SqlCommand(queryString, connection);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(reader.GetValue(0).ToString());
                }
            }
            connection.Close();
            return list;
        }

        public string ValueToString()
        {
            connection.Open();
            string value = "";
            SqlCommand command = new SqlCommand(queryString, connection);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                try
                {
                    reader.Read();
                    if (!reader.IsDBNull(0))
                    {
                        value = reader.GetValue(0).ToString();
                        connection.Close();
                        return value;
                    }
                    else
                    {
                        connection.Close();
                        return "";
                    }
                }
                catch
                {
                    connection.Close();
                    return "";
                }
            }
        }

        public int ValueToInt()
        {
            connection.Open();
            int value = 0;
            SqlCommand command = new SqlCommand(queryString, connection);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                try {
                    reader.Read();
                    if (!reader.IsDBNull(0))
                    {
                        value = Convert.ToInt32(reader.GetValue(0));
                        connection.Close();
                        return value;
                    }
                    else
                    {
                        connection.Close();
                        return 0;
                    }
                }
                catch 
                {
                    connection.Close();
                    return 0;
                }
            }
            
        }
        public Dictionary<int,string> IntStrColsToDictionary()
        {
            connection.Open();
            Dictionary<int, string> dictionary = new Dictionary<int, string>();
            SqlCommand command = new SqlCommand(queryString, connection);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    dictionary.Add(Convert.ToInt32(reader.GetValue(0)), reader.GetValue(1).ToString());
                }
            }
            connection.Close();
            return dictionary;
        }

        public void Update()
        {
            connection.Open();
            SqlCommand command = new SqlCommand(queryString, connection);
            command.ExecuteNonQuery();
            connection.Close();
        }

    }
}
