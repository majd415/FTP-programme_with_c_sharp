using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;//new
using System.Linq;
using System.Net;//new 
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FTP_pro
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
        //add ftp sitting that we will use it
        struct FtpSetting//haecal baeanat
        {
            public string Server { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public string FileName { get; set; }
            public string FullName { get; set; }
        }
        FtpSetting _inputParameter;//add value after click on buttom 
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            string fileName = ((FtpSetting)e.Argument).FileName;
            string fullName = ((FtpSetting)e.Argument).FullName;
            string userNmae = ((FtpSetting)e.Argument).Username;
            string password = ((FtpSetting)e.Argument).Password;
            string server = ((FtpSetting)e.Argument).Server;
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(string.Format("{0}/{1}", server, fileName)));//create ftp recuest 
            request.Method = WebRequestMethods.Ftp.UploadFile;//select alterbute method 
            request.Credentials = new NetworkCredential(userNmae, password);//add sitting hosting username and password
            Stream ftpStream = request.GetRequestStream();//start ftp stream//tadafok
            FileStream fs = File.OpenRead(fullName);//get file and read it
            byte[] buffer = new byte[1024];//array buffer
            double total = (double)fs.Length;//add length file 
            int bytRead = 0;
            double read = 0;
            do
            { //hesab takadom progress par 
                if (!backgroundWorker1.CancellationPending)
                {
                    bytRead = fs.Read(buffer, 0, 1024);//read byte and rite in buffer matrix[] startpoint count 
                    ftpStream.Write(buffer, 0, bytRead);
                    read += (double)bytRead;
                    double percentage = read / total * 100;
                    backgroundWorker1.ReportProgress((int)percentage);
                }
            }
            while (bytRead != 0);
                fs.Close();
                ftpStream.Close();
            

        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lbpStatus.Text = $"Uploaded {e.ProgressPercentage} %";
            progressBar1.Value = e.ProgressPercentage;
            progressBar1.Update();

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lbpStatus.Text = "Upload Completed";
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            using(OpenFileDialog ofd=new OpenFileDialog() { Multiselect=false,ValidateNames=true,Filter="All files|*.*" })//select file by file dialog and select sitting this file
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    FileInfo fi = new FileInfo(ofd.FileName);//after that take the file and set information and sitting server to send  
                    _inputParameter.Username = txtUseName.Text;
                    _inputParameter.Password= txtPassword.Text;
                    _inputParameter.Server = txtServer.Text;
                    _inputParameter.FileName = fi.Name;
                    _inputParameter.FullName = fi.FullName;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
