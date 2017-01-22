using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTBaseNew.Phones
{
    public class Phone
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { set; get; }
        /// <summary>
        /// Номер телефона
        /// </summary>
        public string number { set; get; }      
        /// <summary>
        /// Список владельцев
        /// </summary>
        public List<int> Users_Ides { set; get; }
        public Phone()
        {
            //Новый список
            Users_Ides = new List<int>();
        }
        /// <summary>
        /// Сохраняет телефон в базу
        /// </summary>
        public void Save()
        {
            //Строка-запрос
            string query = string.Format("SELECT * FROM `Phones` WHERE id={0}", Id);
            //База данных
            MySqlWorker.DataBase db = SQL.SqlConnect.db;
            //Создает запрос и возвращает результат
            var list = db.MakeRequest<Phone>(query);
            //Если есть элементы в списке, то такая должность есть в базе
            if (list.Count != 0)
            {
                //Строка-запрос
                query = string.Format("UPDATE Phones SET `number`='{0}' WHERE id={1}", number, Id);
                //Создает запрос и возвращает результат
                db.MakeRequest(query);
                #region Работа с пользователями
                //Прогон по идентификаторам пользователей
                foreach (var i in Users_Ides)
                {
                    //Строка-запрос
                    query = string.Format("SELECT * FROM `PhonesAndUsers` WHERE Phone_Id={0} && User_Id={1}", Id.ToString(), i.ToString());
                    //Ответ или есть уже такая запись
                    var answer = db.MakeRequest(query);
                    //Если такой записи нету, то дабавляет
                    if (answer.Count == 0)
                    {
                        //Строка-запрос
                        query = string.Format("INSERT INTO `PhonesAndUsers`(`Phone_id`, `User_id`) VALUES ({0},{1})", Id.ToString(), i.ToString());
                        //Запуск запроса
                        db.MakeRequest(query);
                    }
                }
                //Строка-запрос
                query = string.Format("Select * from `PhonesAndUsers` where Phone_id={0}", Id);
                //Ищет всех пользователей которые имеют это номер
                var asnwer1 = db.MakeRequest(query);
                //Прогон по пользователях
                foreach (var i in asnwer1)
                {
                    //Есть ли телефон в списке
                    bool exist = false;
                    //Прогон по айди
                    foreach (var j in Users_Ides)
                    {
                        //Если телефон сущесвтует, то его удалять не надо
                        if (Convert.ToInt32(i["User_id"]) == j)
                        {
                            //Телефон существует
                            exist = true;
                            break;
                        }
                    }
                    //Если не сущесвует
                    if (!exist)
                    {
                        //Срока-запрос
                        query = string.Format("Delete from `PhonesAndUsers` where Id={0}", Convert.ToInt32(i["Id"]));
                        //Удаляем ее из базы
                        db.MakeRequest(query);
                    }
                }
                #endregion
            }
            //В другом случае создать новую запись, и присвоить должности ID
            else
            {
                //Строка-запрос
                query = string.Format("INSERT INTO `Phones`(`Number`) VALUES ('{0}'); SELECT * FROM `Phones` order by id desc;", number);
                //Создает запрос и возвращает результат
                var answer = db.MakeRequest(query);
                //Присвоить id
                Id = (int)(answer[0])["Id"];
                #region Работа с пользователями

                //Прогон по идентификаторам пользователей
                foreach (var i in Users_Ides)
                {
                    //Строка-запрос
                    query = string.Format("INSERT INTO `PhonesAndUsers`(`Phone_id`, `User_id`) VALUES ({0},{1})", Id.ToString(), i.ToString());
                    //Запуск запроса
                    db.MakeRequest(query);
                }
                #endregion
            }
        }
        /// <summary>
        /// Удаляет телефон из базы
        /// </summary>
        public void Delete()
        {
            if (Id != 0)
            {
                //База данных
                MySqlWorker.DataBase db = SQL.SqlConnect.db;
                //Строка-запрос для удаления из таблицы Телефон-Пользователь
                string query = string.Format("DELETE FROM  `PhonesAndUsers` WHERE  `Phone_id` = {0}", Id);
                //Создает запрос и возвращает результат
                db.MakeRequest(query);
                //Строка-запрос
                 query = string.Format("DELETE FROM  `Phones` WHERE  `id` = {0}", Id);
                //Создает запрос и возвращает результат
                db.MakeRequest(query);
            }
        }
        /// <summary>
        /// Ищет телефон по ИД
        /// </summary>
        /// <param name="id">ИД</param>
        /// <returns>Телефон</returns>
        public static Phone FindById(int id)
        {
            //Запрос
            string query = string.Format("SELECT * FROM `Phones` WHERE id={0}", id);
            //База данных
            MySqlWorker.DataBase db = SQL.SqlConnect.db;
            //Создает запрос и возвращает результат
            var list = db.MakeRequest<Phone>(query);
            //Если ничего не нашло, то возвращает ноль
            if (list.Count == 0)
            {
                return null;
            }
            //Если нашло, то создает объект должности
            else
            {
                //Создаю телефон
                Phone us = list[0];
                //Строка-запрос на получение пользователя
                query = string.Format("SELECT * FROM `PhonesAndUsers` WHERE Phone_id={0}", id);
                //Выполнение запроса
                var answer = db.MakeRequest(query);
                //Прогон по пользователях
                foreach (var i in answer)
                {
                    //Добавление пользователя в список
                    us.Users_Ides.Add(Convert.ToInt32(i["User_id"]));
                }
                //Возвращает телефон
                return us;
            }
        }
        public static Phone FindByNumber(string number)
        {
            //Запрос
            string query = string.Format("SELECT * FROM `Phones` WHERE number={0}", number);
            //База данных
            MySqlWorker.DataBase db = SQL.SqlConnect.db;
            //Создает запрос и возвращает результат
            var list = db.MakeRequest<Phone>(query);
            //Если ничего не нашло, то возвращает ноль
            if (list.Count == 0)
            {
                return null;
            }
            //Если нашло, то создает объект должности
            else
            {
                //Создаю телефон
                Phone us = list[0];
                //Строка-запрос на получение пользователя
                query = string.Format("SELECT * FROM `PhonesAndUsers` WHERE Phone_id={0}", us.Id.ToString());
                //Выполнение запроса
                var answer = db.MakeRequest(query);
                //Прогон по пользователях
                foreach (var i in answer)
                {
                    //Добавление пользователя в список
                    us.Users_Ides.Add(Convert.ToInt32(i["User_id"]));
                }
                //Возвращает телефон
                return us;
            }
        }
        /// <summary>
        /// Владельцы телефона
        /// </summary>
        public List<Users.User> GetUsers
        {
            set
            {
                Users_Ides.Clear();
                foreach (var i in value)
                {
                    Users_Ides.Add(i.Id);
                }
            }
            get
            {
                List<Users.User> users = new List<Users.User>();
                foreach (var i in Users_Ides)
                {
                    users.Add(Users.User.FindById(i));
                }
                return users;
            }
        }
  
    }
}
