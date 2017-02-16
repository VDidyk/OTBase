using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTBaseNew.Actions
{
    public class Action
    {
        public int Id { set; get; }
        /// <summary>
        /// Название
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime Created { private set; get; }
        /// <summary>
        /// ID запроса
        /// </summary>
        public int Request_id { set; get; }
        /// <summary>
        /// ID пользователя
        /// </summary>
        public int User_id { set; get; }
        public Action()
        {
            Created = DateTime.Now;
        }
        public void Save()
        {
            //Строка-запрос
            string query = string.Format("SELECT * FROM `Actions` WHERE id={0}", Id);
            //База данных
            MySqlWorker.DataBase db = SQL.SqlConnect.db;
            //Создает запрос и возвращает результат
            var list = db.MakeRequest(query);
            //Если есть элементы в списке, то такая должность есть в базе
            if (list.Count != 0)
            {
                //Строка-запрос
                query = string.Format("UPDATE Actions SET name='{0}',request_id='{1}' User_id='{2}' WHERE id={3}", Name, Request_id.ToString(), User_id.ToString(), Id);
                //Создает запрос и возвращает результат
                db.MakeRequest(query);
            }
            //В другом случае создать новую запись, и присвоить должности ID
            else
            {
                //Строка-запрос
                query = string.Format("INSERT INTO `Actions`(`name`, `created`,`request_id`,`User_id`) VALUES ('{0}','{1}','{2}','{3}'); SELECT * FROM `Actions` order by id desc;LIMIT 0 , 1;", Name, MySqlWorker.DataBase.ConvertDateToMySqlString(Created), Request_id.ToString(), User_id.ToString());
                //Создает запрос и возвращает результат
                list = db.MakeRequest(query);
                //Присвоить id
                Id = (int)(list[0])["id"];
            }
            //Операция закончена, возвращает тру
        }
        public static Action FindById(int id)
        {
            //Запрос
            string query = string.Format("SELECT * FROM `Actions` WHERE id={0}", id);
            //База данных
            MySqlWorker.DataBase db = SQL.SqlConnect.db;
            //Создает запрос и возвращает результат
            var list = db.MakeRequest<Action>(query);
            //Если ничего не нашло, то возвращает ноль
            if (list.Count == 0)
            {
                return null;
            }
            //Если нашло, то создает объект должности
            else
            {
                //создает объект должности
                Action position = list[0];
                return position;
            }
        }
        public void Delete()
        {
            if (Id != 0)
            {
                //База данных
                MySqlWorker.DataBase db = SQL.SqlConnect.db;
                //Строка-запрос
                string query = string.Format("DELETE FROM  `Actions` WHERE  `id` = {0}", Id);
                //Создает запрос и возвращает результат
                db.MakeRequest(query);
            }
        }


    }
}
