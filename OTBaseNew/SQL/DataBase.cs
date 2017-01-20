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
            string myConnectionString = "server=zlkmagaz.mysql.ukraine.com.ua;uid=zlkmagaz_otbase;pwd=5l6v6wba;database=zlkmagaz_otbase;Allow Zero Datetime=true;";
            connect.ConnectionString = myConnectionString;
        }
        /// <summary>
        /// Создание запроса к БД. Возвращает список, который содержит в себе словари, которые в свою очередь содержат ключи и значения запроса
        /// </summary>
        /// <param name="request">Строка запроса</param>
        /// <returns>Значения, возвращаемые запросом</returns>
        public List<Dictionary<string, object>> MakeRequest(string request)
        {
            //Открыть конект
            connect.Open();
            List<Dictionary<string, object>> list_of_values = new List<Dictionary<string, object>>();
            //Команда для создания запроса
            MySqlCommand command = connect.CreateCommand();
            //Присвоение запроса к команде
            command.CommandText = request;
            //Выполнение запроса
            MySqlDataReader reader = command.ExecuteReader();
            //Словарь, в котором сохраняются значения
            //Считывание
            while (reader.Read())
            {
                Dictionary<string, object> values = new Dictionary<string, object>();
                //Добавление значений и ключей в словарь
                values = Enumerable.Range(0, reader.FieldCount)
                   .ToDictionary(reader.GetName, reader.GetValue);
                //Добавление словаря в список
                list_of_values.Add(values);
            }
            //Закрыть конект
            connect.Close();
            //Возврат данных
            return list_of_values;
        }
        /// <summary>
        /// Возвращает строку, конвертированную под строку даты для MySQL
        /// </summary>
        /// <param name="date">Дата</param>
        /// <returns>Готовая строка</returns>
        public static string ConvertDateToMySqlString(DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm:ss");
        }
        /// <summary>
        /// Конвертирует булевскую переменную в инт. True = 1 False = 0
        /// </summary>
        /// <param name="param">Тру или фолс</param>
        /// <returns>1 или 0</returns>
        public static int ConvertBoolToInt(bool param)
        {
            if(param)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
