#!/usr/bin/sh

psql -U postgres -h localhost my_db -f export.sql
