using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clang_wrapper
{
    class Program
    {
        struct Compiler
        {
            public string cxx;
            public string cc;

            public static implicit operator Compiler(bool isGcc)
            {
                if (isGcc)
                {
                    return new Compiler() { cxx = "g++_.exe", cc = "gcc_.exe" };
                }
                else
                {
                    return new Compiler() { cxx = "clang++_.exe", cc = "clang_.exe" };
                }
            }
        }
        static void RunClang(string fullArg, bool isCompilerCxx, Compiler compiler)
        {
            string currentDir = AppContext.BaseDirectory;

            Process clang = new Process();
            if (isCompilerCxx)
            {
                clang.StartInfo.FileName = currentDir + compiler.cxx;
            }
            else
            {
                clang.StartInfo.FileName = currentDir + compiler.cc;
            }
            clang.StartInfo.Arguments = fullArg;
            clang.StartInfo.UseShellExecute = false;
            clang.StartInfo.RedirectStandardOutput = false;
            try
            {
                clang.Start();
                clang.WaitForExit();
            }
            catch (Exception ex)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
        }
        static void Main(string[] args)
        {
            bool flto = false;

            bool isCompilerCxx = AppDomain.CurrentDomain.FriendlyName.Contains("++") ||
                                 AppDomain.CurrentDomain.FriendlyName.Contains("cpp");

            bool isGcc = !AppDomain.CurrentDomain.FriendlyName.Contains("clang") &&
                         AppDomain.CurrentDomain.FriendlyName.Contains("g++") ||
                         AppDomain.CurrentDomain.FriendlyName.Contains("gcc") ||
                         AppDomain.CurrentDomain.FriendlyName.Contains("cc") ||
                         AppDomain.CurrentDomain.FriendlyName.Contains("cpp") ||
                         AppDomain.CurrentDomain.FriendlyName.Contains("c++");

            string fullArg = null;

            string optimizationArg = "-Ofast -msse4.2 -mpclmul -mbmi2 -mpopcnt -mlzcnt -mvzeroupper -mavx2 -mfma -maes -mtune=haswell -ffast-math -funroll-loops";
            if (!isGcc)
            {
                optimizationArg += "-fvectorize";
            }
            Compiler compiler = isGcc;

            if (File.Exists(AppContext.BaseDirectory + "clang_enable_lto"))
            {
                flto = true;
            }
            if(File.Exists(AppContext.BaseDirectory + "compiler_flags.txt"))
            {
                try
                {
                    optimizationArg = File.ReadLines("compiler_flags.txt").First();
                }
                catch { }
            }

            foreach(string arg in args)
            {
                if(arg.Contains("-O2") || arg.Contains("-O3"))
                {
                    fullArg += $" {optimizationArg} ";
                    if(flto && !isGcc) 
                        fullArg += " -flto=full ";
                }
                else
                {
                    fullArg += " " + arg;
                }
            }

            RunClang(fullArg, isCompilerCxx, compiler);
        }
    }
}
