using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTBaseNew.Requests
{
    public class Request
    {
        public int Id { set; get; }
        public string notice { set; get; }
        /// <summary>
        /// Получен ли пасспорт
        /// </summary>
        public bool Get_passport { set; get; }
        /// <summary>
        /// Нужна ли виза
        /// </summary>
        public bool Visa_is_important { set; get; }
        /// <summary>
        /// Получена ли виза
        /// </summary>
        public bool Get_visa { set; get; }
        /// <summary>
        /// Цена тура
        /// </summary>
        public double Price_of_tour { set; get; }
        /// <summary>
        /// Цена для клиента
        /// </summary>
        public double Price_of_client { set; get; }
        /// <summary>
        /// Уплаченая сумма
        /// </summary>
        public double Paid_sum { set; get; }
        /// <summary>
        /// Место прилета
        /// </summary>
        public string Where_to_fly { set; get; }
        /// <summary>
        /// Место вылета
        /// </summary>
        public string From_where_to_fly { set; get; }
        /// <summary>
        /// Время вылета
        /// </summary>
        public DateTime Date_to_go { set; get; }
        /// <summary>
        /// Время прилета
        /// </summary>
        public DateTime Date_to_arrive { set; get; }
        /// <summary>
        /// Готель
        /// </summary>
        public string Hotel { set; get; }
        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime Created { set; get; }
        /// <summary>
        /// Оператор
        /// </summary>
        public int Operator_id { set; get; }
        /// <summary>
        /// Пользователь, создавший заявку
        /// </summary>
        public int Created_user_id { set; get; }
        public int working_user_id { set; get; }
        /// <summary>
        /// Смотриться ли заявка
        /// </summary>
        public bool Look { set; get; }
        /// <summary>
        /// Статус
        /// </summary>
        public int Status_id { set; get; }
        /// <summary>
        /// Номер заявки у оператора
        /// </summary>
        public string Serial_number { set; get; }
        /// <summary>
        /// Клиенты
        /// </summary>
        public List<int> Clients_Ides { set; get; }
        public Request()
        {
            Created = DateTime.Now;
            Clients_Ides = new List<int>();
        }
        public void Save()
        {
            //Строка-запрос
            string query = string.Format("SELECT * FROM `Request` WHERE id={0}", Id);
            //База данных
            MySqlWorker.DataBase db = SQL.SqlConnect.db;
            //Создает запрос и возвращает результат
            var list = db.MakeRequest(query);
            //Если есть элементы в списке, то такая должность есть в базе
            if (list.Count != 0)
            {
                //Строка-запрос
                query = string.Format("UPDATE Request SET Get_passport='{0}',Visa_is_important='{1}',Get_visa='{2}',Price_of_tour='{3}',Price_of_client='{4}',Paid_sum='{5}',Where_to_fly='{6}',From_where_to_fly='{7}',Date_to_go='{8}',Date_to_arrive='{9}',Hotel='{10}',Created='{11}',Operator_id='{12}',Created_user_id='{13}',Look='{14}',Status_id='{15}',Serial_number='{16}',working_user_id='{17}',notice='{18}' WHERE id={19}",
                    MySqlWorker.DataBase.ConvertBoolToInt(Get_passport), MySqlWorker.DataBase.ConvertBoolToInt(Visa_is_important), MySqlWorker.DataBase.ConvertBoolToInt(Get_visa), Price_of_tour.ToString(), Price_of_client.ToString(), Paid_sum.ToString(), Where_to_fly, From_where_to_fly, MySqlWorker.DataBase.ConvertDateToMySqlString(Date_to_go), MySqlWorker.DataBase.ConvertDateToMySqlString(Date_to_arrive),
                    Hotel, MySqlWorker.DataBase.ConvertDateToMySqlString(Created),
                    Operator_id.ToString(), Created_user_id.ToString(), MySqlWorker.DataBase.ConvertBoolToInt(Look), Status_id.ToString(), Serial_number, working_user_id, notice,Id);
                //Создает запрос и возвращает результат
                db.MakeRequest(query);
                #region Работа с клиентами
                //Прогон по идентификаторам пользователей
                foreach (var i in Clients_Ides)
                {
                    //Строка-запрос
                    query = string.Format("SELECT * FROM `ClientsAndRequests` WHERE Client_id={0} && Request_id={1}", i.ToString(), Id.ToString());
                    //Ответ или есть уже такая запись
                    var answer = db.MakeRequest(query);
                    //Если такой записи нету, то дабавляет
                    if (answer.Count == 0)
                    {
                        //Строка-запрос
                        query = string.Format("INSERT INTO `ClientsAndRequests`(`Client_id`, `Request_id`) VALUES ({0},{1})", i.ToString(), Id.ToString());
                        //Запуск запроса
                        db.MakeRequest(query);
                    }
                }
                //Строка-запрос
                query = string.Format("Select * from `ClientsAndRequests` where Request_id={0}", Id);
                //Ищет всех пользователей которые имеют это номер
                var asnwer1 = db.MakeRequest(query);
                //Прогон по пользователях
                foreach (var i in asnwer1)
                {
                    //Есть ли телефон в списке
                    bool exist = false;
                    //Прогон по айди
                    foreach (var j in Clients_Ides)
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
                        query = string.Format("Delete from `ClientsAndRequests` where Id={0}", Convert.ToInt32(i["Id"]));
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
                query = string.Format("INSERT INTO `Request`(`get_passport`, `get_visa`, `visa_is_important`, `price_of_tour`, `price_of_client`, `paid_sum`, `where_to_fly`, `from_where_to_fly`, `date_to_go`, `date_to_arrive`, `hotel`, `created`, `operator_id`, `created_user_id`, `look`, `Status_id`, `serial_number`,`working_user_id`) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}',{18}); SELECT * FROM `Request` order by id desc LIMIT 0 , 1;", MySqlWorker.DataBase.ConvertBoolToInt(Get_passport), MySqlWorker.DataBase.ConvertBoolToInt(Get_visa), MySqlWorker.DataBase.ConvertBoolToInt(Visa_is_important), Price_of_tour.ToString(), Price_of_client.ToString(), Paid_sum.ToString(), Where_to_fly, From_where_to_fly, MySqlWorker.DataBase.ConvertDateToMySqlString(Date_to_go), MySqlWorker.DataBase.ConvertDateToMySqlString(Date_to_arrive), Hotel, MySqlWorker.DataBase.ConvertDateToMySqlString(Created), Operator_id.ToString(), Created_user_id.ToString(), MySqlWorker.DataBase.ConvertBoolToInt(Look), Status_id.ToString(), Serial_number, working_user_id,notice);
                //Создает запрос и возвращает результат
                list = db.MakeRequest(query);
                //Присвоить id
                Id = (int)(list[0])["id"];
                #region Работа с клиентами
                //Прогон по идентификаторам пользователей
                foreach (var i in Clients_Ides)
                {
                    //Строка-запрос
                    query = string.Format("INSERT INTO `ClientsAndRequests`(`Client_id`, `Request_id`) VALUES ({0},{1})", i.ToString(), Id.ToString());
                    //Запуск запроса
                    db.MakeRequest(query);
                }
                #endregion

            }
            //Операция закончена, возвращает тру
        }
        public static Request FindById(int id)
        {
            //Запрос
            string query = string.Format("SELECT * FROM `Request` WHERE id={0}", id);
            //База данных
            MySqlWorker.DataBase db = SQL.SqlConnect.db;
            //Создает запрос и возвращает результат
            var list = db.MakeRequest<Request>(query);
            //Если ничего не нашло, то возвращает ноль
            if (list.Count == 0)
            {
                return null;
            }
            //Если нашло, то создает объект должности
            else
            {
                //Создаю пользователя
                Request us = list[0];
                //Строка-запрос на получение телефонов
                query = string.Format("SELECT * FROM `ClientsAndRequests` WHERE Request_id={0}", id);
                //Выполнение запроса
                var answer = db.MakeRequest(query);
                //Прогон по телефонах
                foreach (var i in answer)
                {
                    //Добавление телефонов в список
                    us.Clients_Ides.Add(Convert.ToInt32(i["Client_id"]));
                }


                //Возвращает пользователя
                return us;
            }
        }
        public void Delete()
        {
            if (Id != 0)
            {
                //База данных
                MySqlWorker.DataBase db = SQL.SqlConnect.db;
                //Строка-запрос для удаления из таблицы Телефон-Пользователь
                string query = string.Format("DELETE FROM  `ClientsAndRequests` WHERE  `Request_id` = {0}", Id);

                foreach(var i in GetActions)
                {
                    i.Delete();
                }

                //Создает запрос и возвращает результат
                db.MakeRequest(query);
                //Строка-запрос для удаления из таблицы Пользователь-Запрос
                db.MakeRequest(query);
                //Строка-запрос
                query = string.Format("DELETE FROM  `Request` WHERE  `id` = {0}", Id);
                //Создает запрос и возвращает результат
                db.MakeRequest(query);
                //Удаляет адрес с базы данных
            }
        }
        /// <summary>
        /// Клиенты
        /// </summary>
        public List<Clients.Client> GetClients
        {
            set
            {
                Clients_Ides.Clear();
                foreach (var i in value)
                {
                    Clients_Ides.Add(i.Id);
                }
            }
            get
            {
                List<Clients.Client> clients = new List<Clients.Client>();
                foreach (var i in Clients_Ides)
                {
                    clients.Add(Clients.Client.FindById(i));
                }
                return clients;
            }
        }
        /// <summary>
        /// Статус
        /// </summary>
        public Statuses.Status GetStatus
        {
            set
            {
                Status_id = value.Id;
            }
            get
            {
                return Statuses.Status.FindById(Status_id);
            }
        }
        /// <summary>
        /// Оператор
        /// </summary>
        public Operators.Operator GetOperator
        {
            set
            {
                Operator_id = value.Id;
            }
            get
            {
                return Operators.Operator.FindById(Operator_id);
            }
        }
        /// <summary>
        /// Пользователь который создал заявку
        /// </summary>
        public Users.User GetCreated_user
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
        public Users.User GetWorking_user
        {
            set
            {
                working_user_id = value.Id;
            }
            get
            {
                return Users.User.FindById(working_user_id);
            }
        }
        public List<Actions.Action> GetActions
        {
            private set { ;}
            get
            {
                List<Requests.Request> requests = new List<Requests.Request>();
                string query = string.Format("SELECT * FROM `Actions` WHERE Request_id={0}", Id);
                //База данных
                MySqlWorker.DataBase db = SQL.SqlConnect.db;
                var list = db.MakeRequest<Actions.Action>(query);
                return list;
            }
        }
        public static List<Request> GetAllRequests(int number)
        {
            string request = string.Format("SELECT * FROM Request LIMIT {0},{1}", number, number + 20);
            MySqlWorker.DataBase db = SQL.SqlConnect.db;
            var list = db.MakeRequest<Request>(request);
            return list;
        }
    }
}
