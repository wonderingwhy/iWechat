using CE.iPhone.PList;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace iWechat
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            btnYes.IsEnabled = false;
            
        }
        static string fileName;
        static string dirName;
        static string WeChatName = "WeChat";
        static ZipClass zipClass = new ZipClass();
        static UnZipClass unzipClass = new UnZipClass();
        static int iconNum = 4;
        void Zip()
        {
            string route = dirName + "\\" + fileName;
            string files = route + @"\Payload%" + route + @"\iTunesArtwork%" + route + @"\iTunesMetadata.plist";
            string zipRoute = dirName + "\\" + Guid.NewGuid().ToString() + ".ipa";
            try
            {
                zipClass.Zip(files, zipRoute, "");
                zipClass.Zip(route + ".ipa", route, "");
            }
            catch (Exception ex)
            {
                MessageBox.Show("压缩错误");
            }
        }
        void Modify()
        {

            string src = dirName + "\\" + fileName + @"\Payload\WeChat.app\Info.plist";
            try
            {
                PListRoot root = PListRoot.Load(src);
                PListDict dic = (PListDict)root.Root;
                dic["CFBundleDisplayName"] = new PListString(WeChatName);
                dic["CFBundleIdentifier"] = new PListString(Guid.NewGuid().ToString());
                root.Root = dic;
                root.Save(src, PListFormat.Xml);
            }
            catch (Exception ex)
            {
                MessageBox.Show("修改失败");
            }
            if (iconNum != 3)
            {
                string route = dirName + "\\" + fileName + @"\Payload\WeChat.app";
                string desPath = route + @"\AppIcon60x60@2x.png";
                del(desPath, true);
                System.IO.File.Copy(System.AppDomain.CurrentDomain.BaseDirectory + @"img\" + iconNum.ToString() + ".png", desPath);
                string infoPlist = route + @"\zh_CN.lproj\InfoPlist.strings";
                del(infoPlist, true);
                ChangeFileName(route + @"\zh_CN.lproj\" + iconNum.ToString() + ".plist", route + @"\zh_CN.lproj\InfoPlist.strings");
            }
            
        }
        void UnZip()
        {
            string route = dirName + "\\" + fileName;
            try
            {
                if (!Directory.Exists(route))
                {
                    Directory.CreateDirectory(route);
                }
                FileInfo info = new FileInfo(route);
                info.Attributes = FileAttributes.Hidden;
                if (ZipArchive.UnZip2(route + ".ipa", route) == false)
                {
                    MessageBox.Show("解压错误");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("解压错误");
            }
        }
        void del(string path, bool isFile)
        {
            try
            {
                if (isFile)
                {
                    File.Delete(path);
                }
                else
                {
                    Directory.Delete(path, true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string str = TxtBox.Text;
            fileName = str.Substring(str.LastIndexOf("\\") + 1);
            fileName = fileName.Substring(0, fileName.Length - 4);
            dirName = str.Substring(0, str.LastIndexOf("\\"));
            //MessageBox.Show(fileName + " " + dirName);
            btnFind.IsEnabled = false;
            UnZip();
            Modify();
            Zip();
            del(dirName + "\\" + fileName, false);
            MessageBox.Show("修改成功");
            btnYes.IsEnabled = btnFind.IsEnabled = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog op = new Microsoft.Win32.OpenFileDialog();
            op.InitialDirectory = @"c:\";
            op.RestoreDirectory = true;
            op.Filter = "安装程序(*.ipa)|*.ipa";
            op.ShowDialog();
            TxtBox.Text = op.FileName;
            if (TxtBox.Text.Length > 0)
            {
                btnYes.IsEnabled = true;
            }
        }

        static BitmapImage img0 = new BitmapImage(new Uri("img/0.png", UriKind.Relative));
        static BitmapImage img1 = new BitmapImage(new Uri("img/1.png", UriKind.Relative));
        static BitmapImage img2 = new BitmapImage(new Uri("img/2.png", UriKind.Relative));
        static BitmapImage img3 = new BitmapImage(new Uri("img/3.png", UriKind.Relative));
        static BitmapImage img4 = new BitmapImage(new Uri("img/4.png", UriKind.Relative));
        static BitmapImage img5 = new BitmapImage(new Uri("img/5.png", UriKind.Relative));
        static BitmapImage img6 = new BitmapImage(new Uri("img/6.png", UriKind.Relative));
        static BitmapImage img7 = new BitmapImage(new Uri("img/7.png", UriKind.Relative));
        private void radio0_Checked(object sender, RoutedEventArgs e)
        {
            iconNum = 0;
            icon.Source = img0;
        }

        private void radio1_Checked(object sender, RoutedEventArgs e)
        {
            iconNum = 1;
            icon.Source = img1;
        }

        private void radio2_Checked(object sender, RoutedEventArgs e)
        {
            iconNum = 2;
            icon.Source = img2;
        }

        private void radio3_Checked(object sender, RoutedEventArgs e)
        {
            iconNum = 3;
            icon.Source = img3;
        }

        private void radio4_Checked(object sender, RoutedEventArgs e)
        {
            iconNum = 4;
            icon.Source = img4;
        }

        private void radio5_Checked(object sender, RoutedEventArgs e)
        {
            iconNum = 5;
            icon.Source = img5;
        }

        private void radio6_Checked(object sender, RoutedEventArgs e)
        {
            iconNum = 6;
            icon.Source = img6;
        }

        private void radio7_Checked(object sender, RoutedEventArgs e)
        {
            iconNum = 7;
            icon.Source = img7;
        }

        public bool ChangeFileName(string srcRelativePath, string desRelativePath)
        {
            try
            {
                if (File.Exists(desRelativePath))
                {
                    Directory.Delete(desRelativePath, true);
                }
                if (File.Exists(srcRelativePath))
                {
                    File.Move(srcRelativePath, desRelativePath);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
