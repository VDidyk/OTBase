using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MySql;

namespace OTBaseNew.Documents
{
    class Document
    {
        /// <summary>
        /// ИД
        /// </summary>
        public int Id { set; get; }
        /// <summary>
        /// Название
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// Расширение
        /// </summary>
        public string Extension { set; get; }
        /// <summary>
        /// Байты в строке
        /// </summary>
        public byte[] Bytes { set; get; }
        /// <summary>
        /// Ид оператора
        /// </summary>
        public int Operator_id { set; get; }
        /// <summary>
        /// Когда создан
        /// </summary>
        public DateTime Created { set; get; }
        public Document()
        {
            Created = DateTime.Now;
        }
        /// <summary>
        /// Сохраняет документ
        /// </summary>
        public void Save()
        {
            //Строка-запрос
            string query = string.Format("SELECT * FROM `Documents` WHERE id={0}", Id);
            //База данных
            MySqlWorker.DataBase db = SQL.SqlConnect.db;
            //Создает запрос и возвращает результат
            var list = db.MakeRequest(query);
            //Если есть элементы в списке, то такая должность есть в базе
            if (list.Count != 0)
            {
                //Строка-запрос
                query = string.Format("UPDATE Documents SET name='{0}', Extension='{1}',Bytes='@bytes',Operator_id={2},' WHERE id={3}", Name, Extension, Operator_id.ToString(), Id);
                MySql.Data.MySqlClient.MySqlCommand command = new MySql.Data.MySqlClient.MySqlCommand(query);
                command.Parameters.AddWithValue("@bytes", Bytes);
                //Создает запрос и возвращает результат
                db.MakeRequest(command);
            }
            //В другом случае создать новую запись, и присвоить должности ID
            else
            {
                //Строка-запрос
                query = string.Format("INSERT INTO `Documents`(`name`, `extension`,`bytes`,`operator_id`,created) VALUES ('{0}','{1}',@bytes,'{3}','{4}'); SELECT * FROM `Documents` order by id desc LIMIT 0 , 1;", Name, Extension, Bytes, Operator_id.ToString(), MySqlWorker.DataBase.ConvertDateToMySqlString(Created));
                MySql.Data.MySqlClient.MySqlCommand command = new MySql.Data.MySqlClient.MySqlCommand(query);
                command.Parameters.AddWithValue("@bytes", Bytes);
                //Создает запрос и возвращает результат
                list = db.MakeRequest(command);
                //Присвоить id
                Id = (int)(list[0])["id"];
            }
            //Операция закончена, возвращает тру
        }
        /// <summary>
        /// Удаляет документ
        /// </summary>
        public void Delete()
        {
            if (Id != 0)
            {
                //База данных
                MySqlWorker.DataBase db = SQL.SqlConnect.db;
                //Строка-запрос
                string query = string.Format("DELETE FROM  `Documents` WHERE  `id` = {0}", Id);
                //Создает запрос и возвращает результат
                db.MakeRequest(query);
            }
        }
        /// <summary>
        /// Ищет документ по ИД
        /// </summary>
        /// <param name="id">ИД</param>
        /// <returns>Документ</returns>
        public static Document FindById(int id)
        {
            //Запрос
            string query = string.Format("SELECT * FROM `Documents` WHERE id={0}", id);
            //База данных
            MySqlWorker.DataBase db = SQL.SqlConnect.db;
            //Создает запрос и возвращает результат
            var list = db.MakeRequest<Document>(query);
            //Если ничего не нашло, то возвращает ноль
            if (list.Count == 0)
            {
                return null;
            }
            //Если нашло, то создает объект должности
            else
            {
                //создает объект должности
                Document position = list[0];
                //Возвращает должность
                return position;
            }
        }
        /// <summary>
        /// Возвращает байты файла
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <returns>Миссив байтов</returns>
        public static byte[] GetBytesFromFile(string path)
        {
            return File.ReadAllBytes(path);
        }
        /// <summary>
        /// Возвращает расширение файла
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <returns>Расширение</returns>
        public static string GetExtensionFromFile(string path)
        {
            return Path.GetExtension(path);
        }
        /// <summary>
        /// Возвращает имя файла
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <returns>Имя</returns>
        public static string GetNameFromFile(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }
        /// <summary>
        /// Конвертирует массив байтов в строку
        /// </summary>
        /// <param name="array">Массив байтов</param>
        /// <returns>Строка</returns>
        public static string ConvertBytesToString(byte[] array)
        {
            return System.Text.Encoding.UTF8.GetString(array);
        }
        public string WriteDocument(string path)
        {
            string donepath=string.Format(@"{0}\{1}{2}", path, Document.GetNameFromFile(Name), Extension);
            File.WriteAllBytes(donepath, Bytes);
            return donepath;
        }
    }
}
