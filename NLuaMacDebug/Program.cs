using System;
using NLua;

namespace NLuaMacDebug
{
    class Program
    {
        public static void Main(string[] args)
        {
            // swap options for different success/failure states
            if (args.Length > 0 && int.TryParse(args[0], out int result))
            {
                LuaEngine.LoadOption = result;
            }

            try
            {
                // try/catch does nothing, it just dumps out
                LuaEngine.Reload();
            }
            catch (Exception e)
            {
                Console.WriteLine("Caught load error");
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }

            Console.WriteLine("Basic Interactive Lua Console");
            while (true)
            {
                string val = Console.ReadLine();
                if (val == "exit") break;
                try
                {
                    string res = LuaEngine.RunString(val);
                    Console.WriteLine(res);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}

