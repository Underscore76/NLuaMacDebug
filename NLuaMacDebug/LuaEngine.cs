using System;
using NLua;
using NLua.Exceptions;
using System.Text;

namespace NLuaMacDebug
{
    public class LuaEngine
    {
        public static int LoadOption = -1;

        public static Lua LuaState = null;
        public static void Init()
        {
            if (LuaState != null) LuaState.Close();
            LuaState = new Lua();
            LuaState.LoadCLRPackage();
        }

        public static void SetupImports()
        {
            Console.WriteLine($"Running option: {LoadOption}");
            switch(LoadOption)
            {
                case 0:
                    // works
                    LuaState.DoString(@"
                        import ('NLuaMacDebug')
                        import ('NLuaMacDebug.One')
                    ");
                    break;
                case 1:
                    // works
                    LuaState.DoString(@"
                        import ('NLuaMacDebug')
                        import ('NLuaMacDebug.One')
                    ");
                    LuaState.DoString(@"
                        import ('NLuaMacDebug.Two')
                    ");
                    break;
                case 2:
                    // crashes
                    LuaState.DoString(@"
                        import ('NLuaMacDebug')
                        import ('NLuaMacDebug.One')
                        import ('NLuaMacDebug.Two')
                    ");
                    break;
                case 3:
                    // works
                    LuaState.DoString(@"
                        import ('NLuaMacDebug')
                        import ('NLuaMacDebug.One')
                    ");
                    LuaState.DoString(@"
                        import ('NLuaMacDebug.Two')
                    ");
                    LuaState.DoString(@"
                        import ('NLuaMacDebug.Three')
                    ");
                    break;
                case 4:
                    // crashes
                    LuaState.DoString(@"
                        import ('NLuaMacDebug')
                        import ('NLuaMacDebug.One')
                    ");
                    LuaState.DoString(@"
                        import ('NLuaMacDebug.Two')
                        import ('NLuaMacDebug.Three')
                    ");
                    break;
                case 5:
                    // works
                    LuaState.DoString(@"import ('NLuaMacDebug')");
                    LuaState.DoString(@"import ('NLuaMacDebug.One')");
                    LuaState.DoString(@"import ('NLuaMacDebug.Two')");
                    LuaState.DoString(@"import ('NLuaMacDebug.Three')");
                    break;
                case -1:
                default:
                    // crashes
                    LuaState.DoString(@"
                        import ('NLuaMacDebug')
                        import ('NLuaMacDebug.One')
                        import ('NLuaMacDebug.Two')
                        import ('NLuaMacDebug.Three')
                    ");
                    break;
            }
        }

        public static string Reload()
        {
            try
            {
                Init();
                SetupImports();
                return "";
            }
            catch (LuaScriptException e)
            {
                string err = e.Message;
                if (e.InnerException != null)
                    err += "\n\t" + e.InnerException.Message;
                return err;
            }
            catch(Exception e)
            {
                string err = e.Message;
                if (e.InnerException != null)
                    err += "\n\t" + e.InnerException.Message;
                return err;
            }
        }

        public static string RunString(string command)
        {
            try
            {
                if (command == "reload")
                {
                    Reload();
                    return "";
                }
                string message = "";
                object[] results = LuaState.DoString(Encoding.ASCII.GetBytes(command));
                foreach (var r in results)
                {
                    if (r == null)
                        message += "null\t";
                    else
                        message += string.Format("{0}\t", r.ToString());
                }
                return message;

            }
            catch (LuaScriptException e)
            {
                return FormatError(e.Message, e.InnerException);
            }
            catch (TypeInitializationException e)
            {
                return FormatError(e.Message, e.InnerException);
            }
            catch (Exception e)
            {
                return FormatError(e.Message, e.InnerException);
            }
        }
        public static string FormatError(string message, Exception innerException)
        {
            string err = message;
            if (innerException == null)
                return err;
            string[] items = innerException.Message.Split(" ");
            string curr = "";
            foreach (var item in items)
            {
                if (curr.Length > 65)
                {
                    err += "\n\t" + curr;
                    curr = "";
                }
                curr += item + " ";
            }
            err += "\n\t" + curr;
            return err;
        }
    }
}

