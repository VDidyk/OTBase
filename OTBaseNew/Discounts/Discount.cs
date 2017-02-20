using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace OTBaseNew.Discounts
{
    public class Discount
    {
        /// <summary>
        /// ИД
        /// </summary>
        public int Id { set; get; }
        /// <summary>
        /// Тур
        /// </summary>
        public string Tour { set; get; }
        /// <summary>
        /// Скидка
        /// </summary>
        public string discount { set; get; }
        /// <summary>
        /// Клиента ИД
        /// </summary>
        public int Client_id { set; get; }
        /// <summary>
        /// ID оператора
        /// </summary>
        public int Operator_id { set; get; }
        /// <summary>
        /// Поиск по ИД
        /// </summary>
        /// <param name="Id">ИД</param>
        /// <returns>Скидка</returns>
        public static Discount FindById(int Id)
        {
            string query = string.Format("SELECT * FROM `Discounts` WHERE id={0}", Id);
            //База данных
            MySqlWorker.DataBase db = SQL.SqlConnect.db;
            //Создает запрос и возвращает результат
            var list = db.MakeRequest<Discount>(query);
            if (list.Count != 0)
                return list[0];
            else
            {
                return null;
            }
        }
        public void Save()
        {
            //Строка-запрос
            string query = string.Format("SELECT * FROM `Discounts` WHERE id={0}", Id);
            //База данных
            MySqlWorker.DataBase db = SQL.SqlConnect.db;
            //Создает запрос и возвращает результат
            var list = db.MakeRequest(query);
            //Если есть элементы в списке, то такая должность есть в базе
            if (list.Count != 0)
            {
                //Строка-запрос
                query = string.Format("UPDATE Discounts SET Tour='{0}',Discount='{1}' Client_id='{2}', Operator_id WHERE id={4}",Tour,discount,Client_id.ToString(),Operator_id, Id);
                //Создает запрос и возвращает результат
                db.MakeRequest(query);
            }
            //В другом случае создать новую запись, и присвоить должности ID
            else
            {
                //Строка-запрос
                query = string.Format("INSERT INTO `Discounts`(`tour`, `discount`, `client_id`,`operator_id`) VALUES ('{0}','{1}','{2}','{3}'); SELECT * FROM `Discounts` order by id desc LIMIT 0 , 1;", Tour, discount, Client_id.ToString(), Operator_id.ToString());
                //Создает запрос и возвращает результат
                list = db.MakeRequest(query);
                //Присвоить id
                Id = (int)(list[0])["id"];
            }
        }
        public void Delete()
        {
            if (Id != 0)
            {
                //База данных
                MySqlWorker.DataBase db = SQL.SqlConnect.db;
                //Строка-запрос
                string query = string.Format("DELETE FROM  `Discounts` WHERE  `id` = {0}", Id);
                //Создает запрос и возвращает результат
                db.MakeRequest(query);
            }
        }
    }
}
