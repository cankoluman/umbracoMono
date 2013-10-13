#!/bin/bash
BASE=/home/kol3/development/umbraco/umbracomono/tools/Mono
xbuild $BASE/targets/RestoreNugetPackages.targets "$@"
