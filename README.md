Emby.Theater.Plugins
====================

This repository contains all of the Emby Theater plugins managed by the Emby community.

Each of the projects has a build event that copies it's output to the programdata-theater/plugins folder. 

By default this assumes you have the MBT repository side by side in a folder called 'MediaBrowser.Theater'. If this is not the case, or if you've installed the server than you'll need to update the build events manually in order to test code changes.


## More Information ##

[How to Build an MBT Plugin](https://github.com/MediaBrowser/MediaBrowser.Theater/wiki/Page-Development)
