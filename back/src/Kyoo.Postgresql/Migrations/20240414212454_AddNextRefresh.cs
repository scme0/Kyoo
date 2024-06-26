using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kyoo.Postgresql.Migrations;

/// <inheritdoc />
public partial class AddNextRefresh : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AddColumn<DateTime>(
			name: "next_metadata_refresh",
			table: "shows",
			type: "timestamp with time zone",
			nullable: true,
			defaultValueSql: "now() at time zone 'utc' + interval '2 hours'"
		);

		migrationBuilder.AddColumn<DateTime>(
			name: "next_metadata_refresh",
			table: "seasons",
			type: "timestamp with time zone",
			nullable: true,
			defaultValueSql: "now() at time zone 'utc' + interval '2 hours'"
		);

		migrationBuilder.AddColumn<DateTime>(
			name: "next_metadata_refresh",
			table: "movies",
			type: "timestamp with time zone",
			nullable: true,
			defaultValueSql: "now() at time zone 'utc' + interval '2 hours'"
		);

		migrationBuilder.AddColumn<DateTime>(
			name: "next_metadata_refresh",
			table: "episodes",
			type: "timestamp with time zone",
			nullable: true,
			defaultValueSql: "now() at time zone 'utc' + interval '2 hours'"
		);

		migrationBuilder.AddColumn<DateTime>(
			name: "next_metadata_refresh",
			table: "collections",
			type: "timestamp with time zone",
			nullable: true,
			defaultValueSql: "now() at time zone 'utc' + interval '2 hours'"
		);

		// language=PostgreSQL
		migrationBuilder.Sql(
			"""
			update episodes as e set external_id = (
				SELECT jsonb_build_object(
					'themoviedatabase', jsonb_build_object(
						'ShowId', s.external_id->'themoviedatabase'->'DataId',
						'Season', e.season_number,
						'Episode', e.episode_number,
						'Link', null
					)
				)
				FROM shows AS s
				WHERE s.id = e.show_id
			);
			"""
		);
	}

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropColumn(name: "next_metadata_refresh", table: "shows");

		migrationBuilder.DropColumn(name: "next_metadata_refresh", table: "seasons");

		migrationBuilder.DropColumn(name: "next_metadata_refresh", table: "movies");

		migrationBuilder.DropColumn(name: "next_metadata_refresh", table: "episodes");

		migrationBuilder.DropColumn(name: "next_metadata_refresh", table: "collections");

		// language=PostgreSQL
		migrationBuilder.Sql("update episodes as e set external_id = '{}';");
	}
}
