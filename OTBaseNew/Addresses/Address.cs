using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTBaseNew.Addresses
{
    /// <summary>
    /// Адресс
    /// </summary>
    public class Address
    {
        /// <summary>
        /// ID
        /// </summary>
        public int id { set; get; }
        /// <summary>
        /// Город
        /// </summary>
        public int city_id { set; get; }
        /// <summary>
        /// Улица и дома
        /// </summary>
        public string address { set; get; }
        /// <summary>
        /// Сохранить объект
        /// </summary>
        public void Save()
        {
            //Строка-запрос
            string query = string.Format("SELECT * FROM `Addressess` WHERE id={0}", id);
            //База данных
            MySqlWorker.DataBase db = SQL.SqlConnect.db;
            //Создает запрос и возвращает результат
            var list = db.MakeRequest(query);
            //Если есть элементы в списке, то такая должность есть в базе
            if (list.Count != 0)
            {
                //Строка-запрос
                query = string.Format("UPDATE Addressess SET city_id='{0}', address='{1}' WHERE id={2}",city_id.ToString(),address , id);
                //Создает запрос и возвращает результат
                db.MakeRequest(query);
            }
            //В другом случае создать новую запись, и присвоить должности ID
            else
            {
                //Строка-запрос
                query = string.Format("INSERT INTO `Addressess`(`city_id`, `address`) VALUES ('{0}','{1}'); SELECT * FROM `Addressess` order by id desc;", city_id.ToString(),address);
                //Создает запрос и возвращает результат
                list = db.MakeRequest(query);
                //Присвоить id
                id = (int)(list[0])["id"];
            }
            //Операция закончена, возвращает тру
        }
        /// <summary>
        /// Ищет адресс по ИД
        /// </summary>
        /// <param name="id">ИД</param>
        /// <returns>Адресс</returns>
        public static Address FindById(int id)
        {
            //Запрос
            string query = string.Format("SELECT * FROM `Addressess` WHERE id={0}", id);
            //База данных
            MySqlWorker.DataBase db = SQL.SqlConnect.db;
            //Создает запрос и возвращает результат
            var list = db.MakeRequest<Address>(query);
            //Если ничего не нашло, то возвращает ноль
            if (list.Count == 0)
            {
                return null;
            }
            //Если нашло, то создает объект пасспорта
            else
            {
                //создает объект паспорта
                Address address = list[0];
                //Возвращает должность
                return address;
            }
        }
        /// <summary>
        /// Удаляет адресс с базы данных
        /// </summary>
        public void Delete()
        {
            if (id != 0)
            {
                //База данных
                MySqlWorker.DataBase db = SQL.SqlConnect.db;
                //Строка-запрос
                string query = string.Format("DELETE FROM  `Addressess` WHERE  `id` = {0}", id);
                //Создает запрос и возвращает результат
                db.MakeRequest(query);
            }
        }
        public Cities.City GetCity
        {
            set
            {
               city_id = value.Id;
            }
            get
            {
                return Cities.City.FindById(city_id);
            }
        }
        /// <summary>
        /// Владельцы аддресса
        /// </summary>
        public List<Clients.Client> ClientOwners
        {
            private set { }
            get
            {
                string query = string.Format(" select * from Clients where Clients.address_id = {0}", id);
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
