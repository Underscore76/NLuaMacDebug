# NLuaMacDebug - Basic Repo for NLua Crash

This repo contains an extremely dumb interactive lua console concept. Write text into the console and it'll run the associated lua, until the "exit" command is written.

Accepts a command line arg of type `int` to swap to different loading options which are different combinations of importing namespaces: see https://github.com/Underscore76/NLuaMacDebug/blob/8955b5b17df869e4695247fa330e450adc73814b/NLuaMacDebug/LuaEngine.cs#L23


It's a bit strange, it seems like individual imports work fine, but weird combos or too many imports in the same DoString will cause a crash.

* `dotnet run 0` -> works (load 2)
* `dotnet run 1` -> works (load 2, then load 1)
* `dotnet run 2` -> crashes (load 3)
* `dotnet run 3` -> works (load 2, 1, 1)
* `dotnet run 4` -> crashes (load 2, 2)
* `dotnet run 5` -> works (load 1, 1, 1, 1)
* `dotnet run` -> crashes (load 4)
