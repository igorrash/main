using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MySql.Data.MySqlClient;

namespace External
{
    public class DbConnect
    {
        private MySqlConnection _connection;
        private string _server;
        private string _database;
        private string _uid;
        private string _password;

        public DbConnect(string dataBase, string uid, string password)
        {
            InitializeConnection(dataBase, uid, password);
        }
        private void InitializeConnection(string dataBase, string uid, string password)
        {
            _server = "74.220.207.173";
            _database = dataBase;
            _uid = uid;
            _password = password;
            var connectionString = "SERVER=" + _server + ";" +
                                   "DATABASE=" + _database + ";" +
                                   "UID=" + _uid + ";" +
                                   "PASSWORD=" + _password + ";" +
                                   "CHARSET=utf8;";
            _connection = new MySqlConnection(connectionString);
        }

        //open connection to database
        private bool OpenConnection()
        {
            try
            {
                _connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        throw new Exception("Cannot connect to server.  Contact administrator");

                    case 1045:
                        throw new Exception("Invalid username/password, please try again");
                }
                return false;
            }
        }

        //Close connection
        private void CloseConnection()
        {
            _connection.Close();
        }
   
        private string AddEscapeChars(string str)
        {
            return str.Replace("'", "\'");
        }

        public DataTable Select(string tableName, List<string> columnNames, List<string> whereClauses = null, string orderBy = null)
        {
            var dt = new DataTable();

            //Open connection
            if (OpenConnection())
            {
                //Create Command
                string where = (whereClauses == null || !whereClauses.Any())
                    ? "1=1"
                    : string.Join(" AND ", whereClauses);
                string sql = "SELECT " + string.Join(",", columnNames) + " FROM " + tableName + " WHERE " +
                             AddEscapeChars(@where);

                var cmd = new MySqlCommand(sql, _connection);

                //Create a data reader and Execute the command
                var dataReader = cmd.ExecuteReader();

                dt.Load(dataReader);
                //close Data Reader
                dataReader.Close();

                //close Connection
                CloseConnection();
            }
            return dt;
        }

        public void Insert(string tableName, List<string> parameters)
        {
            //open connection
            if (OpenConnection())
            {

                string sql = "INSERT INTO " + tableName + " VALUES('" + string.Join("','", parameters) + "')";
                //create command and assign the query and connection from the constructor

                var cmd = new MySqlCommand(sql, _connection);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                CloseConnection();
            }
        }

        public void Update(string tableName, List<string> pairs, List<string> whereClauses)
        {
            //open connection
            if (OpenConnection())
            {
                pairs.ForEach(x => AddEscapeChars(x));
                string where = (whereClauses == null || !whereClauses.Any())
                    ? " 1=1 "
                    : string.Join(" AND ", whereClauses);
                string sql = "UPDATE " + tableName + " SET " + string.Join(",", pairs) + " WHERE " +
                             AddEscapeChars(@where);
                //create command and assign the query and connection from the constructor

                var cmd = new MySqlCommand(sql, _connection);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                CloseConnection();
            }
        }

        public void Delete(string tableName, List<string> whereClause)
        {
            if (whereClause == null || !whereClause.Any())
            {
                throw new Exception("Where clause cannot be empty.");
            }
            //open connection
            if (OpenConnection())
            {

                string sql = "DELETE FROM " + tableName + " WHERE " + AddEscapeChars(string.Join(" AND ", whereClause));
                //create command and assign the query and connection from the constructor

                var cmd = new MySqlCommand(sql, _connection);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                CloseConnection();
            }
        }

        public DataTable RunSql(string sql)
        {
            var dt = new DataTable();
            //Open connection
            if (OpenConnection())
            {
                //create mysql command
                var cmd = new MySqlCommand
                {
                    CommandText = sql,
                    Connection = _connection
                };

                //Create a data reader and Execute the command
                var dataReader = cmd.ExecuteReader();

                dt.Load(dataReader);
                //close Data Reader
                dataReader.Close();
                //Execute query
                //cmd.ExecuteNonQuery();

                //close connection
                CloseConnection();
            }
            return dt;
        }

        public int Count()
        {
            const string query = "SELECT Count(*) FROM tableinfo";
            var count = -1;

            //Open Connection
            if (OpenConnection())
            {
                //Create Mysql Command
                var cmd = new MySqlCommand(query, _connection);

                //ExecuteScalar will return one value
                count = int.Parse(cmd.ExecuteScalar() + "");

                //close Connection
                CloseConnection();

                return count;
            }
            return count;
        }

