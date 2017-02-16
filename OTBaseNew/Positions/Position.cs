using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlWorker;
namespace OTBaseNew.Positions
{
    /// <summary>
    /// Класс Должность
    /// </summary>
    public class Position
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
        public DateTime Created {private set; get; }
        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public Position()
        {
            Created = DateTime.Now;
        }
        /// <summary>
        /// Сохраняет клас в БД
        /// </summary>
        public void Save()
        {
            //Строка-запрос
            string query = string.Format("SELECT * FROM `Positions` WHERE id={0}", Id);
            //База данных
            MySqlWorker.DataBase db = SQL.SqlConnect.db;
            //Создает запрос и возвращает результат
            var list = db.MakeRequest(query);
            //Если есть элементы в списке, то такая должность есть в базе
            if (list.Count != 0)
            {
                //Строка-запрос
                query = string.Format("UPDATE Positions SET name='{0}' WHERE id={1}", Name, Id);
                //Создает запрос и возвращает результат
                db.MakeRequest(query);
            }
            //В другом случае создать новую запись, и присвоить должности ID
            else
            {
                //Строка-запрос
                query = string.Format("INSERT INTO `Positions`(`name`, `created`) VALUES ('{0}','{1}'); SELECT * FROM `Positions` order by id desc;LIMIT 0 , 1;", Name, MySqlWorker.DataBase.ConvertDateToMySqlString(Created));
                //Создает запрос и возвращает результат
                list = db.MakeRequest(query);
                //Присвоить id
                Id = (int)(list[0])["id"];
            }
            //Операция закончена, возвращает тру
        }
        /// <summary>
        /// Удаляет должность с базы данных
        /// </summary>
        public void Delete()
        {
            if (Id != 0)
            {
                //База данных
                MySqlWorker.DataBase db = SQL.SqlConnect.db;
                //Прогон по работникам должности
                foreach(var i in UsersOwners)
                {
                    //Обнуляет должность
                    i.Position_id = 0;
                    //Дохраняет пользователя
                    i.Save();
                }
                //Строка-запрос
                string query = string.Format("DELETE FROM  `Positions` WHERE  `id` = {0}", Id);
                //Создает запрос и возвращает результат
                db.MakeRequest(query);
            }
        }
        /// <summary>
        ///Пользователи-владельцы
        /// </summary>
        public List<Users.User> UsersOwners
        {
            private set { }
            get
            {
                string query = string.Format(" select * from Users where Users.position_id = {0}", Id);
                //База данных
                MySqlWorker.DataBase db = SQL.SqlConnect.db;
                //Создает запрос и возвращает результат
                var list = db.MakeRequest(query);
                //Список, в который будут добавяться пользователи
                List<Users.User> users = new List<Users.User>();
                //Прогон по результату с запроса
                foreach (var i in list)
                {
                    //Добавляем пользователя ищя его по ИД
                    users.Add(Users.User.FindById(Convert.ToInt32(i["id"])));
                }
                //Возврат пользователей
                return users;
            }
        }
        /// <summary>
        /// Ищет должность по ID и возвращает ее.Если ничего не нашло, то возвращает ноль
        /// </summary>
        /// <param name="id">ИД</param>
        /// <returns>Должнсть</returns>
        public static Position FindById(int id)
        {
            //Запрос
            string query = string.Format("SELECT * FROM `Positions` WHERE id={0}", id);
            //База данных
            MySqlWorker.DataBase db = SQL.SqlConnect.db;
            //Создает запрос и возвращает результат
            var list = db.MakeRequest(query);
            //Если ничего не нашло, то возвращает ноль
            if (list.Count == 0)
            {
                return null;
            }
            //Если нашло, то создает объект должности
            else
            {
                //создает объект должности
                Position position = new Position();
                //Присваивает ИД
                position.Id = id;
                //Присваивает имя с запроса
                //Присваивает имя с запроса
                position.Name = (list[0])["name"].ToString();
                //Присваивает дату создания с запроса
                position.Created = Convert.ToDateTime((list[0])["created"].ToString());
                //Возвращает должность
                return position;
            }
        }
        public static Position FindByName(string name)
        {
            //Запрос
            string query = string.Format("SELECT * FROM `Positions` WHERE name='{0}'", name);
            //База данных
            MySqlWorker.DataBase db = SQL.SqlConnect.db;
            //Создает запрос и возвращает результат
            var list = db.MakeRequest(query);
            //Если ничего не нашло, то возвращает ноль
            if (list.Count == 0)
            {
                return null;
            }
            //Если нашло, то создает объект должности
            else
            {
                //создает объект должности
                Position position = new Position();
                //Присваивает ИД
                position.Id = Convert.ToInt32((list[0])["id"].ToString());
                //Присваивает имя с запроса
                //Присваивает имя с запроса
                position.Name = (list[0])["name"].ToString();
                //Присваивает дату создания с запроса
                position.Created = Convert.ToDateTime((list[0])["created"].ToString());
                //Возвращает должность
                return position;
            }
        }
        public static List<Position> GetAllPositions
        {
            private set { }
            get
            {
                string query = string.Format(" select * from Positions");
                //База данных
                MySqlWorker.DataBase db = SQL.SqlConnect.db;
                //Создает запрос и возвращает результат
                List<Position> users = db.MakeRequest<Position>(query);
                return users;
            }
        }
    }
}
