using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlWorker;

namespace OTBaseNew.SQL
{
    public static class SqlConnect
    {
        static string connectionstring = "server=zlkmagaz.mysql.ukraine.com.ua;uid=zlkmagaz_otbase;pwd=5l6v6wba;database=zlkmagaz_otbase;Allow Zero Datetime=true;";
        public static MySqlWorker.DataBase db = new DataBase(connectionstring);
    }
}
