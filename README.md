# Open Douban plugin for Jellyfin

This plugin is another implementation based on https://github.com/caryyu/douban-openapi-server without any `ApiKey` preset, which's originated from the plugin
 (https://github.com/Libitum/jellyfin-plugin-douban) and has already made a lot changes, ideally, these two plugins can coexist in Jellyfin, really appreciate the original author for providing a Douban-integrated plugin 

## Status

- [DONE] Movies
- [ToDo] TV Shows
- [In Progress] Movies - Refine `backdrop` from back-end

## Install Plugin

Only support `10.7.0` and above and manual installation at current presence:

```shell
dotnet build Jellyfin.Plugin.OpenDouban
cp Jellyfin.Plugin.OpenDouban/bin/Debug/net5.0/linux-64/Jellyfin.Plugin.OpenDouban.dll <Jellyfin_config>/plugins/Jellyfin.Plugin.OpenDouban/
```

> Note: Don't forget the folder of `plugins/Jellyfin.Plugin.OpenDouban`, otherwise, Jellyfin won't load

