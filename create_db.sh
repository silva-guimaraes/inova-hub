#!/usr/bin/bash
set -e


echo 'create database inova;' | psql -U postgres --host localhost inova
psql -U postgres --host localhost inova < internal/database/schema.sql
