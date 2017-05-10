using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using MobileAppUnitTestWithAppium.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Remote;

namespace UnitTestProject
{
    [TestFixture]
    public class UnitTest1
    {
        //宣告 Appium Driver，並指定使用 Android Element
        AppiumDriver<AndroidElement> _driver;

        [Test]
        public void TestMobileApp()
        {
            DesiredCapabilities desiredCapabilities = new DesiredCapabilities();

            //指定平台為安卓
            desiredCapabilities.SetCapability( MobileCapabilityType.PlatformName , MobilePlatform.Android );

            //指定裝置名稱，裝置名稱可以透過在 Tools -> Android Adb Command Prompt... 中輸入 adb devices -l 取得
            desiredCapabilities.SetCapability( MobileCapabilityType.DeviceName , "kate" );

            //指定要測試的 App，基本上就是 Android 專案的名稱
            desiredCapabilities.SetCapability( "appPackage" , "MobileAppUnitTestWithAppium.Android" );

            //指定 App 的 MainActivity，這個值可以在 Android 專案下的 obj\Debug\android\AndroidManifest.xml 檔裡面找到
            desiredCapabilities.SetCapability( "appActivity" , "md566d58bce9157a88432d9c294e8892f90.MainActivity" );

            //建立 AppiumDriver 的 Instance ，並指定 Appium Server 的路徑
            _driver = new AndroidDriver<AndroidElement>( new Uri( "http://127.0.0.1:4723/wd/hub" ) , desiredCapabilities );


            AndroidElement listView = _driver.FindElementById( "listView" );

            var listViewItems = listView.FindElementsByClassName( "android.widget.LinearLayout" );

            IEnumerable<Book> expectedBooks = null;

            var directory = Path.GetDirectoryName( System.Reflection.Assembly.GetExecutingAssembly().Location );

            using( StreamReader file = File.OpenText( $"{directory}\\Books.json" ) )
            {
                string jsonString = file.ReadToEnd();
                expectedBooks = JsonConvert.DeserializeObject<IEnumerable<Book>>( jsonString );
            }

            Size size = _driver.Manage().Window.Size;

            //抓取螢幕高度的中心點
            int screenHeightStart = (int) (size.Height * 0.5);

            foreach(var listViewItem in listViewItems )
            {
                //取得目前抓到的項目
                Book actualBook = new Book
                {
                    Name = listViewItem.FindElementById( "txtName" ).Text ,
                    Author = listViewItem.FindElementById( "txtAuthor" ).Text ,
                    Price = Double.Parse( listViewItem.FindElementById( "txtPrice" ).Text )
                };

                Book expectedBook = expectedBooks.First( b => b.Name == actualBook.Name );

                Assert.AreEqual( expectedBook.Name , actualBook.Name );

                Assert.AreEqual( expectedBook.Author , actualBook.Author );

                Assert.AreEqual( expectedBook.Price , actualBook.Price );

                listViewItem.Click();

                actualBook = new Book
                {
                    Name = _driver.FindElementById( "txtName" ).Text ,
                    Author = _driver.FindElementById( "txtAuthor" ).Text ,
                    Isbn = _driver.FindElementById( "txtIsbn" ).Text ,
                    Price = Double.Parse( _driver.FindElementById( "txtPrice" ).Text ) ,
                    ReleaseDate = DateTime.Parse( _driver.FindElementById( "txtReleaseDate" ).Text ) ,
                };

                Assert.AreEqual( expectedBook.Name , actualBook.Name );

                Assert.AreEqual( expectedBook.Author , actualBook.Author );

                Assert.AreEqual( expectedBook.Isbn , actualBook.Isbn );

                Assert.AreEqual( expectedBook.ReleaseDate , actualBook.ReleaseDate );

                Assert.AreEqual( expectedBook.Price , actualBook.Price );

                //找到返回上一頁的按鈕
                AndroidElement backButton = _driver.FindElementByClassName( "android.widget.ImageButton" );

                //點選返回上一頁的按鈕
                backButton.Click();

                //每次都把 ListView 往上滑一點點 
                _driver.Swipe( 0 , screenHeightStart , 0 , screenHeightStart - listViewItem.Size.Height / 2 , 1000 );
            }
        }

        [TearDown]
        public void TearDown()
        {
            _driver?.CloseApp();
        }

    }
}
