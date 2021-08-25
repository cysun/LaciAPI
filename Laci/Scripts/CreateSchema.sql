﻿CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE TABLE "Cities" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "Name" character varying(255) NOT NULL,
    "Population" integer NOT NULL,
    CONSTRAINT "PK_Cities" PRIMARY KEY ("Id")
);

CREATE TABLE "Records" (
    "CityId" integer NOT NULL,
    "Date" date NOT NULL,
    "Tests" integer NULL,
    "Cases" integer NULL,
    "Deaths" integer NULL,
    CONSTRAINT "PK_Records" PRIMARY KEY ("CityId", "Date"),
    CONSTRAINT "FK_Records_Cities_CityId" FOREIGN KEY ("CityId") REFERENCES "Cities" ("Id") ON DELETE CASCADE
);

CREATE UNIQUE INDEX "IX_Cities_Name" ON "Cities" ("Name");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20210825175054_InitialSchema', '5.0.9');

COMMIT;

