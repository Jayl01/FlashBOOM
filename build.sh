#!/bin/sh

# make sure that dotnet runs the Intel x64 version only
# if on mac os
if [[ "$OSTYPE" =~ ^darwin ]]; then
    export PATH="/usr/local/share/dotnet/x64:$PATH"
fi

# build and automtically make ready the mgcb pipeline toolkit
dotnet build