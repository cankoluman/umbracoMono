umbracoMono
===========

umbraco port to Mono

### Welcome to the umbraco port for mono.

Please have a look at the wiki: 
https://github.com/cankoluman/umbracoMono/wiki

This site replaces the old repository at:
https://github.com/cankoluman/umbraco-mono

The development blog is at:
http://www.cankoluman.com

You can find work on each ported (or porting) version of Umbraco
as a branch, 
e.g. 4.7.2 -> https://github.com/cankoluman/umbracoMono/tree/u4.7.2

# Branch notes
rev 2fdbb90 - This branch should now compile in the Monodevelop (v 3.x) IDE, with mono
v. 3.x

>mono -V 
Mono JIT compiler version 3.0.3 (master/a26a1f8 Sat Jan  5 12:49:42 GMT 2013)
Copyright (C) 2002-2012 Novell, Inc, Xamarin Inc and Contributors. www.mono-project.com
	TLS:           __thread
	SIGSEGV:       altstack
	Notifications: epoll
	Architecture:  x86
	Disabled:      none
	Misc:          softdebug 
	LLVM:          supported, not enabled.
	GC:            Included Boehm (with typed GC and Parallel Mark)


# Working approach 
This repository snapshots, umbraco release versions 
from the original codeplex repository: http://umbraco.codeplex.com/

This retains the original commit history.

At present we have 4.7.2 and 4.11.3, with 4.7.2 being in beta 1, 
and 4.11.3 being newly added.

Our changes are added on top of the existing commit history.

The fork on umbraco.codeplex has now been deleted.
