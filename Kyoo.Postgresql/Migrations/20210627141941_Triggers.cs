﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace Kyoo.Postgresql.Migrations
{
	public partial class Triggers : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			// language=PostgreSQL
			migrationBuilder.Sql(@"
			CREATE FUNCTION season_slug_update()
			RETURNS TRIGGER
			LANGUAGE PLPGSQL
			AS $$
			BEGIN
			    NEW.slug := CONCAT(
					(SELECT slug FROM shows WHERE id = NEW.show_id), 
					'-s',
					NEW.season_number
				);
				RETURN NEW;
			END
			$$;");
			
			// language=PostgreSQL
			migrationBuilder.Sql(@"
			CREATE TRIGGER season_slug_trigger BEFORE INSERT OR UPDATE OF season_number, show_id ON seasons 
			FOR EACH ROW EXECUTE PROCEDURE season_slug_update();");
			
			
			// language=PostgreSQL
			migrationBuilder.Sql(@"
			CREATE FUNCTION episode_slug_update()
			RETURNS TRIGGER
			LANGUAGE PLPGSQL
			AS $$
			BEGIN
				NEW.slug := CONCAT(
					(SELECT slug FROM shows WHERE id = NEW.show_id),
					CASE
					    WHEN NEW.season_number IS NULL THEN CONCAT('-', NEW.absolute_number)
					    ELSE CONCAT('-s', NEW.season_number, 'e', NEW.episode_number)
				    END
				);
				RETURN NEW;
			END
			$$;");
			
			// language=PostgreSQL
			migrationBuilder.Sql(@"
			CREATE TRIGGER episode_slug_trigger 
			BEFORE INSERT OR UPDATE OF absolute_number, episode_number, season_number, show_id ON episodes
			FOR EACH ROW EXECUTE PROCEDURE episode_slug_update();");


			// language=PostgreSQL
			migrationBuilder.Sql(@"
			CREATE FUNCTION show_slug_update()
			RETURNS TRIGGER
			LANGUAGE PLPGSQL
			AS $$
			BEGIN
				UPDATE seasons SET slug = CONCAT(NEW.slug, '-s', season_number) WHERE show_id = NEW.id;
				UPDATE episodes SET slug = CASE
				    WHEN season_number IS NULL THEN CONCAT(NEW.slug, '-', absolute_number) 
				    ELSE CONCAT(NEW.slug, '-s', season_number, 'e', episode_number)
				END
				WHERE show_id = NEW.id;
				RETURN NEW;
			END
			$$;");
	        
			// language=PostgreSQL
			migrationBuilder.Sql(@"
			CREATE TRIGGER show_slug_trigger AFTER UPDATE OF slug ON shows
			FOR EACH ROW EXECUTE PROCEDURE show_slug_update();");
			
			
			// language=PostgreSQL
			migrationBuilder.Sql(@"
			CREATE VIEW library_items AS
				SELECT s.id, s.slug, s.title, s.overview, s.status, s.start_air, s.end_air, s.poster, CASE
					WHEN s.is_movie THEN 'movie'::item_type
					ELSE 'show'::item_type
					END AS type
				FROM shows AS s
				WHERE NOT (EXISTS (
					SELECT 1
					FROM link_collection_show AS l
					INNER JOIN collections AS c ON l.first_id = c.id
					WHERE s.id = l.second_id))
				UNION ALL
				SELECT -c0.id, c0.slug, c0.name AS title, c0.overview, 'unknown'::status AS status, 
				       NULL AS start_air, NULL AS end_air, c0.poster, 'collection'::item_type AS type
				FROM collections AS c0");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			// language=PostgreSQL
			migrationBuilder.Sql("DROP TRIGGER show_slug_trigger ON shows;");
			// language=PostgreSQL
			migrationBuilder.Sql(@"DROP FUNCTION show_slug_update;");
			// language=PostgreSQL
			migrationBuilder.Sql(@"DROP TRIGGER season_slug_trigger ON seasons;");
			// language=PostgreSQL
			migrationBuilder.Sql(@"DROP FUNCTION season_slug_update;");
			// language=PostgreSQL
			migrationBuilder.Sql("DROP TRIGGER episode_slug_trigger ON episodes;");
			// language=PostgreSQL
			migrationBuilder.Sql(@"DROP FUNCTION episode_slug_update;");
			// language=PostgreSQL
			migrationBuilder.Sql(@"DROP VIEW library_items;");
		}
	}
}