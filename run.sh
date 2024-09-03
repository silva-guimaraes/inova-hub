#!/usr/bin/bash

pg_isready -h localhost -p 5432 > /dev/null 2>&1

if [ $? -ne 0 ]; then
    echo "iniciando servidor postgres... senha de super usuário poderá ser necessária."
    ./docker.sh
fi

templ generate cmd/web && go run -v cmd/api/main.go;
