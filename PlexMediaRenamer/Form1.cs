using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace PlexMediaRenamer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string DirPath=Application.StartupPath;
        string FileType;
        string IncConst;
        string SName;
        string SeasonNo;
        string OrigName;
        string NewName;
        string AutoDirPath;
        bool MultConst = false;
        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Increments mean what stays constant between 2 episodes' file names\nFor example The Flash Season 4:\n\nTheFlash.4.1.mkv\nTheFlash.4.2.mkv\nTheFlash.4.3.mkv\n\n " +
                "In this case, the increment is TheFlash.4. as this is what stays constant between different file names\nYou can also use 2 increments, separated by |, for example:\n\n" +
                "TheFlash.4.1.1080p.mkv\nTheFlash.4.2.1080p.mkv\nTheFlash.4.3.1080p.mkv\n\n In this case, the increment would be TheFlash.4.|.1080p");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                DirPath = folderBrowserDialog1.SelectedPath;
                label6.Text = "Directory: " + DirPath;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SName = textBox1.Text;
            SeasonNo = textBox2.Text;
            IncConst = textBox3.Text;
            FileType = textBox4.Text;
            string[] output1;
            if (IncConst.Contains("|"))
            {
                MultConst = true;
                char splitter = '|';
                output1 = IncConst.Split(splitter);
                OrigName = output1[0] + "1" + output1[1] + "." + FileType;
                if(File.Exists(DirPath + "\\" + OrigName))
                {
                    MessageBox.Show("Files verified, can now rename!");
                    NewName = SName + " - " + "S" + SeasonNo + "E01." + FileType;
                    label5.Text = "Output: " + OrigName + "  -----> " + NewName;
                }
                else
                {
                    OrigName = output1[0] + "01" + output1[1] + "." + FileType;
                    if(File.Exists(DirPath + "\\" + OrigName))
                    {
                        MessageBox.Show("Files verified, can now rename!");
                        NewName = SName + " - " + "S" + SeasonNo + "E01." + FileType;
                        label5.Text = "Output: " + OrigName + "  -----> \n       " + NewName;
                    }
                    else
                    {
                        MessageBox.Show("Error, files not found with parsed name");
                    }
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string EpParser1 = "E";
            string EpParser2 = "EP";
            string SParser1 = "S0";
            string SParser2 = "S";
            bool FoundSeason = false;
            bool FoundPattern = false;
            DirectoryInfo BaseDir = new DirectoryInfo(AutoDirPath);
            FileInfo[] DirFiles = BaseDir.GetFiles();
            bool FoundEpisodes = false;
            bool FoundEpisodesPattern = false;
            bool FirstEp = true;
            bool ConsistEp = false;
            int CorrectEp = 0;
            string EpMethod="";
            string DoubleDigitEpMethod = "";
            string SeasonMethod ="";
            int NEpisodes = 0;
            int SeasonNo = 0;
            int CorrectSeason=0;
            foreach (FileInfo eachfile in DirFiles)
            {
                bool WorkedInEp = false;
                if (FirstEp == true)
                {
                    FirstEp = false;
                }
            StartGetPattern:;
                if (FoundEpisodesPattern == false)
                {
                    for (int count = 10; count < 99; count++)
                    {
                        if (eachfile.Name.Contains(EpParser1 + "" + count))
                        {
                            FoundEpisodesPattern = true;
                            DoubleDigitEpMethod = "1,none";
                        }
                        if (eachfile.Name.Contains(EpParser2 + "" + count))
                        {
                            FoundEpisodesPattern = true;
                            DoubleDigitEpMethod = "2,none";
                        }

                    }
                    //MessageBox.Show("double digit ep method is " + DoubleDigitEpMethod);
                    if (FoundEpisodesPattern == true)
                    {
                        goto EndGetPattern;
                    }
                    for (int count = 1; count < 10; count++)
                    {
                        if (eachfile.Name.Contains(EpParser1 + "0" + count))
                        {
                            FoundEpisodesPattern = true;
                            EpMethod = "1,0";
                        }
                        if (eachfile.Name.Contains(EpParser1 + "" + count))
                        {
                            FoundEpisodesPattern = true;
                            EpMethod = "1,none";
                        }
                        if (eachfile.Name.Contains(EpParser2 + "0" + count))
                        {
                            FoundEpisodesPattern = true;
                            EpMethod = "2,0";
                        }
                        if (eachfile.Name.Contains(EpParser2 + "" + count))
                        {
                            FoundEpisodesPattern = true;
                            EpMethod = "2,none";
                        }
                        
                    }
                    //MessageBox.Show("ep pattern is " + EpMethod);
                EndGetPattern:;
                }
                else
                {
                    if (EpMethod == "1,0")
                    {
                        for (int count = 1; count < 11; count++)
                        {
                            if(eachfile.Name.Contains(EpParser1 + "0" + count))
                            {
                                CorrectEp = CorrectEp + 1;
                                WorkedInEp = true;
                            }
                        }
                    }
                    if (EpMethod == "1,none")
                    {
                        for (int count = 1; count < 11; count++)
                        {
                            if (eachfile.Name.Contains(EpParser1 + "" + count))
                            {
                                CorrectEp = CorrectEp + 1;
                                WorkedInEp = true;
                            }
                        }
                    }
                    if (EpMethod == "2,0")
                    {
                        for (int count = 1; count < 11; count++)
                        {
                            if (eachfile.Name.Contains(EpParser2 + "0" + count))
                            {
                                CorrectEp = CorrectEp + 1;
                                WorkedInEp = true;
                            }
                        }

                    }
                    if (EpMethod == "2,none")
                    {
                        for (int count = 1; count < 11; count++)
                        {
                            if (eachfile.Name.Contains(EpParser1 + "0" + count))
                            {
                                CorrectEp = CorrectEp + 1;
                                WorkedInEp = true;
                            }
                        }
                    }
                    if (DoubleDigitEpMethod == "1,none")
                    {
                        for(int count = 10; count < 100; count++)
                        {
                            if (eachfile.Name.Contains(EpParser1 + "" + count))
                            {
                                CorrectEp = CorrectEp + 1;
                                WorkedInEp = true;
                            }
                        }
                    }
                    if (DoubleDigitEpMethod == "2,none")
                    {
                        for (int count = 10; count < 100; count++)
                        {
                            if (eachfile.Name.Contains(EpParser2 + "" + count))
                            {
                                CorrectEp = CorrectEp + 1;
                                WorkedInEp = true;
                            }
                        }
                    }
                    if (WorkedInEp == false)
                    {
                       // MessageBox.Show("maybe >10 now..." + eachfile.Name);
                        FoundEpisodesPattern = false;
                        goto StartGetPattern;
                    }
                }
                
            }
            if (CorrectEp > 2)
            {
                foreach(FileInfo eachfile in DirFiles)
                {
                    bool AddedEpisode = false;
                    if (DoubleDigitEpMethod == "1,none")
                    {
                       // MessageBox.Show("using 1,none double");
                        for (int count = 10; count < 100; count++)
                        {
                            if (eachfile.Name.Contains(EpParser1 + "" + count))
                            {
                                NEpisodes = NEpisodes + 1;
                                AddedEpisode = true;
                                //MessageBox.Show("found ep " + eachfile.Name);
                                //WorkedInEp = true;
                            }
                        }
                    }
                    if (DoubleDigitEpMethod == "2,none")
                    {
                        //MessageBox.Show("using 2,none double");
                        for (int count = 10; count < 100; count++)
                        {
                            if (eachfile.Name.Contains(EpParser2 + "" + count))
                            {
                                NEpisodes = NEpisodes + 1;
                                AddedEpisode = true;
                                //MessageBox.Show("found ep " + eachfile.Name);
                                //WorkedInEp = true;
                            }
                        }
                    }
                    if (AddedEpisode == true)
                    {
                        goto EndAddEpisode;
                    }
                    if (EpMethod == "1,0")
                    {
                        //MessageBox.Show("using 1,0 single");
                        for (int count = 1; count < 10; count++)
                        {
                            if (eachfile.Name.Contains(EpParser1 + "0" + count))
                            {
                                NEpisodes = NEpisodes + 1;
                                //MessageBox.Show("found ep " + eachfile.Name);
                                //WorkedInEp = true;
                            }
                        }
                    }
                    if (EpMethod == "1,none")
                    {
                        //MessageBox.Show("using 1,none single");
                        for (int count = 1; count < 10; count++)
                        {
                            if (eachfile.Name.Contains(EpParser1 + "" + count))
                            {
                                NEpisodes = NEpisodes + 1;
                                //MessageBox.Show("found ep " + eachfile.Name);
                                //WorkedInEp = true;
                            }
                        }
                    }
                    if (EpMethod == "2,0")
                    {
                        MessageBox.Show("using 2,0 single");
                        for (int count = 1; count < 10; count++)
                        {
                            if (eachfile.Name.Contains(EpParser2 + "0" + count))
                            {
                                NEpisodes = NEpisodes + 1;
                                //MessageBox.Show("found ep " + eachfile.Name);
                                //WorkedInEp = true;
                            }
                        }
                    }
                    if (EpMethod == "2,none")
                    {
                        MessageBox.Show("using 2,none single");
                        for (int count = 1; count < 10; count++)
                        {
                            if (eachfile.Name.Contains(EpParser2 + "" + count))
                            {
                                NEpisodes = NEpisodes + 1;
                                //MessageBox.Show("found ep " + eachfile.Name);
                                //WorkedInEp = true;
                            }
                        }
                    }
                EndAddEpisode:;
                    
                }
            }
            //MessageBox.Show(NEpisodes.ToString() + " " + EpMethod + " " + DoubleDigitEpMethod + " correct ep " + CorrectEp.ToString());
            foreach(FileInfo eachfile in DirFiles)
            {
                if (FoundSeason == false)
                {
                    bool GotSeason = false;
                    for(int count = 10; count < 99; count++)
                    {
                        if (eachfile.Name.Contains(SParser2 + count))
                        {
                            FoundSeason = true;
                            SeasonMethod = "10+,2";
                            GotSeason = true;
                            SeasonNo = count;
                        }
                    }
                    if (GotSeason == true)
                    {
                        goto EndFindSeason;
                    }
                    for(int count = 1; count < 10; count++)
                    {
                        if(eachfile.Name.Contains(SParser1 + count))
                        {
                            FoundSeason = true;
                            SeasonMethod = "<10,1";
                            SeasonNo = count;
                        }
                        if (eachfile.Name.Contains(SParser2 + count))
                        {
                            FoundSeason = true;
                            SeasonMethod = "<10,2";
                            SeasonNo = count;
                        }
                    }
                EndFindSeason:;
                }
                else
                {
                    if (SeasonMethod == "10+,2")
                    {
                        for(int count = 10; count < 100; count++)
                        {
                            if(eachfile.Name.Contains(SParser2 + count))
                            {
                                CorrectSeason = CorrectSeason + 1;
                            }
                        }
                    }
                    if (SeasonMethod == "<10,1")
                    {
                        for (int count = 1; count < 10; count++)
                        {
                            if (eachfile.Name.Contains(SParser1 + count))
                            {
                                CorrectSeason = CorrectSeason + 1;
                            }
                        }
                    }
                    if (SeasonMethod == "<10,2")
                    {
                        for (int count = 1; count < 10; count++)
                        {
                            if (eachfile.Name.Contains(SParser2 + count))
                            {
                                CorrectSeason = CorrectSeason + 1;
                            }
                        }
                    }
                }
            }
            //MessageBox.Show("correct files with season: + 1 " + CorrectSeason.ToString() + ". This is season " + SeasonNo.ToString());
            label12.Text = "Season: " + SeasonNo.ToString();
            label13.Text = "No of episodes: " + NEpisodes.ToString();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                AutoDirPath = folderBrowserDialog1.SelectedPath;
                label11.Text = "Directory: " + AutoDirPath;
            }
        }
    }
}
