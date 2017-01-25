using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTBaseNew.Emails
{
    public class Email
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { set; get; }
        /// <summary>
        /// Номер телефона
        /// </summary>
        public string name { set; get; }      
        /// <summary>
        /// Список владельцев
        /// </summary>
        public List<int> Users_Ides { set; get; }
        /// <summary>
        /// Клиенты
        /// </summary>
        public List<int> Clients_Ides { set; get; }
        public Email()
        {
            //Новый список
            Users_Ides = new List<int>();
            Clients_Ides = new List<int>();
        }
        /// <summary>
        /// Сохраняет mail в базу
        /// </summary>
        public void Save()
        {
            //Строка-запрос
            string query = string.Format("SELECT * FROM `Emails` WHERE id={0}", Id);
            //База данных
            MySqlWorker.DataBase db = SQL.SqlConnect.db;
            //Создает запрос и возвращает результат
            var list = db.MakeRequest<Email>(query);
            //Если есть элементы в списке, то такая должность есть в базе
            if (list.Count != 0)
            {
                //Строка-запрос
                query = string.Format("UPDATE Emails SET `name`='{0}' WHERE id={1}", name, Id);
                //Создает запрос и возвращает результат
                db.MakeRequest(query);
                #region Работа с пользователями
                //Прогон по идентификаторам пользователей
                foreach (var i in Users_Ides)
                {
                    //Строка-запрос
                    query = string.Format("SELECT * FROM `EmailsAndUsers` WHERE Email_Id={0} && User_Id={1}", Id.ToString(), i.ToString());
                    //Ответ или есть уже такая запись
                    var answer = db.MakeRequest(query);
                    //Если такой записи нету, то дабавляет
                    if (answer.Count == 0)
                    {
                        //Строка-запрос
                        query = string.Format("INSERT INTO `EmailsAndUsers`(`Email_Id`, `User_id`) VALUES ({0},{1})", Id.ToString(), i.ToString());
                        //Запуск запроса
                        db.MakeRequest(query);
                    }
                }
                //Строка-запрос
                query = string.Format("Select * from `EmailsAndUsers` where Email_id={0}", Id);
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
                        query = string.Format("Delete from `EmailsAndUsers` where Id={0}", Convert.ToInt32(i["Id"]));
                        //Удаляем ее из базы
                        db.MakeRequest(query);
                    }
                }
                #endregion
                #region Работа с клиентами
                //Прогон по идентификаторам пользователей
                foreach (var i in Clients_Ides)
                {
                    //Строка-запрос
                    query = string.Format("SELECT * FROM `EmailsAndClients` WHERE Email_Id={0} && Client_Id={1}", Id.ToString(), i.ToString());
                    //Ответ или есть уже такая запись
                    var answer = db.MakeRequest(query);
                    //Если такой записи нету, то дабавляет
                    if (answer.Count == 0)
                    {
                        //Строка-запрос
                        query = string.Format("INSERT INTO `EmailsAndUsers`(`Email_Id`, `Client_id`) VALUES ({0},{1})", Id.ToString(), i.ToString());
                        //Запуск запроса
                        db.MakeRequest(query);
                    }
                }
                //Строка-запрос
                query = string.Format("Select * from `EmailsAndClients` where Email_id={0}", Id);
                //Ищет всех пользователей которые имеют это номер
                asnwer1 = db.MakeRequest(query);
                //Прогон по пользователях
                foreach (var i in asnwer1)
                {
                    //Есть ли телефон в списке
                    bool exist = false;
                    //Прогон по айди
                    foreach (var j in Users_Ides)
                    {
                        //Если телефон сущесвтует, то его удалять не надо
                        if (Convert.ToInt32(i["Client_id"]) == j)
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
                        query = string.Format("Delete from `EmailsAndClients` where Id={0}", Convert.ToInt32(i["Id"]));
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
                query = string.Format("INSERT INTO `Emails`(`Name`) VALUES ('{0}'); SELECT * FROM `Emails` order by id desc;", name);
                //Создает запрос и возвращает результат
                var answer = db.MakeRequest(query);
                //Присвоить id
                Id = (int)(answer[0])["Id"];
                #region Работа с пользователями

                //Прогон по идентификаторам пользователей
                foreach (var i in Users_Ides)
                {
                    //Строка-запрос
                    query = string.Format("INSERT INTO `EmailsAndUsers`(`Email_id`, `User_id`) VALUES ({0},{1})", Id.ToString(), i.ToString());
                    //Запуск запроса
                    db.MakeRequest(query);
                }
                #endregion
                #region Работа с клиентами

                //Прогон по идентификаторам пользователей
                foreach (var i in Clients_Ides)
                {
                    //Строка-запрос
                    query = string.Format("INSERT INTO `EmailsAndClients`(`Email_id`, `Client_id`) VALUES ({0},{1})", Id.ToString(), i.ToString());
                    //Запуск запроса
                    db.MakeRequest(query);
                }
                #endregion
            }
        }
        /// <summary>
        /// Удаляет mail из базы
        /// </summary>
        public void Delete()
        {
            if (Id != 0)
            {
                //База данных
                MySqlWorker.DataBase db = SQL.SqlConnect.db;
                //Строка-запрос для удаления из таблицы Мейл-Пользователь
                string query = string.Format("DELETE FROM  `EmailsAndUsers` WHERE  `Email_id` = {0}", Id);
                //Создает запрос и возвращает результат
                db.MakeRequest(query);
                //Строка-запрос для удаления из таблицы Мейл-Клиент
                query = string.Format("DELETE FROM  `EmailsAndClients` WHERE  `Email_id` = {0}", Id);
                //Создает запрос и возвращает результат
                db.MakeRequest(query);
                //Строка-запрос
                 query = string.Format("DELETE FROM  `Emails` WHERE  `id` = {0}", Id);
                //Создает запрос и возвращает результат
                db.MakeRequest(query);
            }
        }
        /// <summary>
        /// Ищет Email по ИД
        /// </summary>
        /// <param name="id">ИД</param>
        /// <returns>Email</returns>
        public static Email FindById(int id)
        {
            //Запрос
            string query = string.Format("SELECT * FROM `Emails` WHERE id={0}", id);
            //База данных
            MySqlWorker.DataBase db = SQL.SqlConnect.db;
            //Создает запрос и возвращает результат
            var list = db.MakeRequest<Email>(query);
            //Если ничего не нашло, то возвращает ноль
            if (list.Count == 0)
            {
                return null;
            }
            //Если нашло, то создает объект должности
            else
            {
                //Создаю мейл
                Email us = list[0];
                //Строка-запрос на получение пользователя
                query = string.Format("SELECT * FROM `EmailsAndUsers` WHERE Email_id={0}", id);
                //Выполнение запроса
                var answer = db.MakeRequest(query);
                //Прогон по пользователях
                foreach (var i in answer)
                {
                    //Добавление пользователя в список
                    us.Users_Ides.Add(Convert.ToInt32(i["User_id"]));
                }

                //Строка-запрос на получение клиентов
                query = string.Format("SELECT * FROM `EmailsAndClients` WHERE Email_id={0}", id);
                //Выполнение запроса
                answer = db.MakeRequest(query);
                //Прогон по клинтах
                foreach (var i in answer)
                {
                    //Добавление клиента в список
                    us.Clients_Ides.Add(Convert.ToInt32(i["Client_id"]));
                }

                //Возвращает мейл
                return us;
            }
        }
        /// <summary>
        /// Ищет мейл по имени
        /// </summary>
        /// <param name="name">имя</param>
        /// <returns>Мейл</returns>
        public static Email FindByName(string name)
        {
            //Запрос
            string query = string.Format("SELECT * FROM `Emails` WHERE name='{0}'", name);
            //База данных
            MySqlWorker.DataBase db = SQL.SqlConnect.db;
            //Создает запрос и возвращает результат
            var list = db.MakeRequest<Email>(query);
            //Если ничего не нашло, то возвращает ноль
            if (list.Count == 0)
            {
                return null;
            }
            //Если нашло, то создает объект должности
            else
            {
                //Создаю мейл
                Email us = list[0];
                //Строка-запрос на получение пользователя
                query = string.Format("SELECT * FROM `EmailsAndUsers` WHERE Email_id={0}", us.Id.ToString());
                //Выполнение запроса
                var answer = db.MakeRequest(query);
                //Прогон по пользователях
                foreach (var i in answer)
                {
                    //Добавление пользователя в список
                    us.Users_Ides.Add(Convert.ToInt32(i["User_id"]));
                }
                //Возвращает мейл
                return us;
            }
        }
        /// <summary>
        /// Владельцы мейла
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
        public List<Clients.Client> GetClients
        {
            set
            {
                Users_Ides.Clear();
                foreach (var i in value)
                {
                    Clients_Ides.Add(i.Id);
                }
            }
            get
            {
                List<Clients.Client> clients = new List<Clients.Client>();
                foreach (var i in Users_Ides)
                {
                    clients.Add(Clients.Client.FindById(i));
                }
                return clients;
            }
        }
    }
}
