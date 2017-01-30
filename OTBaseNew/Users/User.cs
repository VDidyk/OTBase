using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace OTBaseNew.Users
{
    /// <summary>
    /// Класс Пользователь
    /// </summary>
    public class User
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { set; get; }
        /// <summary>
        /// Имя
        /// </summary>
        public string FName { set; get; }
        /// <summary>
        /// Фамилия
        /// </summary>
        public string LName { set; get; }
        /// <summary>
        /// Отчество
        /// </summary>
        public string MName { set; get; }
        /// <summary>
        /// Логин
        /// </summary>
        public string Login { set; get; }
        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { set; get; }
        /// <summary>
        /// Админ?
        /// </summary>
        public bool IsAdmin { set; get; }
        /// <summary>
        /// Id должности
        /// </summary>
        public int Position_id { set; get; }
        /// <summary>
        /// Дата регестрации
        /// </summary>
        public DateTime Created { set; get; }
        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime Bday { set; get; }
        /// <summary>
        /// Должность пользователя
        /// </summary>
        public Positions.Position Position
        {
            set { Position_id = value.Id; }
            get { return Positions.Position.FindById(Position_id); }
        }
        /// <summary>
        /// Список ИД телефонов
        /// </summary>
        public List<int> Phones_Ides { set; get; }
        /// <summary>
        /// Айди мейлов
        /// </summary>
        public List<int> Emails_Ides { set; get; }
        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public User()
        {
            //Дата когда был создан пользователь
            Created = DateTime.Now;
            //Не админ по умолчанию
            IsAdmin = false;
            //Создает новый список
            Phones_Ides = new List<int>();
            Emails_Ides = new List<int>();
        }
        /// <summary>
        /// Сохраняет клас в БД
        /// </summary>
        public void Save()
        {
            //Строка-запрос
            string query = string.Format("SELECT * FROM `Users` WHERE id={0}", Id);
            //База данных
            MySqlWorker.DataBase db = SQL.SqlConnect.db;
            //Создает запрос и возвращает результат
            var list = db.MakeRequest(query);
            //Если есть элементы в списке, то такая должность есть в базе
            if (list.Count != 0)
            {
                //Строка-запрос
                query = string.Format("UPDATE Users SET fname='{0}',lname='{1}',mname='{2}',password='{3}',IsAdmin='{4}',position_id='{5}',bday='{6}' WHERE id={7}", FName, LName, MName, Password, MySqlWorker.DataBase.ConvertBoolToInt(IsAdmin).ToString(), Position_id.ToString(), MySqlWorker.DataBase.ConvertDateToMySqlString(Bday), Id.ToString());
                //Создает запрос и возвращает результат
                db.MakeRequest(query);
                #region Работа с телефонами
                //Прогон по идентификаторам пользователей
                foreach (var i in Phones_Ides)
                {
                    //Строка-запрос
                    query = string.Format("SELECT * FROM `PhonesAndUsers` WHERE Phone_Id={0} && User_Id={1}", i.ToString(), Id.ToString());
                    //Ответ или есть уже такая запись
                    var answer = db.MakeRequest(query);
                    //Если такой записи нету, то дабавляет
                    if (answer.Count == 0)
                    {
                        //Строка-запрос
                        query = string.Format("INSERT INTO `PhonesAndUsers`(`Phone_id`, `User_id`) VALUES ({0},{1})", i.ToString(), Id.ToString());
                        //Запуск запроса
                        db.MakeRequest(query);
                    }
                }
                //Строка-запрос
                query = string.Format("Select * from `PhonesAndUsers` where User_id={0}", Id);
                //Ищет всех пользователей которые имеют это номер
                var asnwer1 = db.MakeRequest(query);
                //Прогон по пользователях
                foreach (var i in asnwer1)
                {
                    //Есть ли телефон в списке
                    bool exist = false;
                    //Прогон по айди
                    foreach (var j in Phones_Ides)
                    {
                        //Если телефон сущесвтует, то его удалять не надо
                        if (Convert.ToInt32(i["Phone_id"]) == j)
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
                #region Работа с мейлами
                //Прогон по идентификаторам пользователей
                foreach (var i in Phones_Ides)
                {
                    //Строка-запрос
                    query = string.Format("SELECT * FROM `EmailsAndUsers` WHERE Email_Id={0} && User_Id={1}", i.ToString(), Id.ToString());
                    //Ответ или есть уже такая запись
                    var answer = db.MakeRequest(query);
                    //Если такой записи нету, то дабавляет
                    if (answer.Count == 0)
                    {
                        //Строка-запрос
                        query = string.Format("INSERT INTO `EmailsAndUsers`(`Email_id`, `User_id`) VALUES ({0},{1})", i.ToString(), Id.ToString());
                        //Запуск запроса
                        db.MakeRequest(query);
                    }
                }
                //Строка-запрос
                query = string.Format("Select * from `EmailsAndUsers` where User_id={0}", Id);
                //Ищет всех пользователей которые имеют это мейл
                asnwer1 = db.MakeRequest(query);
                //Прогон по пользователях
                foreach (var i in asnwer1)
                {
                    //Есть ли мейл в списке
                    bool exist = false;
                    //Прогон по айди
                    foreach (var j in Phones_Ides)
                    {
                        //Если мейл сущесвтует, то его удалять не надо
                        if (Convert.ToInt32(i["Email_id"]) == j)
                        {
                            //Мейл существует
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
            }
            //В другом случае создать новую запись, и присвоить должности ID
            else
            {
                //Строка-запрос
                query = string.Format("INSERT INTO `Users` (`fname`, `lname`, `mname`, `password`, `isadmin`,  `position_id`, `bday`, `created`,`login`) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}'); SELECT * FROM `Users` order by id desc;", FName, LName, MName, Password, MySqlWorker.DataBase.ConvertBoolToInt(IsAdmin).ToString(), Position_id.ToString(), MySqlWorker.DataBase.ConvertDateToMySqlString(Bday), MySqlWorker.DataBase.ConvertDateToMySqlString(DateTime.Now), Login);
                //Создает запрос и возвращает результат
                list = db.MakeRequest(query);
                //Присвоить id
                Id = (int)(list[0])["id"];
                #region Работа с телефонами
                //Прогон по идентификаторам пользователей
                foreach (var i in Phones_Ides)
                {
                    //Строка-запрос
                    query = string.Format("INSERT INTO `PhonesAndUsers`(`Phone_id`, `User_id`) VALUES ({0},{1})", i.ToString(), Id.ToString());
                    //Запуск запроса
                    db.MakeRequest(query);
                }
                #endregion
                #region Работа с мейлами
                //Прогон по идентификаторам пользователей
                foreach (var i in Phones_Ides)
                {
                    //Строка-запрос
                    query = string.Format("INSERT INTO `EmailsAndUsers`(`Email_id`, `User_id`) VALUES ({0},{1})", i.ToString(), Id.ToString());
                    //Запуск запроса
                    db.MakeRequest(query);
                }
                #endregion
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
                //Строка-запрос для удаления из таблицы Телефон-Пользователь
                string query = string.Format("DELETE FROM  `PhonesAndUsers` WHERE  `User_id` = {0}", Id);
                //Создает запрос и возвращает результат
                db.MakeRequest(query);
                //Строка-запрос для удаления из таблицы Мейлы-Пользователь
                query = string.Format("DELETE FROM  `EmailsAndUsers` WHERE  `User_id` = {0}", Id);
                //Создает запрос и возвращает результат
                db.MakeRequest(query);
                //Строка-запрос
                query = string.Format("DELETE FROM  `Users` WHERE  `id` = {0}", Id);
                //Создает запрос и возвращает результат
                db.MakeRequest(query);
            }
        }
        /// <summary>
        /// Ищет пользователя по ID и возвращает ее.Если ничего не нашло, то возвращает ноль
        /// </summary>
        /// <param name="id">ИД</param>
        /// <returns>Пользователь</returns>
        public static User FindById(int id)
        {
            //Запрос
            string query = string.Format("SELECT * FROM `Users` WHERE id={0}", id);
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
                //Создаю пользователя
                User us = MakeUserFromReaderList(list);
                //Строка-запрос на получение телефонов
                query = string.Format("SELECT * FROM `PhonesAndUsers` WHERE User_id={0}", id);
                //Выполнение запроса
                var answer = db.MakeRequest(query);
                //Прогон по телефонах
                foreach (var i in answer)
                {
                    //Добавление телефонов в список
                    us.Phones_Ides.Add(Convert.ToInt32(i["Phone_id"]));
                }

                query = string.Format("SELECT * FROM `EmailsAndUsers` WHERE User_id={0}", id);
                //Выполнение запроса
                answer = db.MakeRequest(query);
                //Прогон по мейлах
                foreach (var i in answer)
                {
                    //Добавление мейл в список
                    us.Emails_Ides.Add(Convert.ToInt32(i["Email_id"]));
                }

                //Возвращает пользователя
                return us;
            }
        }
        /// <summary>
        /// Ищет пользователя по Логину и возвращает ее.Если ничего не нашло, то возвращает ноль
        /// </summary>
        /// <param name="Login">Логин</param>
        /// <returns>Пользователь</returns>
        public static User FindByLogin(string Login)
        {
            //Запрос
            string query = string.Format("SELECT * FROM `Users` WHERE login='{0}'", Login);
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
                //Создаю пользователя
                User us = MakeUserFromReaderList(list);
                //Строка-запрос на получение телефонов
                query = string.Format("SELECT * FROM `PhonesAndUsers` WHERE User_id={0}", us.Id.ToString());
                //Выполнение запроса
                var answer = db.MakeRequest(query);
                //Прогон по телефонах
                foreach (var i in answer)
                {
                    //Добавление телефонов в список
                    us.Phones_Ides.Add(Convert.ToInt32(i["Phone_id"]));
                }

                query = string.Format("SELECT * FROM `EmailsAndUsers` WHERE User_id={0}", us.Id.ToString());
                //Выполнение запроса
                answer = db.MakeRequest(query);
                //Прогон по мейлах
                foreach (var i in answer)
                {
                    //Добавление мейл в список
                    us.Emails_Ides.Add(Convert.ToInt32(i["Email_id"]));
                }

                //Возвращает пользователя
                return us;

            }
        }
        /// <summary>
        /// Создает и возвращает пользователя, которого создает со списка со словарем с запроса
        /// </summary>
        /// <param name="list">Список со словарем с запроса</param>
        /// <returns>Пользователь</returns>
        public static User MakeUserFromReaderList(List<Dictionary<string, object>> list)
        {
            //создает объект должности
            User user = new User();
            //Присваивает ИД
            user.Id = Convert.ToInt32((list[0])["id"].ToString());
            //Присваивает имя с запроса
            user.FName = (list[0])["fname"].ToString();
            //Присваивает фамилию с запроса
            user.LName = (list[0])["lname"].ToString();
            //Присваивает отчество с запроса
            user.MName = (list[0])["mname"].ToString();
            //Присваивает логин с запроса
            user.Login = (list[0])["login"].ToString();
            //Присваивает пароль с запроса
            user.Password = (list[0])["password"].ToString();
            //Присваивает админовскую булевую переменную с запроса
            user.IsAdmin = Convert.ToBoolean((list[0])["isadmin"].ToString());
            //Присваивает айди должности с запроса
            user.Position_id = Convert.ToInt32((list[0])["position_id"].ToString());
            //Присваивает дату создания с запроса
            user.Created = Convert.ToDateTime((list[0])["created"].ToString());
            //Присваивает дату рождения с запроса
            user.Bday = Convert.ToDateTime((list[0])["bday"].ToString());
            return user;
        }
        /// <summary>
        /// Телефоны пользователя
        /// </summary>
        public List<Phones.Phone> GetPhones
        {
            set
            {
                Phones_Ides.Clear();
                foreach (var i in value)
                {
                    Phones_Ides.Add(i.Id);
                }
            }
            get
            {
                List<Phones.Phone> phones = new List<Phones.Phone>();
                foreach (var i in Phones_Ides)
                {
                    phones.Add(Phones.Phone.FindById(i));
                }
                return phones;
            }
        }
        /// <summary>
        /// Емейлы
        /// </summary>
        public List<Emails.Email> GetEmails
        {
            set
            {
                Emails_Ides.Clear();
                foreach (var i in value)
                {
                    Emails_Ides.Add(i.Id);
                }
            }
            get
            {
                List<Emails.Email> emails = new List<Emails.Email>();
                foreach (var i in Phones_Ides)
                {
                    emails.Add(Emails.Email.FindById(i));
                }
                return emails;
            }
        }

        /// <summary>
        /// Клиенты, которые были созданы пользователем
        /// </summary>
        public List<Clients.Client> GetClientsCreated
        {
            private set { }
            get
            {
                string query = string.Format(" select * from Clients where Clients.Created_user_id = {0}", Id);
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

        /// <summary>
        /// Клиенты, которые были изменены пользователем
        /// </summary>
        public List<Clients.Client> GetClientsEdited
        {
            private set { }
            get
            {
                string query = string.Format(" select * from Clients where Clients.Last_edit_user_id = {0}", Id);
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

        /// <summary>
        /// Клиенты,с которыми работает пользователь
        /// </summary>
        public List<Clients.Client> GetClientsWorking
        {
            private set { }
            get
            {
                string query = string.Format(" select * from Clients where Clients.Working_user_id = {0}", Id);
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
        /// <summary>
        /// Заявки, которые пользователь создал
        /// </summary>
        public List<Clients.Client> GetRequestsCreated
        {
            private set { }
            get
            {
                string query = string.Format(" select * from Request where Clients.Created_user_id = {0}", Id);
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
        /// <summary>
        /// Выдает всех пользователей
        /// </summary>
        static public List<User> GetAllUsers
        {
            private set { }
            get
            {
                string query = string.Format(" select * from Users");
                //База данных
                MySqlWorker.DataBase db = SQL.SqlConnect.db;
                //Создает запрос и возвращает результат
                List<User> users= db.MakeRequest<User>(query);
                if (users.Count != 0)
                users.RemoveAt(0);
                return users;
            }
        }

    }
}
