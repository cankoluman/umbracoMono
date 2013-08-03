#!/bin/bash

RELEASE=6.0.6
COMMENT=
VERSION=$RELEASE

if ["$COMMENT" -eq ""]; then
	VERSION=$RELEASE 
else 
	VERSION=$RELEASE-$COMMENT
fi

xbuild "Build.mono.proj" /p:BUILD_RELEASE=$RELEASE /p:BUILD_COMMENT=$COMMENT

echo "This file is only here so that the containing folder will be included in the NuGet package, it is safe to delete. > .\_BuildOutput\WebApp\App_Code\dummy.txt"
echo "This file is only here so that the containing folder will be included in the NuGet package, it is safe to delete. > .\_BuildOutput\WebApp\App_Data\dummy.txt"
echo "This file is only here so that the containing folder will be included in the NuGet package, it is safe to delete. > .\_BuildOutput\WebApp\App_Plugins\dummy.txt"
echo "This file is only here so that the containing folder will be included in the NuGet package, it is safe to delete. > .\_BuildOutput\WebApp\css\dummy.txt"
echo "This file is only here so that the containing folder will be included in the NuGet package, it is safe to delete. > .\_BuildOutput\WebApp\macroScripts\dummy.txt"
echo "This file is only here so that the containing folder will be included in the NuGet package, it is safe to delete. > .\_BuildOutput\WebApp\masterpages\dummy.txt"
echo "This file is only here so that the containing folder will be included in the NuGet package, it is safe to delete. > .\_BuildOutput\WebApp\media\dummy.txt"
echo "This file is only here so that the containing folder will be included in the NuGet package, it is safe to delete. > .\_BuildOutput\WebApp\scripts\dummy.txt"
echo "This file is only here so that the containing folder will be included in the NuGet package, it is safe to delete. > .\_BuildOutput\WebApp\usercontrols\dummy.txt"
echo "This file is only here so that the containing folder will be included in the NuGet package, it is safe to delete. > .\_BuildOutput\WebApp\Views\Partials\dummy.txt"
echo "This file is only here so that the containing folder will be included in the NuGet package, it is safe to delete. > .\_BuildOutput\WebApp\Views\MacroPartials\dummy.txt"
echo "This file is only here so that the containing folder will be included in the NuGet package, it is safe to delete. > .\_BuildOutput\WebApp\xslt\dummy.txt"

mono ../src/.nuget/NuGet.exe pack NuSpecs/UmbracoCms.Core.mono.nuspec -Version $VERSION
mono ../src/.nuget/NuGet.exe pack NuSpecs/UmbracoCms.mono.nuspec -Version $VERSION

if [ "$?" = "0" ]; then
	echo "Build succeded..."
else
	echo "Build failed..." 2>&1
	exit 1
fi
