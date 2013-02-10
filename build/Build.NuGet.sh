#!/usr/bin/env bash
export EnableNuGetPackageRestore="true"
nuget pack NuSpecs/UmbracoCms.Core.nuspec
nuget pack NuSpecs/UmbracoCms.nuspec
