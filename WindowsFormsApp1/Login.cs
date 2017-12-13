﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using WebApplication2.Models;
using WebApplication2.CustomAttribute;

namespace WindowsFormsApp1
{
    public partial class Login : Form
    {
        static Uri baseUrl = new Uri("http://localhost:54962/");
        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {  
                string username = txtUsername.Text;
                string password = txtPass.Text;
                if (username == "")
                {
                    MessageBox.Show("Vui lòng nhập tên đăng nhập");
                }
                else if(password == "")
                {
                    MessageBox.Show("Vui lòng nhập mật khẩu");
                }
                else
                {
                    HttpClient httpClient = new HttpClient();
                    httpClient.BaseAddress = baseUrl;
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    LoginModel loginModel = new LoginModel();
                    loginModel.email = username;
                    loginModel.password = password;
                    HttpContent httpContent = new ObjectContent<LoginModel>(loginModel, new JsonMediaTypeFormatter());

                    var response = httpClient.PostAsync("api/account/login", httpContent).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("success");
                        this.Hide();
                        Product pr = new Product();
                        pr.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Đăng nhập thất bại");
                        username = "";
                        password = "";
                    }
                }
                
            }
            catch(Exception)
            {

            }
        }

        private void txtPass_TextChanged(object sender, EventArgs e)
        {
            txtPass.PasswordChar = '*';
        }
    }
}
