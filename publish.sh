#!/bin/sh

# make sure that dotnet runs the Intel x64 version only
if [[ "$OSTYPE" =~ ^darwin ]]; then
    export PATH="/usr/local/share/dotnet/x64:$PATH"
    # build and automtically make ready the mgcb pipeline toolkit
dotnet publish --framework net6.0 -c Release -r osx-arm64 /p:PublishReadyToRun=false /p:TieredCompilation=false --self-contained false
dotnet publish --framework net6.0 -c Release -r osx-x64 /p:PublishReadyToRun=false /p:TieredCompilation=false --self-contained false
fi

# TODO windows , and linux 
