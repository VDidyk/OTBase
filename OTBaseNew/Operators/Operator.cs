using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTBaseNew.Operators
{
    public class Operator
    {
        /// <summary>
        /// ИД
        /// </summary>
        public int Id { set; get; }
        /// <summary>
        /// Имя
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// Сайт
        /// </summary>
        public string Site { set; get; }
        /// <summary>
        /// Документы
        /// </summary>
        public List<int> Documents_Ides { set; get; }
        /// <summary>
        /// Скидки
        /// </summary>
        public List<int> Discount_Ides { set; get; }
        public Operator()
        {
            Documents_Ides = new List<int>();
            Discount_Ides = new List<int>();
        }
        public void Save()
        {
            //Строка-запрос
            string query = string.Format("SELECT * FROM `Operators` WHERE id={0}", Id);
            //База данных
            MySqlWorker.DataBase db = SQL.SqlConnect.db;
            //Создает запрос и возвращает результат
            var list = db.MakeRequest(query);
            //Если есть элементы в списке, то такая должность есть в базе
            if (list.Count != 0)
            {
                //Строка-запрос
                query = string.Format("UPDATE Operators SET name='{0}',site='{1}' WHERE id={2}", Name, Site, Id);
                //Создает запрос и возвращает результат
                db.MakeRequest(query);           
            }
            //В другом случае создать новую запись, и присвоить должности ID
            else
            {
                //Строка-запрос
                query = string.Format("INSERT INTO `Operators`(`name`,`site`) VALUES ('{0}','{1}'); SELECT * FROM `Operators` order by id desc LIMIT 0 , 1;", Name, Site);
                //Создает запрос и возвращает результат
                list = db.MakeRequest(query);
                //Присвоить id
                Id = (int)(list[0])["id"];
            }
            //Операция закончена, возвращает тру
        }
        public static Operator FindById(int id)
        {
            //Запрос
            string query = string.Format("SELECT * FROM `Operators` WHERE id={0}", id);
            //База данных
            MySqlWorker.DataBase db = SQL.SqlConnect.db;
            //Создает запрос и возвращает результат
            var list = db.MakeRequest<Operator>(query);
            //Если ничего не нашло, то возвращает ноль
            if (list.Count == 0)
            {
                return null;
            }
            //Если нашло, то создает объект должности
            else
            {
                //создает объект должности
                Operator position = list[0];
                return position;
            }
        }
        public static Operator FindByName(string name)
        {
            //Запрос
            string query = string.Format("SELECT * FROM `Operators` WHERE Name='{0}'", name);
            //База данных
            MySqlWorker.DataBase db = SQL.SqlConnect.db;
            //Создает запрос и возвращает результат
            var list = db.MakeRequest<Operator>(query);
            //Если ничего не нашло, то возвращает ноль
            if (list.Count == 0)
            {
                return null;
            }
            //Если нашло, то создает объект должности
            else
            {
                //создает объект должности
                Operator position = list[0];
                return position;
            }
        }
        /// <summary>
        /// Заявки по операторах
        /// </summary>
        public List<Requests.Request> GetRequests
        {

            private set { ;}
            get
            {
                List<Requests.Request> requests = new List<Requests.Request>();
                string query = string.Format("SELECT * FROM `Request` WHERE Operator_id={0}", Id);
                //База данных
                MySqlWorker.DataBase db = SQL.SqlConnect.db;
                var list = db.MakeRequest<Requests.Request>(query);
                return list;
            }
        }
        static public List<Operator> GetAllOperators
        {
            private set { }
            get
            {
                string query = string.Format(" select * from Operators");
                //База данных
                MySqlWorker.DataBase db = SQL.SqlConnect.db;
                //Создает запрос и возвращает результат
                return db.MakeRequest<Operator>(query);

            }
        }
        public List<Documents.Document> GetDocuments()
        {
            //Запрос
            string query = string.Format("SELECT * FROM `Documents` WHERE operator_id={0}", Id);
            //База данных
            MySqlWorker.DataBase db = SQL.SqlConnect.db;
            //Создает запрос и возвращает результат
            return db.MakeRequest<Documents.Document>(query);
        }
        public void Delete()
        {
            var docs = GetDocuments();
            foreach(var i in docs)
            {
                i.Delete();
            }
            string query = string.Format("Delete FROM `Operators` WHERE id={0}", Id);
            //База данных
            MySqlWorker.DataBase db = SQL.SqlConnect.db;
            //Создает запрос и возвращает результат
            var list = db.MakeRequest(query);
        }
    }
}
