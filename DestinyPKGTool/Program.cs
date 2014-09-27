using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DestinyPKGTool
{
class Program
{

    static bool flag_verbose = false;
    static bool flag_ignorecomp = false;
    static PKG myParser = null;
    static string Me_WorkingPath = null;
    static string In_FilePath = null;
    static string Out_FolderPath = null;




    static void Main(string[] args)
    {
        Console.WriteLine(" ---Destiny PKGTool XBOX360/PS3 ---");
        Console.WriteLine(" ---Contributors: Cra0kalo, Aluigi, Patriot---");
        Console.WriteLine(" ---Fork from http://github.com/cra0kalo/DestinyPKGTool---");
        Console.WriteLine(string.Empty);
        CMDCheck();
        Console.WriteLine(string.Empty);
        Console.WriteLine("Press Enter to exit");
        Console.Read();


    }



    static public void CMDCheck()
    {


        // Get the values of the command line in an array
        // Index  Discription
        // 0      Full path of executing prograsm with program name
        // 1      First switch in command in your example
        string[] clArgs = Environment.GetCommandLineArgs();

        if (clArgs.Count() < 4 || clArgs.Count() > 6)
        {
            Console.WriteLine("Usage: pkgtool -p path/to/package.pkg path/to/outputfolder");
            Console.WriteLine("Optional: --ic (Ignore compressed entries)");
            Console.WriteLine("Example: pkgtool --ic -p O:/RamBox/360_ui_menus.pkg O:/RamBox/output");
        }
        else
        {

            Console.WriteLine("Starting...");
            flag_verbose = true;

            //set working path
            Me_WorkingPath = Path.GetDirectoryName(clArgs[0]);


            //check input -p followed by it's path
            int ac = 0;
            foreach (var arg in clArgs)
            {
                if (arg == "-p")
                {
                    In_FilePath = clArgs[ac + 1];
                    Out_FolderPath = clArgs[ac + 2];
                    break;
                }

                if (arg == "--ic")
                {
                    flag_ignorecomp = true;
                }


                ac += 1;
            }

            if (In_FilePath == "" || Out_FolderPath == "")
            {
                Console.WriteLine("Error check input arguments!");
                return;
            }



            //filechecks I/O
            if (!(File.Exists(In_FilePath)))
            {

                //maybe its in the working folder
                if (File.Exists(Path.Combine(Me_WorkingPath, In_FilePath)))
                {
                    In_FilePath = Path.Combine(Me_WorkingPath, In_FilePath);
                }
                else
                {
                    In_FilePath = string.Empty;
                    Console.WriteLine("Error Input file doesn't seem to exist!");
                    return;
                }
            }
            else
            {
                //working folder
                In_FilePath = Path.Combine(Me_WorkingPath, In_FilePath);
            }

            try
            {
                Cra0Utilz.CreatePath(Out_FolderPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return;
            }



            Console.WriteLine("----------------------------------------------------------------------------");
            Console.WriteLine("InputFile: " + Path.GetFileName(In_FilePath));
            Console.WriteLine("InputFilePath: " + In_FilePath);
            Console.WriteLine("ExportPath: " + Out_FolderPath);


            //Pass to class object to export
            VText("");
            myParser = new PKG(In_FilePath, Out_FolderPath,flag_ignorecomp);
            //Call export and finish
            myParser.DebugExport();
            //myParser.ParseExport();
        }
    }


    public static void VText(string text)
    {
        if (flag_verbose == true)
        {
            Console.WriteLine(text);
        }
    }


    public static void PError(string text)
    {
        Console.Clear();
        Console.WriteLine("----------------------------------------------------------------------------");
        Console.WriteLine("----------------------------------------------------------------------------");
        Console.WriteLine(text);
        Console.WriteLine("----------------------------------------------------------------------------");
        Console.WriteLine("----------------------------------------------------------------------------");
    }



}
}
