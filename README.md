# Open Douban plugin for Jellyfin

This plugin amis to scrape `Douban` media informations for your self-hosted Jellyfin server, which relies on the project of https://github.com/caryyu/douban-openapi-server that uses `Web Spider` technology crawling public sites without any AuthN/AuthZ setup, however, this approach could be prevented by its `Rate-Limiting` mechanism of a certain period of time unable to visit.

> Note: This project initially comes from https://github.com/Libitum/jellyfin-plugin-douban, but both can coexist in the same Jellyfin server

# Discussion

[#jellyfin-odb:matrix.org](https://matrix.to/#/#jellyfin-odb:matrix.org)

## Install Plugin

### Versions Comparison

- Plugin Version `1.x.x` is for `Jellyfin 10.7.x` and ONLY supports `10.7.0-rc3` and above
- Plugin Version `2.x.x` is for `Jellyfin 10.8.x` and ONLY supports `10.8.0-alpha1` and above


Automatically:

- ~~China: https://gitee.com/caryyu/jellyfin-plugin-repo/raw/master/manifest-cn.json~~ (Gitee has shut its Pages down)
- U.S: https://github.com/caryyu/jellyfin-plugin-repo/raw/master/manifest-us.json

Manually:
 
```shell
dotnet build Jellyfin.Plugin.OpenDouban
```

Move `Jellyfin.Plugin.OpenDouban.dll` to `<jellyfin_config>/plugins/Jellyfin.Plugin.OpenDouban`

> Note: Don't forget the folder of `plugins/Jellyfin.Plugin.OpenDouban`, otherwise, Jellyfin won't load

## Docker Compose Reference

```yaml
version: "2"
services:
  doubanos:
    image: caryyu/douban-openapi-server:latest
    container_name: doubanos
    network_mode: "host"
    restart: "unless-stopped"

  jellyfin:
    image: jellyfin/jellyfin:10.7.5
    container_name: jellyfin
    user: <uid>:<gid>
    network_mode: "host"
    volumes:
      - <path>/jellyfin-config:/config
      - <path>/jellyfin-cache:/cache
      - <path>/media:/media
    restart: "unless-stopped"
```

# About deploy-exec

the `deploy-exec` script can be debugged by the following method:

```shell
docker run --rm -v $(pwd):/app -e SSH_PRIVKEY_GITHUB=$SSH_PRIVKEY_GITHUB -e SSH_PRIVKEY_GITEE=$SSH_PRIVKEY_GITEE -e TRAVIS_TAG=v1.0.3 --workdir /app caryyu-ubuntu:16.04 bash /app/deploy-exec
```

> ubuntu should have `git/jq/md5deep/zip` installed


<a href="https://www.buymeacoffee.com/caryyu" target="_blank"><img src="https://www.buymeacoffee.com/assets/img/custom_images/yellow_img.png" alt="Buy Me A Coffee" style="height: auto !important;width: auto !important;" ></a>

