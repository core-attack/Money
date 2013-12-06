using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.IO;
namespace Money
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Логирование сообщений
        /// </summary>
        enum Logger { INFO, DEBUG, ERROR };

        /// <summary>
        /// Хранимые данные
        /// </summary>
        string path = "data.txt";

        /// <summary>
        /// Используемая кодировка
        /// </summary>
        Encoding encoding = Encoding.UTF8;

        /// <summary>
        /// Определение типа разелителя дробной части числа
        /// </summary>
        System.Globalization.NumberFormatInfo format;

        public MainWindow()
        {
            InitializeComponent();
            download();
            format = new System.Globalization.NumberFormatInfo();
            format.NumberDecimalSeparator = ".";
            textBox1r.Focus();
        }

        bool ok = false;
        private void textBox10_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox)
            {
                if (!Regex.IsMatch(((TextBox)sender).Text, @"\D"))
                {
                        
                }
                else 
                {
                    setLog(Logger.ERROR, "Непонятное значение одной из ячеек", true); 
                }
            }
        }

        /// <summary>
        /// Возвращает конвертированное в целочиленное 16 битной число значение текстового поля
        /// </summary>
        /// <param name="sender">Текстовое поле</param>
        /// <returns></returns>
        int getInt16Value(object sender)
        {
            if (sender is TextBox)
            {
                if (!Regex.IsMatch(((TextBox)sender).Text, @"\D"))
                    return Convert.ToInt16(((TextBox)sender).Text, format);
                else
                    setLog(Logger.ERROR, String.Format("Число {0} имеет неверный формат", ((TextBox)sender).Text), true);
            }
            return -1;
        }


        /// <summary>
        /// Записывает в лог сообщение с пометкой логера
        /// </summary>
        /// <param name="l">Тип логера</param>
        /// <param name="s">Сообщение</param>
        void setLog(Logger l, string message, bool isView)
        {
            if (isView)
                richTextBoxLog.AppendText(l + ": " + message + "\n");
        }

        /// <summary>
        /// Возвращает сумму всех монет
        /// </summary>
        /// <returns></returns>
        double getSum()
        {
            
            return 10 * (getInt16Value(textBox10r) + getInt16Value(textBox10ru)) + 5 * getInt16Value(textBox5r) +
                2 * getInt16Value(textBox2r) + getInt16Value(textBox1r) + 0.5 * getInt16Value(textBox50k) +
                0.1 * getInt16Value(textBox10k) + 0.05 * getInt16Value(textBox5k) + 0.01 * getInt16Value(textBox1k);
        }

        

        private void textBox10ru_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                setLog(Logger.INFO, "Монет: ", true);
                setLog(Logger.INFO, "10 рублёвых юбилейных: " + getInt16Value(textBox10ru), true);
                setLog(Logger.INFO, "10 рублёвых: " + getInt16Value(textBox10r), true);
                setLog(Logger.INFO, "5 рублёвых: " + getInt16Value(textBox5r), true);
                setLog(Logger.INFO, "2 рублёвых: " + getInt16Value(textBox2r), true);
                setLog(Logger.INFO, "1 рублёвых: " + getInt16Value(textBox1r), true);
                setLog(Logger.INFO, "50 копеечных: " + getInt16Value(textBox50k), true);
                setLog(Logger.INFO, "10 копеечных: " + getInt16Value(textBox10k), true);
                setLog(Logger.INFO, "5 копеечных: " + getInt16Value(textBox5k), true);
                setLog(Logger.INFO, "1 копеечных: " + getInt16Value(textBox1k), true);
                setLog(Logger.INFO, "Итого имеем: " + getSum() + " рублей", true);
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            setLog(Logger.INFO, "Монет: ", true);
            setLog(Logger.INFO, "10 рублёвых юбилейных: " + getInt16Value(textBox10ru), true);
            setLog(Logger.INFO, "10 рублёвых: " + getInt16Value(textBox10r), true);
            setLog(Logger.INFO, "5 рублёвых: " + getInt16Value(textBox5r), true);
            setLog(Logger.INFO, "2 рублёвых: " + getInt16Value(textBox2r), true);
            setLog(Logger.INFO, "1 рублёвых: " + getInt16Value(textBox1r), true);
            setLog(Logger.INFO, "50 копеечных: " + getInt16Value(textBox50k), true);
            setLog(Logger.INFO, "10 копеечных: " + getInt16Value(textBox10k), true);
            setLog(Logger.INFO, "5 копеечных: " + getInt16Value(textBox5k), true);
            setLog(Logger.INFO, "1 копеечных: " + getInt16Value(textBox1k), true);
            setLog(Logger.INFO, "Итого имеем: " + getSum() + " рублей", true);
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            StreamWriter sr = new StreamWriter(path, true, encoding, 1000);
            sr.WriteLine(String.Format("10r={0},10ru={1},5r={2},2r={3},1r={4},50k={5},10k={6},5k={7},1k={8},sum={9}, date={10}",
                getInt16Value(textBox10r), getInt16Value(textBox10ru), getInt16Value(textBox5r), getInt16Value(textBox2r),
                getInt16Value(textBox1r), getInt16Value(textBox50k), getInt16Value(textBox10k), getInt16Value(textBox5k),
                getInt16Value(textBox1k), getSum(), DateTime.Now));
            sr.Close();
            setLog(Logger.INFO, "Данные сохранены.", true);
        }

        /// <summary>
        /// Загружает последние данные
        /// </summary>
        void download()
        {
            if (File.Exists(path))
            {
                setLog(Logger.INFO, "Загружаем данные последнего сеанса...", true);
                StreamReader sr = new StreamReader(path, encoding, false, 1000);
                string[] moneys = new string[9];
                while (!sr.EndOfStream)
                {
                    string s = sr.ReadLine();
                    if (s.IndexOf("10r") != -1)
                        moneys = s.Split(',');
                }
                textBox10r.Text = moneys[0].Split('=')[1];
                textBox10ru.Text = moneys[1].Split('=')[1];
                textBox5r.Text = moneys[2].Split('=')[1];
                textBox2r.Text = moneys[3].Split('=')[1];
                textBox1r.Text = moneys[4].Split('=')[1];
                textBox50k.Text = moneys[5].Split('=')[1];
                textBox10k.Text = moneys[6].Split('=')[1];
                textBox5k.Text = moneys[7].Split('=')[1];
                textBox1k.Text = moneys[8].Split('=')[1];
                setLog(Logger.INFO, "Данные загружены.", true);
                setLog(Logger.INFO, "Предыдущий результат: " + getSum() + " рублей", true);
            }
        }

        private void textBox1k_GotFocus(object sender, RoutedEventArgs e)
        {
            //if (sender is TextBox)
            //{
            //    ((TextBox)sender).SelectAll();
            //}
        }

    }
}
