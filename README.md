
[![AppVeyor build status](https://ci.appveyor.com/api/projects/status/github/dandevpc/dengine?branch=master&svg=true)](https://ci.appveyor.com/project/dandevpc/dengine/branch/master) [![Gitter](https://badges.gitter.im/dandevpc/gitter.svg)](https://gitter.im/dandevpc/engine)

# dEngine
An open source engine based on the ROBLOX API for standalone games. It's currently early in development.
## Requirements
[.NET Framework 4.6](https://www.microsoft.com/en-gb/download/details.aspx?id=48130)  
[Visual Studio 2015](https://beta.visualstudio.com/downloads/)
## Setup
1. [Download](https://github.com/DanDevPC/dEngine/archive/master.zip) or clone the repository.
2. Build the solution for `x64`.
3. Start dEditor.
4. Create a new project.  

You can load a ROBLOX XML place by typing ```workspace:LoadRbxlx("path/to/place.rbxlx") ``` into the command bar.

## Documentation
You can view the (incomplete) documentation for the Lua API at http://dandevpc.github.io/dEngine.
Use the `DumpAPI` configuration to update the `dump.json` in the [Docs](https://github.com/DanDevPC/dEngine/tree/master/docs) project.

## License
dEngine is a library licensed under the LGPL 3.0.  
dEditor is an application licensed under the GPL 3.0.
