using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTBaseNew.Cities
{
    public class City
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
        /// ИД области
        /// </summary>
        public int Region_id { set; get; }
        /// <summary>
        /// Сохранить объект
        /// </summary>
        public void Save()
        {
            //Строка-запрос
            string query = string.Format("SELECT * FROM `Cities` WHERE id={0}", Id);
            //База данных
            MySqlWorker.DataBase db = SQL.SqlConnect.db;
            //Создает запрос и возвращает результат
            var list = db.MakeRequest(query);
            //Если есть элементы в списке, то такая должность есть в базе
            if (list.Count != 0)
            {
                //Строка-запрос
                query = string.Format("UPDATE Cities SET name='{0}',region_id={2} WHERE id={1}", Name, Id,Region_id);
                //Создает запрос и возвращает результат
                db.MakeRequest(query);
            }
            //В другом случае создать новую запись, и присвоить должности ID
            else
            {
                //Строка-запрос
                query = string.Format("INSERT INTO `Cities`(`name`) VALUES ('{0}',{1}); SELECT * FROM `Cities` order by id desc;LIMIT 0 , 1;", Name, Region_id);
                //Создает запрос и возвращает результат
                list = db.MakeRequest(query);
                //Присвоить id
                Id = (int)(list[0])["id"];
            }
        }
        /// <summary>
        /// Ищет город по ИД
        /// </summary>
        /// <param name="id">ИД</param>
        /// <returns>Город</returns>
        public static City FindById(int id)
        {
            //Запрос
            string query = string.Format("SELECT * FROM `Cities` WHERE id={0}", id);
            //База данных
            MySqlWorker.DataBase db = SQL.SqlConnect.db;
            //Создает запрос и возвращает результат
            var list = db.MakeRequest<City>(query);
            //Если ничего не нашло, то возвращает ноль
            if (list.Count == 0)
            {
                return null;
            }
            //Если нашло, то создает объект пасспорта
            else
            {
                //создает объект паспорта
                City address = list[0];
                //Возвращает должность
                return address;
            }
        }
        /// <summary>
        /// Ищет город по названию
        /// </summary>
        /// <param name="name">Название</param>
        /// <returns>Город</returns>
        public static City FindByName(string name)
        {
            //Запрос
            string query = string.Format("SELECT * FROM `Cities` WHERE name='{0}'", name);
            //База данных
            MySqlWorker.DataBase db = SQL.SqlConnect.db;
            //Создает запрос и возвращает результат
            var list = db.MakeRequest<City>(query);
            //Если ничего не нашло, то возвращает ноль
            if (list.Count == 0)
            {
                return null;
            }
            //Если нашло, то создает объект пасспорта
            else
            {
                //создает объект паспорта
                City address = list[0];
                //Возвращает должность
                return address;
            }
        }
        /// <summary>
        /// Удаляет город из базы
        /// </summary>
        public void Delete()
        {
            if (Id != 0)
            {
                //База данных
                MySqlWorker.DataBase db = SQL.SqlConnect.db;

                //Удаляет город со всех адресов
                foreach (var i in GetAddresses)
                {
                    i.city_id = 0;
                    i.Save();
                }

                //Строка-запрос
                string query = string.Format("DELETE FROM  `Cities` WHERE  `id` = {0}", Id);
                //Создает запрос и возвращает результат
                db.MakeRequest(query);
            }
        }
        /// <summary>
        /// Область
        /// </summary>
        public Regions.Region GetRegion
        {
            set
            {
                Region_id = value.Id;
            }
            get
            {
                return Regions.Region.FindById(Region_id);
            }
        }
        public List<Addresses.Address> GetAddresses
        {
            get
            {
                //Запрос
                string query = string.Format("SELECT * FROM `Addressess` WHERE city_id='{0}'", Id);
                //База данных
                MySqlWorker.DataBase db = SQL.SqlConnect.db;
                //Создает запрос и возвращает результат
                var list = db.MakeRequest<Addresses.Address>(query);
                return list;
            }
        }
        /// <summary>
        /// Возвращает все города
        /// </summary>
        static public List<City> GetAllCities
        {
            private set { }
            get
            {
                string query = string.Format(" select * from Cities");
                //База данных
                MySqlWorker.DataBase db = SQL.SqlConnect.db;
                //Создает запрос и возвращает результат
                return db.MakeRequest<City>(query);

            }
        }
    }
}
