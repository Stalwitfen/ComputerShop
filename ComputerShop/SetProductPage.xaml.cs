﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ComputerShop
{

    public partial class SetProductPage : Page
    {
        private MySqlConnection conn;
        private int? CPC;   //current product code, to shorter
        private string productType;

        public SetProductPage(MySqlConnection conn, int currentProductCode)
        {
            InitializeComponent();

            this.conn = conn;
            CPC = currentProductCode; //to shorten

            // set label content

            if (CPC == 1 || (CPC >= 1000 && CPC < 2000))    //processor
            {
                productType = "processors";
                l_Char1.Content = "Ядер (шт)";
                l_Char2.Content = "Частота (ГГц)";
                l_Char3.Content = "Частота памяти (МГц)";
                l_Char4.Content = "Тип памяти";
            }
            else if (CPC == 2 || (CPC >= 2000 && CPC < 3000))   //RAM
            {
                productType = "RAM";
                l_Char1.Content = "Тип";
                l_Char2.Content = "Объём (Гб)";
                l_Char3.Content = "Частота (МГц)";
            }
            else if (CPC == 3 || (CPC >= 3000 && CPC < 4000))   //videocard
            {
                productType = "videocards";
                l_Char1.Content = "Тип видеопамяти";
                l_Char2.Content = "Объём (Гб)";
                l_Char3.Content = "Производитель видеочипа";
            }
            else if (CPC == 4 || (CPC >= 4000 && CPC < 5000))   //data storage
            {
                productType = "dataStorage";
                l_Char1.Content = "Тип";
                l_Char2.Content = "Объём (Гб)";
                l_Char3.Content = "Скорость чтения (Мб/с)";
                l_Char4.Content = "Скорость записи (Мб/с)";
            }

            // if this is a change to an existing product

            if (CPC > 4)
            {
                try
                {
                    string query = "SELECT * FROM " + productType + " WHERE code = " + CPC;
                    MySqlCommand command = new MySqlCommand(query, conn);
                    MySqlDataReader reader = command.ExecuteReader();
                    reader.Read();

                    tb_Model.Text = reader[1].ToString();
                    tb_Char1.Text = reader[2].ToString();
                    tb_Char2.Text = reader[3].ToString();
                    tb_Char3.Text = reader[4].ToString();

                    int a = 5;

                    if (productType == "processors" || productType == "dataStorage")
                    {
                        tb_Char4.Text = reader[a].ToString();
                        a++;
                    }
                    

                    tb_Manufacturer.Text = reader[a].ToString();
                    a++;
                    tb_Price.Text = reader[a].ToString();
                    a++;
                    cb_Availability.IsChecked = Convert.ToBoolean(reader[a]);

                    reader.Close();
                }

                catch (Exception e)
                {
                    WarningMessage.Show("Ошибка! " + e);
                }
            }
        }

        private void Btn_OK_Click(object sender, RoutedEventArgs e)
        {
            string model = tb_Model.Text;
            string char1 = tb_Char1.Text;
            string char2 = tb_Char2.Text;
            string char3 = tb_Char3.Text;
            string char4 = tb_Char4.Text;
            string manufacturer = tb_Manufacturer.Text;
            string price = tb_Price.Text;
            int availability = Convert.ToInt32(cb_Availability.IsChecked);

            string query="";
            int lastProductCode = Convert.ToInt32(new MySqlCommand("SELECT MAX(code) FROM " + productType, conn).ExecuteScalar());

            if (CPC == 1)
            {
                query = "INSERT INTO processors VALUES (" + (lastProductCode+1) + ", '"
                                                          + model + "', '"
                                                          + char1 + "', '"
                                                          + char2 + "', '"
                                                          + char3 + "', '"
                                                          + char4 + "', '"
                                                          + manufacturer + "', "
                                                          + price + ", "
                                                          + availability + ")";
            }
            //else if ()

            try
            {
                MySqlCommand command = new MySqlCommand(query, conn);
                command.ExecuteScalar();
            }
            catch (Exception err)
            {
                //WarningMessage.Show("Неверно введены данные!");
                WarningMessage.Show(err.ToString());
            }
        }
    }
}