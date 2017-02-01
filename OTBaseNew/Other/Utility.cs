using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTBaseNew.Other
{
    static class Utility
    {
        /// <summary>
        /// Конвертирует строку в дату, или возвращает нул если ошибка
        /// </summary>
        /// <param name="date">строка</param>
        /// <returns>дата</returns>
        static public DateTime? ConvertStringToDateTime(string date)
        {
            try
            {
                return Convert.ToDateTime(date);
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Убирает со строки все символы кроме цифры
        /// </summary>
        /// <param name="str">Строка</param>
        /// <returns>Готовая строка</returns>
        static public string ConvertStringToPhoneString(string str)
        {
            string number = "";
            foreach (var j in str)
            {
                try
                {
                    int a=Convert.ToInt32(j.ToString());
                    if ( a< 10 && a > -1)
                    {
                        number += j;
                    }
                }
                catch
                {

                }
            }
            return number;
        }
    }
}
