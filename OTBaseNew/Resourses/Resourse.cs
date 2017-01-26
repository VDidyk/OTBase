using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTBaseNew.Resourses
{
    public class Resourse
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { set; get; }
        /// <summary>
        /// Название
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime Created { set; get; }
        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public Resourse()
        {
            Created = DateTime.Now;
        }
        /// <summary>
        /// Сохраняет ресурс в базе данных
        /// </summary>
        public void Save()
        {
            //Строка-запрос
            string query = string.Format("SELECT * FROM `Resourses` WHERE id={0}", Id);
            //База данных
            MySqlWorker.DataBase db = SQL.SqlConnect.db;
            //Создает запрос и возвращает результат
            var list = db.MakeRequest(query);
            //Если есть элементы в списке, то такая должность есть в базе
            if (list.Count != 0)
            {
                //Строка-запрос
                query = string.Format("UPDATE Resourses SET name='{0}' WHERE id={1}", Name, Id);
                //Создает запрос и возвращает результат
                db.MakeRequest(query);
            }
            //В другом случае создать новую запись, и присвоить должности ID
            else
            {
                //Строка-запрос
                query = string.Format("INSERT INTO `Resourses`(`name`, `created`) VALUES ('{0}','{1}'); SELECT * FROM `Positions` order by id desc;", Name, MySqlWorker.DataBase.ConvertDateToMySqlString(DateTime.Now));
                //Создает запрос и возвращает результат
                list = db.MakeRequest(query);
                //Присвоить id
                Id = (int)(list[0])["id"];
            }
            //Операция закончена, возвращает тру
        }
        /// <summary>
        /// Удаляет ресурс с базы данных
        /// </summary>
        public void Delete()
        {
            if (Id != 0)
            {
                //База данных
                MySqlWorker.DataBase db = SQL.SqlConnect.db;
                //Строка-запрос
                string query = string.Format("DELETE FROM  `Resourses` WHERE  `id` = {0}", Id);
                //Создает запрос и возвращает результат
                db.MakeRequest(query);
            }
        }
        /// <summary>
        /// Ищет должность по ID и возвращает ее.Если ничего не нашло, то возвращает ноль
        /// </summary>
        /// <param name="id">ИД</param>
        /// <returns>Должнсть</returns>
        public static Resourse FindById(int id)
        {
            //Запрос
            string query = string.Format("SELECT * FROM `Resourses` WHERE id={0}", id);
            //База данных
            MySqlWorker.DataBase db = SQL.SqlConnect.db;
            //Создает запрос и возвращает результат
            var list = db.MakeRequest<Resourse>(query);
            //Если ничего не нашло, то возвращает ноль
            if (list.Count == 0)
            {
                return null;
            }
            //Если нашло, то создает объект должности
            else
            {
                //создает объект должности
                Resourse position = list[0];
                return position;
            }
        }

        /// <summary>
        /// Клиенты
        /// </summary>
        public List<Clients.Client> GetClients
        {
            private set { }
            get
            {
                string query = string.Format(" select * from Clients where Clients.resourse_id = {0}", Id);
                //База данных
                MySqlWorker.DataBase db = SQL.SqlConnect.db;
                //Создает запрос и возвращает результат
                var list = db.MakeRequest(query);
                //Список, в который будут добавяться пользователи
                List<Clients.Client> clients = new List<Clients.Client>();
                //Прогон по результату с запроса
                foreach (var i in list)
                {
                    //Добавляем пользователя ищя его по ИД
                    clients.Add(Clients.Client.FindById(Convert.ToInt32(i["id"])));
                }
                //Возврат пользователей
                return clients;
            }
        }
    }
}
