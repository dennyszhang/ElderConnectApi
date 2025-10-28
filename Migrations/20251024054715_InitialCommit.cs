using System;
using ElderConnectApi.Data.Common;
using ElderConnectApi.Data.Entities;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace ElderConnectApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCommit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:booking_status", "cancelled,completed,confirmed,in_progress,paid,pending")
                .Annotation("Npgsql:Enum:gender", "female,male")
                .Annotation("Npgsql:Enum:nurse_account_status", "active,pending,suspended")
                .Annotation("Npgsql:Enum:user_account_status", "active,pending,suspended")
                .Annotation("Npgsql:PostgresExtension:postgis", ",,");

            migrationBuilder.CreateTable(
                name: "nurses",
                columns: table => new
                {
                    nurse_id = table.Column<Guid>(type: "uuid", nullable: false),
                    account_status = table.Column<NurseAccountStatus>(type: "nurse_account_status", nullable: false),
                    name = table.Column<string>(type: "varchar(100)", nullable: false),
                    date_of_birth = table.Column<DateOnly>(type: "date", nullable: false),
                    join_date = table.Column<DateOnly>(type: "date", nullable: true),
                    email = table.Column<string>(type: "varchar(100)", nullable: false),
                    gender = table.Column<Gender>(type: "gender", nullable: false),
                    phone_number = table.Column<string>(type: "varchar(15)", nullable: false),
                    graduated_from = table.Column<string>(type: "varchar(200)", nullable: true),
                    license_number = table.Column<string>(type: "varchar(50)", nullable: true),
                    profile_image_url = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_nurses", x => x.nurse_id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "varchar(100)", nullable: false),
                    email = table.Column<string>(type: "varchar(100)", nullable: false),
                    phone_number = table.Column<string>(type: "varchar(15)", nullable: false),
                    password_hash = table.Column<string>(type: "varchar(256)", nullable: false),
                    salt = table.Column<string>(type: "varchar(256)", nullable: false),
                    account_status = table.Column<UserAccountStatus>(type: "user_account_status", nullable: false),
                    join_date = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "nurse_addresses",
                columns: table => new
                {
                    address_id = table.Column<Guid>(type: "uuid", nullable: false),
                    nurse_id = table.Column<Guid>(type: "uuid", nullable: false),
                    country = table.Column<string>(type: "varchar(100)", nullable: false),
                    province = table.Column<string>(type: "varchar(100)", nullable: false),
                    city = table.Column<string>(type: "varchar(100)", nullable: false),
                    address_line = table.Column<string>(type: "varchar(200)", nullable: true),
                    coordinates = table.Column<Point>(type: "geography (point)", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_nurse_addresses", x => x.address_id);
                    table.ForeignKey(
                        name: "fk_nurse_addresses_nurses_nurse_id",
                        column: x => x.nurse_id,
                        principalTable: "nurses",
                        principalColumn: "nurse_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "nurse_leaves",
                columns: table => new
                {
                    nurse_id = table.Column<Guid>(type: "uuid", nullable: false),
                    leave_date = table.Column<DateOnly>(type: "date", nullable: false),
                    reason = table.Column<string>(type: "varchar(500)", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_nurse_leaves", x => new { x.nurse_id, x.leave_date });
                    table.ForeignKey(
                        name: "fk_nurse_leaves_nurses_nurse_id",
                        column: x => x.nurse_id,
                        principalTable: "nurses",
                        principalColumn: "nurse_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "nurse_schedules",
                columns: table => new
                {
                    nurse_id = table.Column<Guid>(type: "uuid", nullable: false),
                    day_of_week = table.Column<int>(type: "integer", nullable: false),
                    start_time = table.Column<TimeOnly>(type: "time", nullable: false),
                    end_time = table.Column<TimeOnly>(type: "time", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_nurse_schedules", x => new { x.nurse_id, x.day_of_week });
                    table.ForeignKey(
                        name: "fk_nurse_schedules_nurses_nurse_id",
                        column: x => x.nurse_id,
                        principalTable: "nurses",
                        principalColumn: "nurse_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "addresses",
                columns: table => new
                {
                    address_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    country = table.Column<string>(type: "varchar(100)", nullable: false),
                    province = table.Column<string>(type: "varchar(100)", nullable: false),
                    city = table.Column<string>(type: "varchar(100)", nullable: false),
                    address_line = table.Column<string>(type: "varchar(200)", nullable: true),
                    coordinates = table.Column<Point>(type: "geography (point)", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_addresses", x => x.address_id);
                    table.ForeignKey(
                        name: "fk_addresses_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "patients",
                columns: table => new
                {
                    patient_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "varchar(100)", nullable: false),
                    date_of_birth = table.Column<DateOnly>(type: "date", nullable: false),
                    height_in_cm = table.Column<double>(type: "double precision", nullable: false),
                    weight_in_kg = table.Column<double>(type: "double precision", nullable: false),
                    medical_conditions = table.Column<string>(type: "varchar(500)", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_patients", x => x.patient_id);
                    table.ForeignKey(
                        name: "fk_patients_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bookings",
                columns: table => new
                {
                    booking_id = table.Column<Guid>(type: "uuid", nullable: false),
                    reference_code = table.Column<string>(type: "varchar(50)", nullable: true),
                    start_time = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    end_time = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    status = table.Column<BookingStatus>(type: "booking_status", nullable: false),
                    timeline = table.Column<BookingTimeline>(type: "jsonb", nullable: true),
                    payment = table.Column<BookingPayment>(type: "jsonb", nullable: true),
                    nurse_id = table.Column<Guid>(type: "uuid", nullable: false),
                    nurse_snapshot = table.Column<string>(type: "jsonb", nullable: true),
                    patient_id = table.Column<Guid>(type: "uuid", nullable: false),
                    patient_snapshot = table.Column<string>(type: "jsonb", nullable: true),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_snapshot = table.Column<string>(type: "jsonb", nullable: true),
                    address_id = table.Column<Guid>(type: "uuid", nullable: false),
                    address_snapshot = table.Column<string>(type: "jsonb", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_bookings", x => x.booking_id);
                    table.ForeignKey(
                        name: "fk_bookings_addresses_address_id",
                        column: x => x.address_id,
                        principalTable: "addresses",
                        principalColumn: "address_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_bookings_nurses_nurse_id",
                        column: x => x.nurse_id,
                        principalTable: "nurses",
                        principalColumn: "nurse_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_bookings_patients_patient_id",
                        column: x => x.patient_id,
                        principalTable: "patients",
                        principalColumn: "patient_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_bookings_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_addresses_user_id",
                table: "addresses",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_bookings_address_id",
                table: "bookings",
                column: "address_id");

            migrationBuilder.CreateIndex(
                name: "ix_bookings_nurse_id",
                table: "bookings",
                column: "nurse_id");

            migrationBuilder.CreateIndex(
                name: "ix_bookings_patient_id",
                table: "bookings",
                column: "patient_id");

            migrationBuilder.CreateIndex(
                name: "ix_bookings_reference_code",
                table: "bookings",
                column: "reference_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_bookings_status",
                table: "bookings",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "ix_bookings_user_id",
                table: "bookings",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_nurse_addresses_nurse_id",
                table: "nurse_addresses",
                column: "nurse_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_nurse_leaves_nurse_id",
                table: "nurse_leaves",
                column: "nurse_id");

            migrationBuilder.CreateIndex(
                name: "ix_nurse_schedules_day_of_week",
                table: "nurse_schedules",
                column: "day_of_week");

            migrationBuilder.CreateIndex(
                name: "ix_nurse_schedules_nurse_id",
                table: "nurse_schedules",
                column: "nurse_id");

            migrationBuilder.CreateIndex(
                name: "ix_nurses_email",
                table: "nurses",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_nurses_phone_number",
                table: "nurses",
                column: "phone_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_patients_user_id",
                table: "patients",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_users_phone_number",
                table: "users",
                column: "phone_number",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bookings");

            migrationBuilder.DropTable(
                name: "nurse_addresses");

            migrationBuilder.DropTable(
                name: "nurse_leaves");

            migrationBuilder.DropTable(
                name: "nurse_schedules");

            migrationBuilder.DropTable(
                name: "addresses");

            migrationBuilder.DropTable(
                name: "patients");

            migrationBuilder.DropTable(
                name: "nurses");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
