# Docker tags API

This project aims to provide a simple to use API to list docker tags among different providers.

This project is also publicly hosted and available on `https://api.gerwim.com/dockertags/`

# Usage
The API requires two headers:
`registry` and `imageName`. A third option, `searchRegex` is optional and will return matching tags.


| Header  | Required | Value |
| ------------- | ------------- | ------------- |
| registry  | True  | string |
| imageName  | True  | string | 
| searchRegex  | False  | string |

## Supported registries
* hub.docker.com
* mcr.microsoft.com

## List all Ubuntu docker tags
```
curl 'https://api.gerwim.com/dockertags/v1/tags' \
--header 'registry: hub.docker.com' \
--header 'imageName: ubuntu'
```
## Regex examples

### List Ubuntu tags starting with `18.`
```
curl 'https://api.gerwim.com/dockertags/v1/tags' \
--header 'registry: hub.docker.com' \
--header 'imageName: ubuntu' \
--header 'searchRegex: ^18\.'
```
returns
```JSON
{
    "name": "ubuntu",
    "tags": [
        "18.04",
        "18.10"
    ]
}
```

### List base ASP.NET 4.8 images on `windowsservercore`
```
curl 'https://api.gerwim.com/dockertags/v1/tags' \
--header 'registry: mcr.microsoft.com' \
--header 'imageName: dotnet/framework/aspnet' \
--header 'searchRegex: ^4\.8-windowsservercore-(\d{4}|ltsc\d{4})'
```
 returns
```JSON
{
    "name": "dotnet/framework/aspnet",
    "tags": [
        "4.8-windowsservercore-1803",
        "4.8-windowsservercore-1903",
        "4.8-windowsservercore-1909",
        "4.8-windowsservercore-2004",
        "4.8-windowsservercore-2009",
        "4.8-windowsservercore-ltsc2016",
        "4.8-windowsservercore-ltsc2019"
    ]
}
```

# Running your private instance 
The docker image is publicly available on [DockerHub](https://hub.docker.com/r/gerwim/dockertagsapi).

By default, the results are cached in memory for 24 hours.
However, there's also support for the Cloudflare KV store. To use this, set the following environment variables:  
```
Cloudflare__KVUrl = https://api.cloudflare.com/client/v4/accounts/ACCOUNTID/storage/kv/namespaces/NAMESPACEID
Cloudflare__ApiToken = xxxx
```

If you want to run it with the default settings (in memory cache):
```
docker run -it --rm -p 80:80 gerwim/dockertagsapi:latest
```