using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
namespace OTBaseNew.SQL
{
    class DataBase
    {
        /// <summary>
        /// Конект к БД
        /// </summary>
        private MySqlConnection connect = new MySqlConnection();
        public DataBase()
        {
            //Строка конект к БД
            string myConnectionString = "server=zlkmagaz.mysql.ukraine.com.ua;uid=zlkmagaz_zlkbase;pwd=zz2j45bq;database=zlkmagaz_zlkbase;";
            connect.ConnectionString = myConnectionString;
            connect.Open();
        }
        /// <summary>
        /// Создание запроса к БД
        /// </summary>
        /// <param name="request">Строка запроса</param>
        /// <returns>Результат запроса к БД</returns>
        public MySqlDataReader MakeRequest(string request)
        {
            //Команда для создания запроса
            MySqlCommand command = connect.CreateCommand();
            //Присвоение запроса к команде
            command.CommandText = request;
            //Выполнение запроса
            MySqlDataReader reader = command.ExecuteReader();
            //Закрыть конект
            connect.Close();
            //Возврат результата
            return reader;
        }
    }
}
