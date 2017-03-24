using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTBaseNew.Statuses
{
    public class Status
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { set; get; }
        /// <summary>
        /// Имя
        /// </summary>
        public string Name { set; get; }

        public bool main { set; get; }

        /// <summary>
        /// Поиск статуса по ИД
        /// </summary>
        /// <param name="id">ИД</param>
        /// <returns>Статус</returns>
        public static Status FindById(int id)
        {
            //Запрос
            string query = string.Format("SELECT * FROM `Statuses` WHERE id={0}", id);
            //База данных
            MySqlWorker.DataBase db = SQL.SqlConnect.db;
            //Создает запрос и возвращает результат
            var list = db.MakeRequest<Status>(query);
            //Если ничего не нашло, то возвращает ноль
            if (list.Count == 0)
            {
                return null;
            }
            //Если нашло, то создает объект должности
            else
            {
                //создает объект должности
                Status position = list[0];
                return position;
            }
        }
        /// <summary>
        /// Запросы со статусом
        /// </summary>
        public List<Requests.Request> GetRequests
        {
            private set { ;}
            get
            {
                List<Requests.Request> requests = new List<Requests.Request>();
                string query = string.Format("SELECT * FROM `Request` WHERE Status_id={0}", Id);
                //База данных
                MySqlWorker.DataBase db = SQL.SqlConnect.db;
                var list = db.MakeRequest<Requests.Request>(query);
                return list;
            }
        }
        /// <summary>
        /// Ищет статус по названию
        /// </summary>
        /// <param name="name">Название</param>
        /// <returns>Статус</returns>
        public static Status FindByName(string name)
        {
            //Запрос
            string query = string.Format("SELECT * FROM `Statuses` WHERE name='{0}'", name);
            //База данных
            MySqlWorker.DataBase db = SQL.SqlConnect.db;
            //Создает запрос и возвращает результат
            var list = db.MakeRequest<Status>(query);
            //Если ничего не нашло, то возвращает ноль
            if (list.Count == 0)
            {
                return null;
            }
            //Если нашло, то создает объект должности
            else
            {
                //создает объект должности
                Status position = list[0];
                return position;
            }
        }

        public static Status GetMainStatus()
        {
            string query = string.Format("SELECT * FROM `Statuses` WHERE main=1");
            //База данных
            MySqlWorker.DataBase db = SQL.SqlConnect.db;
            var list = db.MakeRequest<Status>(query);
            if(list.Count>0)
            {
                return list[0];
            }
            else
            {
                return null;
            }
        }
    }
}
