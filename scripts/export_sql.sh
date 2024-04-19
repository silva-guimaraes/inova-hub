#!/usr/bin/sh

pg_dump --data-only --inserts -U postgres -h localhost my_db > export.sql
