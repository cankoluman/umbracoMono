#!/bin/bash
BASE=/home/kol3/Development/umbraco/umbracomono/tools/Mono
xbuild $BASE/targets/RestoreNugetPackages.targets "$@"
