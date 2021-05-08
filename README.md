# Open Douban plugin for Jellyfin

This plugin is another implementation based on https://github.com/caryyu/douban-openapi-server without any `ApiKey` preset, which's originated from the plugin
 (https://github.com/Libitum/jellyfin-plugin-douban) and has already made a lot changes, ideally, these two plugins can coexist in Jellyfin, really appreciate the original author for providing a Douban-integrated plugin 

## Status

- [DONE] Movies
- [ToDo] TV Shows
- [In Progress] Movies - Refine `backdrop` from back-end

## Install Plugin

Only support `10.7.0` and above

Automatically:

- China: https://gitee.com/caryyu/jellyfin-plugin-repo/raw/master/manifest-cn.json
- U.S: https://github.com/caryyu/jellyfin-plugin-repo/raw/master/manifest-us.json

Manually:

```shell
dotnet build Jellyfin.Plugin.OpenDouban
```

Move `Jellyfin.Plugin.OpenDouban.dll` to `<jellyfin_config>/plugins/Jellyfin.Plugin.OpenDouban`

> Note: Don't forget the folder of `plugins/Jellyfin.Plugin.OpenDouban`, otherwise, Jellyfin won't load

