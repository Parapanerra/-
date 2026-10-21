#!/bin/sh
set -eu
if [ ! -f /data/PostingDb.db ]; then
  cp /seed/PostingDb.db /data/PostingDb.db
fi
exec dotnet PostService.dll
