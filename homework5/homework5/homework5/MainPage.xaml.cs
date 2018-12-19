using homework5.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace homework5
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        int temperaureType = 0;
        int searchType = 0;
        RootObject weather = null;
        XmlDocument document = null;
        string text;
        public MainPage()
        {
            this.InitializeComponent();
            var titleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            titleBar.BackgroundColor = Colors.Blue;
            titleBar.ButtonBackgroundColor = Colors.Blue;
        }

        private void showMessage()
        {
            if (weather != null || document != null)
            {
                if (searchType == 0)
                    showJsonMessage();
                else
                    showXmlMessage();
            }
        }

        private void FahrenheitRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            temperaureType = 1;
            showMessage();
        }

        private void DegreeCentigradeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            temperaureType = 0;
            showMessage();
        }

        private void StartQuery_Click(object sender, RoutedEventArgs e)
        {
            text = CityNameBox.Text;
            if (searchType == 0)
            {
                showJsonMessage();
            }
            else
            {
                showXmlMessage();
            }
        }

        private async void showXmlMessage()
        {
            try
            {
                document = await GetsojsonXml.GetWeather(text);
                if(document == null || document.ChildNodes[1].FirstChild.NodeName == "status")
                {
                    ResultTextBlock.Text = "您要查询的城市不在我们服务范围内";
                    return;
                }
                var w = document.SelectSingleNode("/resp/forecast/weather");
                double high = double.Parse(getTemprature(w.ChildNodes[1].InnerText));
                double low = double.Parse(getTemprature(w.ChildNodes[2].InnerText));
                if (temperaureType == 1)
                {
                    high = high * 9 / 5 + 32;
                    low = low * 9 / 5 + 32;
                }
                ResultTextBlock.Text = "CityName: " + document.SelectSingleNode("/resp/city").InnerText + "\nDate: " 
                    + w.FirstChild.InnerText + "\nTemprature: " + low.ToString() + "~" + high.ToString();
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                ResultTextBlock.Text = "您要查询的城市不在我们服务范围内";
            }
        }

        private async void showJsonMessage()
        {
            try
            {
                weather = await GetsojsonJson.getWeatherByCityName(text);
                double high = double.Parse(getTemprature(weather.data.forecast[0].high));
                double low = double.Parse(getTemprature(weather.data.forecast[0].low));
                if (temperaureType == 1)
                {
                    high = high * 9 / 5 + 32;
                    low = low * 9 / 5 + 32;
                }
                ResultTextBlock.Text = "CityName: " + weather.city + "\nDate: " + weather.date + "\nTemprature: " 
                    + low.ToString() + "~" + high.ToString();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                ResultTextBlock.Text = "您要查询的城市不在我们服务范围内";
            }
        }

        private string getTemprature(string str)
        {
            string first = str.Substring(3);
            int length = 0;
            string num = "0123456789";
            while (num.IndexOf(first[length]) != -1 || first[length] == '.')
            {
                length++;
            }
            string second = str.Substring(3, length);
            return second;
        }

        private void Xml_Checked(object sender, RoutedEventArgs e)
        {
            searchType = 1;
            if(document != null)
                showXmlMessage();
        }

        private void Json_Checked(object sender, RoutedEventArgs e)
        {
            searchType = 0;
            if (weather != null)
                showJsonMessage();
        }
    }
}
