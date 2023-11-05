#!/bin/sh

# make sure that dotnet runs the Intel x64 version only
if [[ "$OSTYPE" =~ ^darwin ]]; then
    export PATH="/usr/local/share/dotnet/x64:$PATH"
fi

# use the x64 version of dotnet to run mgcb-editor 
dotnet mgcb-editor Content/Content.mgcb
