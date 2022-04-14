## Documentation

See the API docs <a href="https://niggo1243.github.io/Unity3DHelperTools/annotated.html">here</a>

## Setup

### Unity Package Dependency 

To add this toolkit as a package dependency to your Unity project, 
locate your manifest file in "Package/manifest.json".

First you need to add the "scopedRegistry" information to the manifest 
in order to resolve dependencies used by this toolkit.

The current dependency is a fork with performance improvements (https://github.com/niggo1243/NaughtyAttributes) of the original open-source project NaughtyAttributes by dbrizov:
https://github.com/dbrizov/NaughtyAttributes

Add this snipped somewhere adjacent to the "dependencies" section in the manifest.json:

```
"scopedRegistries": 
[
    {
        "name": "NaughtyAttributesPerfFork",
        "url": "https://upm-packages.dev",
        "scopes": [
        "com.nikosassets.naughtyattributes"
        ]
    }
]
```

Lastly add the following line to the "dependencies" section:

"com.nikosassets.u3dhelpertools": "https://github.com/niggo1243/Unity3DHelperTools.git#upm"

You can also choose specific releases and tags after the "#" instead of "upm".

The final result should look something like this in your manifest.json:

```
{
    "scopedRegistries": 
    [
        {
            "name": "NaughtyAttributesPerfFork",
            "url": "https://upm-packages.dev",
            "scopes": [
            "com.nikosassets.naughtyattributes"
            ]
        }
    ], 
    "dependencies" 
    {
        "com.nikosassets.u3dhelpertools": "https://github.com/niggo1243/Unity3DHelperTools.git#upm"
    }
}
```
Or alternatively (without git urls):

```
{
    "scopedRegistries": 
    [
        {
            "name": "NaughtyAttributesPerfFork",
            "url": "https://upm-packages.dev",
            "scopes": [
            "com.nikosassets.naughtyattributes"
            ]
        },
        {
            "name": "Unity3DHelperTools",
            "url": "https://upm-packages.dev",
            "scopes": [
            "com.nikosassets.u3dhelpertools"
            ]
        }
    ], 
    "dependencies" 
    {
        "com.nikosassets.u3dhelpertools": "1.0.0"
    }
}
```

### Unity Project

You can simply download a (release) zip file or just clone this project via the git command: 

```
git clone --recursive https://github.com/niggo1243/Unity3DHelperTools.git
```

## Samples

WIP



