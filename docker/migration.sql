﻿CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE TABLE "Pallets" (
    "Id" bigint GENERATED BY DEFAULT AS IDENTITY,
    "Width" double precision NOT NULL,
    "Height" double precision NOT NULL,
    "Length" double precision NOT NULL,
    "Weight" double precision NOT NULL,
    CONSTRAINT "PK_Pallets" PRIMARY KEY ("Id")
);

CREATE TABLE "Boxes" (
    "Id" bigint GENERATED BY DEFAULT AS IDENTITY,
    "ProductionDate" date,
    "ExpirationDate" date NOT NULL,
    "PalletId" bigint NOT NULL,
    "Width" double precision NOT NULL,
    "Height" double precision NOT NULL,
    "Length" double precision NOT NULL,
    "Weight" double precision NOT NULL,
    CONSTRAINT "PK_Boxes" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Boxes_Pallets_PalletId" FOREIGN KEY ("PalletId") REFERENCES "Pallets" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_Boxes_PalletId" ON "Boxes" ("PalletId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240912223713_InitialCreate', '8.0.4');

COMMIT;

