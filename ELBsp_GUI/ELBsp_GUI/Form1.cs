using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Win32;
using System.Text.RegularExpressions;

namespace ELBsp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
           
            
        }
        about about;
        bool isnav = false;
        bool iscustomsave = false;
        string pypath;
        string lastsave;
        public static String GetFullPathWithoutExtension(String path)
        {
            return System.IO.Path.Combine(System.IO.Path.GetDirectoryName(path), System.IO.Path.GetFileNameWithoutExtension(path));
        }
        string Increment(string file)
        {
            int i = 0;
            string filewoext = GetFullPathWithoutExtension(file);
            
            string newfile = filewoext;
            string fileext = newfile + ".bsp";
            while(File.Exists(fileext))
            {

                newfile = filewoext + "_elbsp_" + Convert.ToString(i);
                fileext = newfile + ".bsp";
                i++;
            }
            return fileext;
        }
        private string GetCSGODir()
        {
            string steamPath = (string)Registry.GetValue("HKEY_CURRENT_USER\\Software\\Valve\\Steam", "SteamPath", "");
            string pathsFile = Path.Combine(steamPath, "steamapps", "libraryfolders.vdf");

            if (!File.Exists(pathsFile))
                return null;

            List<string> libraries = new List<string>();
            libraries.Add(Path.Combine(steamPath));

            var pathVDF = File.ReadAllLines(pathsFile);
            
            Regex pathRegex = new Regex(@"\""(([^\""]*):\\([^\""]*))\""");
            foreach (var line in pathVDF)
            {
                if (pathRegex.IsMatch(line))
                {
                    string match = pathRegex.Matches(line)[0].Groups[1].Value;

                    // De-Escape vdf. 
                    libraries.Add(match.Replace("\\\\", "\\"));
                }
            }

            foreach (var library in libraries)
            {
                string csgoPath = Path.Combine(library, "steamapps\\common\\Counter-Strike Global Offensive\\csgo\\maps");
                if (Directory.Exists(csgoPath))
                {
                    return csgoPath;
                }
            }

            return null;
        }
        public void runCLI(string inputfile,string outputfile)
        {
            System.Diagnostics.Process.Start(pypath, " -i " + "\"" + inputfile + "\"" + " -o " + "\"" + outputfile + "\"");
        }
        
        public void ExtractSaveResource(String filename, String location)
        {
            // Assembly assembly = Assembly.GetExecutingAssembly();           
            Assembly a = System.Reflection.Assembly.GetExecutingAssembly();
            
            // Stream stream = assembly.GetManifestResourceStream("Installer.Properties.mydll.dll"); // or whatever
            // string my_namespace = a.GetName().Name.ToString()
            Stream resFilestream = a.GetManifestResourceStream(filename);
            if (resFilestream != null)
            {
                BinaryReader br = new BinaryReader(resFilestream);
                FileStream fs = new FileStream(location, FileMode.Create); // say
                BinaryWriter bw = new BinaryWriter(fs);
                byte[] ba = new byte[resFilestream.Length];
                resFilestream.Read(ba, 0, ba.Length);
                bw.Write(ba);
                br.Close();
                bw.Close();
                resFilestream.Close();
                
            }
            // this.Close();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            about = new about();
            this.AllowDrop = true;
            pypath = Path.GetTempPath() + "/ELBsp_CLI.exe";
            ExtractSaveResource("ELBsp.ELBsp_CLI.exe", pypath);
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] filelist = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (iscustomsave)
            {
                using (SaveFileDialog sFileDialog = new SaveFileDialog())
                {
                    sFileDialog.InitialDirectory = GetCSGODir();
                    sFileDialog.Filter = "BSP Files (*.bsp) |*.bsp";
                    sFileDialog.FilterIndex = 0;
                    sFileDialog.RestoreDirectory = true;

                    if (sFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        runCLI(filelist[0], sFileDialog.FileName);
                        lastsave = sFileDialog.FileName;
                        
                        if (isnav)
                        {
                            string navfile = GetFullPathWithoutExtension(filelist[0]) + ".nav";
                            if (File.Exists(navfile))
                            {
                                string newnav = GetFullPathWithoutExtension(sFileDialog.FileName) + ".nav";
                                File.Copy(navfile , newnav);
                            } 
                        }
                    }
                }
            }
            else
            {
                string outfile = Increment(filelist[0]);
                runCLI(filelist[0], outfile);
                lastsave = outfile;
                
                if (isnav)
                {
                    string navfile = GetFullPathWithoutExtension(filelist[0]) + ".nav";
                    if (File.Exists(navfile))
                    {
                        string newnav = GetFullPathWithoutExtension(outfile) + ".nav";
                        File.Copy(navfile, newnav);
                    }
                }
            }
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            File.Delete(pypath);
        }

        private void customSave_CheckedChanged(object sender, EventArgs e)
        {
            iscustomsave = customSave.Checked;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void openButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = GetCSGODir();
                openFileDialog.Filter = "BSP Files (*.bsp) |*.bsp";
                openFileDialog.FilterIndex = 0;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    if(iscustomsave == true)
                    {
                        using (SaveFileDialog sFileDialog = new SaveFileDialog())
                        {
                            sFileDialog.InitialDirectory = GetCSGODir();
                            sFileDialog.Filter = "BSP Files (*.bsp) |*.bsp";
                            sFileDialog.FilterIndex = 0;
                            sFileDialog.RestoreDirectory = true;
                            
                            if (sFileDialog.ShowDialog() == DialogResult.OK)
                            {
                                runCLI(openFileDialog.FileName, sFileDialog.FileName);
                                lastsave = sFileDialog.FileName;
                                
                                if (isnav)
                                {
                                    string navfile = GetFullPathWithoutExtension(openFileDialog.FileName) + ".nav";
                                    if (File.Exists(navfile))
                                    {
                                        string newnav = GetFullPathWithoutExtension(sFileDialog.FileName) + ".nav";
                                        File.Copy(navfile, newnav);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        string outfile = Increment(openFileDialog.FileName);
                        runCLI(openFileDialog.FileName, outfile);
                        
                        if (isnav)
                        {
                            string navfile = GetFullPathWithoutExtension(openFileDialog.FileName) + ".nav";
                            if (File.Exists(navfile))
                            {
                                string newnav = GetFullPathWithoutExtension(outfile) + ".nav";
                                File.Copy(navfile, newnav);
                            }
                        }
                    }
                }
            }
        }

        private void navmeshbox_CheckedChanged(object sender, EventArgs e)
        {
            isnav = navmeshbox.Checked;
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            about.Show();
        }

        
    }
}
