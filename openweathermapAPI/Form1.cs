using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace openweathermapAPI
{
    public partial class Form1 : Form
    {

        private RootIPinfo IPinfo;
        private RootOpenweather Weather;
        private string IPinfoAPItoken = ""; // Your API token
        private string OpenweatherAPItoken = ""; // Your API token

        public Form1()
        {
            InitializeComponent();
            label1.BorderStyle = BorderStyle.None;
            label2.BorderStyle = BorderStyle.None;
            richTextBox1.BorderStyle = BorderStyle.None;
        }

        

        private RootIPinfo getIPinfo()
        {
            //https://ipinfo.io/account/home
            
            var webURL = $"https://ipinfo.io/?token={IPinfoAPItoken}";


            try
            {
                var json = new WebClient().DownloadString(webURL);

                RootIPinfo deserializedClass = JsonConvert.DeserializeObject<RootIPinfo>(json);

                return deserializedClass;

            }
            catch (WebException e)
            {
                MessageBox.Show(e.ToString());
                return null;
            }


        }


        private RootOpenweather GetRootOpenweather(string loc)
        {

            //loc = lat,lon
            string[] locSplit = loc.Split((char)44);

            var webURL = $"https://api.openweathermap.org/data/2.5/weather?lat={locSplit[0]}&lon={locSplit[1]}&appid={OpenweatherAPItoken}";


            try
            {
                var json = new WebClient().DownloadString(webURL);

                RootOpenweather deserializedClass = JsonConvert.DeserializeObject<RootOpenweather>(json);

                return deserializedClass;

            }
            catch (WebException e)
            {
                MessageBox.Show(e.ToString());
                return null;
            }

        }


        private void GetWeatherICO(string iconID)
        {
            //http://openweathermap.org/img/wn/10d@2x.png
            //https://openweathermap.org/weather-conditions#Icon-list

            var webURL = $"http://openweathermap.org/img/wn/{iconID}@2x.png";



            byte[] data = new WebClient().DownloadData(webURL);


            var bmp = new Bitmap(new MemoryStream(data));
            Bitmap resized = new Bitmap(bmp, new Size(bmp.Width / 2, bmp.Height / 2));
            label1.Image = resized;
            
            label1.ImageAlign = ContentAlignment.MiddleLeft;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Getdata();
        }


        private void Getdata()
        {

            richTextBox1.Clear();

            if (IPinfo == null)
            {
                IPinfo = getIPinfo();
            }


            
            if (IPinfo != null)
            {
                Weather = GetRootOpenweather(IPinfo.loc);

                if (Weather != null)
                {
                    GetWeatherICO(Weather.weather[0].icon);

                    //label1.Text = string.Format("{0} °C", KelvinCelsius(Weather.main.temp).ToString());

                    StringBuilder builder = new StringBuilder();



                    builder.AppendLine($"Lokalita : {Weather.name} ");


                    // tempconversion in class K->C

                    builder.AppendLine($"Teplota : {Weather.main.temp} °C");
                    builder.AppendLine($"Pocitova Teplota : {Weather.main.feels_like} °C");
                    builder.AppendLine($"MIN Teplota : {Weather.main.temp_min} °C");
                    builder.AppendLine($"MAX Teplota : {Weather.main.temp_max} °C");
                    builder.AppendLine($"Vlhkost : {Weather.main.humidity} %");
                    builder.AppendLine($"Tlak : {Weather.main.pressure} hPa");

                    builder.AppendLine($"Popis : {Weather.weather[0].description} ");


                    label1.Text = $"{Weather.main.temp} °C";
                    label1.TextAlign = ContentAlignment.MiddleCenter;
                    label2.Text = Weather.weather[0].description;

                    richTextBox1.AppendText(builder.ToString());

                    this.Text = $"Weather for {Weather.name}, IP : {IPinfo.ip}";
                }
            }
        }


        private double FahrenheitCelsius(double Fahrenheit)
        {
            return (Fahrenheit - 32) * 5 / 9;
        }

        
        private double KelvinCelsius(double Kelvin)
        {
            return Math.Round(Kelvin - 273.15 , 2);
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            Getdata();
        }
    }
}
