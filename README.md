# Player Sharp

> ⚠️**Player is still in the early phase of development. Nothing in this project is fixed, there are many implicit and explicit TODOs and it is most likely that everything will change**


![sharp-banner](https://user-images.githubusercontent.com/22715034/159367266-2bbd5630-496a-4f5e-b38b-5fd246125c9b.jpg)
<sub>(Photo (Modified) by <a href="https://unsplash.com/@wisdomsky?utm_source=unsplash&utm_medium=referral&utm_content=creditCopyText">Julian Paolo Dayag</a> on <a href="https://unsplash.com/?utm_source=unsplash&utm_medium=referral&utm_content=creditCopyText">Unsplash</a>)</sub>

## 🎯 Concepts / Goals
- **No external database:** everything should be kept In-Memory or using a file-based store. At the moment mainly [SQLite](https://www.sqlite.org/) is used, as it gives some insights into the entity framework. Oh, and also it's [faster than the file system](https://www.sqlite.org/fasterthanfs.html).
Without a external DB the player itself is ephemeral with each deployment unless a persistent claim is used - but that is okay for a player. 
- **Rich UI:** At least the map should be visualized using a rich UI. Conceptually the map is treated as a graph structure and could also be visualized this way. Libraries like [D3.js](https://d3js.org/) could be used. How the UI will be presented is still unclear. React is used as UI Framework but I'm also thinking about some kind of static webpage using [Graphviz](https://graphviz.org/) as it would also be fun. 
- **Websockets:** If the player uses any kind of presentation (Logs, UI or similar) websockets should be used.

## 🚩 Prerequisites
- .NET (Core) 6 and .NET CLI

**or**

- Docker

## 🔧 Configuring

❌ Not Yet Implemented

Production Config: [appsettings.json](./src/Sharp.Player/appsettings.json)

Development Config: [appsettings.Development.json](./src/Sharp.Player/appsettings.Development.json)

## ▶️Running

Either use the dotnet CLI or the provided docker image. It is as easy as:
```sh
$ docker build -t player-sharp . && docker run player-sharp
```
or using the dotnet CLI:
```sh
$ dotnet run --project .\src\Sharp.Player
```