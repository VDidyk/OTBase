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
    class User
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
        private int Position_id { set; get; }
        /// <summary>
        /// Дата регестрации
        /// </summary>
        public DateTime Created { set; get; }
        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime Bdate { set; get; }
        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public User()
        {
            //Дата когда был создан пользователь
            Created = DateTime.Now;
            //Не админ по умолчанию
            IsAdmin = false;
        }
        /// <summary>
        /// Сохраняет клас в БД
        /// </summary>
        public void Save()
        {
            //Строка-запрос
            string query = string.Format("SELECT * FROM `Users` WHERE id={0}", Id);
            //База данных
            SQL.DataBase db = new SQL.DataBase();
            //Создает запрос и возвращает результат
            var list = db.MakeRequest(query);
            //Если есть элементы в списке, то такая должность есть в базе
            if (list.Count != 0)
            {
                //Строка-запрос
                query = string.Format("UPDATE Users SET fname='{0}',lname='{1}',mname='{2}',password='{3}',IsAdmin='{4}',position_id='{5}',bday='{6}' WHERE id={7}", FName, LName, MName, Password, SQL.DataBase.ConvertBoolToInt(IsAdmin).ToString(), Position_id.ToString(), SQL.DataBase.ConvertDateToMySqlString(Bdate), Id.ToString());
                //Создает запрос и возвращает результат
                db.MakeRequest(query);
            }
            //В другом случае создать новую запись, и присвоить должности ID
            else
            {
                //Строка-запрос
                query = string.Format("INSERT INTO `Users` (`fname`, `lname`, `mname`, `password`, `isadmin`,  `position_id`, `bday`, `created`,`login`) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}'); SELECT * FROM `Users` order by id desc;", FName, LName, MName, Password, SQL.DataBase.ConvertBoolToInt(IsAdmin).ToString(), Position_id.ToString(), SQL.DataBase.ConvertDateToMySqlString(Bdate), SQL.DataBase.ConvertDateToMySqlString(DateTime.Now),Login);
                //Создает запрос и возвращает результат
                list = db.MakeRequest(query);
                //Присвоить id
                Id = (int)(list[0])["id"];
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
                SQL.DataBase db = new SQL.DataBase();
                //Строка-запрос
                string query = string.Format("DELETE FROM  `Users` WHERE  `id` = {0}", Id);
                //Создает запрос и возвращает результат
                db.MakeRequest(query);
            }
        }
    }
}
