﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetFileAPI.Database;

#nullable disable

namespace NetFileAPI.Migrations
{
    [DbContext(typeof(MySqlDbContext))]
    partial class MySqlDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("NetFileAPI.Models.FileModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("MimeType")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("mime_type");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("name");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("path");

                    b.Property<int>("StorageType")
                        .HasColumnType("int")
                        .HasColumnName("storage_type");

                    b.HasKey("Id");

                    b.ToTable("file_models");
                });
#pragma warning restore 612, 618
        }
    }
}
