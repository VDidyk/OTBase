using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Reflection;
namespace MySqlWorker
{
    public class DataBase
    {
        /// <summary>
        /// Конект к БД
        /// </summary>
        private MySqlConnection connect = new MySqlConnection();
        /// <summary>
        /// Конструктор, который инициализирует подключение к базе данных
        /// </summary>
        /// <param name="connectionstring">Строка подключения</param>
        public DataBase(string connectionstring)
        {
            //Строка конект к БД
            string myConnectionString = connectionstring;
            connect.ConnectionString = myConnectionString;
            connect.Close();
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
        /// Создание запроса к БД. Возвращает список, который содержит в себе словари, которые в свою очередь содержат ключи и значения запроса
        /// </summary>
        /// <param name="recieved_command">Команда</param>
        /// <returns>Значения, возвращаемые запросом</returns>
        public List<Dictionary<string, object>> MakeRequest(MySqlCommand recieved_command)
        {
            //Открыть конект
            connect.Open();
            List<Dictionary<string, object>> list_of_values = new List<Dictionary<string, object>>();
            //Команда для создания запроса
            MySqlCommand command = recieved_command;
            command.Connection = connect;
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
        /// Метод, котрый принимает запрос и класс, получает данные с БД, обрабатывает, получает ответ, формирует объект переданного класса с полученных данных, и возвращает список с этими объектами 
        /// </summary>
        /// <typeparam name="T">Класс, с которого будет создаваться объект</typeparam>
        /// <param name="request">Строка запроса</param>
        /// <returns>Список объектов</returns>
        public List<T> MakeRequest<T>(string request) where T : new()
        {
            //На всякий случай закрывает конект
            connect.Close();
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
            //Тип переданого объекта
            Type myType = typeof(T);
            //Поля, которые существуют в объекте
            var members = myType.GetProperties();
            //Список объектов переданого класса
            List<T> list = new List<T>();
            //Прогон по всех словарях
            foreach (var j in list_of_values)
            {
                //Создание объекта переданого класса
                T ob = new T();
                //Прогон по элементах словаря
                foreach (var z in j)
                {
                    //Прогон по свойствах сущесвующих в переданном классе
                    foreach (var i in members)
                    {
                        try
                        {

                            //Название поля переданого класса, приведенного в нижний регистр
                            string datafield = i.Name.ToLower();
                            //Название ключа с бд, приведенного в нижний регистр
                            string Key = z.Key.ToLower();
                            //Если ключь словаря одинаковый с названием переменной с переданого класса, то мы входим
                            if (Key == datafield)
                            {
                                //Получаем поле объекта 
                                PropertyInfo propertyInfo = ob.GetType().GetProperty(i.Name);
                                //Если тип поля равен дате
                                if (propertyInfo.PropertyType.FullName == "System.DateTime")
                                {
                                    //Конвертируем строку в дату, и присваиваем
                                    propertyInfo.SetValue(ob, Convert.ToDateTime(z.Value));
                                }
                                //В другом случае
                                else
                                {
                                    //Присваиваем данные
                                    propertyInfo.SetValue(ob, z.Value);
                                }
                                break;
                            }

                        }
                        catch
                        {

                        }
                    }
                }
                list.Add(ob);
            }
            //Возврат данных
            return list;
        }

        /// <summary>
        /// Метод, котрый принимает запрос и класс, получает данные с БД, обрабатывает, получает ответ, формирует объект переданного класса с полученных данных, и возвращает список с этими объектами 
        /// </summary>
        /// <typeparam name="T">Класс, с которого будет создаваться объект</typeparam>
        /// <param name="recieved_command">Команда</param>
        /// <returns>Список объектов</returns>
        public List<T> MakeRequest<T>(MySqlCommand recieved_command) where T : new()
        {
            //На всякий случай закрывает конект
            connect.Close();
            //Открыть конект
            connect.Open();
            List<Dictionary<string, object>> list_of_values = new List<Dictionary<string, object>>();
            //Команда для создания запроса
            MySqlCommand command = recieved_command;
            command.Connection = connect;
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
            //Тип переданого объекта
            Type myType = typeof(T);
            //Поля, которые существуют в объекте
            var members = myType.GetProperties();
            //Список объектов переданого класса
            List<T> list = new List<T>();
            //Прогон по всех словарях
            foreach (var j in list_of_values)
            {
                //Создание объекта переданого класса
                T ob = new T();
                //Прогон по элементах словаря
                foreach (var z in j)
                {
                    //Прогон по свойствах сущесвующих в переданном классе
                    foreach (var i in members)
                    {
                        try
                        {

                            //Название поля переданого класса, приведенного в нижний регистр
                            string datafield = i.Name.ToLower();
                            //Название ключа с бд, приведенного в нижний регистр
                            string Key = z.Key.ToLower();
                            //Если ключь словаря одинаковый с названием переменной с переданого класса, то мы входим
                            if (Key == datafield)
                            {
                                //Получаем поле объекта 
                                PropertyInfo propertyInfo = ob.GetType().GetProperty(i.Name);
                                //Если тип поля равен дате
                                if (propertyInfo.PropertyType.FullName == "System.DateTime")
                                {
                                    //Конвертируем строку в дату, и присваиваем
                                    propertyInfo.SetValue(ob, Convert.ToDateTime(z.Value));
                                }
                                //В другом случае
                                else
                                {
                                    //Присваиваем данные
                                    propertyInfo.SetValue(ob, z.Value);
                                }
                                break;
                            }

                        }
                        catch
                        {

                        }
                    }
                }
                list.Add(ob);
            }
            //Возврат данных
            return list;
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
            if (param)
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
