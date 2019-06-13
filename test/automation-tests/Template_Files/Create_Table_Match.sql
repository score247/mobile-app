CREATE TABLE IF NOT EXISTS public."Match"
(
    "Id" text COLLATE pg_catalog."default" NOT NULL,
    "Value" jsonb NOT NULL,
    "Language" text COLLATE pg_catalog."default" NOT NULL DEFAULT 'en_US'::text,
    "SportId" integer NOT NULL DEFAULT 1,
    "LeagueId" text COLLATE pg_catalog."default" NOT NULL DEFAULT ''::text,
    "EventDate" timestamp with time zone,
	"CreatedTime" timestamp with time zone,
	"ModifiedTime" timestamp with time zone,
	"Region" text COLLATE pg_catalog."default" NOT NULL DEFAULT 'Asia'::text,
    CONSTRAINT "Match_PrimaryKey" PRIMARY KEY ("Id")
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;