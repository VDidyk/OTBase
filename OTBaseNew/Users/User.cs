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




    }
}
