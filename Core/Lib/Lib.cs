using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media;

namespace Weight
{
    public static class Lib
    {

        /// <summary>
        /// Получить бит из слова регистра
        /// </summary>
        /// <param name="word">Слово памяти регистра</param>
        /// <param name="position">Номер бита от 0 до 15</param>
        /// <returns>Значение бита по позиции</returns>
        public static bool GetBitWord(ushort word, int position)
        {
            bool result;
            ushort mask = (ushort)(1 << position); // Маска для извлечения конкретного бита
            result = (word & mask) != 0;
            return result;
        }

        /// <summary>
        /// Получить массив регистров из строки
        /// </summary>
        /// <param name="str">Строка для преобразования</param>
        /// <param name="len">Количество символов для преобразования</param>
        /// <returns>Массив регистров ushort[]</returns>
        public static ushort[] StringToReg(string str, int len)
        {
            int countReg = (len % 2 == 0) ? len / 2 : len / 2 - 1;
            ushort[] res = new ushort[countReg];

            for (int i = 0; i < countReg; i++)
            {
                if (i * 2 < str.Length)
                {
                    res[i] = (ushort)(str[i * 2] << 8);
                }

                if (i * 2 + 1 < str.Length)
                {
                    res[i] = (ushort)(res[i] | str[i * 2 + 1]);
                }
            }
            return res;
        }

        /// <summary>
        /// Получить строку из символьных регистров
        /// </summary>
        /// <param name="reg">Массив регистров</param>
        /// <param name="stIndex">Начальный адрес, с 0</param>
        /// <param name="length">Длинна строки</param>
        /// <returns>Строка формата string</returns>
        public static string RegToString(ushort[] reg, int stIndex, int length)
        {
            
            string str = "";
            int regCount = (length % 2 == 0) ? length / 2 : length / 2 - 1;
            regCount += stIndex;

            for (int i = stIndex; i < regCount; i++)
            {
                ushort s;
                if (reg[i] > 0)
                {
                    s = reg[i];
                    str = string.Concat(str, (char)(s >> 8));
                    str = string.Concat(str, (char)(s & 0x00FF));
                }                         
            }
            //Debug.Print($"str.Length = {str.Length}, str = {str}");
            return str;
        }

        /// <summary>
        /// Получить формат DateTime из int
        /// </summary>
        /// <param name="Y">Год</param>
        /// <param name="M">Месяц</param>
        /// <param name="D">День</param>
        /// <param name="hour">Часы</param>
        /// <param name="min">Минуты</param>
        /// <param name="sec">Секунды</param>
        /// <returns>Дата/Время в формате DateTime</returns>
        public static DateTime SetDateTime(int Y, int M, int D, int hour, int min, int sec)
        {
            if (Y > 1900 && Y < 3000 && M >= 1 && M <= 12 && D >= 1 && D <= 31)
            {
                if (hour >= 0 && Y <= 23 && min >= 0 && min <= 59 && sec >= 0 && sec <= 59)
                {
                    return new DateTime(Y, M, D, hour, min, sec);
                }
            }
            return new DateTime();
        }

        /// <summary>
        /// Из DCR в Int
        /// </summary>
        /// <param name="reg0">Младший регистр</param>
        /// <param name="reg1">Старший регистр</param>
        /// <returns>Int формат из двух регистров DCR</returns>
        public static int DcrToInt(ushort reg0, ushort reg1)
        {
            int intReg0 = reg0;
            int intReg1 = reg1;
            return (intReg0 << 16) | intReg1;
        }

        public static ushort[] UIntToDcrReg(uint value)
        {
            ushort[] registers = new ushort[2];           
            registers[0] = (ushort)(value >> 16);
            registers[1] = (ushort)(value & 0xFFFF);

            return registers;
        }

        /// <summary>
        /// Получить младший байт из слова
        /// </summary>
        /// <param name="register"></param>
        /// <returns>Младший byte из слова</returns>
        public static int GetLowByte(ushort register)
        {
            return register & 0xFF;
        }

        /// <summary>
        /// Получить старший байт из слова
        /// </summary>
        /// <param name="register"></param>
        /// <returns>Старший byte из слова</returns>
        public static int GetHiByte(ushort register)
        {
            return (register >> 8) & 0xFF;
        }

        /// <summary>
        /// Форат IP-адреса в виде строки
        /// </summary>
        /// <param name="register">Регистр адреса в uint</param>       
        /// <returns>Сетевой адрес в string</returns>
        public static string RegIpToString(uint register)
        {
            string str = (register >> 24 | 0xFF).ToString();
            str += ".";
            str += (register >> 16 | 0xFF).ToString();
            str += ".";
            str += (register >> 8 | 0xFF).ToString();
            str += ".";
            str += (register | 0xFF).ToString();
            return str;
        }


        /// <summary>
        /// Зеленый цвет для отображения
        /// </summary>
        public static IBrush MyGreen = new SolidColorBrush(Color.FromRgb(0, 255, 0));

        /// <summary>
        /// Красный цвет для отображения
        /// </summary>
        public static IBrush MyRed = new SolidColorBrush(Color.FromRgb(255, 0, 0));
    }
}
