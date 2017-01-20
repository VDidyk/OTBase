using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTBaseNew.Positions
{
    class Position
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { set; get; }
        /// <summary>
        /// Название
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime Created { set; get; }
        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public Position()
        {
            Created = DateTime.Now;
        }
        /// <summary>
        /// Сохраняет клас в БД
        /// </summary>
        public void Save()
        {
            //Строка-запрос
            string query = string.Format("SELECT * FROM `Positions` WHERE id={0}", Id);
            //База данных
            SQL.DataBase db = new SQL.DataBase();
            //Создает запрос и возвращает результат
            var list = db.MakeRequest(query);
            //Если есть элементы в списке, то такая должность есть в базе
            if (list.Count != 0)
            {
                //Строка-запрос
                query = string.Format("UPDATE Positions SET name='{0}' WHERE id={1}", Name, Id);
                //Создает запрос и возвращает результат
                db.MakeRequest(query);
            }
            //В другом случае создать новую запись, и присвоить должности ID
            else
            {
                //Строка-запрос
                query = string.Format("INSERT INTO `Positions`(`name`, `created`) VALUES ('{0}','{1}'); SELECT * FROM `Positions` order by id desc;", Name, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
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
                string query = string.Format("DELETE FROM  `Positions` WHERE  `id` = {0}", Id);
                //Создает запрос и возвращает результат
                db.MakeRequest(query);
            }
        }
    }
}
