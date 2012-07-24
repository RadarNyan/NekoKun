﻿using System;
using System.Collections.Generic;

using System.Windows.Forms;

namespace NekoKun
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        /// 
        public static string ProjectPath;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                ProjectPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
                while (true)
                {
                    string path = (System.IO.Path.Combine(System.IO.Path.Combine(ProjectPath, @"Game"), "Game.exe"));
                    if (System.IO.File.Exists(path))
                    {
                        ProjectPath = System.IO.Path.GetDirectoryName(System.IO.Path.GetFullPath(path));
                        break;
                    }
                    ProjectPath = System.IO.Directory.GetParent(ProjectPath).FullName;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("可以给我一个工程目录吃吗？", "NekoKun", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ProjectManager.OpenProject(
                System.IO.Path.Combine(
                    ProjectPath,
                    "Game.nkproj"
                )
            );

            Logger.Log("工程路径: {0}", ProjectPath);

            ToolStripManager.Renderer = new Office2007Renderer();

            Application.Run(Workbench.Instance);
        }

        public static LogFile Logger = new LogFile();

        public static object CreateInstanceFromTypeName(string typeName, params object[] param)
        {
            return System.AppDomain.CurrentDomain.CreateInstance(
                        System.Reflection.Assembly.GetExecutingAssembly().FullName,
                        typeName,
                        false,
                        System.Reflection.BindingFlags.CreateInstance,
                        null,
                        param,
                        null,
                        null,
                        null
                    ).Unwrap();
        }

        public static System.Drawing.Color ParseColor(string name)
        {
            System.Drawing.Color col = System.Drawing.Color.FromName(name);
            if (!col.IsKnownColor)
            {
                System.Text.RegularExpressions.Match match;
                match = System.Text.RegularExpressions.Regex.Match(name, @"#?([0-9a-fA-F]{2})([0-9a-fA-F]{2})([0-9a-fA-F]{2})#?");
                if (match.Success)
                {
                    return System.Drawing.Color.FromArgb(System.Convert.ToInt32(match.Groups[1].Value, 16), System.Convert.ToInt32(match.Groups[2].Value, 16), System.Convert.ToInt32(match.Groups[3].Value, 16));
                }
                match = System.Text.RegularExpressions.Regex.Match(name, @"#?([0-9a-fA-F]{2})([0-9a-fA-F]{2})([0-9a-fA-F]{2})([0-9a-fA-F]{2})#?");
                if (match.Success)
                {
                    return System.Drawing.Color.FromArgb(System.Convert.ToInt32(match.Groups[1].Value, 16), System.Convert.ToInt32(match.Groups[2].Value, 16), System.Convert.ToInt32(match.Groups[3].Value, 16), System.Convert.ToInt32(match.Groups[4].Value, 16));
                }
                return System.Drawing.Color.Empty;
            }
            else
                return System.Drawing.Color.FromName(name);
        }
    }
}