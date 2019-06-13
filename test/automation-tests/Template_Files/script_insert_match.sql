-- PROCEDURE: public.insert_match(jsonb)

-- DROP PROCEDURE public.insert_match(jsonb);

CREATE OR REPLACE PROCEDURE public.insert_match(
	jsonValue jsonb)
LANGUAGE 'plpgsql'

AS $BODY$
BEGIN
	create temporary table temp_json ("values" JSONB) on commit drop;
	
	WITH json_array AS (SELECT jsonb_array_elements(jsonValue))
	INSERT INTO temp_json
	SELECT * FROM json_array;
	
	INSERT INTO public."Match"(
	"Id", "Value", "Language", "SportId", "LeagueId", "EventDate", "CreatedTime", "ModifiedTime", "Region" )
	select cast("values" ->> 'Id' as text) as Id,
		   "values" as "Value",
		   'en-US' as "Language",
			1 as "SportId",
		   cast( "values"->'League' ->> 'Id'  as text) as LeagueId,
		   cast( "values" ->'EventDate' as text)::timestamp with time zone as EventDate,
		   cast( "values" ->'EventDate' as text)::timestamp with time zone as CreatedTime,
		   cast( "values" ->'EventDate' as text)::timestamp with time zone as ModifiedTime,
		   'Asia' as "Region"
		   
		   
	from  temp_json;
	
END ;
$BODY$;
