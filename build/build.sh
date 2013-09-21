#!/bin/bash

RELEASE=6.0.6
COMMENT=
VERSION=$RELEASE
CONFIGURATION=

if ["$COMMENT" -eq ""]; then
	VERSION=$RELEASE 
else 
	VERSION=$RELEASE-$COMMENT
fi

xbuild "Build.mono.proj" /p:BUILD_RELEASE=$RELEASE /p:BUILD_COMMENT=$COMMENT /p:BUILD_CONFIGURATION=$1

if [ "$?" = "0" ]; then
	echo "Build succeded..."
else
	echo "Build failed..." 2>&1
	exit 1
fi
