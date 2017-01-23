﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTBaseNew.Passports
{
    /// <summary>
    /// Паспорт
    /// </summary>
    public class Passport
    {
        /// <summary>
        /// ID
        /// </summary>
        public int id { set; get; }
        /// <summary>
        /// Номер пасспорта
        /// </summary>
        public string series { set; get; }
        /// <summary>
        /// Когда выдан
        /// </summary>
        public DateTime given_when { set; get; }
        /// <summary>
        /// До когда выдан
        /// </summary>
        public DateTime given_the_time { set; get; }
        /// <summary>
        /// Кем выдан
        /// </summary>
        public string given_by { set; get; }
        /// <summary>
        /// Сохранить объект
        /// </summary>
        public void Save()
        {
            if(given_when>=given_the_time)
            {
                throw new Exception("Wrong dates");
            }
            //Строка-запрос
            string query = string.Format("SELECT * FROM `Passports` WHERE id={0}", id);
            //База данных
            MySqlWorker.DataBase db = SQL.SqlConnect.db;
            //Создает запрос и возвращает результат
            var list = db.MakeRequest(query);
            //Если есть элементы в списке, то такая должность есть в базе
            if (list.Count != 0)
            {
                //Строка-запрос
                query = string.Format("UPDATE Passports SET series='{0}', given_when='{1}', given_the_time='{2}',given_by='{3}' WHERE id={4}", series,MySqlWorker.DataBase.ConvertDateToMySqlString(given_when),MySqlWorker.DataBase.ConvertDateToMySqlString(given_the_time),given_by, id);
                //Создает запрос и возвращает результат
                db.MakeRequest(query);
            }
            //В другом случае создать новую запись, и присвоить должности ID
            else
            {
                //Строка-запрос
                query = string.Format("INSERT INTO `Passports`(`series`, `given_when`, `given_the_time`, `given_by`) VALUES ('{0}','{1}','{2}','{3}'); SELECT * FROM `Positions` order by id desc;", series, MySqlWorker.DataBase.ConvertDateToMySqlString(given_when), MySqlWorker.DataBase.ConvertDateToMySqlString(given_the_time), given_by);
                //Создает запрос и возвращает результат
                list = db.MakeRequest(query);
                //Присвоить id
                id = (int)(list[0])["id"];
            }
            //Операция закончена, возвращает тру
        }
        /// <summary>
        /// Ищет пасспорт по ИД
        /// </summary>
        /// <param name="id">ИД</param>
        /// <returns>Паспорт</returns>
        public static Passport FindById(int id)
        {
            //Запрос
            string query = string.Format("SELECT * FROM `Passports` WHERE id={0}", id);
            //База данных
            MySqlWorker.DataBase db = SQL.SqlConnect.db;
            //Создает запрос и возвращает результат
            var list = db.MakeRequest<Passport>(query);
            //Если ничего не нашло, то возвращает ноль
            if (list.Count == 0)
            {
                return null;
            }
            //Если нашло, то создает объект пасспорта
            else
            {
                //создает объект паспорта
                Passport passport = list[0];
                //Возвращает должность
                return passport;
            }
        }
        /// <summary>
        /// Удаляет паспорт с базы данных
        /// </summary>
        public void Delete()
        {
            if (id != 0)
            {
                //База данных
                MySqlWorker.DataBase db = SQL.SqlConnect.db;
                //Строка-запрос
                string query = string.Format("DELETE FROM  `Passports` WHERE  `id` = {0}", id);
                //Создает запрос и возвращает результат
                db.MakeRequest(query);
            }
        }
    }
    
}
