FROM postgres:latest

COPY migration.sql /docker-entrypoint-initdb.d/

EXPOSE 5432