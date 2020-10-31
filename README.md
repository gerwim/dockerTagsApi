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
curl --location --request GET 'https://api.gerwim.com/dockertags/v1/tags' \
--header 'registry: hub.docker.com' \
--header 'imageName: ubuntu'
```
## Regex examples

### List Ubuntu tags starting with `18.`
```
curl --location --request GET 'https://api.gerwim.com/dockertags/v1/tags' \
--header 'registry: hub.docker.com' \
--header 'imageName: ubuntu'
--header 'searchRegex: ^18\.'
```

### List base ASP.NET 4.8 images on `windowsservercore`
```
curl --location --request GET 'https://api.gerwim.com/dockertags/v1/tags' \
--header 'registry: mcr.microsoft.com' \
--header 'imageName: dotnet/framework/aspnet' \
--header 'searchRegex: ^4\.8-windowsservercore-(\d{4}|ltsc\d{4})'
```

# Running your instance
The docker image is publicly available on [DockerHub](https://hub.docker.com/r/gerwim/dockertagsapi).

By default, the results are cached in memory for 24 hours.
However, there's also support for the Cloudflare KV store. To use this, set the following environment variables:  
```
Cloudflare:KVUrl = https://api.cloudflare.com/client/v4/accounts/ACCOUNTID/storage/kv/namespaces/NAMESPACEID
Cloudflare:ApiToken = xxxx
```