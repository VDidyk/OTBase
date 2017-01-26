using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTBaseNew.Clients
{
    public class Client
    {
        /// <summary>
        /// ИД
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
        /// Дата рождения
        /// </summary>
        public DateTime Bday { set; get; }
        /// <summary>
        /// Дата регестрации
        /// </summary>
        public DateTime Created { set; get; }
        /// <summary>
        /// Заметка
        /// </summary>
        public string Notice { set; get; }
        /// <summary>
        /// Ид Адресса
        /// </summary>
        public int Address_id { set; get; }
        /// <summary>
        /// Ид паспорта
        /// </summary>
        public int Passport_id { set; get; }
        /// <summary>
        /// Ид ресурса
        /// </summary>
        public int Resourse_id { set; get; }
        /// <summary>
        /// Ид пользователя, который создал клиента
        /// </summary>
        public int Created_user_id { set; get; }
        /// <summary>
        /// Ид пользователя, который последний редактировал клиента
        /// </summary>
        public int Last_edit_user_id { set; get; }
        /// <summary>
        /// Ид пользователя, который работает с клиентом
        /// </summary>
        public int Working_user_id { set; get; }
        /// <summary>
        /// Ид телефонов
        /// </summary>
        public List<int> Phones_Ides;
        /// <summary>
        /// Мейлы
        /// </summary>
        public List<int> Emails_Ides;
        /// <summary>
        /// Скидки
        /// </summary>
        public List<int> Discounts_Ides;
        public Client()
        {
            Created = DateTime.Now;
            Phones_Ides = new List<int>();
            Emails_Ides = new List<int>();
            Discounts_Ides = new List<int>();
        }
        /// <summary>
        /// Сохраняет клиента в БД
        /// </summary>
        public void Save()
        {
            //Строка-запрос
            string query = string.Format("SELECT * FROM `Clients` WHERE id={0}", Id);
            //База данных
            MySqlWorker.DataBase db = SQL.SqlConnect.db;
            //Создает запрос и возвращает результат
            var list = db.MakeRequest(query);
            //Если есть элементы в списке, то такая должность есть в базе
            if (list.Count != 0)
            {
                //Строка-запрос
                query = string.Format("UPDATE Clients SET fname='{0}',lname='{1}',mname='{2}',notice='{3}',Address_id='{4}',Passport_id='{5}',bday='{6}',Resourse_id={8},created_user_id={9}, Last_edit_user_id={10},Working_user_id={11},created='{12}' WHERE id={7}", FName, LName, MName, Notice, Address_id.ToString(), Passport_id.ToString(), MySqlWorker.DataBase.ConvertDateToMySqlString(Bday), Id.ToString(), Resourse_id.ToString(), Created_user_id.ToString(), Last_edit_user_id.ToString(), Working_user_id.ToString(), MySqlWorker.DataBase.ConvertDateToMySqlString(Created));
                //Создает запрос и возвращает результат
                db.MakeRequest(query);
                #region Работа с телефонами
                //Прогон по идентификаторам пользователей
                foreach (var i in Phones_Ides)
                {
                    //Строка-запрос
                    query = string.Format("SELECT * FROM `PhonesAndClients` WHERE Phone_Id={0} && Client_Id={1}", i.ToString(), Id.ToString());
                    //Ответ или есть уже такая запись
                    var answer = db.MakeRequest(query);
                    //Если такой записи нету, то дабавляет
                    if (answer.Count == 0)
                    {
                        //Строка-запрос
                        query = string.Format("INSERT INTO `PhonesAndClients`(`Phone_id`, `Client_Id`) VALUES ({0},{1})", i.ToString(), Id.ToString());
                        //Запуск запроса
                        db.MakeRequest(query);
                    }
                }
                //Строка-запрос
                query = string.Format("Select * from `PhonesAndClients` where Client_Id={0}", Id);
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
                        query = string.Format("Delete from `PhonesAndClients` where Id={0}", Convert.ToInt32(i["Id"]));
                        //Удаляем ее из базы
                        db.MakeRequest(query);
                    }
                }
                #endregion
                #region Работа с мейлами
                //Прогон по идентификаторам пользователей
                foreach (var i in Emails_Ides)
                {
                    //Строка-запрос
                    query = string.Format("SELECT * FROM `EmailsAndClients` WHERE Email_Id={0} && Client_Id={1}", i.ToString(), Id.ToString());
                    //Ответ или есть уже такая запись
                    var answer = db.MakeRequest(query);
                    //Если такой записи нету, то дабавляет
                    if (answer.Count == 0)
                    {
                        //Строка-запрос
                        query = string.Format("INSERT INTO `EmailsAndClients`(`Email_id`, `Client_Id`) VALUES ({0},{1})", i.ToString(), Id.ToString());
                        //Запуск запроса
                        db.MakeRequest(query);
                    }
                }
                //Строка-запрос
                query = string.Format("Select * from `EmailsAndClients` where Client_Id={0}", Id);
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
                        query = string.Format("Delete from `EmailsAndClients` where Id={0}", Convert.ToInt32(i["Id"]));
                        //Удаляем ее из базы
                        db.MakeRequest(query);
                    }
                }
                #endregion
                #region Работа со скидками
                //Строка-запрос
                query = string.Format("SELECT * FROM `Discounts` WHERE Client_id='{0}'", Id.ToString());
                var answer1 = db.MakeRequest<Discounts.Discount>(query);
                bool exist1 = false;
                foreach (var j in answer1)
                {
                    foreach (var i in Discounts_Ides)
                    {
                        if (i == j.Id || j.Operator_id!=0)
                        {
                            exist1 = true;
                            break;
                        }
                    }
                    if (!exist1)
                    {
                        j.Delete();
                    }
                }
                #endregion
            }
            //В другом случае создать новую запись, и присвоить должности ID
            else
            {
                //Строка-запрос
                query = string.Format("INSERT INTO `Clients`(`fname`, `lname`, `mname`, `bday`, `address_id`, `passport_id`, `resourse_id`, `created`, `notice`, `created_user_id`, `last_edit_user_id`, `working_user_id`) VALUES ('{0}','{1}','{2}','{3}',{4},{5},{6},'{7}','{8}',{9},'{10}','{11}'); SELECT * FROM `Clients` order by id desc;", FName, LName, MName, MySqlWorker.DataBase.ConvertDateToMySqlString(Bday), Address_id.ToString(), Passport_id.ToString(), Resourse_id.ToString(), MySqlWorker.DataBase.ConvertDateToMySqlString(DateTime.Now), Notice, this.Created_user_id.ToString(), Last_edit_user_id.ToString(), Working_user_id.ToString());
                //Создает запрос и возвращает результат
                list = db.MakeRequest(query);
                //Присвоить id
                Id = (int)(list[0])["id"];
                #region Работа с телефонами
                //Прогон по идентификаторам пользователей
                foreach (var i in Phones_Ides)
                {
                    //Строка-запрос
                    query = string.Format("INSERT INTO `PhonesAndClients`(`Phone_id`, `Client_id`) VALUES ({0},{1})", i.ToString(), Id.ToString());
                    //Запуск запроса
                    db.MakeRequest(query);
                }
                #endregion
                #region Работа с мейлами
                //Прогон по идентификаторам пользователей
                foreach (var i in Emails_Ides)
                {
                    //Строка-запрос
                    query = string.Format("INSERT INTO `EmailsAndClients`(`Email_id`, `Client_id`) VALUES ({0},{1})", i.ToString(), Id.ToString());
                    //Запуск запроса
                    db.MakeRequest(query);
                }
                #endregion
            }
            //Операция закончена, возвращает тру
        }
        public static Client FindById(int id)
        {
            //Запрос
            string query = string.Format("SELECT * FROM `Clients` WHERE id={0}", id);
            //База данных
            MySqlWorker.DataBase db = SQL.SqlConnect.db;
            //Создает запрос и возвращает результат
            var list = db.MakeRequest<Client>(query);
            //Если ничего не нашло, то возвращает ноль
            if (list.Count == 0)
            {
                return null;
            }
            //Если нашло, то создает объект должности
            else
            {
                //Создаю пользователя
                Client us = list[0];
                //Строка-запрос на получение телефонов
                query = string.Format("SELECT * FROM `PhonesAndClients` WHERE Client_id={0}", id);
                //Выполнение запроса
                var answer = db.MakeRequest(query);
                //Прогон по телефонах
                foreach (var i in answer)
                {
                    //Добавление телефонов в список
                    us.Phones_Ides.Add(Convert.ToInt32(i["Phone_id"]));
                }

                query = string.Format("SELECT * FROM `EmailsAndClients` WHERE Client_id={0}", id);
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
        /// Удаляет клиента с базы данных
        /// </summary>
        public void Delete()
        {
            if (Id != 0)
            {
                //База данных
                MySqlWorker.DataBase db = SQL.SqlConnect.db;
                //Строка-запрос для удаления из таблицы Телефон-Пользователь
                string query = string.Format("DELETE FROM  `PhonesAndClients` WHERE  `Client_id` = {0}", Id);
                //Создает запрос и возвращает результат
                db.MakeRequest(query);
                //Строка-запрос для удаления из таблицы Мейлы-Пользователь
                query = string.Format("DELETE FROM  `EmailsAndClients` WHERE  `Client_id` = {0}", Id);
                //Создает запрос и возвращает результат
                db.MakeRequest(query);
                //Строка-запрос
                query = string.Format("DELETE FROM  `Clients` WHERE  `id` = {0}", Id);
                //Создает запрос и возвращает результат
                db.MakeRequest(query);
                //Удаляет адрес с базы данных
                GetAddress.Delete();
                //Удаляет аддресс
                GetPassport.Delete();
                //Удаляет скидки
                foreach(var i in GetDiscounts)
                {
                    i.Delete();
                }
            }
        }
        /// <summary>
        /// Адресс
        /// </summary>
        public Addresses.Address GetAddress
        {
            set
            {
                Address_id = value.id;
            }
            get
            {
                return Addresses.Address.FindById(Address_id);
            }
        }
        /// <summary>
        /// Паспорт
        /// </summary>
        public Passports.Passport GetPassport
        {
            set
            {
                Passport_id = value.id;
            }
            get
            {
                return Passports.Passport.FindById(Passport_id);
            }
        }
        /// <summary>
        /// Ресурс
        /// </summary>
        public Resourses.Resourse GetResourse
        {
            set
            {
                Resourse_id = value.Id;
            }
            get
            {
                return Resourses.Resourse.FindById(Resourse_id);
            }
        }
        /// <summary>
        /// Пользователь создавший клиента
        /// </summary>
        public Users.User GetCreatedUser
        {
            set
            {
                Created_user_id = value.Id;
            }
            get
            {
                return Users.User.FindById(Created_user_id);
            }
        }
        /// <summary>
        /// Пользователь последний редактирующий клиента
        /// </summary>
        public Users.User GetEditdUser
        {
            set
            {
                Last_edit_user_id = value.Id;
            }
            get
            {
                return Users.User.FindById(Last_edit_user_id);
            }
        }
        /// <summary>
        /// Пользователь работающий с клиентом
        /// </summary>
        public Users.User GetWorkingdUser
        {
            set
            {
                Working_user_id = value.Id;
            }
            get
            {
                return Users.User.FindById(Working_user_id);
            }
        }

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
        /// Скидки
        /// </summary>
        public List<Discounts.Discount> GetDiscounts
        {
            set
            {
                Discounts_Ides.Clear();
                foreach (var i in value)
                {
                    Discounts_Ides.Add(i.Id);
                }
            }
            get
            {
                List<Discounts.Discount> discounts = new List<Discounts.Discount>();
                foreach (var i in Discounts_Ides)
                {
                    discounts.Add(Discounts.Discount.FindById(i));
                }
                return discounts;
            }
        }

    }
}
