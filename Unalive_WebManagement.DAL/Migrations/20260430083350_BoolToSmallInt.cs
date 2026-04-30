using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unalive_WebManagement.DAL.Migrations
{
    /// <inheritdoc />
    public partial class BoolToSmallInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // First, identify columns that are already smallint vs those that are still boolean
            // CharacterSkills.LockedState seems to already be smallint in the database based on the error
            
            // Drop existing defaults before changing types
            migrationBuilder.Sql("ALTER TABLE \"Users\" ALTER COLUMN \"IsOnline\" DROP DEFAULT;");
            migrationBuilder.Sql("ALTER TABLE \"Users\" ALTER COLUMN \"IsEmailVerified\" DROP DEFAULT;");
            migrationBuilder.Sql("ALTER TABLE \"Users\" ALTER COLUMN \"Banned\" DROP DEFAULT;");

            // Conditionally cast only if types match
            // We use an anonymous block to check column types before altering
            migrationBuilder.Sql(@"
DO $$ 
BEGIN 
    -- Users table
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'Users' AND column_name = 'IsOnline' AND data_type = 'boolean') THEN
        ALTER TABLE ""Users"" ALTER COLUMN ""IsOnline"" TYPE smallint USING CASE WHEN ""IsOnline"" THEN 1 ELSE 0 END;
    END IF;
    
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'Users' AND column_name = 'IsEmailVerified' AND data_type = 'boolean') THEN
        ALTER TABLE ""Users"" ALTER COLUMN ""IsEmailVerified"" TYPE smallint USING CASE WHEN ""IsEmailVerified"" THEN 1 ELSE 0 END;
    END IF;
    
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'Users' AND column_name = 'Banned' AND data_type = 'boolean') THEN
        ALTER TABLE ""Users"" ALTER COLUMN ""Banned"" TYPE smallint USING CASE WHEN ""Banned"" THEN 1 ELSE 0 END;
    END IF;

    -- Reports table
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'Reports' AND column_name = 'Approved' AND data_type = 'boolean') THEN
        ALTER TABLE ""Reports"" ALTER COLUMN ""Approved"" TYPE smallint USING CASE WHEN ""Approved"" THEN 1 ELSE 0 END;
    END IF;

    -- Notifications table
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'Notifications' AND column_name = 'Read' AND data_type = 'boolean') THEN
        ALTER TABLE ""Notifications"" ALTER COLUMN ""Read"" TYPE smallint USING CASE WHEN ""Read"" THEN 1 ELSE 0 END;
    END IF;

    -- CharacterSkills table
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'CharacterSkills' AND column_name = 'LockedState' AND data_type = 'boolean') THEN
        ALTER TABLE ""CharacterSkills"" ALTER COLUMN ""LockedState"" TYPE smallint USING CASE WHEN ""LockedState"" THEN 1 ELSE 0 END;
    END IF;

    -- CharacterPassives table
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'CharacterPassives' AND column_name = 'LockedState' AND data_type = 'boolean') THEN
        ALTER TABLE ""CharacterPassives"" ALTER COLUMN ""LockedState"" TYPE smallint USING CASE WHEN ""LockedState"" THEN 1 ELSE 0 END;
    END IF;

    -- CharacterAttacks table
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'CharacterAttacks' AND column_name = 'LockedState' AND data_type = 'boolean') THEN
        ALTER TABLE ""CharacterAttacks"" ALTER COLUMN ""LockedState"" TYPE smallint USING CASE WHEN ""LockedState"" THEN 1 ELSE 0 END;
    END IF;
END $$;");

            migrationBuilder.AlterColumn<short>(
                name: "IsOnline",
                table: "Users",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<short>(
                name: "IsEmailVerified",
                table: "Users",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<short>(
                name: "Banned",
                table: "Users",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<short>(
                name: "Approved",
                table: "Reports",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<short>(
                name: "Read",
                table: "Notifications",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<short>(
                name: "LockedState",
                table: "CharacterSkills",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<short>(
                name: "LockedState",
                table: "CharacterPassives",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<short>(
                name: "LockedState",
                table: "CharacterAttacks",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsOnline",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldDefaultValue: (short)0);

            migrationBuilder.AlterColumn<bool>(
                name: "IsEmailVerified",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldDefaultValue: (short)0);

            migrationBuilder.AlterColumn<bool>(
                name: "Banned",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldDefaultValue: (short)0);

            migrationBuilder.AlterColumn<bool>(
                name: "Approved",
                table: "Reports",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.AlterColumn<bool>(
                name: "Read",
                table: "Notifications",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.AlterColumn<bool>(
                name: "LockedState",
                table: "CharacterSkills",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.AlterColumn<bool>(
                name: "LockedState",
                table: "CharacterPassives",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.AlterColumn<bool>(
                name: "LockedState",
                table: "CharacterAttacks",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint");
        }
    }
}
