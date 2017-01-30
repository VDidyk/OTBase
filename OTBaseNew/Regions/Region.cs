using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTBaseNew.Regions
{
    public class Region
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { set; get; }
        /// <summary>
        /// Имя города
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// Сохранить объект
        /// </summary>
        public void Save()
        {
            //Строка-запрос
            string query = string.Format("SELECT * FROM `Region` WHERE id={0}", Id);
            //База данных
            MySqlWorker.DataBase db = SQL.SqlConnect.db;
            //Создает запрос и возвращает результат
            var list = db.MakeRequest(query);
            //Если есть элементы в списке, то такая должность есть в базе
            if (list.Count != 0)
            {
                //Строка-запрос
                query = string.Format("UPDATE Region SET name='{0}' WHERE id={1}", Name, Id);
                //Создает запрос и возвращает результат
                db.MakeRequest(query);
            }
            //В другом случае создать новую запись, и присвоить должности ID
            else
            {
                //Строка-запрос
                query = string.Format("INSERT INTO `Region`(`name`) VALUES ('{0}'); SELECT * FROM `Region` order by id desc;", Name);
                //Создает запрос и возвращает результат
                list = db.MakeRequest(query);
                //Присвоить id
                Id = (int)(list[0])["id"];
            }
        }
        /// <summary>
        /// Ищет область по ИД
        /// </summary>
        /// <param name="id">ИД</param>
        /// <returns>Область</returns>
        public static Region FindById(int id)
        {
            //Запрос
            string query = string.Format("SELECT * FROM `Region` WHERE id={0}", id);
            //База данных
            MySqlWorker.DataBase db = SQL.SqlConnect.db;
            //Создает запрос и возвращает результат
            var list = db.MakeRequest<Region>(query);
            //Если ничего не нашло, то возвращает ноль
            if (list.Count == 0)
            {
                return null;
            }
            //Если нашло, то создает объект области
            else
            {
                //создает объект области
                Region address = list[0];
                //Возвращает область
                return address;
            }
        }
        /// <summary>
        /// Ищет город по названию
        /// </summary>
        /// <param name="name">Название</param>
        /// <returns>Город</returns>
        public static Region FindByName(string name)
        {
            //Запрос
            string query = string.Format("SELECT * FROM `Region` WHERE name='{0}'", name);
            //База данных
            MySqlWorker.DataBase db = SQL.SqlConnect.db;
            //Создает запрос и возвращает результат
            var list = db.MakeRequest<Region>(query);
            //Если ничего не нашло, то возвращает ноль
            if (list.Count == 0)
            {
                return null;
            }
            //Если нашло, то создает объект области
            else
            {
                //создает объект области
                Region address = list[0];
                //Возвращает Область
                return address;
            }
        }
        /// <summary>
        /// Удаляет область из базы
        /// </summary>
        public void Delete()
        {
            if (Id != 0)
            {
                //Удаляет область со всех городов
                foreach(var i in GetCities)
                {
                    i.Region_id = 0;
                    i.Save();
                }

                //База данных
                MySqlWorker.DataBase db = SQL.SqlConnect.db;
                //Строка-запрос
                string query = string.Format("DELETE FROM  `Region` WHERE  `id` = {0}", Id);
                //Создает запрос и возвращает результат
                db.MakeRequest(query);
            }
        }
        /// <summary>
        /// Все города области
        /// </summary>
        public List<Cities.City> GetCities
        {
            get
            {
                //Запрос
                string query = string.Format("SELECT * FROM `Cities` WHERE region_id='{0}'", Id);
                //База данных
                MySqlWorker.DataBase db = SQL.SqlConnect.db;
                //Создает запрос и возвращает результат
                var list = db.MakeRequest<Cities.City>(query);
                return list;
            }
        }
        /// <summary>
        /// Возвращает все области
        /// </summary>
        static public List<Region> GetAllRegions
        {
            private set { }
            get
            {
                string query = string.Format(" select * from Region");
                //База данных
                MySqlWorker.DataBase db = SQL.SqlConnect.db;
                //Создает запрос и возвращает результат
                return db.MakeRequest<Region>(query);                
            }
        }
    }
}