        ////Backup
        //public void Backup()
        //{
        //    try
        //    {
        //        DateTime Time = DateTime.Now;
        //        int year = Time.Year;
        //        int month = Time.Month;
        //        int day = Time.Day;
        //        int hour = Time.Hour;
        //        int minute = Time.Minute;
        //        int second = Time.Second;
        //        int millisecond = Time.Millisecond;

        //        //Save file to C:\ with the current date as a filename
        //        string path;
        //        path = "C:\\" + year + "-" + month + "-" + day + "-" + hour + "-" + minute + "-" + second + "-" + millisecond + ".sql";
        //        StreamWriter file = new StreamWriter(path);


        //        ProcessStartInfo psi = new ProcessStartInfo();
        //        psi.FileName = "mysqldump";
        //        psi.RedirectStandardInput = false;
        //        psi.RedirectStandardOutput = true;
        //        psi.Arguments = string.Format(@"-u{0} -p{1} -h{2} {3}", _uid, _password, _server, _database);
        //        psi.UseShellExecute = false;

        //        Process process = Process.Start(psi);

        //        string output;
        //        output = process.StandardOutput.ReadToEnd();
        //        file.WriteLine(output);
        //        process.WaitForExit();
        //        file.Close();
        //        process.Close();
        //    }
        //    catch (IOException ex)
        //    {
        //        MessageBox.Show("Error , unable to backup!");
        //    }
        //}

        ////Restore
        //public void Restore()
        //{
        //    try
        //    {
        //        //Read file from C:\
        //        string path;
        //        path = "C:\\MySqlBackup.sql";
        //        StreamReader file = new StreamReader(path);
        //        string input = file.ReadToEnd();
        //        file.Close();


        //        ProcessStartInfo psi = new ProcessStartInfo();
        //        psi.FileName = "mysql";
        //        psi.RedirectStandardInput = true;
        //        psi.RedirectStandardOutput = false;
        //        psi.Arguments = string.Format(@"-u{0} -p{1} -h{2} {3}", _uid, _password, _server, _database);
        //        psi.UseShellExecute = false;


        //        Process process = Process.Start(psi);
        //        process.StandardInput.WriteLine(input);
        //        process.StandardInput.Close();
        //        process.WaitForExit();
        //        process.Close();
        //    }
        //    catch (IOException ex)
        //    {
        //        MessageBox.Show("Error , unable to Restore!");
        //    }
        //}
        ////Update statement
        //public void Update()
        //{
        //    string query = "UPDATE tableinfo SET name='Joe', age='22' WHERE name='John Smith'";
        //    //Open connection
        //    if (OpenConnection())
        //    {
        //        //create mysql command
        //        var cmd = new MySqlCommand
        //        {
        //            CommandText = query,
        //            Connection = _connection
        //        };
        //        //Assign the query using CommandText
        //        //Assign the connection using Connection

        //        //Execute query
        //        cmd.ExecuteNonQuery();

        //        //close connection
        //        CloseConnection();
        //    }
        //}

        ////Insert statement
        //public void Insert()
        //{
        //    const string query = "INSERT INTO andrei SET customer = UPPER('a'), name = UPPER('b'), compania = UPPER('c'), category = UPPER('d'), cost = UPPER('e')";
        //    //open connection
        //    if (OpenConnection())
        //    {
        //        //create command and assign the query and connection from the constructor
        //        var cmd = new MySqlCommand(query, _connection);

        //        //Execute command
        //        cmd.ExecuteNonQuery();

        //        //close connection
        //        CloseConnection();
        //    }
        //}
        ////Delete statement
        //public void Delete()
        //{
        //    const string query = "DELETE FROM tableinfo WHERE name='John Smith'";
        //    if (OpenConnection())
        //    {
        //        var cmd = new MySqlCommand(query, _connection);
        //        cmd.ExecuteNonQuery();
        //        CloseConnection();
        //    }
        //}

        ////Delete statement
        //public void Delete(string sql)
        //{
        //    if (OpenConnection())
        //    {
        //        var cmd = new MySqlCommand(sql, _connection);
        //        cmd.ExecuteNonQuery();
        //        CloseConnection();
        //    }
        //}

        ////Select statement
        //public DataTable Select(string query)
        //{
        //    var dt = new DataTable();
        //    //Open connection
        //    if (OpenConnection())
        //    {
        //        var cmd = new MySqlCommand(query, _connection);

        //        //Create a data reader and Execute the command
        //        var dataReader = cmd.ExecuteReader();

        //        dt.Load(dataReader);
        //        //close Data Reader
        //        dataReader.Close();

        //        //close Connection
        //        CloseConnection();
        //    }
        //    return dt;
        //}

        //Select statement



        //Count statement
    }
}
